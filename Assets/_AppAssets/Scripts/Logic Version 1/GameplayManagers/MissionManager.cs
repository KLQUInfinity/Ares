
public class MissionManager : IGameplayEventManager
{

    public Mission currenMission;

    public Mission Mission
    {
        get => default;
        set
        {
        }
    }



    //float missionTime;
    //bool isMissionStarted;
    //public void startTrainingTraining(Mission mission)
    //{
    //    isMissionStarted = true;
    //    if (isMissionStarted)
    //    {
    //        missionTime += Time.deltaTime;
    //    }

    //}
    //public void endMission()
    //{
    //    missionTime = 0;
    //}

    public void fireEvent(IGameplayEvent eventObj)
    {
        GameBrain.Instance.gameplayFSMManager.changeToMissionState();
        currenMission = (Mission)eventObj;
        currenMission.startMission();
        GameBrain.Instance.logMessage(currenMission.missionName + " has started");
    }
}
