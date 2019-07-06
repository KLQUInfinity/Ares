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
public class ConvoyerBuiltScript : MonoBehaviour, IJobWorkflow
{
    #region ConvoyerBuiltScript attributes
    public GameObject spawnPoint;
    public GameObject boxPrefab;
    public Queue<ResourceBox> boxesQueue = new Queue<ResourceBox>();
    Job conveyorBeltJob;
    public JobWorkflowSate currentWorkflowState;
    public BoxWarehousing boxesWarehousingManager;

    [HideInInspector]
    public bool isCharacterAtInitialWorkflowPos;
    public float boxSpeedOnConveyorBeltOnZ = 2;
    public bool isConveyorBuildWorking;

    //------------------------------------------
    bool isBoxCarried;
    bool isAnimationClipSwaped;
    public AnimationClip otherAnimationClip;
    public CharacterAnimationsState waitingAnimationState;
    public AnimationClip jobAnimationClipHolder;
    #endregion

    private void Start()
    {
        currentWorkflowState = JobWorkflowSate.start;
        boxesWarehousingManager = GetComponent<BoxWarehousing>();
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

            if (boxesQueue.Count > 0)
            {
                if (!isAnimationClipSwaped)
                {
                    changeAnimationToWalk();
                }
                switch (boxesQueue.Peek().boxState)
                {
                    case JobLoadState.created:
                        changeAnimationToIdle();
                        break;
                    case JobLoadState.moving:
                        if (!isBoxCarried && currentWorkflowState != JobWorkflowSate.pause)
                        {
                            changeAnimationToIdle();
                        }
                        break;
                    case JobLoadState.carried:
                        break;
                    case JobLoadState.delivered:
                        break;
                }
            }
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
        else
        {

            if (boxesQueue.Count != 0)
            {
                currentWorkflowState = JobWorkflowSate.start;
                Destroy(boxesQueue.Peek().gameObject);
                boxesQueue = new Queue<ResourceBox>();
                isAnimationClipSwaped = false;
                isBoxCarried = false;
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
        if (boxesQueue.Count != 0)
        {
            currentWorkflowState = JobWorkflowSate.pause;
            if (!isBoxCarried)
            {
                doCarryingAnimation();
            }
        }

    }

    public void doCarryingAnimation()
    {

        //conveyorBeltJob.jobAnimationClip = jobAnimationClipHolder;
        //updateCharacterAnimator();
        changeAnimationToJob();
        StartCoroutine("continueWorkflow");

    }

    private void changeAnimationToWalk()
    {
        conveyorBeltJob.jobHolder.characterGameObject.GetComponent<CharacterEntity>().characterAnimationFSM.changeAnimationStateTo(CharacterAnimationsState.Walking);
    }
    private void changeAnimationToIdle()
    {
        conveyorBeltJob.jobHolder.characterGameObject.GetComponent<CharacterEntity>().characterAnimationFSM.changeAnimationStateTo(waitingAnimationState);
    }
    private void changeAnimationToJob()
    {
        conveyorBeltJob.jobHolder.characterGameObject.GetComponent<CharacterEntity>().characterAnimationFSM.changeAnimationStateTo(conveyorBeltJob.jobAnimation);
    }
    private void updateCharacterAnimator()
    {
        conveyorBeltJob.jobHolder.characterGameObject.GetComponent<CharacterEntity>().characterAnimationFSM.resetOriginalAnimationClips();
        conveyorBeltJob.jobHolder.characterGameObject.GetComponent<CharacterEntity>().characterAnimationFSM.populateJobAnimationClip(conveyorBeltJob.jobHolder.job.jobAnimation, conveyorBeltJob.jobAnimationClip);
    }

    IEnumerator continueWorkflow()
    {
        yield return new WaitForSeconds(0.8f);
        isBoxCarried = true;
    }
    public void OnStart()
    {
        ResourceBox aBox = instantiateResourceBox();
        if (aBox != null)
        {
            changeAnimationToIdle();
            isAnimationClipSwaped = true;
            boxesQueue.Enqueue(aBox);
            updateWorkflow();
        }
    }
    public void OnPause()
    {
        if (isBoxCarried)
        {
            boxesQueue.Peek().boxState = JobLoadState.carried;
            boxesQueue.Peek().gameObject.SetActive(false); // I guess it should be removed
            isBoxCarried = false;
        }
    }
    public void OnFinish()
    {
        if (boxesQueue.Count == 1)
        {
            if (boxesQueue.Peek().boxState == JobLoadState.delivered)
            {
                boxesWarehousingManager.appendBox(boxesQueue.Peek());
                boxesQueue.Dequeue().gameObject.SetActive(true);
                startWorkflow();
            }
        }
    }
    public void updateWorkflow()
    {
        currentWorkflowState = JobWorkflowSate.update;
        if (isConveyorBuildWorking)
        {
            //if (boxesQueue.Peek().boxState != JobLoadState.moving)
            {
                Vector3 boxVelocity = boxesQueue.Peek().GetComponent<Rigidbody>().velocity;
                boxVelocity.z = (-Vector3.forward * boxSpeedOnConveyorBeltOnZ).z;
                boxesQueue.Peek().GetComponent<Rigidbody>().velocity = boxVelocity;
            }
        }
        else
        {
            boxesQueue.Peek().GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        boxesQueue.Peek().boxState = JobLoadState.moving;

    }

    public void finishWorkflow()
    {
        currentWorkflowState = JobWorkflowSate.finish;
    }

    public ResourceBox instantiateResourceBox()
    {
        if (Vector3.Distance(conveyorBeltJob.jobHolder.characterGameObject.transform.position,
            calculateJobPosition()) == 0 || isCharacterAtInitialWorkflowPos)
        {
            GameObject box = Instantiate(boxPrefab, spawnPoint.transform.position, Quaternion.identity, null);
            box.transform.parent = transform;
            box.AddComponent<ResourceBox>();
            isCharacterAtInitialWorkflowPos = false;
            return box.GetComponent<ResourceBox>();
        }
        else return null;

    }

    private Vector3 calculateJobPosition()
    {
        return conveyorBeltJob.jobPosition.transform.position + (Vector3.up * -0.6f);
    }

    public Job getWorkFlowJob()
    {
        return conveyorBeltJob;
    }

    public JobLoadState? getJobLoadState()
    {
        if (boxesQueue.Count != 0)
        {
            return boxesQueue.Peek().boxState;
        }
        return null;
    }

    public void setJobLoadState(JobLoadState loadState)
    {
        boxesQueue.Peek().boxState = loadState;
        if (loadState == JobLoadState.delivered)
        {
            finishWorkflow();
        }
    }

    public void setLoadPosition(Vector3 DestinationPosition)
    {
        boxesQueue.Peek().transform.position = DestinationPosition;
    }

    public JobWorkflowSate getWorkflowState()
    {
        return this.currentWorkflowState;
    }

    public void setIsCharacterAtInitialWorkflowPos(bool value)
    {
        isCharacterAtInitialWorkflowPos = value;
    }
}
public enum JobLoadState
{
    created,
    moving,
    carried,
    delivered
}
public class ResourceBox : MonoBehaviour
{
    public JobLoadState boxState;
    public ResourceBox()
    {
        boxState = JobLoadState.created;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.name == "deadPoint")
        {
            transform.gameObject.transform.position += -Vector3.up * 0.2f;
            transform.parent.gameObject.GetComponent<ConvoyerBuiltScript>().pauseWorkflow();
        }
    }

}
