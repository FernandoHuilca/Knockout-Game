using UnityEngine;

public class MenuMusicManager : MonoBehaviour
{
    public static MenuMusicManager Instance { get; private set; }

    [Header("Clips de Música")]
    [SerializeField] private AudioClip menuMusic; // Música compartida entre las tres escenas

    [Header("Componentes")]
    [SerializeField] private AudioSource audioSource; // AudioSource que reproducirá la música

    private void Awake()
    {
        // Verifica si ya existe una instancia
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destruir duplicados
            return;
        }

        // Hacer persistente entre escenas
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Configurar el AudioSource si no está asignado
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true; // Hacer que la música se repita
        }

        // Asignar el clip y reproducir la música
        audioSource.clip = menuMusic;
        audioSource.Play();
    }

    /// <summary>
    /// Reanuda la música si está detenida.
    /// </summary>
    public void ResumeMusic()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        // Detener la música cuando se cargue una escena de pelea
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
