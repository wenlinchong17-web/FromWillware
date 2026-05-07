using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponData weaponData;

    private Collider attackCollider;
    // Start is called before the first frame update
    void Start()
    {
        var child = FindChildWithTag(this.transform, "PlayerAttack");
        child.GetComponent<Collider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    Transform FindChildWithTag(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                return child;
            }

            // 递归查找子物体的子物体
            Transform result = FindChildWithTag(child, tag);
            if (result != null)
                return result;
        }
        return null;
    }
}
