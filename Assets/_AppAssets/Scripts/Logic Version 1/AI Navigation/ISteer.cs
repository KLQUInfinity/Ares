using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISteer
{
    Vector3 SteerForce(Vector3 position, Vector3 velocity);
}
