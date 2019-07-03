using Assets.Scripts.MissionComponent;
using Assets.Scripts.TrainingComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

public class GameBrain : MonoBehaviour
{
    /// <summary>
    /// Variables must be false before building the game.
    /// </summary>
    [Header("Test")]
    public bool testing;

    
    private static GameBrain _Instance;

    [HideInInspector]
    public int eventsCounter;
    [HideInInspector]
    public CutsceneManager cutscenesManager;
    [HideInInspector]
    public TrainingManager trainingsManager;
    [HideInInspector]
    public MissionManager missionsManager;
    [HideInInspector]
    public TutorialManager tutorialsManager;
    [HideInInspector]
    public GameplayTimeline gameplayTimeline;

    public ResourcesManager resourcesManager;
    public TimeManager timeManager;
    public GameplayFSMManager gameplayFSMManager;

    public static GameBrain Instance
    {
        get { return _Instance; }
    }


    private void Awake()
    {

        if (_Instance == null)
        {
            _Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    public void LateStart() {

    }
    // Start is called before the first frame update
    void Start()
    {
        if (testing)
        {
            Debug.Log("Game brain started");
        }
        cutscenesManager = new CutsceneManager();
        trainingsManager = new TrainingManager();
        missionsManager = new MissionManager();
        tutorialsManager = new TutorialManager();
        gameplayTimeline = new GameplayTimeline();
        createGamePlayTimeline();
    }
    public void Update()
    {
        FireEventsByTime();
    }

    private void FireEventsByTime()
    {
        if (eventsCounter < timeManager.eventsTime.Count)
        {
            if (timeManager.gameTime == timeManager.eventsTime[eventsCounter].eventTime)
            {
                moveToNextGamePlayEvent();
                eventsCounter++;
            }
        }
    }

    private void createGamePlayTimeline()
    {
        gameplayTimeline.createAndAddCutscene(new CutScene("Intro"));
        gameplayTimeline.createAndAddMission(MissionName.AsteroidAccident);
        gameplayTimeline.createAndAddTraining(TrainingName.FireExtinguishing);
        gameplayTimeline.createAndAddTutorial(new Tutorial("Fir fighting tutorial"));
    }

    public void moveToNextGamePlayEvent() {
        gameplayTimeline.startNextEvent();
        logMessage(timeManager.gameTime.ToString());
    }
    public void logMessage(string msg) {
        if(testing)
        Debug.Log(msg);
    }

    public void OnSecondChange()
    {//Called each real second
        resourcesManager.OnSecondChange();
    }
    public void OnMinuteChange()
    {//Called each real Minute

    }
    public void OnGameHourChange()
    {//Called each Game Hour

    }
    public void OnGameDayChange()
    {// Called each Game Day

    }


}
