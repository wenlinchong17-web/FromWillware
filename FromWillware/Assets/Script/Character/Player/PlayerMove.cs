using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : Character
{
    public Transform CameraTransform;
    
    [SerializeField]
    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 forward = CameraTransform.forward;
        Vector3 right = CameraTransform.right;

        // 去掉上下分量（防止飞起来）
        forward.y = 0;
        right.y = 0;

        Vector3 move = forward * v + right * h;
        move = move.normalized;

        // 移动
        rb.velocity = new Vector3(move.x * MoveSpeed, rb.velocity.y, move.z * MoveSpeed);

        // 角色朝向移动方向
        if (move != Vector3.zero)
        {
            transform.forward = move;
        }
    }
}
