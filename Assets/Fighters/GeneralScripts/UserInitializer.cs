using UnityEngine;

public class UserInitializer : MonoBehaviour
{
    [SerializeField] private int userIndex; // Índice del usuario (1 o 2)
    [SerializeField] private Transform spawnPosition; // Posición de aparición del luchador
    [SerializeField] private string uiPrefix; // Prefijo de UI ("Fighter1UI" o "Fighter2UI")
    [SerializeField] private KeyCode[] movementKeys; 
    [SerializeField] private KeyCode[] attackKeys;
    [SerializeField] private KeyCode shieldKey;
    [SerializeField] private  UIInitializer UIInitializer;

    private void Start()
    {
        int fighterIndex = PlayerPrefs.GetInt($"User{userIndex}Index"); // Se obtiene el índice del luchador seleccionado por el usuario
        FightersData fighterData = GameManager.gameManagerInstance.fightersData[fighterIndex]; // Se obtienen los datos del luchador seleccionado
        GameObject fighter = fighterData.getFighterPrefab(); // Se obtiene el prefab del luchador seleccionado

        // Se configuran los componentes del luchador
        configureMovement(fighter, fighterData);
        configureAttack(fighter, fighterData);
        configureAttributes(fighter, fighterData);

        // Se configura la UI del luchador
        UIController UIController = fighter.GetComponent<UIController>();
        UIInitializer = GetComponent<UIInitializer>();
        UIInitializer.configureUI(UIController, userIndex, uiPrefix);

        // Se instancia el luchador
        Instantiate(fighter, spawnPosition.position, Quaternion.identity);
    }

    private void configureMovement(GameObject fighter, FightersData fighterData)
    {
        FighterMovement movement = fighter.GetComponent<FighterMovement>();
        movement.setFacingRight(userIndex == 1);
        movement.setAxis(userIndex == 1 ? "Horizontal2" : "Horizontal"); // Se asigna el eje horizontal de movimiento
        movement.setUpKey(movementKeys[0]);
        movement.setDownKey(movementKeys[1]);
        movement.setSpeed(fighterData.getSpeed());
        movement.setJumpForce(fighterData.getJumpForce());
        movement.setGroundCheckRadius(fighterData.getGroundCheckRadius());
    }

    private void configureAttack(GameObject fighter, FightersData fighterData)
    {
        FighterAttack attack = fighter.GetComponent<FighterAttack>();
        attack.setHitKey(attackKeys[0]);
        attack.setKickKey(attackKeys[1]);
        attack.setSpecialPowerKey(attackKeys[2]);

        attack.setAttackRate(fighterData.getAttackRate());
        attack.setAttackRange(fighterData.getAttackRange());

        attack.setHitDamage(fighterData.getHitDamage());
        attack.setKickDamage(fighterData.getKickDamage());
        attack.setSpecialPowerDamage(fighterData.getSpecialPowerDamage());

        attack.setHitDamageToShield(fighterData.getHitDamageToShield());
        attack.setKickDamageToShield(fighterData.getKickDamageToShield());

        attack.setWaitingTimeHit(fighterData.getWaitingTimeHit());
        attack.setWaitingTimeKick(fighterData.getWaitingTimeKick());

        fighter.GetComponent<FighterShield>().setShieldKey(shieldKey);
        fighter.GetComponent<FighterShield>().setShieldDuration(fighterData.getShieldDuration());
        fighter.GetComponent<FighterShield>().setMaxShieldCapacity(fighterData.getMaxShieldCapacity());
        fighter.GetComponent<FighterShield>().setRechargeRate(fighterData.getRechargeRate());
    }

    
    private void configureAttributes(GameObject fighter, FightersData fighterData)
    {
        FighterHealth health = fighter.GetComponent<FighterHealth>();
        health.setMaxHealth(fighterData.getMaxHealth());

        SpecialAttack special = fighter.GetComponent<SpecialAttack>();
        special.setMaxCharge(fighterData.getMaxCharge());
    }
}
