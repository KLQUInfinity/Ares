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
        currentWorkflowState = JobWorkflowSate.start;
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
        if (conveyorBeltJob.jobState == JobState.Occupied)
        {

            switch (currentWorkflowState)
            {
                case JobWorkflowSate.start:
                    startWorkflow();
                    break;
                case JobWorkflowSate.pause:
                    OnPause();
                    break;
                case JobWorkflowSate.update:
                    updateWorkflow();
                    break;
                case JobWorkflowSate.finish:
                    OnFinish();
                    break;
            }
        }

    }

    public void startWorkflow()
    {
        currentWorkflowState = JobWorkflowSate.start;
        OnStart();
    }

    public void pauseWorkflow()
    {
        currentWorkflowState = JobWorkflowSate.pause;
        boxesQueue.Peek().boxState = BoxStae.hidden;
        boxesQueue.Peek().gameObject.SetActive(false);
    }

    public void OnStart()
    {
        boxesQueue.Enqueue(instantiateResourceBox());
        updateWorkflow();
    }
    public void OnPause()
    {

    }
    public void OnFinish()
    {
        if (boxesQueue.Count == 1)
        {
            if (boxesQueue.Peek().boxState == BoxStae.delivered)
            {
                boxesQueue.Dequeue().gameObject.SetActive(true);
                startWorkflow();
            }
        }
    }
    public void updateWorkflow()
    {
        currentWorkflowState = JobWorkflowSate.update;

        //boxesQueue.Peek().boxGameObject.transform.position = Vector3.MoveTowards(boxesQueue.Peek().boxGameObject.transform.position, deadPoint.transform.position
        // , 0.01f*Time.deltaTime);
        boxesQueue.Peek().GetComponent<Rigidbody>().velocity=(-Vector3.forward*2);
        boxesQueue.Peek().boxState = BoxStae.moving;

    }

    public void finishWorkflow()
    {
        currentWorkflowState = JobWorkflowSate.finish;
        boxesQueue.Peek().boxState = BoxStae.delivered;
    }

    public ResourceBox instantiateResourceBox()
    {
        GameObject box = Instantiate(boxPrefab, spawnPoint.transform.position, Quaternion.identity, null);
        box.transform.parent = transform;
        box.AddComponent<ResourceBox>();
        return box.GetComponent<ResourceBox>();
    }

}
public enum BoxStae
{
    created,
    moving,
    hidden,
    delivered
}
public class ResourceBox : MonoBehaviour
{
    public BoxStae boxState;
    public ResourceBox()
    {
        boxState = BoxStae.created;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Character")
        {
            transform.parent.gameObject.GetComponent<ConvoyerBuıltScrıpt>().pauseWorkflow();
        }
        else if (collision.collider.tag == "ConveyorBoxDestination")
        {
            transform.parent.gameObject.GetComponent<ConvoyerBuıltScrıpt>().finishWorkflow();
        }
    }
}
