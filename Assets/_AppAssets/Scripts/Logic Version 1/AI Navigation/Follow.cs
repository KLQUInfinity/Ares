using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SteerBase))]
public class Follow : MonoBehaviour,ISteer
{

        [SerializeField]
        protected Transform target;
        [SerializeField]
        protected float speed = 1;

        public virtual Vector3 SteerForce(Vector3 position, Vector3 velocity)
        {
            Vector3 desiredVelocity = (target.position-(-target.transform.forward- (target.transform.forward.normalized*2))) - position;

            Vector3 seekForce = desiredVelocity - (velocity*2);

            return seekForce*speed;
        }
}
