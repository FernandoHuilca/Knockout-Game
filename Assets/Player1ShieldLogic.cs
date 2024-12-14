using UnityEngine;

public class Player1ShieldLogic : MonoBehaviour
{
    public CircleCollider2D circleCollider2D;
    public SpriteRenderer spriteRenderer;
    public float shieldDuration = 5f;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            circleCollider2D.enabled = !circleCollider2D.enabled;
            spriteRenderer.enabled = !spriteRenderer.enabled;
        }

    }

    public bool isTheShieldActive()
    {
        if (circleCollider2D.enabled)
        {
            Debug.Log("The shield is active");
            return true;
        }
        else
        {
            return false;
            Debug.Log("The shield is not active");
        }
    }
}
