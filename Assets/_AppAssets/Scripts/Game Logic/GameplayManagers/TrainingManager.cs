

public class TrainingManager : IGameplayEventManager
{
    public Training currentTraining;

    public Training Training
    {
        get => default;
        set
        {
        }
    }

    public void fireEvent(IGameplayEvent eventObj)
    {
        GameBrain.Instance.gameplayFSMManager.changeToTrainingState();
        currentTraining = (Training)eventObj;
        currentTraining.startTraining();
        GameBrain.Instance.logMessage(currentTraining.trainingName + " has started");
    }
}
