using UnityEngine;

public class RunState : PlayerState
{
    public RunState(PlayerMovement playerMovement, Player player) : base(player) { }

    // Backwards-compatible constructor for Player-only usage
    public RunState(Player player) : base(player) { }

    private float noMoveTimer = 0f;
    private const float NO_MOVE_THRESHOLD = 0.12f; // require sustained no-input before going to idle

    public override void Enter()
    {
        base.Enter();
        noMoveTimer = 0f;
        player.Animator.Play("Run");
    }

    public override void Update()
    {
        if (player.Input == null)
        {
            player.StateMachine.ChangeState(player.IdleState);
            return;
        }

        float mv = Mathf.Abs(player.Input.MoveInput);

        if (mv < 0.01f)
        {
            noMoveTimer += Time.deltaTime;
            if (noMoveTimer >= NO_MOVE_THRESHOLD)
            {
                player.StateMachine.ChangeState(player.IdleState);
                return;
            }
        }
        else
        {
            noMoveTimer = 0f;
        }

        if (player.Input.JumpPressed && player.IsGrounded())
        {
            player.StateMachine.ChangeState(player.JumpState);
            return;
        }

        // movement physics handled by PlayerMovement component; this state only manages animation and transitions
    }
}
