using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive : MonoBehaviour,ISteer
{
    [SerializeField]
    protected Transform target;
    [SerializeField]
    protected float speed = 1;
    float originalSpeed;

    public void Start()
    {
        originalSpeed = speed;
    }
    public virtual Vector3 SteerForce(Vector3 position, Vector3 velocity)
    {
        Vector3 desiredVelocity = target.position - position;
        Vector3 seekForce;
        if (desiredVelocity.magnitude <30f)
        {
            seekForce = desiredVelocity * (desiredVelocity - velocity).magnitude/30f - velocity;
        }
        else
        {
            seekForce = desiredVelocity - velocity;
        }

            
        return seekForce;
    }

}
