using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterJobState
{//Updated at character have been populated or evacuated from a room.
    Employed,
    Unemployed
}


[System.Serializable]
public class Character
{
    #region Character's Attributes
    //-------------------------------------------
    public string name;
    public string title;
    public string age;
    public int wakeUpDay; //Gameday
    public int wakeUpHour; //Gamehour
    //-------------------------------------------
    public float stamina = 100;
    public float happiness = 100;
    //-------------------------------------------
    public Job job;
    public int jobHourTemp;

    public float productivity;
    public CharacterLevel characterLevel;
    public CharacterJobState jobState;
    public List<CharacterLevel> characterLevels = new List<CharacterLevel>();
    public float overWorkedHoursProduct = 0;
    //-------------------------------------------
    public GameObject container;
    public GameObject containerEntrance;
    [HideInInspector]
    Room containerRoom;


    //-------------------------------------------
    public GameObject characterGameObject;
    #endregion

    #region Constructor(s)

    //Can't be deleted, it is being used.
    public Character(GameObject characterGameObject) {

        this.characterGameObject = characterGameObject;
    }

    public void Init()
    {
        jobState = CharacterJobState.Unemployed;
        container = LevelManager.Instance.roomManager.populateAndGetContainerRoom(characterGameObject);
        characterGameObject.GetComponent<Dragable_Item>().containerRoom = container;

        containerRoom = LevelManager.Instance.roomManager.getRoomWithGameObject(container);
        characterLevel.totalLevelDaysWorkedHours[GameBrain.Instance.timeManager.gameTime.gameDay] = new GameTime();
    }

    #endregion

    #region Character's method

    #region Work, Stamina, happiness and productivity related methods;

    public void updateStamina()
    {//Called over time
        if (jobState == CharacterJobState.Unemployed)
        {
            stamina += characterLevel.staminaRefillSpeedPerSecond;
            stamina = Mathf.Clamp(stamina, 0, 10);
        }
        else if (jobState == CharacterJobState.Employed)
        {
            stamina -= job.staminaReductionRate;
            stamina = Mathf.Clamp(stamina, 0, 10);
        }
    }

    public void updateCurrentCharacterLevelGameTimeDay()
    {
        characterLevel.totalLevelDaysWorkedHours.Add(GameBrain.Instance.timeManager.gameTime.gameDay, new GameTime());
    }

    public void updateCurrentCharacterLevelGametimeHour()
    {//Called each hour to update number of hours the character has worked in this level so far.
     //Calculation of GameTime is starting from 0 for each of the time units;
        GameTime levelGameTime = characterLevel.totalLevelDaysWorkedHours[GameBrain.Instance.timeManager.gameTime.gameDay];
        switch (jobState)
        {
            case CharacterJobState.Employed:

                if (GameBrain.Instance.timeManager.gameTime.gameHour - jobHourTemp == 1)
                {
                    jobHourTemp = GameBrain.Instance.timeManager.gameTime.gameHour;
                    levelGameTime.gameHour++;
                }

                if (characterLevel.totalRoomWorkedHours.Keys.Contains(containerRoom))
                {
                    characterLevel.totalRoomWorkedHours[containerRoom] = GameBrain.Instance.timeManager.gameTime.gameHour - job.jobAcquisionHour;
                }
                else
                {
                    characterLevel.totalRoomWorkedHours.Add
                        (containerRoom, GameBrain.Instance.timeManager.gameTime.gameHour - job.jobAcquisionHour);
                }

                break;
            case CharacterJobState.Unemployed:
                break;
        }

    }

    public void creditProductionCycle()
    {//Called at each production cycle process end.
        float productionCyclesSum = 0;
        foreach (var room in characterLevel.totalRoomWorkedHours.Keys)
        {
            productionCyclesSum += characterLevel.totalRoomWorkedHours[room] - room.productionCyclePeriod;
        }
        characterLevel.doneProductionCycles = Mathf.RoundToInt(productionCyclesSum);
    }

    /// <summary>
    /// This will be used in order to see if the character exceeded a specific total working hours limit
    /// there must be a consiquences for these happenings.
    /// E.g.
    /// That the character may be banned from work for a while.
    /// </summary>
    /// <returns></returns>
    public void calculateOverWorkedHoursProduct()
    {//Called at updateCharacterLevel() which is each game hour.
        float overWork = 0;
        foreach (var level in characterLevels)
        {
            foreach (var day in level.totalLevelDaysWorkedHours.Keys)
            {
                var overWorkTemp = level.workingHoursDailyLimit - level.totalLevelDaysWorkedHours[day].gameHour;
                overWork += overWorkTemp < 0 ? overWorkTemp : 0;
            }
        }
        //Calculate the product of the worked hours of character Levels worked hours history dictionary
        overWorkedHoursProduct = Mathf.Abs(overWork);
    }


    /// <summary>
    ///         Determine the period of new Character level (Gametime).
    ///         Determine what are the daily working hours limit of the new character level
    ///         Determine what are the stamina refill speed of the new character level
    /// </summary>
    public void updateCharacterLevel()
    {//Called each game hour

        //This must be called before leveling the character up to assign the value to the current level workedhours first.
        updateCurrentCharacterLevelGametimeHour();
        calculateOverWorkedHoursProduct();
        if (characterLevel.doneProductionCycles == characterLevel.levelProductionCyclePeriod)
        {
            LevelManager.Instance.characterManager.levelCharacterUp(this);
        }
        //IF character reaches the level period then level'm up.
        //GameTime currentLevelGameTime = totalWorkedHours[characterLevel];
        //GameTime leveLGameTimeLimit = characterLevel.LevelTimeLimit;
    }

    public void calculateCharacterHappiness()
    {//Called each second
        //Stamina
        //ProductTotalWorkedHours
        if (overWorkedHoursProduct < characterLevel.overWorkHoursProductLimit)
        {
            float staminaFactor = (10 - stamina) / 2;
            float overWorkedProductFactor = ((1-(overWorkedHoursProduct/characterLevel.overWorkHoursProductLimit)) * 5);
            happiness = (10 - (staminaFactor + overWorkedProductFactor)) * 10;
            happiness = Mathf.Clamp(happiness, 0, 100);
        }
        else {

            //Ban the character from being able to work for a while.
            happiness = 0;
        }

    }

    public void calculateProductivity()
    {//called each second
        float overWorkedProductFactor = ((1-(overWorkedHoursProduct / characterLevel.overWorkHoursProductLimit)) * 100);
        if (overWorkedProductFactor >= 75)
        {
            productivity = 1;
        }
        else if (overWorkedProductFactor >= 50)
        {
            productivity = 0.75f;

        }
        else if (overWorkedProductFactor >= 25)
        {
            productivity = 0.5f;
        }
        else
        {
            productivity = 0.25f;
        }
    }

    #endregion

    #region Job related methods;

    public void startJob(Job job)
    {
        this.job = job;
        jobState = CharacterJobState.Employed;
    }

    public void leaveJob()
    {
        this.job = null;
        jobState = CharacterJobState.Unemployed;
    }

    public void assignJob(Job job)
    {
        startJob(job);
        jobHourTemp = job.jobAcquisionHour;
    }

    public void deassignJob()
    {
        leaveJob();
    }
    #endregion

    #region Navigation related methods
    public void updateCharacterContainer(GameObject container)
    {
        this.container = container;
    }
    #endregion

    #region Logical operators ovverides

    public static bool operator ==(Character character, GameObject obj)
    {
        if (character.characterGameObject == obj)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool operator !=(Character character, GameObject obj)
    {
        if (character.characterGameObject != obj)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
    
    #endregion
}
