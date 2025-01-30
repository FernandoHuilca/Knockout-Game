using UnityEngine;

public class SoundsController : MonoBehaviour
{
    public static SoundsController Instance;
    private AudioSource audioSource;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip backgroundSound;  // Esto es para asignar la m�sica de fondo
    private bool isPaused = false;  // Variable para manejar el estado de la pausa

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
            Debug.LogError("No se encontr� un componente AudioSource en el objeto SoundsController.");
        }

        // Configurar la m�sica de fondo para que se repita en un loop.
        audioSource.loop = true;  // Esto hace que la m�sica de fondo se repita
    }

    // M�todo para iniciar un sonido (como efectos)
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

    // Pausar todos los sonidos
    public void pauseSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Pause();
            isPaused = true;  // Guardamos el estado de que est� pausado
        }
        else
        {
            Debug.LogWarning("No hay un audio reproduci�ndose para pausar.");
        }
    }

    // Reanudar los sonidos
    public void reactiveSound()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            if (isPaused)
            {
                audioSource.UnPause();  // Reanudar desde el punto donde se paus�
            }
            else
            {
                audioSource.Play();  // En caso de que no est� en pausa, empieza a reproducir desde el principio
            }
        }
        else
        {
            Debug.LogWarning("El audio ya est� reproduci�ndose o no est� configurado.");
        }
    }

    // Esto deber�a ser llamado desde tu GameSceneManager cuando se inicia la escena para evitar reiniciar el audio
    public void StartBackgroundMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.clip = backgroundSound;
            audioSource.Play();
        }
    }
}
