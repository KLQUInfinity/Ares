using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteerBase))]
public class Pursuit : MonoBehaviour,ISteer
{

    [SerializeField]
    protected Transform target;
    [SerializeField]
    protected float speed = 1;
    public Vector3 pursuitPos = new Vector3();
    public virtual Vector3 SteerForce(Vector3 position, Vector3 velocity)
    {
        pursuitPos = target.forward;
        Vector3 desiredVelocity = ((target.position + (pursuitPos * 2)) - position);


        Vector3 seekForce = desiredVelocity ;

        return seekForce * speed;
    }
}
