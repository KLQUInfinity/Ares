using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameTime:ICloneable
{
    [HideInInspector]
    public int gameHour;
    [HideInInspector]
    public int gameDay;
    [SerializeField]
    public float realSecond;
    [SerializeField]
    public float realMinute;
    [HideInInspector]
    public float realHour;

    int totalGameDays;
    [HideInInspector]
    int secondTemp;
    [HideInInspector]
    int minuteTemp;
    [HideInInspector]
    int gameHourTemp;
    [HideInInspector]
    int gameDayTemp;
    public GameTime()
    {

    }
    public GameTime(int totalGameDays)
    {
        this.totalGameDays = totalGameDays;
    }
    public GameTime(int gameHour, int gameDay)
    {
        this.gameHour = gameHour;
        this.gameDay = gameDay;
    }
    public GameTime(int gameHour, int gameDay, float realSecond, float realMinute, float realHour):this(gameHour, gameDay)
    {
        this.realSecond = realSecond;
        this.realMinute = realMinute;
        this.realHour = realHour;
    }
    public void update()
    {

        if (gameDay < totalGameDays)
        {
            realSecond += Time.deltaTime;
            calculateTime();

            if (Mathf.FloorToInt(realSecond)!=secondTemp)
            {
                //if (GameBrain.Instance.testing)
                //{
                //    Debug.Log(Mathf.FloorToInt(realSecond));
                //}
                secondTemp = Mathf.FloorToInt(realSecond);
                OnSecondChange();

            }

            if (realMinute != minuteTemp)
            {
                secondTemp = Mathf.RoundToInt(realMinute);
                OnMinuteChange();
                //if (GameBrain.Instance.testing)
                //{
                //    Debug.Log(secondTemp);
                //}
            }
            if (gameHour != gameHourTemp)
            {
                gameHourTemp = gameHour;
                OnGameHourChange();
                //if (GameBrain.Instance.testing)
                //{
                //    Debug.Log(secondTemp);
                //}
            }
            if (gameDay != gameDayTemp)
            {
                gameDayTemp = gameDay;
                OnGameDayChange();
                //if (GameBrain.Instance.testing)
                //{
                //    Debug.Log(secondTemp);
                //}
            }
            //Calculate OnChange of real minute, Game Hour, Game day 
        }
    }
    public void calculateTime()
    {
        realMinute = Mathf.FloorToInt(realSecond) / 60;
        realHour = (realMinute % 60) == 0 ? (realMinute / 60) : realHour;
        gameHour = Mathf.FloorToInt(Mathf.FloorToInt(realSecond) / 30f);
        gameDay = Mathf.FloorToInt(realMinute / totalGameDays);
        gameHour = (24 - ((gameDay + 1) * 24 - (gameHour + 1))) - 1;

    }
    #region Methods to Call On Time Units Change
    public void OnSecondChange()
    {//Called each real second
        GameBrain.Instance.OnSecondChange();
        LevelManager.Instance.OnSecondChange();
    }
    public void OnMinuteChange()
    {//Called each real Minute
        GameBrain.Instance.OnMinuteChange();
        LevelManager.Instance.OnMinuteChange();
    }
    public void OnGameHourChange()
    {//Called each Game Hour
        GameBrain.Instance.OnGameHourChange();
        LevelManager.Instance.OnGameHourChange();
    }
    public void OnGameDayChange()
    {// Called each Game Day
        GameBrain.Instance.OnGameDayChange();
        LevelManager.Instance.OnGameDayChange();
    }
    #endregion

    public override string ToString()
    {
        return realMinute.ToString();//(realMinute < 60) ? realMinute +"": (realMinute / 60)+ ":"+ (realMinute - 60);
    }

    public object Clone()
    {
        return new GameTime(this.gameHour,this.gameDay,this.realSecond,this.realMinute,this.realHour);
    }
    #region Arithmetic Operators Override;
    public static GameTime operator +(GameTime gameTime1, GameTime gameTime2) {
        GameTime Sum = new GameTime();
        Sum.gameDay = gameTime1.gameDay + gameTime2.gameDay;
        Sum.gameHour = gameTime1.gameHour + gameTime2.gameHour;
        return Sum;
    }
    public static GameTime operator -(GameTime gameTime1, GameTime gameTime2)
    {
        GameTime difference = new GameTime();
        difference.gameDay = gameTime1.gameDay - gameTime2.gameDay;
        difference.gameHour = gameTime1.gameHour - gameTime2.gameHour;
        return difference;
    }
    #endregion
    #region Logical Operators Override;
    public static bool operator ==(GameTime gameTime1, GameTime gameTime2)
    {
        if (gameTime1.realMinute== gameTime2.realMinute)
        {
            return true;
        }
        else
        {
            return false;

        }
    }
    public static bool operator !=(GameTime gameTime1, GameTime gameTime2)
    {
        if (gameTime1.realMinute == gameTime2.realMinute)
        {
            return true;
        }
        else
        {
            return false;

        }
    }
    public static bool operator >(GameTime gameTime1, GameTime gameTime2) {
        bool state=false;
        if (gameTime1.gameDay>gameTime2.gameDay)
        {
            state = true;
        }
        if (gameTime1.gameHour > gameTime2.gameHour)
        {
            state = true;
        }
        return state;
    }
    public static bool operator <(GameTime gameTime1, GameTime gameTime2)
    {
        bool state = false;
        if (gameTime1.gameDay < gameTime2.gameDay)
        {
            state = true;
        }
        if (gameTime1.gameHour < gameTime2.gameHour)
        {
            state = true;
        }
        return state;
    }
    public static bool operator >=(GameTime gameTime1, GameTime gameTime2) {
        bool state=false;
        if (gameTime1.gameDay >= gameTime2.gameDay)
        {
            state = true;
        }
        if (gameTime1.gameHour >= gameTime2.gameHour)
        {
            state = true;
        }
        return state;
    }
    public static bool operator <=(GameTime gameTime1, GameTime gameTime2)
    {
        bool state = false;
        if (gameTime1.gameDay <= gameTime2.gameDay)
        {
            state = true;
        }
        if (gameTime1.gameHour <= gameTime2.gameHour)
        {
            state = true;
        }
        return state;
    }
    #endregion
}
[System.Serializable]
public class EventsTime
{
    public string eventName;
    public GameTime eventTime;
}

public class TimeManager : MonoBehaviour
{
    [Header("Game Duration")]
    public int TotalGameDays;
    [SerializeField]
    public GameTime gameTime;


    [HideInInspector]
    public bool isUpdating;
    public List<EventsTime> eventsTime = new List<EventsTime>();
    private void Awake()
    {
        gameTime = new GameTime(TotalGameDays);
        if (GameBrain.Instance.testing)
        {
            Debug.Log("time is istantiated");
        }
    }
    public void Update()
    {

        if (GameBrain.Instance.testing)
        {
            isUpdating = true;
        }
        if (isUpdating)
        {
            //if (GameBrain.Instance.testing)
            //{
            //    Debug.Log("time is counting");
            //}
            gameTime.update();
        }
    }
}
