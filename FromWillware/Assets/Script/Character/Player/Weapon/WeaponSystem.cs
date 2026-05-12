using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponSystem : MonoBehaviour
{
    public Transform WeaponPoint;
    public List<Transform> Weapons;
    public int CurrentWeaponIndex = 0;
    public Transform CurrentWeapon;
    public int MaxSzie = 2;
    
    public Animator animator;
    public AnimatorOverrideController baseOverride;

    private AnimatorOverrideController runtimeOverride;
    private Player player;
    private WeaponBackPack weaponBackPack;
    private PlayerInputHandler inputHandler;
    
    // Start is called before the first frame update
    void Start()
    {
        Weapons = new List<Transform>(2);
        animator = GetComponent<Animator>();
        weaponBackPack = GetComponent<WeaponBackPack>();
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputHandler.switchWeapon)
        {
            ChangeWeapon();
        }
    }

    public void EquipWeapon(int index)
    {
        if (Weapons.Count == 0) return;

        // 关闭所有武器
        foreach (var w in Weapons)
            w.gameObject.SetActive(false);

        CurrentWeaponIndex = index;
        CurrentWeapon = Weapons[index];

        CurrentWeapon.gameObject.SetActive(true);
    }

    public void ChangeWeapon()
    {
        if (Weapons.Count == 0) return;

        Weapons[CurrentWeaponIndex].gameObject.SetActive(false);

        CurrentWeaponIndex = (CurrentWeaponIndex + 1) % Weapons.Count;

        EquipWeapon(CurrentWeaponIndex);
        ApplyWeaponAnimation(CurrentWeapon.GetComponent<WeaponPickup>().weaponData);
    }
    public void AddWeapon(WeaponData data)
    {
        if (Weapons.Count >= MaxSzie)
        {
            Debug.Log("装备的武器数已满");
            return;
        }
        GameObject weapon = Instantiate(data.WeaponPrefab, WeaponPoint, false);

        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        weapon.transform.localScale = Vector3.one;

        weapon.gameObject.SetActive(false);
        weapon.GetComponentInChildren<Collider>().enabled = false;
        
        if(Weapons.Count<MaxSzie)
            Weapons.Add(weapon.transform);

        if (CurrentWeapon == null)
        {
            EquipWeapon(0);
            ApplyWeaponAnimation(Weapons[0]
                .GetComponent<WeaponPickup>().weaponData);
        }
    }

    public void SetToWeaponPoint(int backIndex, int pointIndex)
    {
        WeaponData data = weaponBackPack.Weapons[backIndex];

        while (Weapons.Count <= pointIndex)
        {
            Weapons.Add(null);
        }
        // 删除原来的武器
        if (Weapons[pointIndex] != null)
        {
            Destroy(Weapons[pointIndex].gameObject);
        }

        // 实例化新武器
        GameObject weaponObj =
            Instantiate(data.WeaponPrefab, WeaponPoint, false);

        weaponObj.transform.localPosition = Vector3.zero;
        weaponObj.transform.localRotation = Quaternion.identity;
        weaponObj.transform.localScale = Vector3.one;

        weaponObj.GetComponentInChildren<Collider>().enabled = false;

        // 替换列表中的武器
        Weapons[pointIndex] = weaponObj.transform;

        // 激活状态处理
        if (pointIndex == CurrentWeaponIndex)
        {
            weaponObj.SetActive(true);

            CurrentWeapon = weaponObj.transform;

            ApplyWeaponAnimation(data);
        }
        else
        {
            weaponObj.SetActive(false);
        }
    }
    
    void ApplyWeaponAnimation(WeaponData weapon)
    {
        runtimeOverride = new AnimatorOverrideController(baseOverride);

        runtimeOverride["SwordAttack1"] = weapon.combo1;
        runtimeOverride["SwordAttack2"] = weapon.combo2;
        runtimeOverride["SwordAttack3"] = weapon.combo3;
        runtimeOverride["Sword And Shield Idle"] = weapon.idle;
        runtimeOverride["Running"] = weapon.run;
        runtimeOverride["Walking"] = weapon.walk;
        
        animator.runtimeAnimatorController = runtimeOverride;
    }
}
