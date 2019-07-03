using System.Collections.Generic;
using UnityEngine;

public enum productionJobType
{
    production,
    maintenance
}
public class Room
{

    #region Constructors

    public Room(GameObject roomGameObject)
    {
        this.roomGameObject = roomGameObject;

    }

    public Room(GameObject roomGameObject, int productionCyclePeriod,
        float buildingCost, float maintenanceCost) : this(roomGameObject)
    {
        this.productionCyclePeriod = productionCyclePeriod;
        this.buildingCost = buildingCost;
        this.maintenanceCost = maintenanceCost;
    }

    public Room(GameObject roomGameObject, float buildingCost, float maintenanceCost) : this(roomGameObject)
    {
        this.buildingCost = buildingCost;
        this.maintenanceCost = maintenanceCost;
    }

    public Room(GameObject roomGameObject, int productionCyclePeriod) : this(roomGameObject)
    {
        this.productionCyclePeriod = productionCyclePeriod;
    }

    #endregion


    #region Room Data Members

    public GameObject roomGameObject;

    public List<GameObject> contents = new List<GameObject>();
    public List<Job> roomJobs = new List<Job>();
    public productionJobType productionJobType;
    public float roomProductivity;
    public Resource roomProductionResource;
    public float roomProductionRate;
    #region Resources Related Data Members
    public float roomWorkedHours;  //As a counter to be reset at the end of the production cycle
    public int productionCyclePeriod = 2; // A total game hour to a complete production cycle
    public float buildingCost;
    public float maintenanceCost;
    public CapsuleProcessesData debuggingUI;

    #endregion

    #endregion

    #region Room Methods
    #region Logic methods

    /// <summary>
    /// Debugger UI is updated each second in testing mode
    /// </summary>
    public void reflectInRoomDebuggerUI()
    {
        debuggingUI.setProductionProgressValue(roomWorkedHours / productionCyclePeriod);
    }

    /// <summary>
    /// Calculate the product each second.
    /// There must be two methods for each of the jobType types.
    /// But in the jobType production job the product will be multiplied by the 
    /// Resource production rate of the room.
    /// Conclusion: the product of blue collars factors will be between 0 to 1
    /// Hence for example 0 means no one is working,1 means that jobs are occupied.
    /// Note: this is a naiive example the product calculation is more complecated.
    /// </summary>
    public void calculateRoomProductivity()
    { //Called each second
        bool isOneOrBothJobsOccupied = false;
        isOneOrBothJobsOccupied = checkForJobsStates(isOneOrBothJobsOccupied);
        if (isOneOrBothJobsOccupied)
        {
            float jobsProductionRates = 0;
            switch (productionJobType)
            {
                case productionJobType.production:
                    jobsProductionRates = calculateProductProductionRate();
                    break;
                case productionJobType.maintenance:
                    jobsProductionRates = calculateMaintenanceProductionRate();
                    break;
            }
            roomProductivity = jobsProductionRates * roomProductionRate;
            changeInResourceOverTime();
            if (GameBrain.Instance.testing)
            {
                debuggingUI.setRoomProductivity(roomProductivity);
            }
        }
    }

    private bool checkForJobsStates(bool isOneOrBothJobsOccupied)
    {
        foreach (var job in roomJobs)
        {
            if (job.jobState == JobState.Occupied)
            {
                isOneOrBothJobsOccupied = true;
            }
        }

        return isOneOrBothJobsOccupied;
    }

    public void changeInResourceOverTime()
    {//Called each second;
        roomProductionResource.load += roomProductivity;
    }

    private float calculateMaintenanceProductionRate()
    {//Called each second
        return 0;
    }

    private float calculateProductProductionRate()
    {//Called each second
        float jobsProductionRates = 0;
        foreach (var job in roomJobs)
        {
            switch (job.jobState)
            {
                case JobState.Vacant:
                    break;
                case JobState.Occupied:
                    jobsProductionRates += (job.jobHolder.productivity / 2);
                    break;
            }
        }
        return jobsProductionRates;
    }

    /// <summary>
    /// This will update the production cycle variables in order to
    /// produce the resource.
    /// There are two states according to the jobType for this method:
    /// Production job: this will use the production rate factors of the blue collars.
    /// Maintenance job: it depends of the state of the room it self how much it is damaged.
    /// Both states are using the production period GameTime variables.
    /// </summary>
    public void calculateProductionCycle()
    {//Called each game hour
        roomWorkedHours += (roomProductionResource.load / 60)/2;
        reflectInRoomDebuggerUI(); //Debugger ui method.

        if (roomWorkedHours >= productionCyclePeriod)
        {
            roomWorkedHours =0;
            produceProduct();
            addResourceLoad();
        }

    }

    private void addResourceLoad()
    {
        roomProductionResource.valueInPercentage +=
            (roomProductionResource.valueInPercentage + roomProductionResource.load <= 100)
            ? roomProductionResource.load
            : 100;
        roomProductionResource.load = 0;
    }

    /// <summary>
    /// Produce product to be picked up.
    /// It will reflect to the room, the UI,etc.
    /// </summary>
    public void produceProduct()
    {
        foreach (var obj in contents)
        {
            var character = LevelManager.Instance.characterManager.getCharacterWithGameObject(obj);
            if (character != null)
            {
                //Used to count how many production cycles passed
                character.creditProductionCycle();
            }
        }
    }



    /// <summary>
    /// Randomly or in order assign a job to the character.
    /// </summary>
    /// <param name="character"></param>
    /// <returns>The assigned job to the character to be used in:
    /// assigning the character to the job.
    /// </returns>
    public Job getRandomVacantJob(Character character)
    {
        List<Job> vacantJobs = roomJobs.FindAll(j => j.jobState == JobState.Vacant);
        if (roomJobs.Count == 0)
        {
            return null;
        }

        Job job = vacantJobs[UnityEngine.Random.Range(0, (roomJobs.Count - 1))];
        character.assignJob(job);
        return job;
    }


    public bool searchForFreeJob()
    {
        List<Job> vacantJobs = roomJobs.FindAll(j => j.jobState == JobState.Vacant);
        if (vacantJobs.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    #endregion

    #region Logical operators overrides

    public static bool operator ==(Room room1, Room room2)
    {
        if (room1.roomGameObject == room2.roomGameObject)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool operator !=(Room room1, Room room2)
    {
        if (room1.roomGameObject != room2.roomGameObject)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool operator ==(Room room, GameObject gameObject)
    {
        if (room.roomGameObject == gameObject)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool operator !=(Room room, GameObject gameObject)
    {
        if (room.roomGameObject != gameObject)
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
