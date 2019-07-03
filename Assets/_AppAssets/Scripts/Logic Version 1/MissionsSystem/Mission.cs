using System.Collections.Generic;


using Assets.Scripts.MissionComponent;
using UnityEngine;

public abstract class Mission: IGameplayEvent
{
    //public LogicCore logicCore;
    GameplayEventsTypes eventType;
    public MissionName missionName;
    public Queue<string> tutorialText = new Queue<string>();
    public Queue<GameObject> tools = new Queue<GameObject>();
    public Queue<string> tips = new Queue<string>(); //List class type is not final;
    public Queue<Animation> animations = new Queue<Animation>();
    public Queue<GameObject> VFX = new Queue<GameObject>(); //List class types not decided yet;
    public Queue<GameObject> SFX = new Queue<GameObject>(); //List class types not decided yet;
    //public Queue<TimeLine> timeLine = new Queue<TimeLine>(); //List class types not decided yet;
    public GameObject Room; //Hazard room
    public Mission(GameObject Room/*LogicCore levelLogicCore*/)
    {
        //this.logicCore = levelLogicCore;
        this.Room = Room;
        eventType = GameplayEventsTypes.MISSION;
    }


    public abstract void startMission();

    public GameplayEventsTypes getEventType()
    {
        return this.eventType;
    }
}
