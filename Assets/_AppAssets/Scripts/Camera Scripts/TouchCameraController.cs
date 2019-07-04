using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TouchCameraController : MonoBehaviour
{
    public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
    public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.
    public Camera camera;

    Vector2 startPos;
    Vector2 direction;
    bool directionChosen;

    float posX;
    float posY;
    public float swapeSpeed = 0.2f;
    public float clampValue = 1;
    public void Start()
    {
        camera = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        handleTouch();
    }

    void handleTouch()
    {
        //If there are any touches
        if (Input.touchCount > 0)
        {
            //Single touch
            if (Input.touchCount == 1)
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
                        posX =Mathf.Clamp( movePos.x, clampValue, -clampValue) *Time.deltaTime * swapeSpeed;
                        posY = Mathf.Clamp(movePos.y, clampValue, -clampValue) * Time.deltaTime * swapeSpeed;
                        camera.transform.position+=new Vector3(-posX, posY, 0);
                        //camera.transform.position = Vector3.ClampMagnitude(camera.transform.position,0.2f);
                        // camera.transform.position = camera.transform.position * Vector3.right;
                       //camera.transform.position = new Vector3(posX, -posY, 0) * Time.deltaTime;//* swapeSpeed;
                        break;

                    // Report that a direction has been chosen when the finger is lifted.
                    case TouchPhase.Ended:
                        startPos = Vector2.zero;
                        direction = Vector2.zero;
                        directionChosen = true;
                        break;
                }
            }
            // If there are two touches on the device...
            else if (Input.touchCount == 2)
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

                //// If the camera is orthographic...
                //if (camera.orthographic)
                //{
                //    // ... change the orthographic size based on the change in distance between the touches.
                //    camera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

                //    // Make sure the orthographic size never drops below zero.
                //    camera.orthographicSize = Mathf.Max(camera.orthographicSize, 0.1f);
                //}
                //else
                //{
                if (deltaMagnitudeDiff > 0)
                {
                    zoomIn(deltaMagnitudeDiff);
                }
                else if (deltaMagnitudeDiff < 0)
                {
                    zoomOut(deltaMagnitudeDiff);
                }


                // Clamp the field of view to make sure it's between 0 and 180.
                camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, 0.1f, 179.9f);
                //}
                Debug.Log("Hey yaw");
            }
        }
    }

    private void zoomIn(float deltaMagnitudeDiff)
    {
        //Otherwise change the field of view based on the change in distance between the touches.
        camera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
    }

    private void zoomOut(float deltaMagnitudeDiff)
    {
        //Otherwise change the field of view based on the change in distance between the touches.
        camera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
    }

    //Touch initTouch = new Touch();
    //float rotX = 0f;
    //float rotY = 0f;

    //private Vector3 origRot;
    //public float rotSpeed = 0.5f;
    //public float dir = -1;

    //public void Start()
    //{
    //    origRot = camera.transform.eulerAngles;
    //    rotX = origRot.x;
    //    rotY = origRot.y;
    //}
    //private void FixedUpdate()
    //{
    //    foreach (Touch touch in Input.touches) //Doesn't matter you can use directoly the first touch 
    //    {
    //        if (touch.phase == TouchPhase.Began)
    //        {
    //            initTouch = touch;
    //        }
    //        else if (touch.phase== TouchPhase.Moved)
    //        {
    //            //Swiping
    //            float deltaX = initTouch.position.x - touch.position.x;
    //            float deltaY = initTouch.position.y - touch.position.y;
    //            rotX -= deltaX * Time.deltaTime *rotSpeed *dir;
    //            rotY += deltaY * Time.deltaTime * rotSpeed * dir;
    //            camera.transform.eulerAngles = new Vector3(rotX, rotY, 0f);
    //        }
    //        else if (touch.phase==TouchPhase.Ended)
    //        {
    //            initTouch = new Touch();
    //        }
    //    }
    //}

}
