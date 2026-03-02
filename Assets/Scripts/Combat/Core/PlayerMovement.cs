using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    // Bỏ [Header] ở đây vì đây là các Property
    public Rigidbody2D RB { get; private set; }
    public Animator Anim { get; private set; }
    public PlayerInputHandler Input { get; private set; }
    public Hitbox Hitbox { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }
    public Player Player { get; private set; }

    [Header("Movement Settings")] // Header này hợp lệ vì đặt trên field bên dưới
    public float moveSpeed = 10f;
    public float jumpForce = 16f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public LayerMask groundLayer;

    [Header("Combat Data")]
    public AttackData lightAttackData;

    // Các Instance của State
    public IdleState IdleState { get; private set; }
    public RunState RunState { get; private set; }
    public JumpState JumpState { get; private set; }
    //public FallState FallState { get; private set; }
    public AttackState LightAttackState { get; private set; }

    // Kiểm tra mặt đất
    public bool IsGrounded => Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

    private void Awake()
    {
        // Lấy các Component
        RB = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        Input = GetComponent<PlayerInputHandler>();
        Hitbox = GetComponentInChildren<Hitbox>();
        Player = GetComponent<Player>();

        // Thiết lập State Machine
        StateMachine = new PlayerStateMachine();

        // Pass both the movement component and the Player component to states
        IdleState = new IdleState(this, Player);
        RunState = new RunState(this, Player);
        JumpState = new JumpState(this, Player);
        //FallState = new FallState(this, Player);
        // AttackState (shared with Player) expects the Player and AttackData
        //LightAttackState = new AttackState(Player, lightAttackData);

        // Gán chủ sở hữu cho Hitbox
        // if (Hitbox != null) Hitbox.owner = this; 
        // Lưu ý: Nếu script Hitbox của bạn dùng class 'Player', hãy đổi 'this' thành biến phù hợp
    }

    private void Start()
    {
        Application.targetFrameRate = 60; // Chuẩn cho game đối kháng
        StateMachine.ChangeState(IdleState);
    }

    private void Update()
    {
        StateMachine.Update();
        HandleFlip();
    }

    private void FixedUpdate()
    {
        // Physics-handling states can react via PlayerMovement.RB in their Update; no separate FixedUpdate in the state machine
    }

    private void HandleFlip()
    {
        // Xoay hướng nhân vật
        if (Input.MoveInput > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (Input.MoveInput < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    // Vẽ vòng tròn để kiểm tra lỗi kẹt Ground
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = IsGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}