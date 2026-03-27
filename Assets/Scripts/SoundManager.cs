using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Sounds")]
    public AudioClip clickSound;
    public AudioClip hitSound;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;
        audioSource.volume = 1f;
    }

    public void PlayClick()
    {
        if (clickSound == null)
        {
            Debug.LogWarning("SoundManager: clickSound is not assigned.");
            return;
        }

        audioSource.PlayOneShot(clickSound);
    }

    public void PlayHit()
    {
        if (hitSound == null)
        {
            Debug.LogWarning("SoundManager: hitSound is not assigned.");
            return;
        }

        audioSource.PlayOneShot(hitSound);
    }
}