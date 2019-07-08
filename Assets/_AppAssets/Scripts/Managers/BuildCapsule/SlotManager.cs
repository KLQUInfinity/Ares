using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    [HideInInspector] public List<Slot> rightSlots = new List<Slot>();
    [HideInInspector] public List<Slot> leftSlots = new List<Slot>();

    public bool RightBuild = true;
    public bool LeftBuild = true;

    [SerializeField] private GameObject rightConnector;
    [SerializeField] private GameObject leftConnector;

    private int rightIndex = 0;
    private int leftIndex = 0;

    private void Awake()
    {
        // Get all slots attached to slot manger
        AddAllAttachedSlots();
    }

    private void AddAllAttachedSlots()
    {
        foreach (Slot i in GetComponentsInChildren<Slot>())
        {
            i.MySlotManger = this;
            if (i.MyDir == SlotDir.Right)
            {
                rightSlots.Add(i);
            }
            else if (i.MyDir == SlotDir.Left)
            {
                leftSlots.Add(i);
            }
        }
    }

    /// <summary>
    /// Show all available Slots
    /// </summary>
    public void ShowAvailableSlots()
    {
        if (RightBuild)
        {
            rightSlots[rightIndex].SlotHighlight.enabled = true;
            rightSlots[rightIndex].MyCollider.enabled = true;
        }
        if (LeftBuild)
        {
            leftSlots[leftIndex].SlotHighlight.enabled = true;
            leftSlots[leftIndex].MyCollider.enabled = true;
        }
    }

    /// <summary>
    /// Close All Available Slots
    /// </summary>
    public void CloseAvailableSlots()
    {
        if (RightBuild)
        {
            rightSlots[rightIndex].SlotHighlight.enabled = false;
            rightSlots[rightIndex].MyCollider.enabled = false;
        }
        if (LeftBuild)
        {
            leftSlots[leftIndex].SlotHighlight.enabled = false;
            leftSlots[leftIndex].MyCollider.enabled = false;
        }
    }

    /// <summary>
    /// Prepare next slot for the next build if it allowed
    /// </summary>
    /// <param name="dir">the dir of current built room</param>
    /// <param name="CanBuildOther">if the room allow next build or not</param>
    public void PrepareNextSlot(SlotDir dir, bool CanBuildOther = true)
    {
        // Check dir
        if (dir.Equals(SlotDir.Right))
        {
            if (rightIndex == 0)
            {
                rightConnector.SetActive(false);
            }

            // Increment index
            rightIndex++;

            // Assign allowence of build next to current
            RightBuild = CanBuildOther;
            if (rightIndex == rightSlots.Count)
            {
                RightBuild = false;
            }
        }
        else
        {
            if (leftIndex == 0)
            {
                leftConnector.SetActive(false);
            }

            leftIndex++;
            LeftBuild = CanBuildOther;
            if (leftIndex == leftSlots.Count)
            {
                LeftBuild = false;
            }
        }
    }

    public void CreateRoomFromData(string id, string dir, string slotIndex, GameObject roomPrefab)
    {
        int index = int.Parse(slotIndex);
        Slot slot = (dir.Equals("R")) ? rightSlots[index] : leftSlots[index];
        GameObject capsole = Instantiate(roomPrefab, slot.transform.position, Quaternion.identity) as GameObject;
        capsole.name = id;
        capsole.transform.SetParent(LevelManager.Instance.Environment.transform);
        capsole.GetComponentInChildren<RoomEntity>().mySlot = slot;

        if (index == 0)
        {
            if (dir.Equals("R"))
            {
                rightConnector.SetActive(false);
            }
            else
            {
                leftConnector.SetActive(false);
            }
        }

        slot.RoomObj = capsole;
        slot.BuildThisSlot(roomPrefab.name);
        LevelManager.Instance.CalculateThisRoomBounds(new Room(capsole));

        int num = PlayerPrefs.GetInt(id + " CharNum");

        PlayerPrefs.SetInt(id + " CharNum",0);

        for (int i = 0; i < num; i++)
        {
            LevelManager.Instance.CreateChar(capsole.GetComponentInChildren<RoomEntity>());
        }
    }
}
