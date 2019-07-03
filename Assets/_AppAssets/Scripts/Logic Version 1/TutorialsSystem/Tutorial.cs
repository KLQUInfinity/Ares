using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial: IGameplayEvent
{
    GameplayEventsTypes eventType;
    public string tutorialName;
    public Tutorial(string tutorialName)
    {
        this.tutorialName = tutorialName;
        eventType = GameplayEventsTypes.TUTORIAL;
    }
    public GameplayEventsTypes getEventType()
    {
        return eventType;
    }

    public void startEvent()
    {
        //event start code goes here.
    }
}
