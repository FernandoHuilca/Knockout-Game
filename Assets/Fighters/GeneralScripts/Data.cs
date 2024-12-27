using UnityEngine;

[CreateAssetMenu(fileName = "New Fighter", menuName = "Fighter")]

public class FightersData : ScriptableObject
{
    // 1. DECLARACIÓN DE ATRIBUTOS

    // Atributos generales del luchador
    [Header("Datos generales del luchador")]
    [SerializeField] private Sprite fighterImage;
    [SerializeField] private string fighterName;
    [SerializeField] private string fighterDescription;
    [SerializeField] private GameObject fighterPrefab ; // Prefab del luchador


    // Atributos de Script Fighter Movement
    [Header("Script Fighter Movement")]
    //[SerializeField] private float speed; // Velocidad de movimiento
    //[SerializeField] private float jumpForce; // Fuerza de salto
    [SerializeField] private float groundCheckRadius = 0.1f; // Radio de comprobación de suelo (Tiene un valor por defecto)
    // Atributos que dependen si es player 1 o player 2: axis, jumpKey y downKey.


    // Atributos de Script Fighter Health
    [Header("Script Fighter Health")]
    [SerializeField] private float maxHealth = 100f;
    // Atributos que dependen si es player 1 o player 2: healthBarFill y lives.


    // Atributos de Script Fighter Attack
    [Header("Script Fighter Attack")]
    [SerializeField] private float attackRange = 0.5f; // Rango de ataque (Tiene un valor por defecto)
    
    //[SerializeField] private float hitDamage; 
    //[SerializeField] private float kickDamage;
    //[SerializeField] private float specialPowerDamage;
    
    //[SerializeField] private float hitDamageToShield; // Daño al escudo con golpe
    //[SerializeField] private float kickDamageToShield; // Daño al escudo con patada
    
    [SerializeField] private float attackRate = 1f; // Tasa de ataque: número de ataques por segundo permitidos (Tiene un valor por defecto)
    //[SerializeField] private float waitingTimeHit; // Tiempo de espera entre golpes
    //[SerializeField] private float waitingTimeKick; // Tiempo de espera entre patadas
    // Atributos que dependen si es player 1 o player 2: hitKey, kickKey y specialPowerKey.


    // Atributos de Script Special Attack
    [Header("Script Special Attack")]
    [SerializeField] private float maxCharge = 100f;
    // Atributos que dependen si es player 1 o player 2: specialBarFill.


    // Atributos de Script Fighter Shield
    [Header("Script Fighter Shield")]
    [SerializeField] private float shieldDuration = 5f; // Tiempo de recarga si el escudo se desactiva.
    [SerializeField] private float maxShieldCapacity = 100f;
    [SerializeField] private float rechargeRate = 10f; // Cantidad de recarga por segundo.
    // Atributos que dependen si es player 1 o player 2: shieldKey.


    // ------------------------------------------------------------------------------------------------------------------------------------------
    // 2. MÉTODOS

    public Sprite getFighterImage() { return fighterImage; }
    public string getFighterName() { return fighterName; }
    public string getFighterDescription() { return fighterDescription; }
    public GameObject getFighterPrefab() { return fighterPrefab; }
    //public float getSpeed() { return speed; }
    //public float getJumpForce() { return jumpForce; }
    public float getGroundCheckRadius() { return groundCheckRadius; }
    public float getMaxHealth() { return maxHealth; }
    public float getAttackRange() { return attackRange; }
    //public float getHitDamage() { return hitDamage; }
    //public float getKickDamage() { return kickDamage; }
    //public float getSpecialPowerDamage() { return specialPowerDamage; }
    //public float getHitDamageToShield() { return hitDamageToShield; }
    //public float getKickDamageToShield() { return kickDamageToShield; }
    public float getAttackRate() { return attackRate; }
    //public float getWaitingTimeHit() { return waitingTimeHit; }
    //public float getWaitingTimeKick() { return waitingTimeKick; }
    public float getMaxCharge() { return maxCharge; }
    public float getShieldDuration() { return shieldDuration; }
    public float getMaxShieldCapacity() { return maxShieldCapacity; }
    public float getRechargeRate() { return rechargeRate; }
}
