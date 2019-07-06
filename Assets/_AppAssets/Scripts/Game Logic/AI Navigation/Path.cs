using UnityEngine;

public class Path : MonoBehaviour
{
    public bool showThisOnly;
    private void OnDrawGizmos()
    {

        if (UnityEditor.Selection.activeGameObject == this.gameObject)
        {
            Gizmos.color = Color.blue;

        }
        else
        {
            //if (showThisOnly)
            {
                Gizmos.color = Color.clear;
            }
            //else 
            {
                Gizmos.color = new Color(Color.green.r, Color.green.g, Color.green.b, 0.5f);
            }
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.DrawSphere(transform.GetChild(i).position, 0.1f);
            int nextChild = (i + 1); //% transform.childCount;
            if (nextChild <= transform.childCount - 1)
            {
                Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(nextChild).position);
            }
        }


    }


}
