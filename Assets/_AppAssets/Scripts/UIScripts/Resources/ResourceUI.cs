using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private Image resourceImg;

    private TextMeshProUGUI foodTxt;
    private int value;
    private Resource resource;

    private void Start()
    {
        foodTxt = GetComponent<TextMeshProUGUI>();
        resource = GameBrain.Instance.resourcesManager.getResource(resourceType);
    }

    void Update()
    {
        if (resource != null)
        {
            value = (int)GameBrain.Instance.resourcesManager.getResource(resourceType).valueInPercentage;
            foodTxt.text = value + " %";

            if (value >= 75)
            {
                foodTxt.color = LevelUIManager.Instance.GoodColor;
                if (resourceImg)
                {
                    resourceImg.color = LevelUIManager.Instance.GoodColor;
                }
            }
            else if (value >= 40 && value < 75)
            {
                foodTxt.color = LevelUIManager.Instance.MiddleColor;
                if (resourceImg)
                {
                    resourceImg.color = LevelUIManager.Instance.MiddleColor;
                }
            }
            else if (value > 0 && value < 40)
            {
                foodTxt.color = LevelUIManager.Instance.BadColor;
                if (resourceImg)
                {
                    resourceImg.color = LevelUIManager.Instance.BadColor;
                }
            }
        }
    }
}
