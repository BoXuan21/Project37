using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float life = 1f;
    public HeartUI heartUI;

    [Header("Hit Reaction")]
    public float hitstunDuration = 0.2f;
    public float knockbackForceX = 6f;
    public float knockbackForceY = 2f;

    [Header("Hit Effects")]
    public GameObject hitEffect;

    private Animator anim;
    private Rigidbody2D rb;
    private PlayerMovement2D movement;
    private MeleeAttack meleeAttack;

    private bool isInHitstun = false;
    private bool isDead = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement2D>();
        meleeAttack = GetComponent<MeleeAttack>();
    }

    public void TakeDamage(float damage, Vector2 hitDirection)
    {
        if (isDead) return;
        if (isInHitstun) return;

        life -= damage;
        life = Mathf.Clamp(life, 0f, 1f);

        Debug.Log(name + " life: " + life);

        // ❤️ UI Update
        if (heartUI != null)
            heartUI.UpdateHeart(life);

        // ❄️ Hit Freeze
        if (HitFreezeManager.instance != null)
            HitFreezeManager.instance.Freeze(0.05f);

        // 💥 Hit Effekt
        if (hitEffect != null)
          Instantiate(hitEffect,
    transform.position + (Vector3)hitDirection * 0.6f,
    Quaternion.LookRotation(Vector3.forward, hitDirection));

        // 📳 Camera Shake
        if (CameraShake.instance != null)
            CameraShake.instance.Shake(0.08f, 0.08f);

        // ☠️ Death Check
        if (life <= 0f)
        {
            Die(hitDirection);
            return;
        }

        // 🤕 Hurt Animation
        if (anim != null)
            anim.SetTrigger("Hurt");

        // Hitstun starten
        StartCoroutine(HitstunCoroutine(hitDirection));
    }

    private IEnumerator HitstunCoroutine(Vector2 hitDirection)
    {
        isInHitstun = true;

        if (movement != null)
            movement.enabled = false;

        if (meleeAttack != null)
            meleeAttack.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            rb.linearVelocity = new Vector2(hitDirection.x * knockbackForceX, knockbackForceY);
        }

        yield return new WaitForSeconds(hitstunDuration);

        if (!isDead)
        {
            if (movement != null)
                movement.enabled = true;

            if (meleeAttack != null)
                meleeAttack.enabled = true;
        }

        isInHitstun = false;
    }

    private void Die(Vector2 hitDirection)
    {
        isDead = true;

        Debug.Log(name + " died!");

        if (movement != null)
            movement.enabled = false;

        if (meleeAttack != null)
            meleeAttack.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(hitDirection.x * knockbackForceX, knockbackForceY);
        }

        if (anim != null)
            anim.SetTrigger("Death");

       RoundManager rm = FindFirstObjectByType<RoundManager>();
        if (rm != null)
            rm.PlayerDied(gameObject);
    }

    public void ResetHealth()
    {
        life = 1f;

        if (heartUI != null)
            heartUI.UpdateHeart(life);

        isDead = false;

        GetComponent<PlayerMovement2D>().enabled = true;
        GetComponent<MeleeAttack>().enabled = true;

        if (anim != null)
        {
            anim.Rebind();
            anim.Update(0f);
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}