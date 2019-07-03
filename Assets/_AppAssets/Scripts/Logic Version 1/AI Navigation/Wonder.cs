using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteerBase))]
public class Wonder : MonoBehaviour, ISteer
{
    public float circleDistance = 3;
    public float angleChange = 0.02f;
    public float steeringForce = 10;

    float wonderAngle;


    public Vector3 SteerForce(Vector3 position, Vector3 velocity)
    {
        Vector3 circleCenter;
        Vector3 displacement;
        float magnitude;
        circleCenter = velocity.normalized * (1 + circleDistance);
        displacement = Vector3.one * Random.value;
        magnitude = displacement.magnitude*10;
        displacement = new Vector3(magnitude*Mathf.Cos(wonderAngle), 0, magnitude * Mathf.Sin(wonderAngle));
        wonderAngle += Random.value * angleChange+Random.Range(-0.1f, 0.1f); //- angleChange * 0.5f ;
        return (circleCenter + displacement -velocity).normalized*steeringForce;
    }
    private void OnDrawGizmos()
    {
        //Gizmos.DrawCube(circleCenter, Vector3.one * 5);
    }
}
