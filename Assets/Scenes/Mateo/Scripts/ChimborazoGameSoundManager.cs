using UnityEngine;

public class ChimborazoGameSoundManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip backgroundSound;
    [SerializeField] private AudioClip fightSound;

    private void Start()
    {
        SoundsController.Instance.RunSound(fightSound);
        SoundsController.Instance.RunSound(backgroundSound);
    }
}

