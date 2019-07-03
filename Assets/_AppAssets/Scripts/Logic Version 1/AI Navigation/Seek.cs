using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteerBase))]
public class Seek : MonoBehaviour, ISteer
{
    public Transform target;
    [HideInInspector]
    public bool isReveresed;
    [SerializeField]
    protected float speed = 1;

    public virtual Vector3 SteerForce(Vector3 position, Vector3 velocity)
    {
  
        Vector3 desiredVelocity = target.position - position;
        desiredVelocity.y = 0;
        Vector3 distanceToJobPos = (target.position - transform.position);
        distanceToJobPos.y = 0;
        if (distanceToJobPos.magnitude <= 0.01f)
        {
            target = null;
            GetComponent<CharacterEntity>().OnPathPositionReached(isReveresed);
        }
        Vector3 seekForce = desiredVelocity;// - velocity;

        return seekForce.normalized * speed;
    }
}
