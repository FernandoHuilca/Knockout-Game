using UnityEngine;

public class VictorCalderoniAttack : Attack
{
    [Header("Weapon Settings")]
    [SerializeField] private GameObject gun;

    void Start()
    {
        base.Start();
        gun = transform.Find("Gun").gameObject;
    }

    protected override void performAttack1()
    {
        getAnimator().SetTrigger("attack1"); // Activa la animación de ataque
        ApplyMeleeDamage(attack1Damage, attack1DamageToShield);
    }

    protected override void performAttack2()
    {
        // Activa la animación de ataque
        getAnimator().SetTrigger("attack2");
        gun.GetComponent<Gun2D>().shoot(attack2Damage, attack2DamageToShield, gameObject.tag);
        //base.base.applyDamageToEnemies(damage, damageToShield, weaponHitBox, attackRange); // Aplica daño a los enemigos detectados
        //applyDamageToEnemies(attack2Damage, attack2DamageToShield);
    }

}
