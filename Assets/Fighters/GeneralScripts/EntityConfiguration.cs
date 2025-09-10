using UnityEngine;

public class EntityConfiguration : MonoBehaviour
{
    [SerializeField] private bool facingRight;

    public void setFacingRight(bool facingRight)
    {
        this.facingRight = facingRight;
    }

    public bool getFacingRight()
    {
        return facingRight;
    }
}
