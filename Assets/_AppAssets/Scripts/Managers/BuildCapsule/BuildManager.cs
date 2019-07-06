using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    #region Singleton
    public static BuildManager instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [HideInInspector] public bool IsBuild;
    [HideInInspector] public SlotManager[] slotMangers;

    [SerializeField] private GameObject bg;
    [SerializeField] private UIElement closeBuildingBtnObj;
    [SerializeField] private List<GameObject> buildPrefabs;

    private string currChosenBuildPrefab;
    private int roomId = 0;

    private void Start()
    {
        slotMangers = GetComponentsInChildren<SlotManager>();
        GetAllBuildRooms();
    }

    private void GetAllBuildRooms()
    {
        if (PlayerPrefs.GetString("BuiltRooms").Length > 0)
        {
            string[] allRooms = PlayerPrefs.GetString("BuiltRooms").Split(';');
            string[] roomProb;
            roomId = allRooms.Length;

            for (int i = 0; i < allRooms.Length; i++)
            {
                roomProb = allRooms[i].Split(',');
                int index = int.Parse(roomProb[0].Split('_')[0]);
                slotMangers[int.Parse(roomProb[1])].CreateRoomFromData(roomProb[0],
                    roomProb[2],
                    roomProb[3],
                    buildPrefabs[index]);
            }
        }
    }

    void Update()
    {
        // Detect click on slots to build
        if (IsBuild)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.tag.Equals("BuildSlot"))
                    {
                        // Close Build Slots
                        CloseBuildSlots();

                        // Create Instance of the room
                        int index = GetTheBuildPrefab(hit.transform.GetComponent<Slot>().MyDir);
                        GameObject capsole = Instantiate(buildPrefabs[index], hit.transform.position, Quaternion.identity) as GameObject;
                        capsole.name = index + "_" + roomId.ToString();
                        capsole.transform.SetParent(LevelManager.Instance.Environment.transform);
                        LevelManager.Instance.roomManager.rooms.Add(new Room(capsole));
                        capsole.GetComponentInChildren<RoomEntity>().mySlot = hit.transform.GetComponent<Slot>();
                        AddToSavedData(capsole.name, hit.transform.GetComponent<Slot>());
                        roomId++;
                        LevelManager.Instance.CalculateThisRoomBounds(new Room(capsole));

                        // Rest the build panel UI
                        closeBuildingBtnObj.SwitchVisibilityImmediate();
                        ZUIManager.Instance.ClosePopup("BuildPopup");
                        bg.SetActive(false);


                        // Check in that the slot is taken
                        hit.transform.GetComponent<Slot>().RoomObj = capsole;
                        hit.transform.GetComponent<Slot>().BuildThisSlot(currChosenBuildPrefab);
                    }
                }
            }
        }
    }

    private void AddToSavedData(string id, Slot slot)
    {
        if (SaveTest.Instance.IsSaveData)
        {
            string roomData = (roomId > 0) ? ";" : "";
            roomData += id + ",";
            roomData += slot.MySlotManger.transform.GetSiblingIndex() + ",";
            roomData += ((slot.MyDir == SlotDir.Right) ? "R" : "L") + ",";
            roomData += slot.transform.GetSiblingIndex();

            PlayerPrefs.SetString("BuiltRooms", PlayerPrefs.GetString("BuiltRooms") + roomData);
        }
    }

    /// <summary>
    /// Get the build prefab by currChosenBuildPrefab Name
    /// </summary>
    /// <returns>The index of build prefab </returns>
    private int GetTheBuildPrefab(SlotDir dir = SlotDir.Right)
    {
        if (currChosenBuildPrefab.Equals("GrapperRoom"))
        {
            if (dir == SlotDir.Right)
            {
                return buildPrefabs.FindIndex(x => x.name.Equals("GrapperRoomRight"));
            }
            else
            {
                return buildPrefabs.FindIndex(x => x.name.Equals("GrapperRoomLeft"));
            }
        }
        return buildPrefabs.FindIndex(x => x.name.Equals(currChosenBuildPrefab));
    }

    /// <summary>
    /// Set the name of chosenBuild prefab
    /// </summary>
    /// <param name="buildType">The selected build prefab type</param>
    public void ChoseBuildPrefab(string buildType)
    {
        currChosenBuildPrefab = buildType;
    }

    /// <summary>
    /// Open all Available Build slots
    /// </summary>
    public void OpenBuildSlots()
    {
        // Enable Click Detection on slot
        IsBuild = true;

        // Open all available build slots
        foreach (SlotManager i in slotMangers)
        {
            i.ShowAvailableSlots();
        }
    }

    /// <summary>
    /// Hide All Available build slots
    /// </summary>
    public void CloseBuildSlots()
    {
        // disable Click Detection on slot
        IsBuild = false;

        // Hide all available build slots
        foreach (SlotManager i in slotMangers)
        {
            i.CloseAvailableSlots();
        }
    }
}
