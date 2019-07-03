using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameplayState {
    TycoonState,
    CutsceneState,
    MissionState,
    TrainingState,
    TutorialState
}
public class GameplayFSMManager : MonoBehaviour
{
    /// <summary>
    /// Declaration of dynamic variables for surving the logic goes here.
    /// Eg.
    ///     public int chasingRange;
    ///     public int shootingRange;
    ///     public int alertRange;
    /// </summary>


    Stack<IGameplayState> stateStack = new Stack<IGameplayState>();

    /// <summary>
    /// Declaration of states Instances goes here.
    /// </summary>
    [HideInInspector]
    public TycoonState tycoonState;
    [HideInInspector]
    public CutsceneState cutsceneState;
    [HideInInspector]
    public MissionState missionState;
    [HideInInspector]
    public TrainingState trainingState;
    [HideInInspector]
    public TutorialState tutorialState;



    /// <summary>
    /// Declaration of references will be used for the states logic goes here
    /// Eg. 
    ///     public ISteer steeringScript;
    ///     public GameObject pathRoute;
    ///     public Queue<GameObject> enemyQueue = new Queue<GameObject>();
    /// 
    /// </summary>
    private void Awake()
    {
        /// <summary>
        /// Instantiation of states Instances goes here.
        /// Eg.
        /// chaseEnemy = new ChaseState()
        ///        {
        ///     chasingRange = this.chasingRange,
        ///     shootingRange = this.shootingRange,
        ///     alertRange = this.alertRange,
        ///     movementController = this
        ///         };
        /// </summary>

        //Instantiate the first stat
        cutsceneState = new CutsceneState() {
            gameplayFSMManager = this
        };
        tycoonState = new TycoonState() {
            gameplayFSMManager = this
        };
        missionState = new MissionState(){
            gameplayFSMManager = this
        };
        trainingState = new TrainingState(){
            gameplayFSMManager = this
        };
        tutorialState = new TutorialState(){
            gameplayFSMManager = this
        };
        stateStack.Push(cutsceneState);
    }

    // Update is called once per frame
    void Update()
    {
        stateStack.Peek().OnStateUpdate();
    }


    public void PopState()
    {
        stateStack.Pop().OnStateExit();
    }
    public void PushState(IGameplayState newState)
    {
        newState.OnStateEnter();
        stateStack.Push(newState);
    }

    /// <summary>
    /// States relative logic goes here.
    /// This logic will be used from inside each state itself.
    /// </summary>


    public void changeToTycoonState() {
       PopState();
       PushState(tycoonState);
    }
    public void changeToCutSceneState() {
        PopState();
        PushState(cutsceneState);
    }
    public void changeToMissionState()
    {
        PopState();
        PushState(missionState);
    }
    public void changeToTrainingState() {
        PopState();
        PushState(trainingState);
    }
    public void changeToTutorialState() {
        PopState();
        PushState(tutorialState);
    }

    public GameplayState getCurrentState() {
        return stateStack.Peek().GetStateName();
    }

    #region States relative logic reference

    //public void wonder()
    //{
    //    if (steeringScript==null)
    //    {
    //        steeringScript= this.gameObject.AddComponent<Wonder>();
    //    }
    //}
    //public void seekEnemy()
    //{
    //    if (steeringScript == null)
    //    {
    //        steeringScript=  this.gameObject.AddComponent<Seek>();
    //    }
    //}
    //public void followPath()
    //{
    //    if (steeringScript == null)
    //    {
    //        steeringScript= this.gameObject.AddComponent<PathFinding>();
    //        (steeringScript as PathFinding).path = pathRoute;
    //    }
    //}
    //public void removeSteeringScript() {
    //    GetComponent<ISteer>().destroyMe();
    //}
    #endregion
}
