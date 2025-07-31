// using UnityEngine;

// public abstract class GameState
// {
//     protected GameStateMachine stateMachine;

//     public GameState(GameStateMachine stateMachine)
//     {
//         this.stateMachine = stateMachine;
//     }

//     public virtual void Update() { }
//     public virtual void Enter() { }
//     public virtual void Exit() { }
// }

// public class GameStateMachine : MonoBehaviour
// {
//     public bool isPaused = false;

//     //GameState Instances
//     public GamePausedState PausedState { get; private set; }
//     //public GameRunningState RunningState { get; private set; }

//     private GameState currentState;

//     void Awake()
//     {

//     }

// }

// public class GamePausedState : GameState
// {
//     public GamePausedState(GameStateMachine stateMachine) : base(stateMachine) { }
//     public override void Update() { }
//     public override void Enter()
//     {
//         stateMachine.isPaused = true;
//         Time.timeScale = 0;

//      }
//     public override void Exit()
//     {
//         stateMachine.isPaused = false;
//         Time.timeScale = 1;
//      }
// }