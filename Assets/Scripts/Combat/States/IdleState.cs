public class IdleState : PlayerState
{
    public IdleState(PlayerMovement playerMovement, Player player) : base(player) { }

    // Backwards-compatible constructor for Player-only usage
    public IdleState(Player player) : base(player) { }

    public override void Enter()
    {
        player.Animator.Play("Idle");
    }
}