using UnityEngine;

public static class InputBindings
{
    public static KeyCode GetKey(string key, KeyCode defaultKey)
    {
        return (KeyCode)PlayerPrefs.GetInt(key, (int)defaultKey);
    }

    public static void SetKey(string key, KeyCode newKey)
    {
        PlayerPrefs.SetInt(key, (int)newKey);
        PlayerPrefs.Save();
    }

    // P1 defaults
    public static KeyCode P1Attack => GetKey("P1Attack", KeyCode.J);
    public static KeyCode P1Parry  => GetKey("P1Parry", KeyCode.K);
    public static KeyCode P1Rage   => GetKey("P1Rage", KeyCode.L);

    // P2 defaults
    public static KeyCode P2Attack => GetKey("P2Attack", KeyCode.I);
    public static KeyCode P2Parry  => GetKey("P2Parry", KeyCode.O);
    public static KeyCode P2Rage   => GetKey("P2Rage", KeyCode.P);
}