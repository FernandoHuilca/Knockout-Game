using UnityEngine;

public class Player2AttackLogic : MonoBehaviour
{
    public Animator animator;

    public Transform weaponHitBox;
    public float attackRange = 0.5f;
    public LayerMask otherPlayer;

    private float attackDamage = 10f;
    [SerializeField] private AudioClip soundAttack1;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Ataque normal.
        {
            Attack();
            SoundsController.Instance.RunSound(soundAttack1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) // Ataque especial.
        {
            //FindObjectOfType<SpecialAttack>().UseSpecialAttack();
        }
    }


    void Attack()
    {
        animator.SetTrigger("Attack");

      

        Collider2D[] hitOtherPlayers = Physics2D.OverlapCircleAll(weaponHitBox.position, attackRange, otherPlayer);

        foreach (Collider2D playerEnemy in hitOtherPlayers)
        {
            if(playerEnemy.GetComponent<Player1Health>() != null)
            {
                if (!playerEnemy.GetComponent<Player1ShieldLogic>().IsShieldActive())
                {
                    playerEnemy.GetComponent<Player1Health>().decreaselife(attackDamage);
                    Debug.Log("We hit "+ playerEnemy.name);
                    // Cargar barra de ataque especial con cada golpe acertado.
                    //FindObjectOfType<SpecialAttack>().IncreaseCharge(10f);  
                }
                else
                {
                    if(playerEnemy.GetComponent<Player1ShieldLogic>().getMaxHits() > 3)
                    {

                    }
                }

            }
           
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
