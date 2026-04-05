using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLockOn : MonoBehaviour
{
    public CameraFollow cameraFollow;
    public float RotateSpeed = 10f;
    public Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        cameraFollow = FindObjectOfType<CameraFollow>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraFollow.CurrentTarget == null) return;
        
        Vector3 dir = cameraFollow.CurrentTarget.position - transform.position;
        dir.y = 0;
        if (dir.sqrMagnitude < 0.01f) return;
        
        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, RotateSpeed * Time.deltaTime);
        
        float angleDiff = Quaternion.Angle(transform.rotation, rot);

        // 旋转角色
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, RotateSpeed * Time.deltaTime * 100f);

        // 设定动画参数
        if (angleDiff > 5f)  // 角度差大于 5 度才播放转身动画
        {
            float dot = Vector3.Cross(transform.forward, dir).y; 
            // dot>0 → 右转, dot<0 → 左转
            animator.SetFloat("TurnSpeed", dot);
        }
        else
        {
            animator.SetFloat("TurnSpeed", 0f); // 停止转身动画
        }
    }
}
