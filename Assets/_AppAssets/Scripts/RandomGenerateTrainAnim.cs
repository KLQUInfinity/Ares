using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomGenerateTrainAnim : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController[] animControllers;
    [SerializeField] private Animator[] animBtnObj;
    [SerializeField] private RoomEntity trainingRoom;

    [SerializeField] private RuntimeAnimatorController charTrainAnime;
    [SerializeField] private Button[] animBtns;

    private void Start()
    {
        animBtns = GetComponentsInChildren<Button>();
        GenerateRandome();
    }

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
            int slot = objs[index]+1;
            print(index + "  " + (objs[index] + 1));
            // Add listener to button
            animBtns[i].onClick.AddListener(() => charcter.GetComponent<TrainingAnimationsManager>().runThisAnimation(slot));

            // Remove the index from the list
            objs.Remove(objs[index]);
        }
    }
}
