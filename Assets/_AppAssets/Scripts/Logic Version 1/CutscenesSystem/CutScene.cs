using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene : IGameplayEvent
{
    GameplayEventsTypes eventType;
    public string cutsceneName;
    public CutScene(string cutsceneName)
    {
        this.cutsceneName = cutsceneName;
        eventType = GameplayEventsTypes.CUTSCENE;
    }
    public GameplayEventsTypes getEventType()
    {
        return eventType;
    }

    public void startEvent() {
        //event start code goes here.
    }
   
}
