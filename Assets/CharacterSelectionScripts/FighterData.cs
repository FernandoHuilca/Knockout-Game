using UnityEngine;

public class FighterData : ScriptableObject
{
    private float speed;
    private float jumpForce;
    private float maxHealth;
    
    private float attackRange; //CUIDADOOO CON ESTE VALOR
    
    private float hitDamage;            // Daño del ataque normal
    private float kickDamage;          // Daño del ataque de patada
    private float specialPowerDamage;   // Daño del poder especial

    private float hitDamageToShield; // Daño al escudo con golpe
    private float kickDamageToShield; // Daño al escudo con patada

    public float waitingTimeHit; // Tiempo de espera entre golpes
    public float waitingTimeKick; // Tiempo de espera entre patadas

    public float maxCharge; // CAMBIAR POR NUMERO DE GOLPES ACERTADOS

    public float shieldDuration = 5f; // Tiempo de recarga si el escudo se desactiva.
    private float shieldCapacity = 100f; // Capacidad total del escudo INICIAL.
    private const float maxShieldCapacity = 100f; // Capacidad máxima del escudo.
    private const float rechargeRate = 10f; // Cantidad de recarga por segundo.
}
