using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    private Collider2D hitboxCollider;
    private bool hasHitThisAttack = false;

    void Awake()
    {
        hitboxCollider = GetComponent<Collider2D>();

        if (hitboxCollider != null)
            hitboxCollider.enabled = false;
    }

    public void EnableHitbox()
    {
        hasHitThisAttack = false;

        if (hitboxCollider != null)
            hitboxCollider.enabled = true;
    }

    public void DisableHitbox()
    {
        if (hitboxCollider != null)
            hitboxCollider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHitThisAttack) return;
        if (other.transform.root == transform.root) return;

        GameObject attacker = transform.root.gameObject;
        GameObject defender = other.transform.root.gameObject;

        ParryController parry = defender.GetComponent<ParryController>();
        PlayerHealth health = defender.GetComponent<PlayerHealth>();

        if (health == null) return;

        Vector2 hitDirection = (defender.transform.position.x > transform.position.x)
            ? Vector2.right
            : Vector2.left;

        // Defender is parrying
        if (parry != null && parry.IsParrying())
        {
            bool perfectParry = parry.IsPerfectParrying();

            // Give rage to the defender / player who parried
            RageTracker defenderRageTracker = defender.GetComponent<RageTracker>();
            if (defenderRageTracker != null)
                defenderRageTracker.RegisterSuccessfulParry();

            // Let parry controller handle reactions / knockback rules
            parry.OnSuccessfulParry(attacker, perfectParry);

            // Only frame-perfect parry deals damage
            if (perfectParry)
            {
                health.TakeDamage(0.5f, hitDirection);
                SoundManager.Instance?.PlayHit();
            }

            hasHitThisAttack = true;
            return;
        }

        // Normal hit
        health.TakeDamage(0.5f, hitDirection);
        hasHitThisAttack = true;
        SoundManager.Instance?.PlayHit();
    }
}