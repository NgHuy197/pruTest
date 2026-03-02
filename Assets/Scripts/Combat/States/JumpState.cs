using UnityEngine;

public class JumpState : PlayerState
{
    public JumpState(PlayerMovement playerMovement, Player player) : base(player) { }

    // Backwards-compatible constructor for Player-only usage
    public JumpState(Player player) : base(player) { }

    public override void Enter()
    {
        player.Animator.Play("Jump");
        // actual vertical impulse is handled by PlayerMovement which reads the same input
        hasLeftGround = false;
        landingTimer = 0f;
    }

    public override void Update()
    {
        // Wait until player leaves ground (actual jump) then wait for a stable landing
        if (!hasLeftGround)
        {
            if (!player.IsGrounded())
                hasLeftGround = true;
            return;
        }

        if (player.IsGrounded())
        {
            landingTimer += Time.deltaTime;
            if (landingTimer >= LANDING_STABLE_TIME)
            {
                if (player.Input != null && Mathf.Abs(player.Input.MoveInput) > 0.01f)
                    player.StateMachine.ChangeState(player.WalkState);
                else
                    player.StateMachine.ChangeState(player.IdleState);
            }
        }
        else
        {
            landingTimer = 0f;
        }
    }

    private bool hasLeftGround = false;
    private float landingTimer = 0f;
    private const float LANDING_STABLE_TIME = 0.05f; // require short stable ground contact to avoid flicker
}
