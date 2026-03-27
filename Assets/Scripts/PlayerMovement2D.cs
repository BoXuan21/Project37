using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    public int playerIndex = 1;
    public float moveSpeed = 6f;

    [Header("Jump")]
    public float jumpForce = 12f;

    [Header("Dash")]
    public float dashSpeed = 14f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.6f;
    public float doubleTapTime = 0.25f;

    [Header("Air Dash")]
    public float airDashSpeed = 12f;
    public float airDashDuration = 0.12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;

    [Header("Effects")]
    public GameObject dashEffect;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    private float moveInput;

    private bool isGrounded;
    private bool wasGrounded;

    private bool isDashing = false;
    private bool isAirDashing = false;
    private bool hasAirDashed = false;

    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;
    private float dashDirection = 0f;

    private float lastLeftPress = -1f;
    private float lastRightPress = -1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        CheckGrounded();
        HandleMovementInput();
        HandleJumpInput();
        HandleDoubleTapDash();
        UpdateAnimator();
        UpdateFacing();
    }

    void FixedUpdate()
    {
        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.fixedDeltaTime;

        if (isDashing || isAirDashing)
        {
            dashTimer -= Time.fixedDeltaTime;

            float speed = isAirDashing ? airDashSpeed : dashSpeed;
            rb.linearVelocity = new Vector2(dashDirection * speed, 0f);

            if (dashTimer <= 0f)
            {
                isDashing = false;
                isAirDashing = false;
            }

            return;
        }

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    void CheckGrounded()
    {
        wasGrounded = isGrounded;

        if (groundCheck != null)
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        else
            isGrounded = false;

        if (isGrounded && !wasGrounded)
        {
            hasAirDashed = false;
            isAirDashing = false;
        }
    }

    void HandleMovementInput()
    {
        moveInput = 0f;

        if (playerIndex == 1)
        {
            if (Input.GetKey(KeyCode.A)) moveInput = -1f;
            if (Input.GetKey(KeyCode.D)) moveInput = 1f;
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow)) moveInput = -1f;
            if (Input.GetKey(KeyCode.RightArrow)) moveInput = 1f;
        }
    }

    void HandleJumpInput()
    {
        if (!isGrounded) return;
        if (isDashing || isAirDashing) return;

        if (GetJumpPressed())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void HandleDoubleTapDash()
    {
        if (isDashing || isAirDashing) return;
        if (isGrounded && dashCooldownTimer > 0f) return;

        if (playerIndex == 1)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (Time.time - lastRightPress < doubleTapTime)
                    TryStartDash(1f);

                lastRightPress = Time.time;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                if (Time.time - lastLeftPress < doubleTapTime)
                    TryStartDash(-1f);

                lastLeftPress = Time.time;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (Time.time - lastRightPress < doubleTapTime)
                    TryStartDash(1f);

                lastRightPress = Time.time;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (Time.time - lastLeftPress < doubleTapTime)
                    TryStartDash(-1f);

                lastLeftPress = Time.time;
            }
        }
    }

    void TryStartDash(float direction)
    {
        if (isGrounded)
        {
            StartGroundDash(direction);
        }
        else
        {
            if (hasAirDashed) return;
            StartAirDash(direction);
        }
    }

    void StartGroundDash(float direction)
    {
        isDashing = true;
        dashDirection = direction;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;

        SpawnDashEffect();
    }

    void StartAirDash(float direction)
    {
        isAirDashing = true;
        hasAirDashed = true;
        dashDirection = direction;
        dashTimer = airDashDuration;

        SpawnDashEffect();
    }

    void SpawnDashEffect()
    {
        if (dashEffect != null)
        {
            Instantiate(dashEffect, transform.position + Vector3.down * 0.4f, Quaternion.identity);
        }
    }

   bool GetJumpPressed()
{
    if (playerIndex == 1)
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log(name + " P1 jump pressed");
            return true;
        }
    }
    else
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log(name + " P2 jump pressed");
            return true;
        }
    }

    return false;
}

    void UpdateAnimator()
    {
        if (anim == null) return;

        float speed = (isDashing || isAirDashing) ? 1f : Mathf.Abs(moveInput);

        if (HasParameter("Speed"))
            anim.SetFloat("Speed", speed);

        if (HasParameter("Grounded"))
            anim.SetBool("Grounded", isGrounded);

        if (HasParameter("VerticalSpeed"))
            anim.SetFloat("VerticalSpeed", rb.linearVelocity.y);
    }

    void UpdateFacing()
    {
        if (sr == null) return;

        float facing = (isDashing || isAirDashing) ? dashDirection : moveInput;

        if (facing != 0f)
            sr.flipX = facing < 0f;
    }

    bool HasParameter(string name)
    {
        if (anim == null) return false;

        foreach (var param in anim.parameters)
        {
            if (param.name == name)
                return true;
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public void ResetMovementState()
{
    moveInput = 0f;

    isDashing = false;
    isAirDashing = false;
    hasAirDashed = false;

    dashTimer = 0f;
    dashCooldownTimer = 0f;
    dashDirection = 0f;

    lastLeftPress = -1f;
    lastRightPress = -1f;

    if (rb != null)
        rb.linearVelocity = Vector2.zero;
}
}