using UnityEngine;

public class ReflectiveRaycaster : MonoBehaviour
{
    public Transform center;        
    public float orbitDistance = 5f; 
    public float rotationSpeed = 30f; 
    public Transform raycastOrigin;  
    public float rayDistance = 10f;  

    private float currentAngle = 0f; 

    private void Update()
    {
        if (center == null || raycastOrigin == null) return;

        float rotationInput = Input.GetAxis("Horizontal"); 
        currentAngle += rotationInput * rotationSpeed * Time.deltaTime;

        Vector2 offset = new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle)) * orbitDistance;
        transform.position = (Vector2)center.position + offset;
        Vector2 directionToCenter = (center.position - transform.position).normalized;
        float angleToCenter = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angleToCenter);
    }

    private void OnDrawGizmos()
    {
        if (raycastOrigin == null) return;

        Vector2 origin = raycastOrigin.position;
        Vector2 direction = raycastOrigin.up;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, rayDistance);
        if (hit.collider != null && hit.collider.CompareTag("Mirror"))
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(origin, hit.point);

            Vector2 normal = hit.normal;
            Vector2 reflectedDirection = Vector2.Reflect(direction, normal);
            float angleOfIncidence = Vector2.Angle(-direction, normal);
            float angleOfReflection = Vector2.Angle(reflectedDirection, normal);
            float dotProduct = Vector2.Dot(reflectedDirection, normal);

            if (angleOfIncidence > 45f)  
            {
                if (dotProduct > 0)  
                {
                    reflectedDirection = Quaternion.Euler(0f, 0f, -30f) * reflectedDirection; 
                }
                else  
                {
                    reflectedDirection = Quaternion.Euler(0f, 0f, 30f) * reflectedDirection; 
                }
            }
            else if (angleOfIncidence < 45f)  
            {
                if (dotProduct > 0)  
                {
                    reflectedDirection = Quaternion.Euler(0f, 0f, 30f) * reflectedDirection; 
                }
                else  
                {
                    reflectedDirection = Quaternion.Euler(0f, 0f, -30f) * reflectedDirection; 
                }
            }

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(hit.point, hit.point + reflectedDirection * rayDistance);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(hit.point, hit.point + normal);

       
#if UNITY_EDITOR
            UnityEditor.Handles.color = Color.magenta;

            DrawAngleArc(hit.point, -direction, normal, angleOfIncidence, "Inc: ");

            DrawAngleArc(hit.point, reflectedDirection, normal, angleOfReflection, "Refl: ");
#endif
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(origin, origin + direction * rayDistance);
        }
    }

#if UNITY_EDITOR
    private void DrawAngleArc(Vector2 origin, Vector2 dir1, Vector2 dir2, float angle, string label)
    {

        Vector2 midDir = (dir1.normalized + dir2.normalized).normalized;
        UnityEditor.Handles.DrawWireArc(origin, Vector3.forward, dir1, angle, 0.5f);
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.black; 
        style.fontSize = 12; 
        Vector2 labelPosition = origin + midDir * 0.7f; 
        UnityEditor.Handles.Label(labelPosition, $"{label}{angle:0.0}°", style);
    }
#endif
}
