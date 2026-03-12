using UnityEngine;
using System.Collections;

public class ParryController : MonoBehaviour
{
    public int playerIndex = 1;

    [Header("Parry")]
    public float parryDuration = 0.2f;
    public float parryCooldown = 0.8f;
    public float parryKnockbackForce = 8f;

    [Header("Visual")]
    public Color parryColor = Color.blue;
    public float blinkInterval = 0.05f;

    private bool isParrying = false;
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
        canParry = false;

        float timer = 0f;
        bool blueOn = false;

        while (timer < parryDuration)
        {
            timer += blinkInterval;

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

        yield return new WaitForSeconds(parryCooldown);
        canParry = true;
    }

    public bool IsParrying()
    {
        return isParrying;
    }

    public void OnSuccessfulParry(GameObject attacker)
    {
        if (anim != null)
            anim.SetTrigger("Attack");

        Rigidbody2D attackerRb = attacker.GetComponent<Rigidbody2D>();
        if (attackerRb != null)
        {
            float dir = attacker.transform.position.x > transform.position.x ? 1f : -1f;
            attackerRb.linearVelocity = new Vector2(dir * parryKnockbackForce, 2f);
        }
    }
}