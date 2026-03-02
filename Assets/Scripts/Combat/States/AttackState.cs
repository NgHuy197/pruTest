using UnityEngine;

public class AttackState : PlayerState
{
    private AttackData data;
    private int frameCounter;

    public AttackState(PlayerMovement playerMovement, Player player, AttackData data) : base(player)
    {
        this.data = data;
    }

    // Backwards-compatible constructor for Player-only usage
    public AttackState(Player player, AttackData data) : base(player)
    {
        this.data = data;
    }

    public override void Enter()
    {
        frameCounter = 0;
        player.Animator.Play(data.attackName);
    }

    private float frameTimer;
    private const float FRAME_TIME = 1f / 60f;

    public override void Update()
    {
        frameTimer += Time.deltaTime;

        while (frameTimer >= FRAME_TIME)
        {
            frameTimer -= FRAME_TIME;
            ProcessFrame();
        }
    }

    private void ProcessFrame()
    {
        frameCounter++;

        if (frameCounter == data.startupFrames)
            player.Hitbox.EnableHitbox(data);

        if (frameCounter == data.startupFrames + data.activeFrames)
            player.Hitbox.DisableHitbox();

        if (frameCounter >= data.startupFrames + data.activeFrames + data.recoveryFrames)
            player.StateMachine.ChangeState(player.IdleState);
    }
}