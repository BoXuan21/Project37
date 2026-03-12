using UnityEngine;
using System.Collections;

public class HitFreezeManager : MonoBehaviour
{
    public static HitFreezeManager instance;

    void Awake()
    {
        instance = this;
    }

    public void Freeze(float duration)
    {
        StartCoroutine(FreezeCoroutine(duration));
    }

    IEnumerator FreezeCoroutine(float duration)
    {
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1f;
    }
}