using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterAnimationFSM))]
public class CharacterEntity : MonoBehaviour
{
    public Character character;
    [HideInInspector]
    public CharacterAnimationFSM characterAnimationFSM;
    public CharController characterController;
    [HideInInspector]
    public bool isMovingInnerMovement;
    private float walkingSpeed;


    // Start is called before the first frame update

    void Start()
    {
        if (LevelManager.Instance.Testing)
        {
            //character = new Character(this.gameObject);
        }
        LevelManager.Instance.characterManager.characters.Add(character);
        characterAnimationFSM = GetComponent<CharacterAnimationFSM>();
        characterController = GetComponent<CharController>();
        fillCharacterData();
    }
    private void Update()
    {
        if (true)
        {

        }
    }

    public void fillCharacterData()
    {
        //Filling character data logic goes here.
    }

    public void OnContainerChanged()
    {

    }

    public void OnPathFollowingEnd(SlotDir enteranceDir)
    {
        RoomEntity roomEntity = character.container.GetComponentInChildren<RoomEntity>();
        if (enteranceDir == SlotDir.Left)
        {
            character.containerEntrance = roomEntity.leftEntrance;
        }
        else if (enteranceDir == SlotDir.Right)
        {
            character.containerEntrance = roomEntity.rightEntrance;
        }
        //characterController.TriggerGravity(true);

        character.characterGameObject.transform.position += (Vector3.up * -0.6f);
        followRoomInnerPath(roomEntity,false);

    }

    public void followRoomInnerPath(RoomEntity roomEntity, bool isReversed)
    {
        if (character.job != null)
        {
            JobPathFinder pathFinder = RoomEntity.getJobPathObject(character.job, character, roomEntity.jobPathFinders);
            pathFinder.isReversed = isReversed;
            pathFinder.isFollowingPath = true;
            characterAnimationFSM.changeAnimationStateTo(CharacterAnimationsState.Walking);
            isMovingInnerMovement = true;
            if (LevelManager.Instance.Testing)
            {
                Debug.Log("Right now character should follow: " + pathFinder.thisJobPath.ToString() + " path");
            }
        }
        else {
            characterAnimationFSM.changeAnimationStateTo(CharacterAnimationsState.Idle);
            startJobWorkFlow();
        }
    }
    /// <summary>
    /// Will call workflow classes methods that is build using component pattern
    /// </summary>
    private void startJobWorkFlow()
    {
        if (character.job.jobWorkflow!=null)
        {
            character.job.jobWorkflow.startWorkflow();
        }
    }

    public void OnMovingInOuterPath()
    {
        GameObject Backbone = GameObject.FindGameObjectWithTag("Backbone");

        if (tellIsCharacterInsideBackbone(Backbone))
        {
            characterController.TriggerGravity(false);
            character.container = Backbone;
            characterAnimationFSM.changeAnimationStateTo(CharacterAnimationsState.Floating);
        }
        else
        {
            //Put the character on the ground here :)

            //if (character.container.tag!="BaseRoom" && character.container.tag != "TrainingRoom")
            //{
            //    characterController.TriggerGravity(true);
            //}
            characterAnimationFSM.changeAnimationStateTo(CharacterAnimationsState.Walking);
        }
    }

    public void OnPathPositionReached(bool isRevesed)
    {//Job to entrance or vise versia
        if (!isRevesed)
        {
            characterAnimationFSM.changeAnimationStateTo(character.job.jobAnimation);
            characterAnimationFSM.populateJobAnimationClip(character.job.jobAnimation, 
                character.job.jobAnimationClip);
            //characterController.TriggerGravity(false);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    bool tellIsCharacterInsideBackbone(GameObject Backbone) {
        if (Backbone. GetComponent<Renderer>().bounds
            .Contains(character.characterGameObject.transform.position))
        {
            return true;
        }
        else
        {
            return false;
        }
    }




    #region Deprecated JobPathFindingCode
    //public void placePlayerAtJobPosition()
    //{
    //    if (putCharacterAtJobPos)
    //    {
    //        characterAnimationFSM.changeAnimationStateTo(CharacterAnimationsState.Walking);
    //        if (!isPlayerIsAtJobPosition())
    //        {
    //            /** This is the algorith SUDO code;
    //             * Compare character position to both jobs in the room.
    //             *      if character is closer to his assigned job:
    //             *              Move directly to the jobPosition; there for they are
    //             *              extreemly close on the z-axis.
    //             *      else:
    //             *              Move diagonally towards the jobPositon; there for they
    //             *              are not close on the z-axis
    //             * **/
    //            if (compareCharacterToRoomJobsPositions())
    //            {
    //                moveDirectlyToJobPos();
    //            }
    //            else
    //            {
    //                moveDiagonallyToJobPos();
    //            }
    //        }
    //    }
    //}

    //public void moveDirectlyToJobPos()
    //{
    //    /**This is the algorith SUDO code;
    //     * Move a 2 steps to the direction of the room entring:
    //     *          if came from the left entrance:
    //     *                  the direction will be to the right.
    //     *          else if came from the right entrance:
    //     *                  the direction will be to the left.
    //     * Then move 1 step in towards the job position on the z-axis
    //     * Then move another 2 steps in the other direction.
    //     * Repeat unitl the z-axis and x-axis positions of both became the same.
    //     * **/

    //    moveTowardsOppositeDirectionOfEntrance(2);
    //    moveInZDirectionOfJobPos(1);
    //    moveTowardsEntranceDirection(2);
    //}

    //public void moveDiagonallyToJobPos()
    //{
    //    /**This is the algorith SUDO code;
    //     * Move in the direction of the room entring:
    //     *         if came from the left entrance:
    //     *                  the direction will be to the right.
    //     *          else if came from the right entrance:
    //     *                  the direction will be to the left.
    //     * While moving on the z-axis towards the job position.
    //     * Repeat unitl the z-axis and x-axis positions of both became the same.
    //     **/
    //    moveTowardsOppositeDirectionOfEntrance(1);
    //    moveInZDirectionOfJobPos(1);
    //}

    //void moveTowardsOppositeDirectionOfEntrance(int steps)
    //{
    //    /**
    //     * Calculate the movement direction.
    //     * if the z-axis of the character's position not equal to the z-axis of his job position
    //     *      Move the player with the defined steps in the movement direction.
    //     * else
    //     *      Move the player in the x-axis direction towards his job x-axis position
    //     * Flip the object horizontally according to the horizontal velocity
    //     *      
    //     * **/
    //    //Calculate movemnt direction
    //    Vector3 entranceOppositeDirection =
    //       compareCharacterPosToEntrancesPositions().transform.position -
    //       character.characterGameObject.transform.position;

    //    //Execlude the Z-axis and the Y-Axis from the direction calculation.
    //    entranceOppositeDirection = entranceOppositeDirection.x * Vector3.right;

    //    //Get the direction movement without the magnitude
    //    entranceOppositeDirection.Normalize();

    //    if (character.characterGameObject.transform.position.z <
    //        character.job.jobPosition.transform.position.z)
    //    {
    //        //Move the character with the defined steps.
    //        for (int i = 0; i < steps; i++)
    //        {
    //            characterRidgidBody.MovePosition(entranceOppositeDirection * walkingSpeed * Time.fixedDeltaTime);
    //        }
    //    }
    //    else
    //    {
    //        characterRidgidBody.MovePosition(
    //           calculateXDirectonToJobPos()
    //        * walkingSpeed * Time.fixedDeltaTime);

    //    }
    //    //Flip the character to face the movement direction
    //    if (characterRidgidBody.velocity.x > 0)
    //    {
    //        characterAnimationFSM.flipToRight();
    //    }
    //    else if (characterRidgidBody.velocity.x < 0)
    //    {
    //        characterAnimationFSM.flipToLeft();
    //    }
    //}

    //void moveInZDirectionOfJobPos(int steps)
    //{
    //    /**
    //     * Move the player.
    //     * **/
    //    if (character.characterGameObject.transform.position.z <
    //            character.job.jobPosition.transform.position.z)
    //    {
    //        for (int i = 0; i < steps; i++)
    //        {
    //            characterRidgidBody.MovePosition(Vector3.forward * walkingSpeed * Time.fixedDeltaTime);
    //        }
    //    }
    //}

    //void moveTowardsEntranceDirection(int steps)
    //{
    //    /**
    //    * Calculate the movement direction.
    //    * if the z-axis of the character's position not equal to the z-axis of his job position
    //    *      Move the player with the defined steps in the movement direction.
    //    * else
    //    *      Move the player in the x-axis direction towards his job x-axis position
    //    * Flip the object horizontally according to the horizontal velocity
    //    *      
    //    * **/
    //    //Calculate movemnt direction
    //    Vector3 entranceOppositeDirection =
    //       compareCharacterPosToEntrancesPositions().transform.position -
    //       character.characterGameObject.transform.position;

    //    //Execlude the Z-axis and the Y-Axis from the direction calculation.
    //    entranceOppositeDirection = entranceOppositeDirection.x * Vector3.left;

    //    //Get the direction movement without the magnitude
    //    entranceOppositeDirection.Normalize();

    //    if (character.characterGameObject.transform.position.z <
    //        character.job.jobPosition.transform.position.z)
    //    {
    //        //Move the character with the defined steps.
    //        for (int i = 0; i < steps; i++)
    //        {
    //            characterRidgidBody.MovePosition(entranceOppositeDirection * walkingSpeed * Time.fixedDeltaTime);
    //        }
    //    }
    //    else
    //    {
    //        characterRidgidBody.MovePosition(
    //           calculateXDirectonToJobPos()
    //        * walkingSpeed * Time.fixedDeltaTime);

    //    }
    //    //Flip the character to face the movement direction
    //    if (characterRidgidBody.velocity.x > 0)
    //    {
    //        characterAnimationFSM.flipToRight();
    //    }
    //    else if (characterRidgidBody.velocity.x < 0)
    //    {
    //        characterAnimationFSM.flipToLeft();

    //    }
    //}

    ///// <summary>
    ///// This method compares the position of the character an his job position 
    ///// to indicate whether or not he has at the job position. 
    ///// This method assures that the player need to move until he reaches his job
    ///// position inside the room.
    ///// Y-axis positons of both are neglectable hence the job position is not
    ///// necessarily have a y-axis exact position inside the room.
    ///// This was just the concept surrely there are some tweeking need to be done
    ///// instead of cheacking whether the character is at the job position
    ///// we need to consider an area if he reaches it or not. (Test Subject)
    ///// </summary>
    ///// <param name="characterPos"></param>
    ///// <param name="jobPos"></param>
    ///// <returns></returns>
    //public bool isPlayerIsAtJobPosition()
    //{//Needs tweeking
    //    //if (character.characterGameObject.transform.position.x != character.job.jobPosition.transform.position.x)
    //    //{
    //    //    return false;
    //    //}
    //    //if (character.characterGameObject.transform.position.z != character.job.jobPosition.transform.position.z)
    //    //{
    //    //    return false;
    //    //}
    //    return true;
    //}

    //public bool compareCharacterToRoomJobsPositions()
    //{
    //    float distanceToCharacterJobPos = 0;
    //    float distanceToOtherJobPos = 0;
    //    try
    //    {
    //        distanceToCharacterJobPos = subtractLinearXZPositions(
    //                character.characterGameObject.transform.position,
    //                character.job.jobPosition.transform.position);
    //        distanceToOtherJobPos = subtractLinearXZPositions(
    //            character.characterGameObject.transform.position,
    //            LevelManager.Instance.roomManager.getOtherJobInTheRoom(character.job).
    //            jobPosition.transform.position);
    //    }
    //    catch (System.Exception e)
    //    {
    //        if (LevelManager.Instance.Testing)
    //        {
    //            Debug.Log(e.Message);
    //        }
    //    }

    //    if (distanceToCharacterJobPos > distanceToOtherJobPos)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    //public GameObject compareCharacterPosToEntrancesPositions()
    //{
    //    float distanceToLeftEntrance = 0;
    //    float distanceToRightEntrance = 0;
    //    GameObject leftEntrance = null;
    //    GameObject rightEntrance = null;

    //    try
    //    {
    //        LevelManager.Instance.roomManager.getRoomEntrances(character.container, out leftEntrance, out rightEntrance);
    //        distanceToLeftEntrance = subtractLinearXPositions(
    //            character.characterGameObject.transform.position,
    //            leftEntrance.transform.position
    //            );
    //        distanceToLeftEntrance = subtractLinearXPositions(
    //            character.characterGameObject.transform.position,
    //            rightEntrance.transform.position
    //            );
    //    }
    //    catch (System.Exception e)
    //    {
    //        if (LevelManager.Instance.Testing)
    //        {
    //            Debug.Log(e.Message);
    //        }
    //    }
    //    if (leftEntrance && rightEntrance)
    //    {
    //        if (distanceToLeftEntrance > distanceToRightEntrance)
    //        {
    //            return leftEntrance;
    //        }
    //        else
    //        {
    //            return rightEntrance;
    //        }
    //    }
    //    else
    //    {
    //        return null;
    //    }

    //}

    //public float subtractLinearXZPositions(Vector3 position1, Vector3 position2)
    //{
    //    Vector3 difference = position2 - position1;
    //    difference.y = 0;
    //    return difference.magnitude;
    //}

    //public float subtractLinearXPositions(Vector3 position1, Vector3 position2)
    //{
    //    Vector3 difference = position2 - position1;
    //    difference.z = 0;
    //    difference.y = 0;
    //    return difference.magnitude;
    //}

    //public Vector3 calculateXDirectonToJobPos()
    //{
    //    Vector3 direction = (character.job.jobPosition.transform.position - character.characterGameObject.transform.position);
    //    direction = Vector3.right * direction.x;
    //    return direction.normalized;
    //}
    #endregion
}
