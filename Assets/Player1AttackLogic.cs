using System;
using UnityEngine;

public class Player1AttackLogic : MonoBehaviour
{
    public Animator animator;

    public Transform weaponHitBox;
    public float attackRange = 0.5f;
    public LayerMask otherPlayer;

    private float hitDamage = 10f;
    private float kickDamage = 2.5f;
    private float specialPowerDamage = 30f;

    public float attackRate = 1f; // Número de veces que atacaremos por segundo
    float nexAttackTime = 0f;

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nexAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                hit();
                //Cada 1.5 segundos podrá hacer 1 ataque
                nexAttackTime = Time.time + 1.5f / attackRate; 
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                kick();
                nexAttackTime = Time.time + 0.75f / attackRate;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                activeSpecialPower();

            }
        }
        
    }

    private void activeSpecialPower()
    {
        Debug.Log("Special Power");
    }

    private void kick()
    {
        animator.SetTrigger("Attack");

        Collider2D[] hitOtherPlayers = Physics2D.OverlapCircleAll(weaponHitBox.position, attackRange, otherPlayer);

        foreach (Collider2D playerEnemy in hitOtherPlayers)
        {
            playerEnemy.GetComponent<Player2Health>().decreaselife(kickDamage);
            //Debug.Log("We hit "+ playerEnemy.name);
        }
    }

    void hit()
    {
        animator.SetTrigger("Attack");

        Collider2D[] hitOtherPlayers = Physics2D.OverlapCircleAll(weaponHitBox.position, attackRange, otherPlayer);

        foreach (Collider2D playerEnemy in hitOtherPlayers)
        {
            playerEnemy.GetComponent<Player2Health>().decreaselife(hitDamage);
            //Debug.Log("We hit "+ playerEnemy.name);
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (weaponHitBox == null)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(weaponHitBox.position, attackRange);
        //Gizmos.DrawWireSphere(weaponHitBox.position, attackRange);

    }
}
