using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Body))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpStrength;

    private Body body;
    private Vector2 motion;
    private float coyoteTime;
    private float jumpBuffer = -1;

    protected void Awake()
    {
        body = GetComponent<Body>();
        InitializeInputs();
    }

    private void InitializeInputs()
    {
        var input = new GameInput();
        input.Enable();
        input.Gameplay.HorizontalMovement.performed += OnHorizontalMovement;
        input.Gameplay.HorizontalMovement.canceled += OnHorizontalMovement;
        input.Gameplay.Jump.performed += OnJump;
        input.Gameplay.Jump.canceled += OnCancelJump;
    }

    protected void Update()
    {
        UpdateGravity();
        CheckForJump();
        body.Move(motion * Time.deltaTime);
    }

    private void OnHorizontalMovement(InputAction.CallbackContext context)
    {
        motion.x = context.ReadValue<float>() * moveSpeed;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        jumpBuffer = Time.time;
    }

    private void OnCancelJump(InputAction.CallbackContext context)
    {
        if (motion.y <= 0) return;
        motion.y *= 0.6F;
    }

    private void UpdateGravity()
    {
        var isGrounded = body.IsOverlaping(Vector2.down);
        var onCeiling = body.IsOverlaping(Vector2.up);

        if (onCeiling && motion.y > 0)
        {
            motion.y = 0;
        }
        else if (!isGrounded)
        {
            motion.y -= gravity * Time.deltaTime;
        }
        else if (motion.y < 0)
        {
            motion.y = 0;
        }
    }

    private void CheckForJump()
    {
        coyoteTime = body.IsOverlaping(Vector2.down) ? Time.time : coyoteTime;

        if (jumpBuffer + 0.1F < Time.time) return;
        if (coyoteTime + 0.1F < Time.time) return;

        motion.y = jumpStrength;
        jumpBuffer = 0;
    }
}