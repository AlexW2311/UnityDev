using UnityEngine;

//base class for all player states
public abstract class PlayerState
{
    protected PlayerStateMachine stateMachine;

    public PlayerState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Update() { }
    public virtual void Enter() { }
    public virtual void Exit() { }
}

public class PlayerStateMachine : MonoBehaviour
{
    [Header("Speed Multipliers")]
    public float idleMultiplier = 0f;
    public float walkMultiplier = 1f;
    public float sprintMultiplier = 1.70f;
    public float crouchMultiplier = 0.5f;
    [Header("Crouch Settings")]
    public float standingHeight = 2f;
    public float crouchHeight = 1.2f;
    public float heightLerpSpeed = 12f;

    //References
    private CapsuleCollider capsuleCollider;
    private Transform viewRoot;
    private float targetHeight;
    private float viewRootStartLocalY;
    private float crouchEyeLocalY;


    // Latest movement input pushed in from player_movement (Vector2 WASD)
    public Vector2 MoveInput { get; private set; } = Vector2.zero;
    public bool SprintHeld { get; private set; } = false;
    public bool CrouchHeld { get; private set; } = false;


    // Exposed to the  Movement script
    public float MoveSpeedMultiplier { get; private set; } = 0f;

    [Header("State Instances")]
    public PlayerSprintState SprintState { get; private set; }
    public PlayerCrouchState CrouchState { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }

    private PlayerState currentState;

    void Awake()
    {
        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        SprintState = new PlayerSprintState(this);
        CrouchState = new PlayerCrouchState(this);

        capsuleCollider = GetComponent<CapsuleCollider>();
        viewRoot = FindViewRoot();
    }

    void Start()
    {
        Init(IdleState);
        targetHeight = standingHeight;
        viewRootStartLocalY = viewRoot.localPosition.y;
        crouchEyeLocalY = viewRootStartLocalY * (crouchHeight / standingHeight);

    }

    public void SetMoveInput(Vector2 input) => MoveInput = input;
    internal void SetMoveSpeedMultiplier(float value) => MoveSpeedMultiplier = value;
    void Update() => currentState.Update();
    void LateUpdate()
    {
        UpdateCrouchVisuals(Time.deltaTime);
    }

    //EVENT HANDLERS
    public void Init(PlayerState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
    public void OnSprintPressed()
    {
        if (currentState != SprintState)
        {
            ChangeState(SprintState);
            SprintHeld = true;
        }
    }
    public void OnSprintReleased()
    {
        if (currentState == SprintState)
        {
            ChangeState(WalkState);
            SprintHeld = false;
        }
    }

    public void OnCrouchPressed()
    {
        if (currentState != CrouchState)
        {
            ChangeState(CrouchState);
        }
    }

    public void OnCrouchReleased()
    {
        if (currentState == CrouchState)
        {
            if (MoveInput.sqrMagnitude > 0.0001f)
            {
                ChangeState(WalkState);
            }
            else
            {
                ChangeState(IdleState);
            }
        }
    }

    internal void SetCrouchTarget(bool crouch)
    {
        targetHeight = crouch ? crouchHeight : standingHeight;
    }

    //Helper Methods
    private Transform FindViewRoot()
    {
        Transform t = this.transform.Find("fpv_camera");
        if (t != null) return t;

        Camera camera = GetComponentInChildren<Camera>();
        if (camera != null) return camera.transform;
        return transform;

    }

    private void UpdateCrouchVisuals(float deltaT)
    {
        //collider height
        float height = Mathf.Lerp(capsuleCollider.height, targetHeight, deltaT * heightLerpSpeed);
        capsuleCollider.height = height;
        capsuleCollider.center = new Vector3(0f, height * 0.5f, 0f); // plants user

        //camera root n vertical shift
        float time = (height - crouchHeight) / (standingHeight - crouchHeight);
        Vector3 pos = viewRoot.localPosition;
        pos.y = Mathf.Lerp(crouchEyeLocalY, viewRootStartLocalY, time);
        viewRoot.localPosition = pos;

    }


}

//Idle state

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.SetMoveSpeedMultiplier(stateMachine.idleMultiplier);
        Debug.Log("Entered Idle State");
    }

    public override void Exit()
    {
        Debug.Log("Exited Idle State");
    }
    public override void Update()
    {
        //transition logic
        if (stateMachine.MoveInput.sqrMagnitude > 0.0001f)
        {
            stateMachine.ChangeState(stateMachine.WalkState);
        }
    }
}
public class PlayerWalkState : PlayerState
{
    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.SetMoveSpeedMultiplier(stateMachine.walkMultiplier);
        Debug.Log("Entered Walk State");

    }

    public override void Exit()
    {
        Debug.Log("Exited Walk state");
    }

    public override void Update()
    {
        //transition logic, no wasd input brings the uder to the idle state
        if (stateMachine.MoveInput.sqrMagnitude <= 0.0001f)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }

    }
}

public class PlayerCrouchState : PlayerState
{
    public PlayerCrouchState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.SetMoveSpeedMultiplier(stateMachine.walkMultiplier * stateMachine.crouchMultiplier);
        stateMachine.SetCrouchTarget(true);
        Debug.Log("Entered Crouch State");
    }
    public override void Exit()
    {
        stateMachine.SetCrouchTarget(false);
        Debug.Log("Exited Crouch State");
    }
}

public class PlayerSprintState : PlayerState
{
    public PlayerSprintState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.SetMoveSpeedMultiplier(stateMachine.walkMultiplier * stateMachine.sprintMultiplier);
        Debug.Log("Entered Sprint State");
    }

    public override void Exit()
    {
        Debug.Log("Exited Sprint State");
    }
}