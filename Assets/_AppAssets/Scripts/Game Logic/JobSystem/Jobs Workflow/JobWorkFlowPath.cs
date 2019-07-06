using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobWorkFlowPath : MonoBehaviour
{
    public IJobWorkflow jobWorkflow;
    public List<Transform> jobWorkflowPath;
    public Queue<Transform> workflowPathQueue = new Queue<Transform>();
    public Stack<Transform> workflowPathStack = new Stack<Transform>();
    public bool isJobLoadDelivered;
    public float shiftPositionOnX=2;
    public float shiftPositionOnZ=1;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        initializeWorkflowPathQueue();
    }

    private void Init()
    {
        jobWorkflow = GetComponentInParent<IJobWorkflow>();
        jobWorkflowPath[0].position = calculateJobPosition();
    }

    private Vector3 calculateJobPosition()
    {
        return jobWorkflow.getWorkFlowJob().jobPosition.transform.position + (Vector3.up * -1.2f);
    }

    // Update is called once per frame
    void Update()
    {

        if (jobWorkflow.getWorkflowState() == JobWorkflowSate.pause)
        {

            if (!isJobLoadDelivered)
            {
                //if (jobWorkflow.getJobLoadState()!=null)
                {
                    if (jobWorkflow.getJobLoadState() == JobLoadState.carried)
                    {
                        followJobWorkflowPath(jobWorkflow.getWorkFlowJob().jobHolder);
                    }
                }

            }
            else
            {
                if (jobWorkflow.getJobLoadState() != null)
                {
                    jobWorkflow.setLoadPosition(jobWorkflowPath[jobWorkflowPath.Count - 1].position+(Vector3.right* shiftPositionOnX) +(Vector3.forward* shiftPositionOnZ));
                    jobWorkflow.setJobLoadState(JobLoadState.delivered);
                    StartCoroutine("followWorkflowPathBack");
                }
            }

        }
        else if (jobWorkflow.getWorkflowState() == JobWorkflowSate.start)
        {
            if (jobWorkflow.getJobLoadState() == null && isJobLoadDelivered)
            {
                followJobWorkflowPathBack(jobWorkflow.getWorkFlowJob().jobHolder);
            }
        }
    }
    public void initializeWorkflowPathQueue()
    {

        foreach (var node in jobWorkflowPath)
        {
            if (isJobLoadDelivered)
            {
                workflowPathStack.Push(node);
            }
            else
            {
                workflowPathQueue.Enqueue(node);
            }
        }

    }

    public void followJobWorkflowPath(Character character)
    {

        if (workflowPathQueue.Count > 0)
        {
            if (character.characterGameObject.transform.position != workflowPathQueue.Peek().position + (Vector3.up * 0.6f))
            {
                character.characterGameObject.transform.position = Vector3.MoveTowards(character.characterGameObject.transform.position
                , workflowPathQueue.Peek().position + (Vector3.up * 0.6f), character.characterGameObject.GetComponent<CharController>().getMovementSpeed() / 2 * Time.deltaTime);
                changeAnimationRespectivily(character, CharacterAnimationsState.Walking);
            }
            else
            {
                workflowPathQueue.Dequeue();
            }
        }
        else
        {

            isJobLoadDelivered = !isJobLoadDelivered;
            //changeAnimationRespectivily(character, jobWorkflow.getWorkFlowJob().jobAnimation);
            initializeWorkflowPathQueue();

        }
    }

    public void followJobWorkflowPathBack(Character character)
    {

        if (workflowPathStack.Count > 0)
        {
            if (character.characterGameObject.transform.position != workflowPathStack.Peek().position + (Vector3.up * 0.6f))
            {
                character.characterGameObject.transform.position = Vector3.MoveTowards(character.characterGameObject.transform.position
                , workflowPathStack.Peek().position + (Vector3.up * 0.6f), character.characterGameObject.GetComponent<CharController>().getMovementSpeed() / 2 * Time.deltaTime);
                changeAnimationRespectivily(character, CharacterAnimationsState.Walking);
            }
            else
            {
                workflowPathStack.Pop();
            }
        }
        else
        {

            isJobLoadDelivered = !isJobLoadDelivered;
            jobWorkflow.setIsCharacterAtInitialWorkflowPos(true);
            //changeAnimationRespectivily(character, jobWorkflow.getWorkFlowJob().jobAnimation);
            
            character.characterGameObject.GetComponent<CharacterEntity>().updateJobInitialDirection();
            initializeWorkflowPathQueue();


        }
    }

    public void changeAnimationRespectivily(Character character, CharacterAnimationsState animationState)
    {

        if (jobWorkflow.getWorkFlowJob().jobState == JobState.Occupied)
        {
            character.characterGameObject.GetComponent<CharacterEntity>().characterAnimationFSM.changeAnimationStateTo(animationState);
        }
    }
    IEnumerator followWorkflowPathBack()
    {
        yield return new WaitForSeconds(0.5f);
        followJobWorkflowPathBack(jobWorkflow.getWorkFlowJob().jobHolder);
    }
}
