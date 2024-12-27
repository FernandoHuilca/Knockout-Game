using UnityEngine;

public class UserConfiguration : MonoBehaviour
{
    //[SerializeField] private int userIndex; // Índice del usuario (1 o 2)
    //[SerializeField] private Transform spawnPosition; // Posición de aparición del luchador
    //[SerializeField] private string uiPrefix; // Prefijo de UI ("Fighter1UI" o "Fighter2UI")
    [SerializeField] private KeyCode[] movementKeys;
    [SerializeField] private KeyCode[] attackKeys;
    [SerializeField] private KeyCode shieldKey;
    [SerializeField] private bool facingRight;
    [SerializeField] private string axis;

    //[SerializeField] private UIInitializer UIInitializer;

    public void setMovementKeys(KeyCode[] movementKeys)
    {
        this.movementKeys = movementKeys;
    }

    public void setAttackKeys(KeyCode[] attackKeys){this.attackKeys = attackKeys;}

    public void setShieldKey(KeyCode shieldKey)
    {
        this.shieldKey = shieldKey;
    }

    public void setFacingRight(bool facingRight)
    {
        this.facingRight = facingRight;
    }

    public void setAxis(string axis)
    {
        this.axis = axis;
    }

    public KeyCode getJumpKey()
    {
        return movementKeys[0];
    }

    public KeyCode getDownKey()
    {
        return movementKeys[1];
    }

    public KeyCode getHitKey()
    {
        return attackKeys[0];
    }

    public KeyCode getKickKey()
    {
        return attackKeys[1];
    }

    public KeyCode getSpecialPowerKey()
    {
        return attackKeys[2];
    }

    public KeyCode getShieldKey()
    {
        return shieldKey;
    }

    public bool getFacingRight()
    {
        return facingRight;
    }

    public string getAxis()
    {
        return axis;
    }
}
