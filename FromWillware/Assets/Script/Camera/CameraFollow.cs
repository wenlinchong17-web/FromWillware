using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Player Settings")]
    public Transform PlayerTransform;
    
    [Header("Mouse Settings")]
    public float MouseSensitivity= 200f;
    
    [Header("Camera Settings")]
    public float Distance= 3.5f;
    public float VerticalOffset = 1.5f;
    public float LookAtOffset = 1.5f;
    public float MinDistance = 0.6f;
    
    [Header("Rotation Settings")]
    public float xRotation = 30f;
    public float yRotation = 0f;
    
    [Header("Obstacle Settings")]
    public LayerMask ObstacleLayerMask = ~0;
    
    [Header("LockOn Settings")]
    public Transform CurrentTarget;
    public float LockRotationSpeed = 5f;
    
    
    void Start()
    {
        // 隐藏鼠标并锁定光标
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

        if (!IsLockOn())
        {
            float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -30f, 60f);
        }
       
    }

    void LateUpdate()
    {
       if(IsLockOn())
            LockOnUpdate();
       else
           FreeLookUpdate();
    }

    bool IsLockOn()
    {
        return CurrentTarget != null;
    }

    void FreeLookUpdate()
    {
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);
        Vector3 direction = rotation * new Vector3(0, 0, -Distance);

        Vector3 playerPosition = PlayerTransform.position + new Vector3(0, VerticalOffset, 0);
        
        // 检测镜头与玩家之间是否有遮挡
        float adjustedDistance = Distance;
        RaycastHit hit;
        if (Physics.Raycast(playerPosition, direction.normalized, out hit, Distance, ObstacleLayerMask))
        {
            // 如果有遮挡，将相机拉近到碰撞点
            adjustedDistance = hit.distance - 0.5f;
            adjustedDistance = Mathf.Max(adjustedDistance, MinDistance);
        }
        
        // 计算最终相机位置
        Vector3 adjustedDirection = rotation * new Vector3(0, 0, -adjustedDistance);
        Vector3 cameraPosition = playerPosition + adjustedDirection;
        
        transform.position = cameraPosition;
        transform.LookAt(PlayerTransform.position + new Vector3(0, LookAtOffset, 0));
    }

    void LockOnUpdate()
    {
        Vector3 playerPos = PlayerTransform.position + new Vector3(0, VerticalOffset, 0);
        Vector3 targetPos = CurrentTarget.position;

        // 1️⃣ 只计算水平向量，不考虑y
        Vector3 dir = targetPos - PlayerTransform.position;
        dir.y = 0;
        if (dir.sqrMagnitude < 0.001f) dir = PlayerTransform.forward;

        // 平滑旋转相机朝向敌人
        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * LockRotationSpeed);

        // 相机位置
        Vector3 direction = transform.rotation * new Vector3(0, 0, -Distance);

        float adjustedDistance = Distance;
        RaycastHit hit;
        if (Physics.Raycast(playerPos, direction.normalized, out hit, Distance, ObstacleLayerMask))
        {
            adjustedDistance = Mathf.Max(hit.distance - 0.5f, MinDistance);
        }

        Vector3 finalPos = playerPos + direction.normalized * adjustedDistance;
        transform.position = finalPos;
    }
}