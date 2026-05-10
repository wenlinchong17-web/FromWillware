using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GetHit : MonoBehaviour
{
    private Player player;
    private PlayerMove playerMove;
    private PlayerParry playerParry;
    private Animator animator;
    private PlayerAttack playerAttack;
    private PlayerState playerState;
    
    public AudioSource audioSource;
    public AudioClip ParrySound;
    public AudioClip HitSound;
    public bool IsGetHit = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        playerMove = GetComponent<PlayerMove>();
        playerParry = GetComponent<PlayerParry>();
        animator = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
        playerState = GetComponent<PlayerState>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        if(player.IsDead || playerMove.IsRolling)
            return;

        player.CurrentHP -= damage;

        if (player.CurrentHP <= 0)
        {
            audioSource.PlayOneShot(HitSound);
            player.Die();
            return;
        }

        if (playerState.CanGetHit)
        {
            StartCoroutine(GetHitCoroutine());
        }

        audioSource.PlayOneShot(HitSound);
    }
    
    private Coroutine hitCoroutine;

    IEnumerator GetHitCoroutine()
    {
        if (hitCoroutine != null)
            StopCoroutine(hitCoroutine);

        IsGetHit = true;

        animator.SetTrigger("GetHit");

        // 等待受击动画时间
        yield return new WaitForSeconds(0.5f);

        IsGetHit = false;

        hitCoroutine = null;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAttack"))
        {
            //int damage;
            if (playerParry.IsParrying)
            {
                Debug.Log("Parry Success");
                audioSource.PlayOneShot(ParrySound);
                return;
            }
            Damage ennemyDamage = other.GetComponent<Damage>();
            if (ennemyDamage != null)
                TakeDamage(ennemyDamage.damage);
        }
        else
        {
            Debug.Log("No Tag EnemyAttack");
            return;
        }
    }
    
    public void SetIsGetHit()
    {
        IsGetHit = true;
    }
    public void ResetIsGetHit()
    {
        IsGetHit = false;
    }
}
