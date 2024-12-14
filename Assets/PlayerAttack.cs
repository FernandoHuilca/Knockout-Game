using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    
    public Transform weaponHitBox;
    public float attackRange = 0.5f;
    public LayerMask otherPlayer;

    private float attackDamage = 10f;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && gameObject.layer.Equals(6)){
            Attack();
        }else if(Input.GetKeyDown(KeyCode.F) && gameObject.layer.Equals(3))
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
            playerEnemy.GetComponent<PlayerHealth>().decreaselife(attackDamage);
            //Debug.Log("We hit "+ playerEnemy.name);
        }

    }

    private void OnDrawGizmosSelected()
    {
        if(weaponHitBox == null)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(weaponHitBox.position, attackRange);
        //Gizmos.DrawWireSphere(weaponHitBox.position, attackRange);
        
    }
}
