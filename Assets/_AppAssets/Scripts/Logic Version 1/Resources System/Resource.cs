using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Resource 
{
    [SerializeField]
    public ResourceType resourceType;
    [SerializeField]
    public float valueInPercentage;

    public float load;
    public float totalConsumptionRate;
}

