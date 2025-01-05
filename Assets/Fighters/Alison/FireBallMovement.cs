using System;
using UnityEngine;

public class FireBallMovement : MonoBehaviour
{
    public Vector3 startPosition;  // Posición inicial
    public Vector3 endPosition;    // Posición final
    public float speed = 1.0f;     // Velocidad del movimiento
    [SerializeField] private UserConfiguration userConfiguration;

    private float progress = 0.0f; // Progreso del movimiento

    internal void setUserConfiguration(UserConfiguration userConfiguration)
    {
        this.userConfiguration = userConfiguration;
    }

    void Start()
    {
        startPosition = transform.position;

        float xEndValue = userConfiguration.getFacingRight() ? 10 : -10;

        endPosition = new Vector3(xEndValue, transform.position.y, transform.position.z);
    }

    void Update()
    {
        // Incrementar el progreso del movimiento basado en el tiempo y la velocidad
        progress += Time.deltaTime * speed;

        // Interpolar la posición del objeto entre startPosition y endPosition
        transform.position = Vector3.Lerp(startPosition, endPosition, progress);

        // Detener el movimiento cuando el objeto llega a la posición final
        if (progress >= 1.0f)
        {
            progress = 1.0f;

            Destroy(gameObject);
        }
    }
}
