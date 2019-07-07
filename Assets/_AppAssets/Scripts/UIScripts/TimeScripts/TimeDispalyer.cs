using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeDispalyer : MonoBehaviour
{
    enum TimeUnit
    {
        days,
        hours
    }

    [SerializeField] private TimeUnit timeUnit;

    private TextMeshProUGUI timeTxt;


    private void Start()
    {
        timeTxt = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        switch (timeUnit)
        {
            case TimeUnit.days:
                timeTxt.text = (GameBrain.Instance.timeManager.TotalGameDays -
                    GameBrain.Instance.timeManager.gameTime.gameDay).ToString();
                break;
            case TimeUnit.hours:

                timeTxt.text = (24 - GameBrain.Instance.timeManager.gameTime.gameHour) + ":00";
                break;
        }
    }
}