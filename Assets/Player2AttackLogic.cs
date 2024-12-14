using UnityEngine;

public class Player2AttackLogic : MonoBehaviour
{
    public Animator animator;

    public Transform weaponHitBox;
    public float attackRange = 0.5f;
    public LayerMask otherPlayer;

    private float attackDamage = 10f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Attack();
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");

      

        Collider2D[] hitOtherPlayers = Physics2D.OverlapCircleAll(weaponHitBox.position, attackRange, otherPlayer);

        foreach (Collider2D playerEnemy in hitOtherPlayers)
        {
            //if (!playerEnemy.GetComponent<Player1ShieldLogic>().isTheShieldActive())
            //{
                playerEnemy.GetComponent<Player1Health>().decreaselife(attackDamage);
                //Debug.Log("We hit "+ playerEnemy.name);
                
            //}
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
