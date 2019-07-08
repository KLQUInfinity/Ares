using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    #region Movement
    [Header("Movement")]
    [Range(-1, 1)]
    public int MoveDir;
    public bool IsRun;

    [SerializeField] private float speed;

    private bool facingRight = true;
    #endregion

    #region Jump
    [Header("Jump")]
    public bool IsJump;

    [SerializeField] private float jumpPower;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.02f;

    private bool isGrounded;
    #endregion

    #region Gravity
    [Header("Gravity")]
    public bool UseGravity = true;
    [Range(-1, 1)] public int UpDownGravityMove;
    #endregion

    #region FollowingPath
    public int currBBSlotIndex, currSlotIndex;
    public SlotDir currSlotDir;

    [SerializeField] private List<Vector3> followingPath = new List<Vector3>();
    #endregion

    [HideInInspector] public RuntimeAnimatorController myAnimController;

    private Rigidbody myRB;
    private bool isMovePath;
    [SerializeField] private int pathIndex;
    private Vector2 curPos;
    private CharacterAnimationFSM cAnimFSM;
    private SlotDir entranceDir;

    private void Start()
    {
        myRB = GetComponent<Rigidbody>();
        curPos = transform.position;
        cAnimFSM = GetComponent<CharacterAnimationFSM>();
        myAnimController = GetComponent<Animator>().runtimeAnimatorController;
    }

    private void Update()
    {
        if (isMovePath)
        {
            GetComponent<CharacterEntity>().OnMovingInOuterPath();

            if (transform.position == followingPath[pathIndex])
            {
                pathIndex++;
                if (pathIndex == followingPath.Count)
                {
                    isMovePath = false;
                    GetComponent<Dragable_Item>().containerRoom = LevelManager.Instance.roomManager.populateAndGetContainerRoom(this.gameObject);
                    if (!GetComponent<Dragable_Item>().containerRoom.name.Equals("TrainningRoom"))
                    {
                        GetComponent<Dragable_Item>().containerRoom.GetComponentInChildren<RoomEntity>().AddCharCountToRoom();
                    }
                    GetComponent<CharacterEntity>().OnPathFollowingEnd(entranceDir);
                    return;
                }
                curPos = followingPath[pathIndex - 1];
                //Flip();
            }

            transform.position = Vector3.MoveTowards(transform.position, followingPath[pathIndex], speed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        //Move();
        //Jump();
        //TriggerGravity();
    }

    private void Move()
    {
        float move = MoveDir;
        move = (IsRun) ? move : move / 2;
        myRB.velocity = new Vector2(move * speed * Time.deltaTime, myRB.velocity.y);

        if (!facingRight && move > 0)
        {
            Flip();
        }
        else if (facingRight && move < 0)
        {
            Flip();
        }
    }

    private void Jump()
    {
        isGrounded = (Physics.OverlapSphere(groundCheck.position, groundCheckRadius, groundLayer).Length > 0) ? true : false;

        if (isGrounded && IsJump)
        {
            isGrounded = false;
            IsJump = false;
            myRB.velocity = new Vector2(myRB.velocity.x, jumpPower);
        }

    }

    private void Flip()
    {
        if (followingPath[pathIndex].x > curPos.x)
        {
            facingRight = true;
            cAnimFSM.flipToRight();
        }
        else if (followingPath[pathIndex].x < curPos.x)
        {
            facingRight = false;
            cAnimFSM.flipToLeft();
        }

        Vector3 scaler = transform.localScale;
        scaler.x = (facingRight) ? Mathf.Abs(scaler.x) : Mathf.Abs(scaler.x) * -1;
        transform.localScale = scaler;
    }

    public void TriggerGravity(bool isGravity)
    {
        UseGravity = isGravity;
        myRB.useGravity = UseGravity;
        myRB.isKinematic = !isGravity;

        //if (!UseGravity)
        //{
        //    myRB.velocity = new Vector2(myRB.velocity.x, UpDownGravityMove * speed * Time.deltaTime);
        //}
    }

    public void GenerateFollowPathWayPoins(int nextBBSlotIndex, SlotDir nextSlotDir, int nextSlotIndex, RoomEntity roomEntity)
    {
        followingPath.Clear();
        if (currBBSlotIndex == nextBBSlotIndex)
        {//If on the same vertical level
            followingPath.Add(GetSingleWayPoint(nextBBSlotIndex, nextSlotDir, nextSlotIndex));
            if (followingPath[followingPath.Count - 1].x > transform.position.x)
            {
                followingPath[followingPath.Count - 1] = roomEntity.leftEntrance.transform.position;
                entranceDir = SlotDir.Left;
            }
            else if (followingPath[followingPath.Count - 1].x < transform.position.x)
            {
                followingPath[followingPath.Count - 1] = roomEntity.rightEntrance.transform.position;
                entranceDir = SlotDir.Right;
            }
        }
        else if (currBBSlotIndex != nextBBSlotIndex)
        {//If not on the same vertical level
            Vector3 wayPoint;
            if (currBBSlotIndex > nextBBSlotIndex)
            {// For up move in back bone
                for (int i = currBBSlotIndex; i >= nextBBSlotIndex; i--)
                {
                    wayPoint = BuildManager.instance.slotMangers[i].transform.position;
                    wayPoint.z = transform.position.z;
                    followingPath.Add(wayPoint);
                }
            }
            else
            {// For down move in back bone
                for (int i = currBBSlotIndex; i <= nextBBSlotIndex; i++)
                {
                    wayPoint = BuildManager.instance.slotMangers[i].transform.position;
                    wayPoint.z = transform.position.z;
                    followingPath.Add(wayPoint);
                }
            }

            followingPath.Add(GetSingleWayPoint(nextBBSlotIndex, nextSlotDir, nextSlotIndex));

            if (followingPath[followingPath.Count - 1].x > followingPath[followingPath.Count - 2].x)
            {
                followingPath[followingPath.Count - 1] = roomEntity.leftEntrance.transform.position;
                entranceDir = SlotDir.Left;
            }
            else if (followingPath[followingPath.Count - 1].x < followingPath[followingPath.Count - 2].x)
            {
                followingPath[followingPath.Count - 1] = roomEntity.rightEntrance.transform.position;
                entranceDir = SlotDir.Right;
            }
        }

        currBBSlotIndex = nextBBSlotIndex;
        currSlotDir = nextSlotDir;
        currSlotIndex = nextSlotIndex;
    }

    private Vector3 GetSingleWayPoint(int nextBBSlotIndex, SlotDir nextSlotDir, int nextSlotIndex)
    {
        Vector3 wayPoint;
        if (nextSlotDir == SlotDir.Right)
        {
            wayPoint = BuildManager.instance.slotMangers[nextBBSlotIndex].rightSlots[nextSlotIndex].transform.position;
        }
        else
        {
            wayPoint = BuildManager.instance.slotMangers[nextBBSlotIndex].leftSlots[nextSlotIndex].transform.position;
        }
        wayPoint.z = transform.position.z;

        return wayPoint;
    }

    public void MoveInPath()
    {
        if (LevelManager.Instance.Testing)
        {
            print("Character start moving to new room");
        }
        pathIndex = 0;
        curPos = transform.position;
        //Flip();
        isMovePath = true;
    }
    public float getMovementSpeed()
    {
        return speed;
    }
}
