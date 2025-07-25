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

    

    // Exposed to the  Movement script
    public Vector2 MoveInput { get; private set; } = Vector2.zero;
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

    }

    void Start() => Init(IdleState);
    void Update() => currentState.Update();

    public void SetMoveInput(Vector2 input) => MoveInput = input;
    internal void SetMoveSpeedMultiplier(float value) => MoveSpeedMultiplier = value;
    

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
    // Sprint
    public void OnSprintPressed()
    {
        if (currentState == WalkState) ChangeState(SprintState);
    }
    public void OnSprintReleased()
    {
        if (currentState == SprintState)
        {
            ChangeState(MoveInput.sqrMagnitude > 0.0001f ? WalkState : IdleState);
        }
    }

    public bool IsCrouched => currentState == CrouchState;

    public void OnCrouchPressed()
    {
        if (currentState != CrouchState) ChangeState(CrouchState);
    }
    public void OnCrouchReleased()
    {
        if (currentState == CrouchState)
        {
            ChangeState(MoveInput.sqrMagnitude > 0.0001f ? WalkState : IdleState);
        }
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
        Debug.Log("Entered Crouch State");
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

}