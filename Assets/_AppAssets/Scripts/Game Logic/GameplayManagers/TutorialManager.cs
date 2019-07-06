
public class TutorialManager : IGameplayEventManager
{
    Tutorial currentTutorial;
    float trainingNumber;

    public Tutorial Tutorial
    {
        get => default;
        set
        {
        }
    }

    public void fireEvent(IGameplayEvent eventObj)
    {
        GameBrain.Instance.gameplayFSMManager.changeToTutorialState();
        currentTutorial = (Tutorial)eventObj;
        currentTutorial.startEvent();
        GameBrain.Instance.logMessage(currentTutorial.tutorialName + " has started");
    }

}
