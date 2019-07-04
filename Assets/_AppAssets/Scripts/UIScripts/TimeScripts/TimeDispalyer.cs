using UnityEngine;
using UnityEngine.UI;

public class TimeDispalyer : MonoBehaviour
{
    enum TimeUnit
    {
        days,
        hours
    }
    [SerializeField]
    TimeUnit timeUnit;
    [SerializeField]
    Text timeTxt;
    // Update is called once per frame
    void Update()
    {
        switch (timeUnit)
        {
            case TimeUnit.days:
                timeTxt.text = "Day " + (GameBrain.Instance.timeManager.TotalGameDays-
                    GameBrain.Instance.timeManager.gameTime.gameDay);
                break;
            case TimeUnit.hours:
                timeTxt.text = (24-GameBrain.Instance.timeManager.gameTime.gameHour) + " OClock";
                break;
        }
    }
}