using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Slot : MonoBehaviour
{
    [HideInInspector] public SlotManager MySlotManger;
    [HideInInspector] public SpriteRenderer SlotHighlight;
    [HideInInspector] public Collider MyCollider;
    [HideInInspector] public GameObject RoomObj;

    public SlotDir MyDir;

    void Awake()
    {
        SlotHighlight = GetComponent<SpriteRenderer>();
        MyCollider = GetComponent<Collider>();
    }

    /// <summary>
    /// Build the current slot and prepare the next of her for build if allowed
    /// </summary>
    /// <param name="buildType">Determain next build allowence</param>
    public void BuildThisSlot(string buildType)
    {
        if (buildType.Contains("GrapperRoom"))
        {
            MySlotManger.PrepareNextSlot(MyDir, false);
        }
        else
        {
            MySlotManger.PrepareNextSlot(MyDir);
        }
    }
}

/// <summary>
/// Slot Direction Right or Left
/// </summary>
public enum SlotDir
{
    Right,
    Left
}