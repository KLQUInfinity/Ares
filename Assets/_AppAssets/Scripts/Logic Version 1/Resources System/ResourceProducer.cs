using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ResourceToProduce))]
public class ResourceProducer
{
    public GameObject ProducerGameObject;
    public Dictionary<Resource, float> resourcesProductionRates = new Dictionary<Resource, float>();
    public ResourceProducer(GameObject ProducerGameObject)
    {
        this.ProducerGameObject = ProducerGameObject;
    }
    public void addResource(Resource resource, float productionRate)
    {
        resourcesProductionRates.Add(resource, productionRate);
    }
}
