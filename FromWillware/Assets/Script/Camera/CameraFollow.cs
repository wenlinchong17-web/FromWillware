using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform PlayerTransform;
    public float MouseSensitivity;
    public float Distance;

    public float xRotation = 20f;
    public float yRotation = 0f;

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -30f, 60f);
    }

    void LateUpdate()
    {
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);
        Vector3 direction = rotation * new Vector3(0, 0, -Distance);

        transform.position = PlayerTransform.position + direction;
        transform.LookAt(PlayerTransform);
    }
}