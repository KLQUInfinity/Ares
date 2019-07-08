using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// These enum will be used to send bool states to the animator through casting the enum variables to strings
/// which the animator has a parameter with the same name of each.
/// </summary>
public enum CharacterTrainingAnimationsState
{//Enum variables are case sensetive
    Idle,
    Correct,
    Wrong1,
    Wrong2
}
public class TrainingAnimationsManager : MonoBehaviour
{
    public CharacterTrainingAnimationsState currentCharacterAnimationState;
    public Animator fireFightingTrainingAnimator;
    public RandomGenerateTrainAnim randomGenerateTrain;

    private void Start()
    {
        currentCharacterAnimationState = CharacterTrainingAnimationsState.Idle;
        fireFightingTrainingAnimator = GetComponent<Animator>();
        fireFightingTrainingAnimator.SetBool(currentCharacterAnimationState.ToString(), true);
    }



    public void runThisAnimation(int animationEnumIndex)
    {
        //Convert the enum index to enum variable
        changeAnimationStateTo((CharacterTrainingAnimationsState)animationEnumIndex);
        if ((CharacterTrainingAnimationsState)animationEnumIndex != CharacterTrainingAnimationsState.Idle)
        {
            randomGenerateTrain.ShowResult((CharacterTrainingAnimationsState)animationEnumIndex);
        }
    }

    public void changeAnimationStateTo(CharacterTrainingAnimationsState animationState)
    {
        var states = Enum.GetValues(typeof(CharacterTrainingAnimationsState));
        foreach (int stateNumber in states)
        {

            if ((CharacterTrainingAnimationsState)stateNumber == animationState)
            {
                fireFightingTrainingAnimator.SetBool(animationState.ToString(), true);
                this.currentCharacterAnimationState = animationState;
            }
            else
            {
                fireFightingTrainingAnimator.SetBool(((CharacterTrainingAnimationsState)stateNumber).ToString(), false);
            }
        }
    }
}
