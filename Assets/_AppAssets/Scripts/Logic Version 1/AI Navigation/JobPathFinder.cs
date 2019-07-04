using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JobPath
{
    leftEntranceToJob1,
    leftEntranceToJob2,
    rightEntranceToJob1,
    rightEntranceToJob2
}
public class JobPathFinder : MonoBehaviour
{
    public JobPath thisJobPath;
    public JobEntity jobEntity;
    public Job job;
    public bool isReversed;
    public GameObject entrancePos;
    public GameObject jobPos;
    public GameObject entrancePosNodeMirror;
    public GameObject jobNodePosMirror;
    public bool isFollowingPath;
    /// <summary>
    /// The character must have his job assigned to be used to reach its position, but the job animation will not start 
    /// until the character reaches its position.
    /// The character must hold the job until he reach the enterance on his way out of the room to be used
    /// in finding the path, but the job animation has to be stoped at the moment he will start to leave its position
    /// 
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowingPath)
        {
            followPath();
        }
    }
    private void OnDrawGizmos()
    {
        Init();
    }
    void Init()
    {
        jobEntity = GetComponentInParent<JobEntity>();
        entrancePosNodeMirror.transform.position = entrancePos.transform.position+(Vector3.up*-1.2f);
        jobNodePosMirror.transform.position = jobPos.transform.position+(Vector3.up*-1.2f);
        InitializePathsJobs();
    }


    public void followPath()
    {

        switch (thisJobPath)
        {
            case JobPath.leftEntranceToJob1:
            case JobPath.rightEntranceToJob2:
                followDiagonalPath();
                break;
            case JobPath.leftEntranceToJob2:
            case JobPath.rightEntranceToJob1:
                followDirectPath();
                break;
        }
    }

    /// <summary>
    /// When the isReversed is true this mean that the player is on his way out of the room.
    /// when it is false the player is will be on his to the job position.
    /// </summary>
    /// <param name="isReversed"></param>
    public void followDirectPath()
    {
        if (!isReversed)
        {
            moveSimply();
        }
    }

    public void followDiagonalPath()
    {
        if (!isReversed)
        {
            moveSimply();
        }
    }
    public void InitializePathsJobs()
    {
        switch (thisJobPath)
        {
            case JobPath.leftEntranceToJob1:
            case JobPath.rightEntranceToJob1:
                if (jobEntity)
                {
                    job = jobEntity.roomJobs[0];
                }
                break;
            case JobPath.leftEntranceToJob2:
            case JobPath.rightEntranceToJob2:
                if (jobEntity)
                {
                    job = jobEntity.roomJobs[1];
                }
                break;
        }
    }

    void moveSimply()
    {
        if (job.jobHolder.characterGameObject.transform.position!= jobPos.transform.position+ (Vector3.up * -0.6f))
        {
            job.jobHolder.characterGameObject.transform.position = Vector3.MoveTowards(job.jobHolder.characterGameObject.transform.position
            ,jobPos.transform.position + (Vector3.up * -0.6f), job.jobHolder.characterGameObject.GetComponent<CharController>().getMovementSpeed()/2* Time.deltaTime);
        }
        else
        {
            job.jobHolder.characterGameObject.GetComponent<CharacterEntity>().OnPathPositionReached(isReversed);
            isFollowingPath = false;
        }
    }

}
