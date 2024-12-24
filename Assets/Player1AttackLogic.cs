using System;
using UnityEngine;

public class Player1AttackLogic : MonoBehaviour
{
    // Referencia al Animator para reproducir animaciones de ataque
    public Animator animator;

    // Posición donde se verificará el impacto de las armas
    public Transform weaponHitBox;

    // Rango en el que se pueden detectar jugadores enemigos
    public float attackRange = 0.5f;

    // Capa que representa al otro jugador (enemigo)
    public LayerMask otherPlayer;

    // Valores de daño para diferentes ataques
    private float hitDamage = 10f;            // Daño del ataque normal
    private float kickDamage = 2.5f;          // Daño del ataque de patada
    private float specialPowerDamage = 30f;   // Daño del poder especial

    // Tasa de ataque: número de ataques por segundo permitidos
    public float attackRate = 1f;
    float nexAttackTime = 0f; // Tiempo de espera para el próximo ataque

    // Update se llama una vez por cuadro
    void Update()
    {
        // Solo permite ataques si ha pasado suficiente tiempo desde el último ataque
        if (Time.time >= nexAttackTime)
        {
            // Si se presiona la tecla "1", realiza un golpe
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                hit();
                nexAttackTime = Time.time + 1.5f / attackRate; // 1.5 segundos de enfriamiento
            }
            // Si se presiona la tecla "2", realiza una patada
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                kick();
                nexAttackTime = Time.time + 0.75f / attackRate; // Menor enfriamiento
            }
            // Si se presiona la tecla "3", activa el poder especial
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                activeSpecialPower();
            }
        }
    }

    // Método que activa el poder especial
    private void activeSpecialPower()
    {
        Debug.Log("Special Power"); // Muestra un mensaje en la consola
    }

    // Método para realizar la patada
    private void kick()
    {
        // Activa la animación de ataque
        animator.SetTrigger("Attack");

        // Detecta todos los enemigos en el rango de ataque
        Collider2D[] hitOtherPlayers = Physics2D.OverlapCircleAll(weaponHitBox.position, attackRange, otherPlayer);

        // Aplica daño a cada enemigo detectado
        foreach (Collider2D playerEnemy in hitOtherPlayers)
        {
            if (playerEnemy.GetComponent<Player2Health>() != null)
            {
                playerEnemy.GetComponent<Player2Health>().decreaselife(kickDamage);
            }
        }
    }

    // Método para realizar el golpe
    void hit()
    {
        animator.SetTrigger("Attack"); // Activa la animación de ataque

        // Detecta jugadores enemigos dentro del área del "weaponHitBox"
        Collider2D[] hitOtherPlayers = Physics2D.OverlapCircleAll(weaponHitBox.position, attackRange, otherPlayer);

        // Aplica daño a cada enemigo detectado
        foreach (Collider2D playerEnemy in hitOtherPlayers)
        {
            if (playerEnemy.GetComponent<Player2Health>() != null)
            {
                playerEnemy.GetComponent<Player2Health>().decreaselife(hitDamage);
            }
        }
    }

    // Dibuja un Gizmo para visualizar el área de ataque en la escena
    private void OnDrawGizmosSelected()
    {
        if (weaponHitBox == null)
        {
            return;
        }
        Gizmos.color = Color.red; // Color del Gizmo
        Gizmos.DrawWireSphere(weaponHitBox.position, attackRange); // Área circular del rango de ataque
    }
}
