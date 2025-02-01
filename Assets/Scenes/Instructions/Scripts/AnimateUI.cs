using UnityEngine;
using UnityEngine.UI;

public class AnimateUI : MonoBehaviour
{
    public Sprite[] frames; // Asigna aquí tus sprites en el Inspector.
    public float frameRate = 0.1f; // Tiempo entre cuadros.
    private Image imageComponent;
    private int currentFrame;
    private float timer;

    void Start()
    {
        // Obtén el componente Image
        imageComponent = GetComponent<Image>();

        if (frames.Length > 0)
        {
            imageComponent.sprite = frames[0];
        }
    }

    void Update()
    {
        if (frames.Length == 0) return;

        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer -= frameRate;
            currentFrame = (currentFrame + 1) % frames.Length; // Ciclo de frames.
            imageComponent.sprite = frames[currentFrame];
        }
    }
}
