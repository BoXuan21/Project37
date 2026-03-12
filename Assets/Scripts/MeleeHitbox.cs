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

        // eigene Hitbox ignorieren
        if (other.transform.root == transform.root)
            return;

        GameObject defender = other.transform.root.gameObject;

        // Parry prüfen
        ParryController parry = defender.GetComponent<ParryController>();
        if (parry != null && parry.IsParrying())
        {
            parry.OnSuccessfulParry(transform.root.gameObject);

            hasHitThisAttack = true;
            return;
        }

        // PlayerHealth prüfen
        PlayerHealth health = defender.GetComponent<PlayerHealth>();
        if (health == null)
            return;

        // Treffer-Richtung bestimmen
        Vector2 hitDirection = (defender.transform.position.x > transform.position.x)
            ? Vector2.right
            : Vector2.left;

        // Damage anwenden
        health.TakeDamage(0.5f, hitDirection);

        hasHitThisAttack = true;
    }
}