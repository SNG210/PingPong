using UnityEngine;

public class Player : Paddle
{
    public float sensitivity = 100f;
    private Vector3 playerRestPos;
    public bool allowMovement = true;

    private const string SensitivityPrefKey = "PlayerSensitivity"; 

    protected override void Start()
    {
        base.Start();
        playerRestPos = restPos;

        if (PlayerPrefs.HasKey(SensitivityPrefKey))
        {
            sensitivity = PlayerPrefs.GetFloat(SensitivityPrefKey);
        }
    }

    void Update()
    {
        if (allowMovement)
        {
            float mouseY = Input.GetAxisRaw("Mouse Y");
            float newY = transform.position.y + mouseY * sensitivity * Time.deltaTime;
            MovePaddle(newY);
        }
    }

    public Vector3 GetPlayerRestPosition()
    {
        return playerRestPos;
    }

    public void SetSensitivity(float newSensitivity)
    {
        sensitivity = newSensitivity;
        PlayerPrefs.SetFloat(SensitivityPrefKey, newSensitivity); 
        PlayerPrefs.Save();
    }
}
