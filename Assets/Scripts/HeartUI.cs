using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    [Header("Heart Sprites")]
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    [Header("Current Life (Debug)")]
    [Range(0f, 1f)]
    public float currentLife = 1f;

    private Image heartImage;

    void Awake()
    {
        // Holt automatisch das Image vom selben Objekt
        heartImage = GetComponent<Image>();

        if (heartImage == null)
        {
            Debug.LogError(gameObject.name + " has no Image component!");
        }
    }

    void Start()
    {
        // Initialer Zustand
        UpdateHeart(currentLife);
    }

    public void UpdateHeart(float life)
    {
        currentLife = Mathf.Clamp(life, 0f, 1f);

        Debug.Log(gameObject.name + " UpdateHeart: " + currentLife);

        if (heartImage == null) return;

        if (currentLife >= 1f)
        {
            heartImage.sprite = fullHeart;
        }
        else if (currentLife >= 0.5f)
        {
            heartImage.sprite = halfHeart;
        }
        else
        {
            heartImage.sprite = emptyHeart;
        }
    }
}