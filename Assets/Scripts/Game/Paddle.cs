using UnityEngine;

public class Paddle : MonoBehaviour
{
    public SpriteRenderer confiner; 
    protected float paddleHeight;
    protected float minY, maxY;

    protected Vector3 restPos; 

    protected virtual void Start()
    {
        GameObject playArea = GameObject.Find("PlayArea");
        if (playArea != null)
        {
            confiner = playArea.GetComponent<SpriteRenderer>();
        }
        else
        {
            Debug.LogError("PlayArea not found!");
        }

        paddleHeight = GetComponent<SpriteRenderer>().bounds.extents.y;
        Bounds confinerBounds = confiner.bounds;
        minY = confinerBounds.min.y + paddleHeight;
        maxY = confinerBounds.max.y - paddleHeight;

        restPos = transform.position;
    }

    protected void MovePaddle(float targetY)
    {
        targetY = Mathf.Clamp(targetY, minY, maxY);
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
    }
}
