using UnityEngine;

public class MenuMusicManager : MonoBehaviour
{
    public static MenuMusicManager Instance { get; private set; }

    [Header("Clips de M�sica")]
    [SerializeField] private AudioClip menuMusic; // M�sica compartida entre las tres escenas

    [Header("Componentes")]
    [SerializeField] private AudioSource audioSource; // AudioSource que reproducir� la m�sica

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

        // Configurar el AudioSource si no est� asignado
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true; // Hacer que la m�sica se repita
        }

        // Asignar el clip y reproducir la m�sica
        audioSource.clip = menuMusic;
        audioSource.Play();
    }

    /// <summary>
    /// Reanuda la m�sica si est� detenida.
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
        // Detener la m�sica cuando se cargue una escena de pelea
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
