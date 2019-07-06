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
    public AnimationClip[] originalAnimationClipList;
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
        initializeOriginalAnimationsClipsList();
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

    public void changeWalkAnimationTransform()
    {

    }

    #region Animation Clip population automation logic

    public void initializeOriginalAnimationsClipsList()
    {
        this.originalAnimationClipList = characterAnimator.runtimeAnimatorController.animationClips;
    }

    /// <summary>
    /// Agmad Method fe 2019 ->>>>>>>>>> El method el fashe5aaaaaaaaa
    /// </summary>
    /// <param name="animationState"></param>
    /// <param name="jobAnimationClip"></param>
    public void populateJobAnimationClip(CharacterAnimationsState animationState, AnimationClip jobAnimationClip)
    {
        // Animator animator (get component blablabla)
        // AnimationClip anim (field in inspector)
        if (jobAnimationClip)
        {
            AnimationClip[] animationClipsList = characterAnimator.runtimeAnimatorController.animationClips;
            foreach (var animationClip in animationClipsList)
            {
                if (animationClip.name.ToLower().Equals(animationState.ToString().ToLower()))
                {
                    animationClipsList[Array.IndexOf(animationClipsList, animationClip)] = jobAnimationClip;
                }
            }
            AnimatorOverrideController animatorOverriderController =
                new AnimatorOverrideController(characterAnimator.runtimeAnimatorController);

            var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();//Empty list

            foreach (var animationClip in animatorOverriderController.animationClips)
            {
                if (!animationClip.name.ToLower().Equals(animationClipsList[
                    Array.IndexOf(animatorOverriderController.animationClips, animationClip)].name.ToLower()))
                {
                    anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(animationClip,
                        animationClipsList[
                    Array.IndexOf(animatorOverriderController.animationClips, animationClip)]));
                }
            }
            animatorOverriderController.ApplyOverrides(anims);
            characterAnimator.runtimeAnimatorController = animatorOverriderController;
        }
    }
    /// <summary>
    /// On job deassigning
    /// Resets the animation clips in order for each to take the placeholder 
    /// names to be able and ready for change once again because each at different room will have different clip
    /// </summary>
    public void resetOriginalAnimationClips()
    {
        AnimatorOverrideController animatorOverriderController =
                new AnimatorOverrideController(characterAnimator.runtimeAnimatorController);

        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();//Empty list

        foreach (var animationClip in animatorOverriderController.animationClips)
        {
            anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(animationClip,
                originalAnimationClipList[
            Array.IndexOf(animatorOverriderController.animationClips, animationClip)]));
        }
        animatorOverriderController.ApplyOverrides(anims);
        characterAnimator.runtimeAnimatorController = animatorOverriderController;
    }

    #endregion

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
