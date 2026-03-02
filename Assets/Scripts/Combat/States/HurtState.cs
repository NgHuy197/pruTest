using UnityEngine;

public class HurtState : PlayerState
{
    private int stunFrames;
    private int counter;
    private Vector2 knockback;

    public HurtState(Player player, int stunFrames, Vector2 knockback) : base(player)
    {
        this.stunFrames = stunFrames;
        this.knockback = knockback;
    }

    public override void Enter()
    {
        counter = 0;
        player.Rigidbody.velocity = knockback;
        player.Animator.Play("Hurt");
    }

    public override void Update()
    {
        counter++;
        if (counter >= stunFrames)
            player.StateMachine.ChangeState(player.IdleState);
    }
}