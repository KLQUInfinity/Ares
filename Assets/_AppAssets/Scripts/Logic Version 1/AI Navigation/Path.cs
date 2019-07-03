using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    Vector3 prevPos;
    private void OnDrawGizmos()
    {
        foreach (Transform node in transform)
        {
            if (node.gameObject.name == "Simulator")
            {
                Gizmos.DrawSphere(node.position, 0.1f);
            }
        }
        Gizmos.color = Color.green;
        for (int i = 0; i < transform.childCount; i++)
        {
            //if (transform.GetChild(i).gameObject.name!= "Simulator")
            {
                Gizmos.DrawSphere(transform.GetChild(i).position, 0.1f);
                int nextChild = (i + 1) % transform.childCount;
               // Debug.Log("Current is: "+ i+" Next is: "+nextChild);
                Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(nextChild).position);
            }
            //else
            {
            }
            void OnDrawGizmosSelected()
            {
                // Display the explosion radius when selected
                Gizmos.color = Color.blue;
               // Gizmos.DrawSphere(Input.);
            }
        }
  


    }
}
