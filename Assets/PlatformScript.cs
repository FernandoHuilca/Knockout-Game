using System.Collections;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    private GameObject currentOneWayPlatform;

    public KeyCode downKey;
    
    private int layer; 
    private CapsuleCollider2D playerCollider;

    void OnValidate()
    {
        playerCollider = GetComponent<CapsuleCollider2D>();
        layer = LayerMask.NameToLayer("BaseFighter");
    }

    private void Update()
    {
        if (Input.GetKeyDown(downKey) && gameObject.layer==layer)
        {
            if (currentOneWayPlatform != null) {
                StartCoroutine(DisableCollision());
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();
        
        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}
