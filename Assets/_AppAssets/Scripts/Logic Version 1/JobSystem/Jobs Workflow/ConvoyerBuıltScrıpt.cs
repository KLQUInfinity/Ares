using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JobWorkflowSate
{
    start,
    pause,
    update,
    finish
}
public class ConvoyerBuıltScrıpt : MonoBehaviour, Workflow
{
    public GameObject spawnPoint;
    public GameObject deadPoint;
    public GameObject boxPrefab;
    public Queue<ResourceBox> boxesQueue = new Queue<ResourceBox>();
    Job conveyorBeltJob;
    public JobWorkflowSate currentWorkflowState;
    private void Start()
    {
        currentWorkflowState = JobWorkflowSate.finish;
        List<Job> roomJobs = LevelManager.Instance.roomManager.getRoomWithGameObject(transform.parent.gameObject).roomJobs;
        foreach (var job in roomJobs)
        {
            if (job.jobName == "ConveyorBelt")
            {
                conveyorBeltJob = job;
            }
        }

    }


    private void Update()
    {
        switch (currentWorkflowState)
        {
            case JobWorkflowSate.start:
                 updateWorkflow();
                break;
            case JobWorkflowSate.pause:
                break;
            case JobWorkflowSate.update:
                updateWorkflow();
                break;
            case JobWorkflowSate.finish:
                break;
        }
    }

    public void startWorkflow()
    {
        currentWorkflowState = JobWorkflowSate.start;
        if (boxesQueue.Count == 0)
        {
            //xboxesQueue.Enqueue(Instantiate(boxPrefab, spawnPoint.transform.position, Quaternion.identity, null));
        }
    }

    public void pauseWorkflow()
    {
        currentWorkflowState = JobWorkflowSate.pause;

    }

    public void updateWorkflow()
    {
        currentWorkflowState = JobWorkflowSate.update;
        if (boxesQueue.Count==0)
        {
            //boxesQueue.Enqueue(Instantiate(boxPrefab, spawnPoint.transform.position, Quaternion.identity, null));
        }
        if (boxesQueue.Peek().transform.position!= deadPoint.transform.position)
        {
            boxesQueue.Peek().transform.position = Vector3.MoveTowards(boxesQueue.Peek().transform.position, deadPoint.transform.position
             , 0.1f * Time.deltaTime);
        }else
        {
            //boxesQueue.Dequeue().SetActive(false);
            pauseWorkflow();
        }

    }

    public void finishWorkflow()
    {
        currentWorkflowState = JobWorkflowSate.finish;
    }

}
enum boxStae {
    created,
    moving,
    hidden,
    delivered
}
public class ResourceBox :MonoBehaviour{
    public GameObject boxGameObject;
    public ResourceBox(GameObject boxGameObject)
    {
        this.boxGameObject = boxGameObject;
    }
    public ResourceBox instantiateBox(GameObject boxPrefab,Vector3 spawnPosition) {
       return new ResourceBox( Instantiate(boxPrefab, spawnPosition, Quaternion.identity, null));
    }
}
