
public interface IConsumer  
{
    void appendConsumingResources(Resource resource, float consumptionRate);
    Resource getResource(ResourceType resourceType);
}
