using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public GameObject hoverer;
    public void selectThis()
    {
        if ( this.gameObject.tag =="Character")
        {
            //Select character
        }
        if (this.gameObject.tag == "Room")
        {
            //Select room
        }
    }
    /// <summary>
    /// Deselect all rooms and its contents
    /// </summary>
    public void deselectAll()
    {
        foreach (Transform room in LevelManager.Instance.Environment.transform)
        {
            var content = LevelManager.Instance.roomManager.getRoomWithGameObject(room.gameObject).contents;
            for (int i = 0; i < content.Count; i++)
            {
                foreach (var item in content)
                {
                    //Deselect all current iteration room content
                }
            }
            //Deselect room.
        }
    }
    public void deselectThis(GameObject objectToDeselect)
    {
            //Deselect this object.
    }
}
