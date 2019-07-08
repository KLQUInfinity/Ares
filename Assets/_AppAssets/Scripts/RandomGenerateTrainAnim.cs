using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RandomGenerateTrainAnim : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController[] animControllers;
    [SerializeField] private Animator[] animBtnObj;
    [SerializeField] private RoomEntity trainingRoom;

    [SerializeField] private RuntimeAnimatorController charTrainAnime;
    [SerializeField] private Button[] animBtns;

    #region Tutorial
    [SerializeField] private FixTextMeshPro tutorialTxt;
    [SerializeField] private List<string> arTutorial;
    [SerializeField] private List<string> enTutorial;
    [SerializeField] private float textDelay;
    [SerializeField] private UIElement animChoser;
    [SerializeField] private UIElement animChoserBtns;

    private int lineIndex;

    private bool isArabic;
    private bool isTutorialTextTimerRun;
    #endregion

    GameObject charcter;

    public void GenerateRandome()
    {
        List<int> objs = new List<int>();
        for (int i = 0; i < animControllers.Length; i++)
        {
            objs.Add(i);
        }

        charcter = LevelManager.Instance.roomManager.getRoomWithGameObject(trainingRoom.transform.parent.gameObject)
            .roomJobs[1].jobHolder.characterGameObject;
        charcter.GetComponent<Animator>().runtimeAnimatorController = charTrainAnime;
        charcter.GetComponent<TrainingAnimationsManager>().randomGenerateTrain = this;

        int index;

        for (int i = 0; i < animControllers.Length; i++)
        {
            // Randomize the animation controller
            index = Random.Range(0, objs.Count);
            animBtnObj[i].runtimeAnimatorController = animControllers[objs[index]];
            int slot = objs[index] + 1;

            // Add listener to button
            animBtns[i].onClick.RemoveAllListeners();
            animBtns[i].onClick.AddListener(() => charcter.GetComponent<TrainingAnimationsManager>().runThisAnimation(slot));
            animBtns[i].onClick.AddListener(() => animChoser.SwitchVisibility());

            // Remove the index from the list
            objs.Remove(objs[index]);
        }
    }

    public void StartTutorial()
    {
        isArabic = true; /*(PlayerPrefs.GetString("Lang").Equals("ar")) ? true : false;*/
        isTutorialTextTimerRun = true;
        lineIndex = 0;
        StartCoroutine(TutorialTimer());
    }

    IEnumerator TutorialTimer()
    {
        while (isTutorialTextTimerRun)
        {
            if (lineIndex != 0)
            {
                yield return new WaitForSeconds(textDelay);
            }

            tutorialTxt.text = (isArabic) ? arTutorial[lineIndex] : enTutorial[lineIndex];
            lineIndex++;
            if (lineIndex >= arTutorial.Count)
            {
                isTutorialTextTimerRun = false;
                animChoser.SwitchVisibility();
            }
        }
    }

    public void ShowResult(CharacterTrainingAnimationsState animationState)
    {
        if (animationState == CharacterTrainingAnimationsState.Correct)
        {
            tutorialTxt.text = "صحيح لقد اتقنت التدريب";
            StartCoroutine(WaitToSeeAnim(5.0f, true));
        }
        else
        {
            tutorialTxt.text = "خطا سيعاد التدريب لك مجددا";
            StartCoroutine(WaitToSeeAnim(5.0f, false));
        }

    }

    IEnumerator WaitToSeeAnim(float delay, bool isCorrect)
    {
        yield return new WaitForSeconds(delay);

        if (isCorrect)
        {
            tutorialTxt.text = "";
            RestAnimToIdle();
            animChoserBtns.SwitchVisibility();
        }
        else
        {
            RestAnimToIdle();
            tutorialTxt.text = (isArabic) ? arTutorial[arTutorial.Count - 1] : enTutorial[arTutorial.Count - 1];
            GenerateRandome();
            animChoser.SwitchVisibility();
        }
    }

    public void RestAnimToIdle()
    {
        if (charcter)
        {
            charcter.GetComponent<TrainingAnimationsManager>().runThisAnimation(0);
        }
    }
}
