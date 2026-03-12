using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public int playerIndex = 1;
    public string attackTriggerName = "Attack";
    public float attackLockoutSeconds = 3f;

    private Animator anim;
    private float nextAllowedAttackTime = 0f;
    private bool attackInProgress = false;

    private MeleeHitbox meleeHitbox;

    void Awake()
    {
        anim = GetComponent<Animator>();
        meleeHitbox = GetComponentInChildren<MeleeHitbox>();

        if (anim == null)
            Debug.LogError(name + " has no Animator!");

        if (meleeHitbox == null)
            Debug.LogError(name + " has no child with MeleeHitbox script!");
    }

    void Update()
    {
        if (anim == null) return;

        if (attackInProgress) return;
        if (Time.time < nextAllowedAttackTime) return;

        if (GetMeleePressed())
        {
            attackInProgress = true;
            nextAllowedAttackTime = Time.time + attackLockoutSeconds;

            anim.ResetTrigger(attackTriggerName);
            anim.SetTrigger(attackTriggerName);

            Debug.Log(name + " ATTACK START");
        }
    }

    bool GetMeleePressed()
    {
        if (playerIndex == 1)
            return Input.GetKeyDown(KeyCode.J);

        return Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Keypad1);
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

    // Diese Funktion rufst du als LETZTES Animation Event im Attack-Clip auf
    public void EndAttack()
    {
        attackInProgress = false;
        Debug.Log(name + " ATTACK END");
    }
}