using UnityEngine;

public interface Shieldable
{
    public bool IsShieldActive();
    public void decreaseShieldCapacity(float damageToShield);
}
