using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public int playerIndex = 1;
    public string attackTriggerName = "Attack";

    [Header("Attack Lockout")]
    public float normalAttackLockout = 0.6f;
    public float rageAttackLockout = 0.15f;

    private Animator anim;
    private float nextAllowedAttackTime = 0f;
    private bool attackInProgress = false;

    private MeleeHitbox meleeHitbox;
    private ParryController parry;

    private bool rageMode = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
        meleeHitbox = GetComponentInChildren<MeleeHitbox>();
        parry = GetComponent<ParryController>();

        if (anim == null)
            Debug.LogError(name + " has no Animator!");

        if (meleeHitbox == null)
            Debug.LogError(name + " has no child with MeleeHitbox script!");
    }

    void Update()
    {
        if (anim == null) return;

        if (parry != null && parry.IsParrying())
            return;

        if (attackInProgress && Time.time >= nextAllowedAttackTime)
        {
            attackInProgress = false;
        }

        if (attackInProgress) return;
        if (Time.time < nextAllowedAttackTime) return;

        if (GetMeleePressed())
        {
            attackInProgress = true;

            float currentLockout = rageMode ? rageAttackLockout : normalAttackLockout;
            nextAllowedAttackTime = Time.time + currentLockout;

            anim.ResetTrigger(attackTriggerName);
            anim.SetTrigger(attackTriggerName);

            Debug.Log(name + " ATTACK START");
        }
    }

    bool GetMeleePressed()
    {
        if (playerIndex == 1)
            return Input.GetKeyDown(InputBindings.P1Attack);

        return Input.GetKeyDown(InputBindings.P2Attack);
    }

    public void EnableMeleeHitbox()
    {
        Debug.Log(name + " EnableMeleeHitbox");

        if (meleeHitbox != null)
            meleeHitbox.EnableHitbox();
    }

    public void DisableMeleeHitbox()
    {
        Debug.Log(name + " DisableMeleeHitbox");

        if (meleeHitbox != null)
            meleeHitbox.DisableHitbox();
    }

    public void EndAttack()
    {
        attackInProgress = false;
        Debug.Log(name + " ATTACK END");
    }

    public void SetRageMode(bool active)
    {
        rageMode = active;

        if (!rageMode)
        {
            attackInProgress = false;
            nextAllowedAttackTime = 0f;

            if (meleeHitbox != null)
                meleeHitbox.DisableHitbox();

            if (anim != null)
                anim.ResetTrigger(attackTriggerName);
        }

        Debug.Log(name + " Rage Mode: " + rageMode);
    }

    public bool IsInRageMode()
    {
        return rageMode;
    }
}