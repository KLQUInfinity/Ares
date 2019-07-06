using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobEntity : MonoBehaviour
{
    [SerializeField]
    public List<Job> roomJobs= new List<Job>();
    // Start is called before the first frame update
    void Start()
    {
       Room room= LevelManager.Instance.roomManager.getRoomWithGameObject(this.transform.parent.gameObject);
        foreach (var job in roomJobs)
        {
            job.jobRoom = transform.parent.gameObject;
            room.roomJobs.Add(job);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
