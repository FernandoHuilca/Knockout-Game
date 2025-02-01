using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    public Image imageComponent; // El componente Image del GameObject.
    public Color[] colors; // Lista de colores a alternar.
    public float changeInterval = 1f; // Intervalo de tiempo entre cambios.

    private int currentColorIndex = 0; // Índice del color actual.
    private float timer = 0f; // Temporizador para rastrear el tiempo.

    void Start()
    {
        if (imageComponent == null)
        {
            imageComponent = GetComponent<Image>(); // Busca el componente Image en el GameObject.
            if (imageComponent == null)
            {
                Debug.LogError("No se encontró un componente Image en este GameObject.");
                enabled = false;
                return;
            }
        }

        if (colors.Length > 0)
        {
            imageComponent.color = colors[0]; // Establece el color inicial.
        }
        else
        {
            Debug.LogWarning("No se han asignado colores en la lista.");
        }
    }

    void Update()
    {
        if (colors.Length == 0) return; // No hace nada si no hay colores.

        timer += Time.deltaTime; // Incrementa el temporizador.
        if (timer >= changeInterval)
        {
            timer -= changeInterval; // Reinicia el temporizador.
            ChangeToNextColor(); // Cambia al siguiente color.
        }
    }

    void ChangeToNextColor()
    {
        currentColorIndex = (currentColorIndex + 1) % colors.Length; // Cicla entre los colores.
        imageComponent.color = colors[currentColorIndex]; // Cambia el color del Image.
    }
}
