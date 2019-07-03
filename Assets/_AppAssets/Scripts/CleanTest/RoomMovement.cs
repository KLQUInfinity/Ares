using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomMovementType {
    Direct,
    Diagonally
}
public class RoomMovement : MonoBehaviour
{
   public  RoomMovementType roomMovementType;
    public Animator animator;
    public Transform entrance1;
    public Transform entrance2;
    public Transform jobPos1;
    public Transform jobPos2;
    // Start is called before the first frame update
    void Start()
    {
        roomMovementType = RoomMovementType.Direct;
        animator.SetBool("Direct", true);
        animator.SetBool("Diagonally", false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (roomMovementType)
        {
            case RoomMovementType.Direct:
                animator.SetBool("Direct", true);
                animator.SetBool("Diagonally", false);
                break;
            case RoomMovementType.Diagonally:
                animator.SetBool("Direct", false);
                animator.SetBool("Diagonally", true);
                break;
        }
    }

    private void OnDrawGizmos()
    {
        //float xAvg = ((entrance1.position.x + entrance2.position.x )- (jobPos1.position.x + jobPos2.position.x))/ 4;
        //float yAvg = ((entrance1.position.y + entrance2.position.y )- (jobPos1.position.y + jobPos2.position.y)) / 4;
        //Vector3 center = new Vector3(xAvg, 0, yAvg);
        //Vector3 center = (entrance2.position - jobPos1.position)/ (entrance2.position - jobPos1.position).magnitude;//new Vector3(xAvg, 0, yAvg);
        //Vector3 center = ((jobPos1.position-entrance1.position)- (jobPos2.position - entrance2.position))/4;

        float width = (Mathf.Abs(entrance1.position.x) + Mathf.Abs(entrance2.position.x));
        float height = (Mathf.Abs(jobPos1.position.z) + Mathf.Abs(entrance1.position.z));

        float Cz = (
            (Mathf.Abs(jobPos1.position.z +jobPos2.position.z))
            - (Mathf.Abs(entrance1.position.z + entrance2.position.z))) / 4 ;
        float Cx = (
            (Mathf.Abs(jobPos2.position.x + entrance2.position.x)) -
            (Mathf.Abs(jobPos1.position.x + entrance1.position.x))
            ) / 4;
        Vector3 center = new Vector3(Cx,0,Cz);
        Bounds bounds = new Bounds(center, new Vector3(
            Mathf.Abs((entrance1.position - entrance2.position).x)
            , 0.2f,
            Mathf.Abs((jobPos1.position - entrance1.position).z)
            ));
        Gizmos.DrawWireCube(bounds.center,bounds.size);
        //Gizmos.DrawWireCube(bounds.center, new Vector3(0.1f,0.1f,0.1f));

    }
}
