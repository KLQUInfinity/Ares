using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteerBase))]
public class Fleeing : MonoBehaviour,ISteer
{
    [SerializeField]
    protected Transform target;
    [SerializeField]
    protected float speed = 1;
    float originalSpeed;
    private void Start()
    {
        originalSpeed = speed;
    }
    public virtual Vector3 SteerForce(Vector3 position, Vector3 velocity)
    {
        Vector3 desiredVelocity = (target.position - position);
        Vector3 seekForce;
        if (desiredVelocity.magnitude<10)
        {
            seekForce = (velocity - (desiredVelocity - velocity));
        }
        else
        {
            seekForce =  (desiredVelocity - velocity) * (desiredVelocity - velocity).magnitude / 10-velocity;
            seekForce = velocity.magnitude > 0 ? velocity * -1 : seekForce;
        }
        return seekForce;
    }
}
