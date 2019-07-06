using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SteerBase : MonoBehaviour
{
    public float maxSteer = 3;
    public float maxVelocity = 3;

    [HideInInspector]
    public Vector3 steering;
    [HideInInspector]
    public Vector3 velocity;
    public Vector3 position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }
    public Vector3 Forward => velocity.normalized;
    List<ISteer> steers;

    private void Awake()
    {
        steers = GetComponents<MonoBehaviour>().OfType<ISteer>().ToList();
        int x = steers.Count;
    }

    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, Forward + Forward * 1.5f, Color.black);
        Vector3 steerForce = SteerForce();
        steerForce.y = 0;
        steering = Vector3.ClampMagnitude(steerForce, maxSteer);
        velocity =  Vector3.ClampMagnitude(velocity + steering * Time.fixedDeltaTime, maxVelocity);
        position += velocity * Time.fixedDeltaTime;
    }

    Vector3 SteerForce()
    {
        return steers.Select(steer => steer.SteerForce(position, velocity)).Sum();
    }
}
