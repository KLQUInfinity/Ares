using System.Collections.Generic;


using Assets.Scripts.TrainingComponent;
using UnityEngine;

public abstract class Training: IGameplayEvent
{

    //public LogicCore logicCore;
    GameplayEventsTypes eventType;
    public TrainingName trainingName;

    public Queue<string> tutorialText = new Queue<string>();
    public Queue<GameObject> tools = new Queue<GameObject>();
    public Queue<GameObject> UI = new Queue<GameObject>();
    public Queue<string> tips = new Queue<string>(); //List class type is not final;
    public Queue<Animation> animations = new Queue<Animation>();
    public Queue<GameObject> VFX = new Queue<GameObject>(); //List class types not decided yet;
    public Queue<GameObject> SFX = new Queue<GameObject>(); //List class types not decided yet;
    //public Queue<TimeLine> timeLine = new Queue<TimeLine>(); //List class types not decided yet;

    public Training(/*LogicCore levelLogicCore*/)
    {
        //this.logicCore = levelLogicCore;
        this.eventType = GameplayEventsTypes.TRAINING;
    }

    public abstract void startTraining();

    public GameplayEventsTypes getEventType()
    {
        return this.eventType;
    }
}
