using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Ball Settings")]
    [SerializeField] private float initialSpeed = 5f;
    [SerializeField] private float speedIncrease = 1f;
    [SerializeField] private float randomnessFactor = 0.1f;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask bounceableLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask gameOverBlock;
    [SerializeField] private LayerMask scoreBlock;

    [SerializeField] private Transform restPos;

   private Vector2 velocity;
    private float currentSpeed;

    private void Start()
    {
        ResetPosition(); 
    }

    private void Update()
    {
        if (Time.timeScale > 0)
        {
            MoveBall();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision);
    }

    public void LaunchBall()
    {
        currentSpeed = initialSpeed;
        Vector2 direction = new Vector2(1, Random.Range(-randomnessFactor, randomnessFactor)).normalized;
        velocity = direction * currentSpeed;
    }

    public void ResetPosition()
    {
        if (GameManager.Instance != null)
        {
            transform.position = restPos.position; 
        }
        velocity = Vector2.zero; 
    }

    private void MoveBall()
    {
        transform.Translate(velocity * Time.deltaTime);
    }

    private void HandleCollision(Collision2D collision)
    {
        int collisionLayer = collision.gameObject.layer;

        if (IsLayerInMask(collisionLayer, bounceableLayer | playerLayer))
        {
            if (IsLayerInMask(collisionLayer, playerLayer))
            {
                IncreaseSpeed();
            }

            ReflectVelocity(collision.contacts[0].normal); 
        }

        if (IsLayerInMask(collisionLayer, gameOverBlock))
        {
            GameManager.Instance.TriggerGameOver();
        }

        if (IsLayerInMask(collisionLayer, scoreBlock))
        {
            GameManager.Instance.IncreaseScore(10);            
        }
    }

    private void IncreaseSpeed()
    {
        currentSpeed += speedIncrease;
    }

    private void ReflectVelocity(Vector2 normal)
    {
        velocity = Vector2.Reflect(velocity, normal);
        velocity.y += Random.Range(-randomnessFactor, randomnessFactor);
        velocity = velocity.normalized * currentSpeed;
    }

    private bool IsLayerInMask(int layer, LayerMask mask)
    {
        return ((1 << layer) & mask) != 0;
    }
}
