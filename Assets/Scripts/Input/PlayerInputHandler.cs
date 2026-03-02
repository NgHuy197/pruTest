using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerControls controls;

    public float MoveInput { get; private set; } // A+D
    public bool JumpPressed { get; private set; } // K
    public bool CloseAttackPressed { get; private set; } // j
    public bool RangeAttackPressed { get; private set; } // U
    public bool UltimatePressed { get; private set; } // I
    public bool DashPressed { get; private set; } // L
    public bool DefendPressed { get; private set; } // Hold S
    public bool SupportPressed { get; private set; } // O
    public bool Special1Pressed { get; private set; } // W + U
    public bool Special2Pressed { get; private set; } // W + J

    private void Awake() => controls = new PlayerControls();

    private void OnEnable()
    {
        controls.Fighting.Enable();
        controls.Fighting.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>().x;
        controls.Fighting.Move.canceled += ctx => MoveInput = 0f;

        controls.Fighting.Jump.started += ctx => JumpPressed = true;
        controls.Fighting.CloseAttack.started += ctx => CloseAttackPressed = true;
        controls.Fighting.RangeAttack.started += ctx => RangeAttackPressed = true;
        controls.Fighting.Ultimate.started += ctx => UltimatePressed = true;
        controls.Fighting.Dash.started += ctx => DashPressed = true;
        controls.Fighting.Support.started += ctx => SupportPressed = true;

        controls.Fighting.Defend.performed += ctx => DefendPressed = true;
        controls.Fighting.Defend.canceled += ctx => DefendPressed = false;

        controls.Fighting.Special1.started += ctx => Special1Pressed = true;
        controls.Fighting.Special2.started += ctx => Special2Pressed = true;
    }

    private void LateUpdate()
    {
        JumpPressed = false;
        CloseAttackPressed = false;
        RangeAttackPressed = false;
        UltimatePressed = false;
        DashPressed = false;
        SupportPressed = false;
        Special1Pressed = false;
        Special2Pressed = false;
    }

    private void OnDisable() => controls.Fighting.Disable();
}