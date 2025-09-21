using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    [SerializeField] string videoFileName;

    void Start()
    {
        playVideo();
    }

    public void playVideo()
    {
        UnityEngine.Video.VideoPlayer videoPlayer = GetComponent<UnityEngine.Video.VideoPlayer>();
        if (videoPlayer)
        {
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);

            // LOGS DE DEBUG PARA IDENTIFICAR EL PROBLEMA
            Debug.Log("StreamingAssets path: " + Application.streamingAssetsPath);
            Debug.Log("Video file name: " + videoFileName);
            Debug.Log("Full video path: " + videoPath);

            // Verificar si el archivo existe
            if (System.IO.File.Exists(videoPath))
            {
                Debug.Log("✓ Video file found!");
            }
            else
            {
                Debug.LogError("✗ Video file NOT found at: " + videoPath);
                Debug.LogError("Check if the file exists in StreamingAssets folder");
                return;
            }

            // Configurar el video player
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = videoPath;
            videoPlayer.playOnAwake = false;
            videoPlayer.waitForFirstFrame = true;
            videoPlayer.isLooping = true;

            // Verificar si tiene RenderTexture asignada
            if (videoPlayer.targetTexture == null)
            {
                Debug.LogWarning("⚠ No RenderTexture assigned to VideoPlayer! You won't see the video.");
                Debug.LogWarning("Create a RenderTexture and assign it to the VideoPlayer component.");
            }
            else
            {
                Debug.Log("✓ RenderTexture is assigned");
            }

            // Reproducir el video
            videoPlayer.Play();
            Debug.Log("Video playback started");
        }
        else
        {
            Debug.LogError("No VideoPlayer component found!");
        }
    }
}