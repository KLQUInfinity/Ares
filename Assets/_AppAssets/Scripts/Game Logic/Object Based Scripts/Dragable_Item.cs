using UnityEngine;
using System;
using TMPro;

[RequireComponent(typeof(CharController))]
public class Dragable_Item : MonoBehaviour
{
    [SerializeField] private GameObject hollow;

    private float distance;
    [SerializeField] private bool isDragable;
    [SerializeField] private float timeToDrag = 0.03f;


    public GameObject containerRoom = null;

    private CharController myCharC;

    private void Start()
    {
        distance = (transform.position - Camera.main.transform.position).magnitude;

        myCharC = GetComponent<CharController>();
    }

    private void Update()
    {
        distance = (LevelManager.Instance.Environment.transform.position - Camera.main.transform.position).magnitude;

        MaximizeToIncludeHollowAndCharacter();
    }

    private void MaximizeToIncludeHollowAndCharacter()
    {
        if (isDragable)
        {
            Vector3 viewPos = hollow.transform.position - transform.position;
            viewPos.z = Camera.main.transform.position.z;
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(hollow.GetComponent<Renderer>().bounds.center);
            bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

            if (!onScreen)
            {
                Vector3 moveTowards = Vector3.MoveTowards(Camera.main.transform.position, Camera.main.transform.TransformPoint(new Vector3(viewPos.x, viewPos.y, 0)), 2);
                moveTowards.z = Camera.main.transform.position.z;
                Camera.main.transform.position = moveTowards;
            }

            float fieldOFView = (transform.position - hollow.transform.position).magnitude + (
                (GetComponent<Renderer>().bounds.size.x + GetComponent<Renderer>().bounds.size.y) / 2) +
                ((hollow.GetComponent<Renderer>().bounds.size.x + hollow.GetComponent<Renderer>().bounds.size.y) / 2);

            Camera.main.fieldOfView = fieldOFView > Camera.main.fieldOfView ?
                    fieldOFView : Camera.main.fieldOfView;

        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isDragable)
        {

        }
    }

    private void OnMouseDown()
    {
        // select the Character and open the ui of it
        if (Input.GetMouseButton(0))
        {
            LevelManager.Instance.setDragModeOn();
            if (!isDragable)
            {
                hollow.SetActive(true);
                isDragable = true;

            }
        }
    }

    private void OnMouseDrag()
    {
        if (LevelManager.Instance.controllersStateMachine.holdingTime > timeToDrag)
        {
            if (!isDragable)
            {
                hollow.SetActive(true);
                isDragable = true;

            }
        }
        if (isDragable)
        {
            Vector2 pos = GetMousePos();
            hollow.transform.position = ClampHollowPositionToMapBounds(pos);


            LevelManager.Instance.setDragModeOn();
        }

    }

    /// <summary>
    /// Get the mouse position according to camera
    /// </summary>
    /// <returns>The actual position</returns>
    private Vector3 GetMousePos()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
    /// <summary>
    /// Some other conditions needed to be take in consideration:
    ///     1- If the player going to evacuate a room since he will be at a job position we need 
    ///     to make calculation on which entrance does he will use to evacuate the room and send it to
    ///     the Character -> containerEntrance in order to use with the method "followJobPathIfAny"
    ///     Look At ->>>>> Comment inside the 3rdLevel if condition inside OnMouseUp() method.
    /// </summary>
    private void OnMouseUp()
    {

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)

                    if (hit.transform.tag.Equals("Room") /*&& hit.transform.name != containerRoom.name*/)
                    {
                        GameObject oldContainer;
                        if (oldContainer = LevelManager.Instance.getCharacterPhysicalContainer(this.gameObject))
                        {
                            if (oldContainer == hit.collider.gameObject)
                            {
                                resetDragabbleItemData(); //Cancel Drag operation.
                                return;
                            }
                        }
                        if (!LevelManager.Instance.roomManager.getRoomWithGameObject(hit.collider.gameObject).roomGameObject)
                        {
                            LevelManager.Instance.CalculateThisRoomBounds(new Room(hit.collider.gameObject));
                        }

                        RoomEntity roomEntity = hit.transform.GetComponentInChildren<RoomEntity>();
                        if (LevelManager.Instance.roomManager.getRoomWithGameObject(roomEntity.roomGameObject)
                            .searchForFreeJob())
                        {
                            //this.GetComponent<CharacterEntity>().followRoomInnerPath(roomEntity, true);
                            // ->>>>> LevelManager.Instance.characterManager.getCharacterWithGameObject(gameObject).containerEntrance= Calculated entrance to get out from.

                            oldContainer.GetComponentInChildren<RoomEntity>().SubCharCountToRoom();
                            if (oldContainer.name.Equals("TrainningRoom"))
                            {
                                GetComponent<Animator>().runtimeAnimatorController = myCharC.myAnimController;
                                oldContainer.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
                            }

                            Slot s = roomEntity.mySlot;
                            myCharC.GenerateFollowPathWayPoins(
                                s.MySlotManger.transform.GetSiblingIndex(),
                                s.MyDir,
                                s.transform.GetSiblingIndex(),
                                hit.transform.GetComponentInChildren<RoomEntity>()
                                );
                            myCharC.MoveInPath();

                            if (containerRoom)
                            {
                                LevelManager.Instance.roomManager.evacuateFromARoom(containerRoom, this.gameObject);
                                containerRoom = null;
                            }
                        }
                        else
                        {
                            /// <summary>
                            /// Ask the developer whether or not to proceed and which 
                            /// character he wants to remove from the room and put this character in his place.

                            /// - if he chose to proceed and chose a character to be evacuated then the evacuated character
                            /// Has to be evacutaed to the hibernation room back again, and this character will have his position.
                            ///           - At this point will assign to the local variable job the job of the evacyated embloyee 
                            ///           and the code proceeds which means that the job variable won't be equal to null any more. (Omar)
                            ///           - relocate the evacuated character using the navigation methods' chain. (Bendary)
                            /// - If he chose not to proceed then will do nothing in code and will cancel the navigation process(Bendary)
                            /// </summary>
                        }

                    }
            }
        }

        resetDragabbleItemData(); //Cancel Drag operation.
    }

    public void resetDragabbleItemData()
    {
        hollow.transform.localPosition = Vector3.zero;
        hollow.SetActive(false);
        LevelManager.Instance.setDragModeOff();
        isDragable = false;
    }
    private Vector3 ClampHollowPositionToMapBounds(Vector3 mousPos)
    {
        float yAxisBoundry = Mathf.Clamp(mousPos.y, LevelManager.Instance.BottomBoundry.position.y - 15,
            LevelManager.Instance.TopBoundry.position.y + 15);
        float xAxisBoundry = Mathf.Clamp(mousPos.x,
             LevelManager.Instance.LeftBoundry.position.x - 15, LevelManager.Instance.RightBoundry.position.x + 15);
        return new Vector3(xAxisBoundry, yAxisBoundry, hollow.transform.position.z);
    }
}
