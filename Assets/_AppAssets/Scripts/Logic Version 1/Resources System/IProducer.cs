using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProducer 
{
    void appendProducingResources(Resource resource, float productionRate);
    Resource getResource(ResourceType resourceType);
}
