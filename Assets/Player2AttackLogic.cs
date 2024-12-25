using UnityEngine;

public class Player2AttackLogic : MonoBehaviour
{
    public Animator animator; // TODO: Asignar el componente Animator de cada luchador

    public Transform weaponHitBox; // TODO: Asignar un objeto hijo cerca de las manos del luchador
    public float attackRange = 0.5f; // TODO: Asignar el rango de ataque de cada luchador
    public LayerMask otherPlayer; 

    private float attackDamage = 10f; // TODO: Asignar el daño de ataque de cada luchador

    // Atritubos que depende si es player 1 o player 2
    public KeyCode attackKey = KeyCode.F; // TODO: Toca ver cómo asignar teclas a cada jugador, en base a si es player 1 o player 2
    public KeyCode specialAttackKey = KeyCode.G;

    // Atributos para sonidos
    [SerializeField] private AudioClip soundAttack1;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(attackKey)) // Ataque normal.
        {
            hit();
            SoundsController.Instance.RunSound(soundAttack1);
        }

        if (Input.GetKeyDown(specialAttackKey)) // Ataque especial.
        {
            FindObjectOfType<SpecialAttack>().useSpecialAttack();
        }
    }


    void hit()
    {
        animator.SetTrigger("Attack");

        Collider2D[] hitOtherPlayers = Physics2D.OverlapCircleAll(weaponHitBox.position, attackRange, otherPlayer);

        foreach (Collider2D playerEnemy in hitOtherPlayers)
        {
            if(playerEnemy.GetComponent<FighterHealth>() != null)
            {
                if (!playerEnemy.GetComponent<Player1ShieldLogic>().IsShieldActive())
                {
                    playerEnemy.GetComponent<FighterHealth>().decreaselife(attackDamage);
                    Debug.Log("We hit "+ playerEnemy.name);
                    // Cargar barra de ataque especial con cada golpe acertado.
                    FindObjectOfType<SpecialAttack>().increaseCharge(10f);  
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
