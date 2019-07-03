using System.Collections;
using UnityEngine;

public enum TouchState
{
    IdleStae,
    holdingState,
    singleClickState,
    doubleClickState
}
public class ClickStateMachine : MonoBehaviour
{
    private float firstClickTime;
    public float timeBetweenClicks = 0.2f;
    public float holdingTime;
    private bool coroutineAllowed;
    private int clickCounter;
    //----------------------------------
    Touch firstTouch;
    public TouchState currentTouchState;
    public GameObject currentHoveredObj;
    //----------------------------------
    private Vector2 startPos;
    private Vector2 currentPos;
    private void Start()
    {
        Init();
    }

    /// <summary>
    /// Assign varibles with the default values
    /// </summary>
    private void Init()
    {
        clickCounter = 0;
        firstClickTime = 0;
        coroutineAllowed = true;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = currentPos = touch.position;
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    currentTouchState = TouchState.holdingState;
                    currentPos = touch.position;
                    holdingTime = Time.deltaTime;
                    break;
                case TouchPhase.Ended:
                    currentPos = touch.position;
                    if ((currentPos - startPos).magnitude < 3 || Time.time < firstClickTime + timeBetweenClicks)
                    {
                        clickCounter += 1;
                    }
                    else
                    {
                        Init();
                    }
                    holdingTime = 0;
                    break;
            }
        }

        if (clickCounter == 1 && coroutineAllowed)
        {
            firstClickTime = Time.time;
            StartCoroutine(clickTypeDetection());
        }
    }

    private IEnumerator clickTypeDetection()
    {
        coroutineAllowed = false;
        while (Time.time < firstClickTime + timeBetweenClicks)
        {
            if (clickCounter == 2)
            {

                currentTouchState = TouchState.doubleClickState;
                break;
            }
            else if (clickCounter == 1)
            {
                currentTouchState = TouchState.singleClickState;
            }
            yield return new WaitForEndOfFrame();
        }
        Init();
    }

}
