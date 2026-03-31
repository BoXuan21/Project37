using UnityEngine;
using System.Collections;

public class RageController : MonoBehaviour
{
    [Header("Rage")]
    public int rageBars = 0;
    public int maxRageBars = 2;
    public float rageDuration = 4f;

    [Header("Input")]
    public int playerIndex = 1;

    [Header("Visual")]
    public Color rageColor = Color.red;
    public float blinkInterval = 0.08f;

    private bool rageActive = false;

    private SpriteRenderer sr;
    private Color originalColor;
    private MeleeAttack meleeAttack;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        meleeAttack = GetComponent<MeleeAttack>();

        if (sr != null)
            originalColor = sr.color;
    }

    void Update()
    {
        if (!IsRageAllowed())
        {
            ForceDisableRage();
            return;
        }

        if (rageActive) return;
        if (rageBars < maxRageBars) return;

        if (GetRagePressed())
        {
            StartCoroutine(RageRoutine());
        }
    }

    bool GetRagePressed()
    {
        if (playerIndex == 1)
            return Input.GetKeyDown(InputBindings.P1Rage);

        return Input.GetKeyDown(InputBindings.P2Rage);
    }

    bool IsRageAllowed()
    {
        return GameModeSettings.Instance == null || GameModeSettings.Instance.RageEnabled();
    }

    public void AddRageBar()
    {
        if (!IsRageAllowed())
            return;

        rageBars = Mathf.Clamp(rageBars + 1, 0, maxRageBars);
        Debug.Log(name + " rage bars: " + rageBars + "/" + maxRageBars);
    }

    void ForceDisableRage()
    {
        rageBars = 0;

        if (rageActive)
        {
            rageActive = false;

            if (sr != null)
                sr.color = originalColor;

            if (meleeAttack != null)
                meleeAttack.SetRageMode(false);
        }
    }

    IEnumerator RageRoutine()
    {
        rageActive = true;
        rageBars = 0;

        if (meleeAttack != null)
            meleeAttack.SetRageMode(true);

        float elapsed = 0f;
        bool redOn = false;

        while (elapsed < rageDuration)
        {
            if (!IsRageAllowed())
            {
                ForceDisableRage();
                yield break;
            }

            elapsed += blinkInterval;

            if (sr != null)
            {
                redOn = !redOn;
                sr.color = redOn ? rageColor : originalColor;
            }

            yield return new WaitForSeconds(blinkInterval);
        }

        if (sr != null)
            sr.color = originalColor;

        if (meleeAttack != null)
            meleeAttack.SetRageMode(false);

        rageActive = false;
    }

    public bool IsRageActive()
    {
        return rageActive;
    }
}