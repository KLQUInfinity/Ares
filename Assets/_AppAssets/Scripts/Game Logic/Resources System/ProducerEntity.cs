using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ResourceToProduce))]
public class ProducerEntity : MonoBehaviour, IProducer
{
    ResourceProducer resourceProducer;
    ResourceToProduce[] resourcesToProduce;
    // Start is called before the first frame update
    void Start()
    {
        resourceProducer = new ResourceProducer(this.gameObject);
        resourcesToProduce = this.GetComponents<ResourceToProduce>();
        //if (GameBrain.Instance.testing)
        //{
        //    foreach (var item in resourcesToConsume)
        //    {
        //        Debug.Log("Entity is using "+ item.resourceType.ToString()+" with production rate of "+ item.consumptionRatePerSecond);
        //    }
        //}
        foreach (var resource in resourcesToProduce)
        {

            appendProducingResources(getResource(resource.resourceType), resource.productionRatePerSecond);
        }
        GameBrain.Instance.resourcesManager.producers.Add(resourceProducer);
        LevelManager.Instance.roomManager.InitializeRoomsResources(LevelManager.Instance.roomManager.getRoomWithGameObject(transform.parent.gameObject));
    }


    public Resource getResource(ResourceType resourceType)
    {
        return GameBrain.Instance.resourcesManager.getResource(resourceType);
    }
    public ResourceProducer getResourceProducer()
    {
        return resourceProducer;
    }

    public Resource getfirstResource()
    {
        return getResource(resourcesToProduce[0].resourceType);
    }
    public float getFirstResourceProductionRate()
    {
        return resourcesToProduce[0].productionRatePerSecond; ;
    }
    public void appendProducingResources(Resource resource, float productionRate)
    {
        if (resource!=null)
        {
            resourceProducer.addResource(resource, productionRate);
        }
    }
}
