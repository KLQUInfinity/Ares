using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManger : MonoBehaviour
{
    #region Room Manager Data Members
    #region Room
    [SerializeField]
    public List<Room> rooms = new List<Room>();
    #endregion

    #region Character Navigation Rooms Reflections' Data Members (Bounds,Contents)
    public Dictionary<Room, Bounds> roomsBounds = new Dictionary<Room, Bounds>();

    #endregion

    #endregion

    #region Room Manager Methods
    private void Start()
    {

    }

    public void OnPopulate(Room room, Character character)
    {//On player room population
        LevelManager.Instance.characterManager.OnCharacterPopulation(room, character);
        if (LevelManager.Instance.Testing)
        {
            Debug.Log(room.roomGameObject.transform.name);
        }

    }

    public void OnEvacuate(Character character)
    {
        //On player room evacuation
        //Order of mehtods is critical
        LevelManager.Instance.characterManager.OnCharacterEvacuation(character);
    }

    public Room getRoomWithGameObject(GameObject obj)
    {
        foreach (var room in rooms)
        {
            if (room.roomGameObject == obj)
            {
                return room;
            }
        }
        return null;
    }

    public Job getOtherJobInTheRoom(Job characterJob)
    {
        foreach (var job in getRoomWithGameObject(characterJob.jobRoom).roomJobs)
        {
            if (characterJob != job)
            {
                return job;
            }
        }
        return null;
    }

    public void getRoomEntrances(GameObject roomGameObject, out GameObject leftEntrance, out GameObject rightEntrance)
    {
        leftEntrance = roomGameObject.GetComponentInChildren<RoomEntity>().leftEntrance;
        rightEntrance = roomGameObject.GetComponentInChildren<RoomEntity>().rightEntrance;
    }
    public void InitializeRoomsResources(Room room)
    {
        room.roomProductionResource = room.roomGameObject.GetComponentInChildren<ProducerEntity>().getfirstResource();
        room.roomProductionRate = room.roomGameObject.GetComponentInChildren<ProducerEntity>().getFirstResourceProductionRate();
    }
    public void OnSecondChange()
    {//Called each real second
        foreach (var room in rooms)
        {

            //This up comming line is bad performance need to be relocated, Hence it need to be called once for 
            //initialization but not at the initilization of these concrete classes or its manager if that
            //happened it will fire null pointer exception.
            room.debuggingUI = room.roomGameObject.GetComponentInChildren<RoomEntity>().DebuggingUI;
            //-----------------------------------------------------------------------------------------------------
            if (room.roomProductionResource != null)
            {
                room.calculateRoomProductivity();

            }
        }
    }
    public void OnMinuteChange()
    {//Called each real Minute

    }
    public void OnGameHourChange()
    {//Called each Game Hour
        foreach (var room in rooms)
        {
            if (room.roomProductionResource != null)
            {
                room.calculateProductionCycle();
            }

        }

    }
    public void OnGameDayChange()
    {// Called each Game Day

    }

    #region Character Navigation Rooms Reflections' Methods

    public GameObject populateAndGetContainerRoom(GameObject draggableItem)
    {
        //LevelManager.Instance.roomManager.getRoomWithGameObject(drag)
        foreach (KeyValuePair<Room,Bounds> entry in roomsBounds)
        {
            if (entry.Value.Contains(draggableItem.transform.position))
            {
                populateToARoom(entry.Key.roomGameObject ,draggableItem);
                return entry.Key.roomGameObject;
            }
        }
        return null;
    }
    public void populateToARoom(GameObject roomGameObject, GameObject containedObj)
    {
        Room currentRoom = LevelManager.Instance.roomManager.getRoomWithGameObject(roomGameObject);
        currentRoom.contents.Add(containedObj);
        var obj = LevelManager.Instance.characterManager.getCharacterWithGameObject(containedObj);
        if (obj != null)
            OnPopulate(currentRoom, obj);

    }
    ////rooms[rooms.IndexOf(room)].contents.Add(containedObj);
    //if (!roomsContents.ContainsKey(room))
    //{
    //    roomsContents.Add(room, new List<GameObject>());
    //}
    //List<GameObject> contents = roomsContents[room];
    //contents.Add(containedObj);

    public void evacuateFromARoom(GameObject roomGameObject, GameObject containedObj)
    {
        Room currentRoom = LevelManager.Instance.roomManager.getRoomWithGameObject(roomGameObject);
        currentRoom.contents.Remove(containedObj);
        var obj = LevelManager.Instance.characterManager.getCharacterWithGameObject(containedObj);
        if (obj != null)
            OnEvacuate(obj);
    }
    #endregion

    #region Deprecated Script
    //if (roomsContents.ContainsKey(room))
    //{
    //    List<GameObject> contents = roomsContents[room];
    //    contents.Remove(containedObj);
    //}
    #endregion
    #endregion

}