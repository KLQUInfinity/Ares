using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ClickStateMachine), typeof(RoomManger), typeof(CharacterManager))]
public class LevelManager : MonoBehaviour
{
    private static LevelManager _Instance;

    public GameEvent OnDragOn;
    public GameEvent OnDragOff;

    public GameObject Environment;
    public RoomManger roomManager;
    public CharacterManager characterManager;
    public ClickStateMachine controllersStateMachine;


    public bool Testing;
    public GameObject FPSGraphTools;
    #region Map Boundries
    public Transform TopBoundry;
    public Transform BottomBoundry;
    public Transform LeftBoundry;
    public Transform RightBoundry;
    public Transform FrontBoundry;
    public Transform BackBoundry;
    #endregion

    public static LevelManager Instance
    {
        get { return _Instance; }
    }

    private void Awake()
    {
        /** Order of methods calling is critical**/
        Init();
        calculateRoomsBounds();
        if (_Instance == null)
        {
            _Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        //asdfasdfasdfadsf
        //Debug.Log((roomsBounds[Environment.transform.GetChild(0).gameObject].size.x + roomsBounds[Environment.transform.GetChild(0).gameObject].size.y) / 2 - 2.5f);
    }

    private void Init()
    {
        foreach (Transform roomTransform in Environment.transform)
        {
            roomManager.rooms.Add(new Room(roomTransform.gameObject));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnDrawGizmos()
    {
        foreach (var room in roomManager.roomsBounds.Values)
        {
            Gizmos.DrawWireCube(room.center, room.size);
        }
    }
    public void setDragModeOn()
    {
        OnDragOn.Raise();
    }
    public void setDragModeOff()
    {
        OnDragOff.Raise();
    }

    #region Deprecated 
    //public void calculateRoomsBounds()
    //{

    //    //    foreach (Transform room in Environment.transform.GetChildren())
    //    //    {
    //    //        var renderers = room.GetChild(0).GetChild(0).GetComponentsInChildren<MeshRenderer>();
    //    //        Bounds bounds = new Bounds(renderers[0].bounds.center, new Vector3(0, 0, 0));
    //    //        foreach (var renderer in renderers)
    //    //        {
    //    //            if (renderer != renderers[0])
    //    //                bounds.Encapsulate(renderer.bounds);
    //    //        }
    //    //        roomsBounds.Add(room.gameObject, bounds);
    //    //    }
    //    //}
    //}
    #endregion 

    public void calculateRoomsBounds()
    {//sadfasdfadsf
        foreach (Room room in roomManager.rooms)
        {
            var renderer = room.roomGameObject.transform.GetChild(0).
                GetChild(1).GetComponentInChildren<Renderer>();
            //Bounds bounds = new Bounds(renderers[0].bounds.center, new Vector3(0, 0, 0));
            //foreach (var renderer in renderers)
            //{
            //    if (renderer != renderers[0])
            //        bounds.Encapsulate(renderer.bounds);
            //}
            if (!roomManager.rooms.Contains(room))
            {
                roomManager.rooms.Add(room);
            }
            roomManager.roomsBounds.Add(room, renderer.bounds);
        }
    }

    public void CalculateThisRoomBounds(Room room)
    {//asdfsadfadsf
     //var renderers = room.roomGameObject.transform.GetChild(0)
     //.GetChild(1).gameObject.GetComponentsInChildren<MeshRenderer>();

        //Bounds bounds = new Bounds(renderers[0].bounds.center, new Vector3(0, 0, 0));
        //foreach (var renderer in renderers)
        //{
        //    if (renderer != renderers[0])
        //        bounds.Encapsulate(renderer.bounds);
        //}
        var renderer = room.roomGameObject.transform.GetChild(0).
            GetChild(1).GetComponentInChildren<Renderer>();
        if (!roomManager.rooms.Contains(room))
        {
            roomManager.rooms.Add(room);
        }
        roomManager.roomsBounds.Add(room, renderer.bounds);
    }

    public GameObject getCharacterPhysicalContainer(GameObject charcterGameObject) {

        foreach (KeyValuePair<Room, Bounds> entry in roomManager.roomsBounds)
        {
            if (entry.Value.Contains(charcterGameObject.transform.position))
            {
                return entry.Key.roomGameObject;
            }
        }
        return null; //this is impossible to happen. if it happens it would be a deal breaker glitch
    }
    public void OnSecondChange()
    {//Called each real second
        this.characterManager.OnSecondChange();
        this.roomManager.OnSecondChange();
    }
    public void OnMinuteChange()
    {//Called each real Minute
        this.characterManager.OnMinuteChange();
        this.roomManager.OnMinuteChange();
    }
    public void OnGameHourChange()
    {//Called each Game Hour
        this.characterManager.OnGameHourChange();
        this.roomManager.OnGameHourChange();
    }
    public void OnGameDayChange()
    {// Called each Game Day
        this.characterManager.OnGameDayChange();
        this.roomManager.OnGameDayChange();
    }
    public void toggleTesting()
    {
        Testing = !Testing;
        FPSGraphTools.SetActive(Testing);
    }
}
