using Assets.Scripts.MissionComponent;
using Assets.Scripts.TrainingComponent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameplayEventsTypes {
    TRAINING,
    MISSION,
    TUTORIAL,
    CUTSCENE
}
public class GameplayTimeline : MonoBehaviour
{
    public Queue<IGameplayEvent> eventsTimeLine = new Queue<IGameplayEvent>();
    IGameplayEvent currentEvent;

    public void enqueueGameplayEvent(IGameplayEvent eventObj) {
        eventsTimeLine.Enqueue(eventObj);
    }
    public void dequeueGameplayEvent() {
        if(eventsTimeLine.Count!=0)
        currentEvent = eventsTimeLine.Dequeue();

        fireEvent(currentEvent);
    }

    private void fireEvent(IGameplayEvent currentEvent)
    {
        switch (currentEvent.getEventType())
        {
            case GameplayEventsTypes.CUTSCENE:
                GameBrain.Instance.cutscenesManager.fireEvent(currentEvent);
                break;
            case GameplayEventsTypes.MISSION:
                GameBrain.Instance.missionsManager.fireEvent(currentEvent);
                break;
            case GameplayEventsTypes.TRAINING:
                GameBrain.Instance.trainingsManager.fireEvent(currentEvent);
                break;
            case GameplayEventsTypes.TUTORIAL:
                GameBrain.Instance.tutorialsManager.fireEvent(currentEvent);
                break;
            default:
                break;
        }
    }

    public void createAndAddCutscene( CutScene cutscene) {
        enqueueGameplayEvent(cutscene);
    }
    public void createAndAddMission(MissionName missionName) {
        enqueueGameplayEvent( MissionFactory.createMission(missionName));
    }
    public void createAndAddTraining(TrainingName trainingName)
    {
        enqueueGameplayEvent(TrainingFactory.createTraining(trainingName));
    }
    public void createAndAddTutorial(Tutorial tutorial)
    {
        enqueueGameplayEvent(tutorial);
    }
    public void startNextEvent() {
        dequeueGameplayEvent();
    }
}
