using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class player_movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float speedCalculation;

    private Rigidbody rb;
    private Vector2 movementDirection;
    private PlayerControls playerInputAction;
    private PlayerStateMachine stateMachine;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInputAction = new PlayerControls();
        stateMachine = GetComponent<PlayerStateMachine>();
    }

    void Start()
    {

    }

    void OnEnable()
    {
        //enable player actions
        playerInputAction.Enable();

        //sub to move action
        playerInputAction.Gameplay.Movement.performed += OnMove;
        playerInputAction.Gameplay.Movement.canceled += OnMove;


        playerInputAction.Gameplay.Sprint.started += OnSprint;
        playerInputAction.Gameplay.Sprint.canceled += OnSprint;

        playerInputAction.Gameplay.Crouch.started += OnCrouch;
        playerInputAction.Gameplay.Crouch.canceled += OnCrouch;
    }

    void OnDisable()
    {
        // Unsubscribe from the actions
        playerInputAction.Gameplay.Movement.performed -= OnMove;
        playerInputAction.Gameplay.Movement.canceled -= OnMove;

        playerInputAction.Gameplay.Sprint.started -= OnSprint;
        playerInputAction.Gameplay.Sprint.canceled -= OnSprint;

        playerInputAction.Gameplay.Crouch.started -= OnCrouch;
        playerInputAction.Gameplay.Crouch.canceled -= OnCrouch;

        // Disable the input actions
        playerInputAction.Disable();
    }

    void FixedUpdate()
    {
        //Apply Movement to rigid body
        Vector3 moveDirection = transform.TransformDirection(new Vector3(movementDirection.x, 0, movementDirection.y));
        speedCalculation = moveSpeed * stateMachine.MoveSpeedMultiplier;
        rb.linearVelocity = moveDirection * speedCalculation + new Vector3(0, rb.linearVelocity.y, 0f);
        //rb.angularVelocity = Vector3.zero;   commented out in attempt to fix mouse
    }

    void OnMove(InputAction.CallbackContext context)
    {
        movementDirection = context.ReadValue<Vector2>();
        stateMachine.SetMoveInput(movementDirection);
        //Debug.Log($"Movement Input: {movementDirection}");
    }

    void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            stateMachine.OnSprintPressed();  //walk to sprint

        }
        else if (context.canceled)
        {
            stateMachine.OnSprintReleased(); //sprint to walk
        }
    }

    void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            stateMachine.OnCrouchPressed();
        }
        else if (context.canceled)
        {
            stateMachine.OnCrouchReleased();
        }
    }
    
}
