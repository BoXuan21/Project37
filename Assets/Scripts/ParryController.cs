using UnityEngine;
using System.Collections;

public class ParryController : MonoBehaviour
{
    public int playerIndex = 1;

    [Header("Parry Timing")]
    public float perfectParryWindow = 0.03f;
    public float normalParryWindow = 0.16f;
    public float parryCooldown = 0.8f;

    [Header("Parry Result")]
    public float parryKnockbackForce = 8f;
    public float parryKnockbackY = 2f;

    [Header("Visual")]
    public Color parryColor = Color.blue;
    public float blinkInterval = 0.03f;

    private bool isParrying = false;
    private bool isPerfectParry = false;
    private bool canParry = true;

    private SpriteRenderer sr;
    private Animator anim;
    private Color originalColor;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (sr != null)
            originalColor = sr.color;
    }

    void Update()
    {
        if (GetParryPressed() && canParry && !isParrying)
        {
            StartCoroutine(ParryRoutine());
        }
    }

    bool GetParryPressed()
    {
        if (playerIndex == 1)
            return Input.GetKeyDown(KeyCode.K);

        return Input.GetKeyDown(KeyCode.O);
    }

    IEnumerator ParryRoutine()
    {
        isParrying = true;
        isPerfectParry = true;
        canParry = false;

        float elapsed = 0f;
        bool blueOn = false;

        while (elapsed < normalParryWindow)
        {
            elapsed += blinkInterval;

            if (elapsed > perfectParryWindow)
                isPerfectParry = false;

            if (sr != null)
            {
                blueOn = !blueOn;
                sr.color = blueOn ? parryColor : originalColor;
            }

            yield return new WaitForSeconds(blinkInterval);
        }

        if (sr != null)
            sr.color = originalColor;

        isParrying = false;
        isPerfectParry = false;

        yield return new WaitForSeconds(parryCooldown);
        canParry = true;
    }

    public bool IsParrying()
    {
        return isParrying;
    }

    public bool IsPerfectParrying()
    {
        return isParrying && isPerfectParry;
    }

    public void OnSuccessfulParry(GameObject attacker, bool wasPerfectParry)
    {
        if (anim != null)
            anim.SetTrigger("Attack");

        RageController attackerRage = attacker.GetComponent<RageController>();
        bool attackerIsInRage = attackerRage != null && attackerRage.IsRageActive();

        // In rage mode: allow chain parry, but NO knockback
        if (attackerIsInRage)
            return;

        Rigidbody2D attackerRb = attacker.GetComponent<Rigidbody2D>();
        if (attackerRb != null)
        {
            float dir = attacker.transform.position.x > transform.position.x ? 1f : -1f;
            attackerRb.linearVelocity = new Vector2(dir * parryKnockbackForce, parryKnockbackY);
        }
    }
}