using UnityEngine;

public enum GameModeType
{
    Normal,
    NoRage
}

public class GameModeSettings : MonoBehaviour
{
    public static GameModeSettings Instance;

    public GameModeType currentMode = GameModeType.Normal;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool RageEnabled()
    {
        return currentMode != GameModeType.NoRage;
    }
}