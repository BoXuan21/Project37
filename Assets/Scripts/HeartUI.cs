using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    public Image heartImage;

    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    public float currentLife = 1f;

    public void UpdateHeart(float life)
    {
        currentLife = Mathf.Clamp(life, 0f, 1f);

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

    void Start()
    {
        UpdateHeart(currentLife);
    }
}