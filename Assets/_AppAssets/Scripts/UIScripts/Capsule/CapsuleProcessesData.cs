using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapsuleProcessesData : MonoBehaviour
{
    public Image productionProgressBar;
    public Text job1State;
    public Text job2State;
    public Text productionResourceName;
    public Text roomProductivityTxt;
    public ResourceToProduce producitonResource;
    public JobEntity jobEntity;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.parent.parent.gameObject.name!= "HibernationRoom" && transform.parent.parent.gameObject.name != "TrainningRoom")
        {
            productionResourceName.text = producitonResource.resourceType.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (jobEntity)
        {
            if (jobEntity.roomJobs.Count != 0)
            {
                updateJobsState();
            }
        }

    }

    private void updateJobsState()
    {
        if (jobEntity.roomJobs[0].jobState == JobState.Vacant)
        {
            job1State.text = 0.ToString();
        }
        else
        {
            job1State.text = 1.ToString();
        }
        if (jobEntity.roomJobs[1].jobState == JobState.Vacant)
        {
            job2State.text = 0.ToString();
        }
        else
        {
            job2State.text = 1.ToString();
        }
    }

    public void setProductionProgressValue(float value)
    {
        productionProgressBar.fillAmount = value;
    }
    public void setRoomProductivity(float roomProductivity)
    {
        roomProductivityTxt.text = roomProductivity.ToString();
    }
}
