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

    private int lineIndex;

    private bool isArabic;
    private bool isTutorialTextTimerRun;
    #endregion

    public void GenerateRandome()
    {
        List<int> objs = new List<int>();
        for (int i = 0; i < animControllers.Length; i++)
        {
            objs.Add(i);
        }

        GameObject charcter = LevelManager.Instance.roomManager.getRoomWithGameObject(trainingRoom.transform.parent.gameObject)
            .roomJobs[1].jobHolder.characterGameObject;
        charcter.GetComponent<Animator>().runtimeAnimatorController = charTrainAnime;

        int index;

        for (int i = 0; i < animControllers.Length; i++)
        {
            // Randomize the animation controller
            index = Random.Range(0, objs.Count);
            animBtnObj[i].runtimeAnimatorController = animControllers[objs[index]];
            int slot = objs[index] + 1;

            print(index + " " + slot);
            // Add listener to button
            animBtns[i].onClick.RemoveAllListeners();
            animBtns[i].onClick.AddListener(() => charcter.GetComponent<TrainingAnimationsManager>().runThisAnimation(slot));
            animBtns[i].onClick.AddListener(() => ZUIManager.Instance.OpenMenu("UIMenu"));

            // Remove the index from the list
            objs.Remove(objs[index]);
        }
    }

    public void StartTutorial()
    {
        isArabic = true; /*(PlayerPrefs.GetString("Lang").Equals("ar")) ? true : false;*/
        isTutorialTextTimerRun = true;
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
}
