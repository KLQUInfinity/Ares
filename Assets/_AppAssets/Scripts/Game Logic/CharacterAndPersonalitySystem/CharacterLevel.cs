using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterLevel 
{
    //How many days the character has worked? and how many hours he has worked in each?
    //This Game time must be assigned using clone method (Capture object state not reference).
    //we care only about the gameDay and gameHour
    [HideInInspector]
    public Dictionary<int, GameTime> totalLevelDaysWorkedHours = new Dictionary<int, GameTime>();

    [HideInInspector]
    public Dictionary<Room, int> totalRoomWorkedHours = new Dictionary<Room, int>();

    [Header ("If the player reaches this number he will level up")]
    public int levelProductionCyclePeriod;

    [HideInInspector]
    public int doneProductionCycles;

    [Header("If exceeded the time through all level working days his happeness will be low")]
    public int workingHoursDailyLimit;

    [Header("If exceeded this limit the character will banned for a while")]
    public int overWorkHoursProductLimit;

    [Header("Low stamina means low happiness!")]
    public float staminaRefillSpeedPerSecond;

    
}
