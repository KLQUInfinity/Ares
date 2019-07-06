
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ResourceToConsume))]
public class ResourceConsumer
{
    public GameObject consumerGameObject;
    public Dictionary<Resource, float> resourcesConsumptionRates = new Dictionary<Resource, float>();
    public ResourceConsumer(GameObject consumerGameObject)
    {
        this.consumerGameObject = consumerGameObject;
    }
    public void addResource(Resource resource,float consumptionRate)
    {
        resourcesConsumptionRates.Add(resource, consumptionRate);
    }
}
