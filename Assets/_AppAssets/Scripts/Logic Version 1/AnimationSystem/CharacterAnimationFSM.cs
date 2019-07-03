using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CharacterAnimationsState
{
    Idle,
    Walking,
    Jogging,
    Floating,
    Job1,
    Job2,
    Job3,

}
public enum HorizontalDirecton
{
    Left,
    Right
}
public enum VerticalDirecton
{
    Up,
    Down
}
public class CharacterAnimationFSM : MonoBehaviour
{
    Character character;
    public CharacterAnimationsState currentCharacterAnimationState;
    public HorizontalDirecton horizontalDirection;
    public VerticalDirecton verticalDirection;
    public Animator characterAnimator;
    public Animation walk;
    // Start is called before the first frame update
    void Start()
    {
        currentCharacterAnimationState = CharacterAnimationsState.Idle;
        horizontalDirection = HorizontalDirecton.Right;
        verticalDirection = VerticalDirecton.Up;
        character = GetComponent<CharacterEntity>().character;
        characterAnimator = GetComponent<Animator>();
        Init();
    }

    private void Init()
    {

    }


    // Update is called once per frame
    void Update()
    {
        changeCharacterAnimationState();
    }
    void changeCharacterAnimationState()
    {

        switch (currentCharacterAnimationState)
        {
            case CharacterAnimationsState.Idle:
                activateThisAnimationStateState(CharacterAnimationsState.Idle);
                break;
            case CharacterAnimationsState.Walking:
                activateThisAnimationStateState(CharacterAnimationsState.Walking);
                flipOnWalkingOrJogging();
                break;
            case CharacterAnimationsState.Jogging:
                activateThisAnimationStateState(CharacterAnimationsState.Jogging);
                flipOnWalkingOrJogging();
                break;
            case CharacterAnimationsState.Floating:
                activateThisAnimationStateState(CharacterAnimationsState.Floating);
                flipOnFloating();
                break;
            case CharacterAnimationsState.Job1:
                activateThisAnimationStateState(CharacterAnimationsState.Job1);
                break;
            case CharacterAnimationsState.Job2:
                activateThisAnimationStateState(CharacterAnimationsState.Job2);
                break;
            case CharacterAnimationsState.Job3:
                activateThisAnimationStateState(CharacterAnimationsState.Job3);
                break;
        }
    }

    public void changeAnimationStateTo(CharacterAnimationsState newAnimationState)
    {
        this.currentCharacterAnimationState = newAnimationState;
    }

    public void activateThisAnimationStateState(CharacterAnimationsState animationState)
    {
        var states = Enum.GetValues(typeof(CharacterAnimationsState));
        foreach (int stateNumber in states)
        {

            if ((CharacterAnimationsState)stateNumber == animationState)
            {
                characterAnimator.SetBool(animationState.ToString(), true);
                this.currentCharacterAnimationState = animationState;
            }
            else
            {
                characterAnimator.SetBool(((CharacterAnimationsState)stateNumber).ToString(), false);
            }
        }
    }

    public void flipOnFloating()
    {
        if (this.horizontalDirection == HorizontalDirecton.Right)
        {

        }
        else if (this.horizontalDirection == HorizontalDirecton.Left)
        {

        }

        if (this.verticalDirection == VerticalDirecton.Up)
        {

        }
        else if (this.verticalDirection == VerticalDirecton.Down)
        {

        }
    }
    public void flipOnWalkingOrJogging()
    {
        if (this.horizontalDirection == HorizontalDirecton.Right)
        {

        }
        else if (this.horizontalDirection == HorizontalDirecton.Left)
        {

        }
    }

    public void changeWalkAnimationTransform() {
      
    }
    #region Solid Methods to change Both horizantal and vertical states 
    public void flipToRight()
    {
        this.horizontalDirection = HorizontalDirecton.Right;
    }

    public void flipToLeft()
    {
        this.horizontalDirection = HorizontalDirecton.Left;
    }

    public void flipToUp()
    {
        this.verticalDirection = VerticalDirecton.Up;

    }

    public void flipToDown()
    {
        this.verticalDirection = VerticalDirecton.Down;
    }
    #endregion
}
