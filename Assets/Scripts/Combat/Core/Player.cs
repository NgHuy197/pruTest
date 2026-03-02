using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerID;
    public int maxHP = 100;
    private int currentHP;

    public Rigidbody2D Rigidbody { get; private set; }
    public Animator Animator { get; private set; }
    public Hitbox Hitbox { get; private set; }

    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerState IdleState;
    public AttackState LightAttackState;

    // Movement related states
    public PlayerInputHandler Input { get; private set; }
    public RunState WalkState;
    public JumpState JumpState;

    public AttackData lightAttackData;

    // Ground check for jump state transitions (assign in inspector)
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Hitbox = GetComponentInChildren<Hitbox>();

        Hitbox.owner = this;

        StateMachine = new PlayerStateMachine();

        // input and movement related states
        Input = GetComponent<PlayerInputHandler>();

        IdleState = new IdleState(this);
        LightAttackState = new AttackState(this, lightAttackData);

        WalkState = new RunState(this);
        JumpState = new JumpState(this);

        currentHP = maxHP;
    }

    private void Start()
    {
        StateMachine.ChangeState(IdleState);
    }

    private void Update()
    {
        // basic transitions from idle to movement/jump
        if (StateMachine.CurrentState == IdleState)
        {
            if (Input != null && Mathf.Abs(Input.MoveInput) > 0.01f)
            {
                StateMachine.ChangeState(WalkState);
            }

            if (Input != null && Input.JumpPressed)
            {
                StateMachine.ChangeState(JumpState);
            }
        }

        StateMachine.Update();
    }

    public void PerformLightAttack()
    {
        if (StateMachine.CurrentState is AttackState) return;

        StateMachine.ChangeState(LightAttackState);
    }

    public void TakeDamage(AttackData data, Vector2 direction)
    {
        currentHP -= data.damage;

        if (currentHP <= 0)
        {
            currentHP = 0;
            Debug.Log("Player " + playerID + " Dead");
        }

        Vector2 knockback = direction * data.knockbackForce;

        StateMachine.ChangeState(new HurtState(this, data.hitstunFrames, knockback));
    }

    public bool IsGrounded()
    {
        if (groundCheck == null) return false;

        return Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }
}