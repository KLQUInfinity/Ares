using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum JobState
{
    Vacant,
    Occupied
}

[System.Serializable]
public class Job
{
    [SerializeField]
    public string jobName;
    [SerializeField]
    public CharacterAnimationsState jobAnimation;
    [SerializeField]
    public AnimationClip jobAnimationClip;
    [HideInInspector]
    public Character jobHolder;
    [SerializeField]
    public JobState jobState;
    [SerializeField]
    public float staminaReductionRate; //Over time
    [SerializeField]
    public GameObject jobRoom;

    public int jobAcquisionHour;
    [SerializeField]
    public GameObject jobPosition;

    public HorizontalDirecton jobFacingDirection;

    [SerializeField]
    //Character job work flow;
    public IJobWorkflow jobWorkflow;

    public Job()
    {
        jobState = JobState.Vacant;
        if (jobRoom)
        {
            foreach (var workflowScript in jobRoom.GetComponentsInChildren<IJobWorkflow>())
            {
                if (workflowScript != null)
                {
                    jobWorkflow = workflowScript;
                }
            }
        }
        /**
         * jobRoom it will be dynamically created and the current gameObject (Capsule) will be a child of it
         * In order to access it we need to access the parent game object;
         * **/
    }

    public void assignJobHolder(Character character)
    {//On room population process
        this.jobHolder = character;
        jobState = JobState.Occupied;
        jobAcquisionHour = GameBrain.Instance.timeManager.gameTime.gameHour;
        //Hold the inital direction of the character just at the first time he will start working;
        //initialHorizontalDirection = character.characterGameObject.GetComponent<CharacterEntity>().characterAnimationFSM.horizontalDirection;
    }

    public void deassignJobHolder()
    {//On room evacuation process
        this.jobHolder = null;
        jobState = JobState.Vacant;
    }

    public GameObject getGetJobPosiiton()
    {
        return jobPosition;
    }
}
