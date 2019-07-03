using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchFSMController : MonoBehaviour
{
    //---------------------------------------------------------------
    public float perspectiveZoomSpeed;        // The rate of change of the field of view in perspective mode.
    public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.
    public float distanceFromBoundry = 5;
    public Camera camera;

    Vector2 startPos;
    Vector2 direction;
    bool directionChosen;
    //-------------------------------------------------------------
    [SerializeField]
    float minFOV;
    [SerializeField]
    float maxFOV;


    float posX;
    float posY;
    public float swapeSpeed = 0.2f;
    public float clampValue = 1;
    public bool tryOld;
    //---------------------------------------------------------------
    public ClickStateMachine clickFSM;
    public bool isDraggingCharacter;
    public void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //stateStack.Peek().OnStateUpdate(new GameObject());
        handleTouch();
    }
    private void LateUpdate()
    {
        ClampCameraPositionAndZoom();
    }

    private void ClampCameraPositionAndZoom()
    {
        float yAxisBoundry = Mathf.Clamp(camera.transform.position.y, LevelManager.Instance.BottomBoundry.position.y,
            LevelManager.Instance.TopBoundry.position.y);
        float xAxisBoundry = Mathf.Clamp(camera.transform.position.x,
             LevelManager.Instance.LeftBoundry.position.x, LevelManager.Instance.RightBoundry.position.x);
        float zAxisBoundry = Mathf.Clamp(camera.transform.position.z,
              LevelManager.Instance.FrontBoundry.position.z, LevelManager.Instance.BackBoundry.position.z);
        camera.transform.position = new Vector3(xAxisBoundry, yAxisBoundry, zAxisBoundry);
        swapeSpeed = Mathf.Clamp(swapeSpeed, 1, 6);
        // Clamp the field of view to make sure it's between 0 and 180.
        camera.fieldOfView = Mathf.Clamp(camera.fieldOfView,
            minFOV,
            maxFOV
            );
    }

    void handleTouch()
    {
        //How many touches at the moment
        if (Input.touchCount > 0)
        {
            //Single touch
            if (Input.touchCount == 1)
            {
                if (!checkIfPlayerIsTouched() && !checkIfRoomIsTouched())
                {//IS not a drag state
                    if (!isDraggingCharacter)
                    {
                        SwapAndDeselect();
                    }
                }
                else if (checkIfPlayerIsTouched() || checkIfRoomIsTouched())
                {//IS Touching some object
                    if (checkIfPlayerIsTouched())
                    {
                        switch (clickFSM.currentTouchState)
                        {
                            // Record initial touch position.
                            case TouchState.IdleStae:
                                break;

                            // Determine direction by comparing the current touch position with the initial one.
                            case TouchState.holdingState:
                                //Drag state which will do nothing here
                                break;

                            // Report that a direction has been chosen when the finger is lifted.
                            case TouchState.singleClickState:
                                Select(clickFSM.currentHoveredObj);
                                break;
                            case TouchState.doubleClickState:
                                SelectAndZoom(clickFSM.currentHoveredObj);
                                break;
                        }
                    }
                    else if (checkIfRoomIsTouched())
                    {
                        switch (clickFSM.currentTouchState)
                        {
                            // Record initial touch position.
                            case TouchState.IdleStae:
                                break;

                            // Determine direction by comparing the current touch position with the initial one.
                            case TouchState.holdingState:
                                //Swap case
                                if (!isDraggingCharacter)
                                {
                                    Swap();
                                }
                                break;

                            // Report that a direction has been chosen when the finger is lifted.
                            case TouchState.singleClickState:
                                // Select(clickFSM.currentHoveredObj);
                                break;
                            case TouchState.doubleClickState:
                                SelectAndZoom(clickFSM.currentHoveredObj);
                                break;
                        }
                    }
                }

            }
            // If there are two touches on the device...
            else if (Input.touchCount == 2)
            {
                zoom();
            }
        }
    }
    private void zoomIn(float deltaMagnitudeDiff)
    {
        //Otherwise change the field of view based on the change in distance between the touches.
       // camera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed*Time.deltaTime;
        Vector3 zoomVector= camera.transform.position;
        zoomVector.z = camera.transform.position.z - deltaMagnitudeDiff;
        camera.transform.position = Vector3.Lerp(zoomVector, camera.transform.position, 0.5f);
        swapeSpeed += deltaMagnitudeDiff/8;
    }

    private void zoomOut(float deltaMagnitudeDiff)
    {
        //Otherwise change the field of view based on the change in distance between the touches.
       // camera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed*Time.deltaTime;
        Vector3 zoomVector = camera.transform.position;
        zoomVector.z = camera.transform.position.z - deltaMagnitudeDiff;
        camera.transform.position = Vector3.Lerp(zoomVector, camera.transform.position, 0.5f);
        swapeSpeed += deltaMagnitudeDiff/12;
    }

    public bool checkIfPlayerIsTouched()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag.Equals("Character"))
            {
                return true;
            }
        }
        return false;

    }
    public bool checkIfRoomIsTouched()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag.Equals("Room"))
            {
                return true;
            }
        }
        return false;
    }
    public void Select(GameObject gameObj)
    {//Select on single click
        //Select character's code on single click goes here
    }
    public void SelectAndZoom(GameObject gameObj)
    {//Select on double click
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag.Equals("Room"))
            {
                clickFSM.currentHoveredObj = hit.transform.gameObject;
                clickFSM.currentHoveredObj.GetComponentInParent<SelectableObject>().selectThis();
                StartCoroutine("zoomAtRoom", hit.transform.gameObject);

            }
            else if (hit.transform.tag.Equals("Character"))
            {
                clickFSM.currentHoveredObj = hit.transform.gameObject;
                StartCoroutine("zoomAtCharacter", hit.transform.gameObject);
            }
            else
            {
                Deselect();
            }
        }
        else
        {
            Deselect();
        }
    }
    public void Deselect()
    {
        if (clickFSM.currentHoveredObj)
        {
            //clickFSM.currentHoveredObj.GetComponent<SelectableObject>().deselectThis(clickFSM.currentHoveredObj.gameObject);
            clickFSM.currentHoveredObj = null;
            LevelManager.Instance.Environment.GetComponent<SelectableObject>().deselectAll();
        }
    }
    public void zoom()
    {
        // Store both touches.
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        // Find the position in the previous frame of each touch.
        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        // Find the magnitude of the vector (the distance) between the touches in each frame.
        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        // Find the difference in the distances between each frame.
        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
        deltaMagnitudeDiff *= perspectiveZoomSpeed;
        if (deltaMagnitudeDiff > 0)
        {
            zoomIn(deltaMagnitudeDiff);
        }
        else if (deltaMagnitudeDiff < 0)
        {
            zoomOut(deltaMagnitudeDiff);
        }
    }
    IEnumerator zoomAtRoom(GameObject room)
    {
        yield return new WaitForSeconds(clickFSM.timeBetweenClicks);
        //Adjus the field of view to the zoom;
        Bounds bounds = new Bounds();
        foreach (var Room in LevelManager.Instance.roomManager.roomsBounds.Keys)
        {
            if (Room == room)
            {
                bounds = LevelManager.Instance.roomManager.roomsBounds[Room];
            }
        }
        camera.transform.position = new Vector3(bounds.center.x, bounds.center.y, camera.transform.position.z);
        while ((camera.transform.position.z < LevelManager.Instance.BackBoundry.position.z))
        {
            zoomIn(-0.2f);
        }
    }
    IEnumerator zoomAtCharacter(GameObject character)
    {
        yield return new WaitForSeconds(clickFSM.timeBetweenClicks);
        Bounds bounds = new Bounds();
        if (character.GetComponent<Dragable_Item>().containerRoom)
        {//If the character is inside a room bounds
            foreach (Room room in LevelManager.Instance.roomManager.rooms)
            {
                if (room.contents.Contains(character))
                {
                    bounds = LevelManager.Instance.roomManager.roomsBounds[room];
                }
            }
        }
        else
        {//if the character is not inside a room bounds
            bounds = character.GetComponent<Collider>().bounds;
        }
        camera.transform.position = new Vector3(bounds.center.x, bounds.center.y, camera.transform.position.z);
        while ((camera.transform.position.z < LevelManager.Instance.BackBoundry.position.z))
        {
            zoomIn(-0.2f);
        }
        // Debug.Log("Double click on character is performed");
    }
    int counter;
    public void Swap()
    {
        Touch touch = Input.GetTouch(0);
        switch (touch.phase)
        {
            // Record initial touch position.
            case TouchPhase.Began:
                startPos = touch.position;
                directionChosen = false;
                break;

            // Determine direction by comparing the current touch position with the initial one.
            case TouchPhase.Moved:
                direction = touch.position - startPos;
                // Something that uses the chosen direction...
                Vector3 movePos = direction;
                posX =/* Mathf.Clamp*/(movePos.x/*, clampValue, -clampValue*/) * Time.deltaTime * swapeSpeed;
                posY = /*Mathf.Clamp*/(movePos.y/*, clampValue, -clampValue*/) * Time.deltaTime * swapeSpeed;
                swapCameraTo();
                counter++;
                if (counter==5)
                {
                    startPos = touch.position;
                    counter = 0;
                }
                break;

            // Report that a direction has been chosen when the finger is lifted.
            case TouchPhase.Ended:
                startPos = Vector2.zero;
                direction = Vector2.zero;
                directionChosen = true;
                break;
        }

    }

    public void SwapAndDeselect()
    {
        Deselect();
        Swap();

    }
    private void swapCameraTo()
    {
        if (tryOld)
        {
            camera.transform.position += new Vector3(-posX, posY, 0) * swapeSpeed;
        }
        else
        {
            Vector3 moveTowards = Vector3.MoveTowards(camera.transform.position, (camera.transform.TransformPoint(new Vector3(-posX * swapeSpeed, -posY * swapeSpeed, 0))), 2);
            moveTowards.z = camera.transform.position.z;

            camera.transform.position = Vector3.Slerp(moveTowards, camera.transform.position, 0.8f) ;
        }
        //camera.transform.Translate(new Vector3(posX, posY, 0).normalized * swapeSpeed);
    }
    public void setDragModeOn()
    {
        isDraggingCharacter = true;
        //Debug.Log("Drag Mode on");
    }
    public void setDragModeOff()
    {
        isDraggingCharacter = false;
        //Debug.Log("Drag Mode on");
    }

}
