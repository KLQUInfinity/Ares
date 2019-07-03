using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bendary_LevelManager : MonoBehaviour
{
    private static Bendary_LevelManager _Instance;

    public GameEvent OnDragOn;
    public GameEvent OnDragOff;

    public GameObject Environment;
    public Dictionary<GameObject, Bounds> roomsBounds = new Dictionary<GameObject, Bounds>();
    public Dictionary<GameObject, List<GameObject>> roomsContents = new Dictionary<GameObject, List<GameObject>>();

    public ClickStateMachine controllersStateMachine;
   
    #region Map Boundries
    public Transform TopBoundry;
    public Transform BottomBoundry;
    public Transform LeftBoundry;
    public Transform RightBoundry;
    public Transform FrontBoundry;
    public Transform BackBoundry;
    #endregion

    public static Bendary_LevelManager Instance
    {
        get { return _Instance; }
    }

    private void Awake()
    {
        calculateRoomsBounds();
        if (_Instance == null)
        {
            _Instance = this;
        }
    }

    void Start()
    {
        //asdfasdfasdfadsf
        //Debug.Log((roomsBounds[Environment.transform.GetChild(0).gameObject].size.x + roomsBounds[Environment.transform.GetChild(0).gameObject].size.y) / 2 - 2.5f);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void setDragModeOn() {
        OnDragOn.Raise();
    }
    public void setDragModeOff() {
        OnDragOff.Raise();
    }

    #region
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
        foreach (Transform room in Environment.transform.GetChildren())
        {
            var renderers = room.GetComponentsInChildren<Renderer>();
            Bounds bounds = new Bounds(renderers[0].bounds.center, new Vector3(0, 0, 0));
            foreach (var renderer in renderers)
            {
                if (renderer != renderers[0])
                    bounds.Encapsulate(renderer.bounds);
            }
            roomsBounds.Add(room.gameObject, bounds);
        }
    }

    public void CalculateThisRoomBounds(Transform Room)
    {//asdfsadfadsf
        var renderer = Room.GetChild(0).GetChild(1).gameObject.GetComponent<MeshRenderer>();
        roomsBounds.Add(Room.gameObject, renderer.bounds);
    }
    public GameObject populateAndGetContainerRoom(GameObject draggableItem)
    {
        foreach (var room in roomsBounds.Keys)
        {
            if (roomsBounds[room].Contains(draggableItem.transform.position))
            {
                populateToARoom(room, draggableItem);
                return room;
            }
        }
        return null;
    }
    public void populateToARoom(GameObject room,GameObject containedObj) {
        if (!roomsContents.ContainsKey(room))
        {
            roomsContents.Add(room, new List<GameObject>());
        }
        List<GameObject> contents = roomsContents[room];
        contents.Add(containedObj);
    }
    public void evacuateFromARoom(GameObject room,GameObject containedObj) {
        if (roomsContents.ContainsKey(room))
        {
            List<GameObject> contents = roomsContents[room];
            contents.Remove(containedObj);
        }
    }
}
