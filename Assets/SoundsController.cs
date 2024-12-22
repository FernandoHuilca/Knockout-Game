using UnityEngine;

public class SoundsController : MonoBehaviour
{
    public static SoundsController Instance;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Obtener el componente AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No se encontró un componente AudioSource en el objeto SoundsController.");
        }
    }

    public void RunSound(AudioClip sonido)
    {
        if (audioSource != null && sonido != null)
        {
            audioSource.PlayOneShot(sonido);
        }
        else if (sonido == null)
        {
            Debug.LogWarning("El AudioClip pasado a RunSound es nulo.");
        }
    }
}
