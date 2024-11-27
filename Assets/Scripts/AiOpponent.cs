using UnityEngine;

public class AIOpponent : Paddle
{
    public Transform ball; 
    public float followSpeed = 5f;
    private Vector3 aiRestPos;

    protected override void Start()
    {
        base.Start(); 
        aiRestPos = restPos;
    }

    void Update()
    {
        if (ball != null)
        {
            float targetY = ball.position.y;
            float newY = Mathf.MoveTowards(transform.position.y, targetY, followSpeed * Time.deltaTime);
            MovePaddle(newY);
        }
    }

    public Vector3 GetAiRestPosition()
    {
        return aiRestPos;
    }
}
