using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonSound : MonoBehaviour
{
    void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(PlayClickSound);
    }

    void PlayClickSound()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayClick();
        else
            Debug.LogWarning("UIButtonSound: No SoundManager instance found in scene.");
    }
}