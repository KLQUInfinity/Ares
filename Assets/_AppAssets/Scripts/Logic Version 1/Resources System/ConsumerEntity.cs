using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof(ResourceToConsume))]
public class ConsumerEntity : MonoBehaviour,IConsumer
{
    ResourceConsumer resourceConsumer;
    // Start is called before the first frame update
    void Start()
    {
        resourceConsumer = new ResourceConsumer(this.gameObject);
        ResourceToConsume[] resourcesToProduce = this.GetComponents<ResourceToConsume>();
        //if (GameBrain.Instance.testing)
        //{
        //    foreach (var item in resourcesToConsume)
        //    {
        //        Debug.Log("Character is using "+ item.resourceType.ToString()+" with consumption rate of "+ item.consumptionRatePerSecond);

        //    }
        //}
        foreach (var resource in resourcesToProduce)
        {
            appendConsumingResources(getResource(resource.resourceType), resource.consumptionRatePerSecond);
        }
    }

    public void appendConsumingResources(Resource resource,float consumptionRate)
    {
        resourceConsumer.addResource(resource, consumptionRate);
        GameBrain.Instance.resourcesManager.consumers.Add(resourceConsumer);
    }

    public Resource getResource(ResourceType resourceType)
    {
       return GameBrain.Instance.resourcesManager.getResource(resourceType);
    }

}
