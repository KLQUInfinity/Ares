using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobPathFollowing : MonoBehaviour,ISteer
{
    public GameObject path;
    Queue<Transform> nodes;
    Transform currentNode;
    [SerializeField]
    protected float speed = 1;

    private void Start()
    {
        nodes = new Queue<Transform>(path.transform.GetChildren());
        currentNode = nodes.Dequeue();
    }
    public Vector3 SteerForce(Vector3 position, Vector3 velocity)
    {
        Vector3 desiredVelocity = currentNode.position - position;

        Vector3 seekForce = desiredVelocity - velocity;

        if (Vector3.Distance(currentNode.position, position) <=1)
        {
            if (nodes.Count < 1)
            {
                nodes = new Queue<Transform>(path.transform.GetChildren());
            }
            currentNode = nodes.Dequeue();
        }
        // Debug.Log(Vector3.Distance(currentNode.position, position));

        return seekForce.normalized * speed;
    }
}
