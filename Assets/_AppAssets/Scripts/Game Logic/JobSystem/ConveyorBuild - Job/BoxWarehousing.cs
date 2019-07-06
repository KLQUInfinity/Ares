using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxWarehousing : MonoBehaviour
{
    public List<List<ResourceBox>> boxesWarehouseList = new List<List<ResourceBox>>();
    public int maxBoxesCount = 10;
    public int maxColBoxes = 6;
    public int boxesCount;
    // Start is called before the first frame update



    public void appendBox(ResourceBox newResourceBox)
    {
        int boxesCountTemp = boxesCount;
        if (boxesCount < maxBoxesCount)
        {
            foreach (var boxesCol in boxesWarehouseList)
            {
                if (boxesCol.Count < maxColBoxes)
                {
                    boxesCol.Add(newResourceBox);
                    boxesCount++;
                }
            }

            if (boxesCount == boxesCountTemp)
            {//the box haven't been added yet.
                boxesWarehouseList.Add(new List<ResourceBox>());

                foreach (var boxesCol in boxesWarehouseList)
                {
                    if (boxesCol.Count <6)
                    {
                        boxesCol.Add(newResourceBox);
                        boxesCount++;
                    }
                }
            }
            positionBoxInStorageLine(newResourceBox);
        }
        else
        {
            Destroy(newResourceBox.gameObject);
        }
    }

    public void positionBoxInStorageLine(ResourceBox newBox)
    {
        newBox.GetComponent<Rigidbody>().isKinematic = true;
        int colCounter = 0;
        int rowCounter = 0;
        if (boxesCount > 0)
        {
            foreach (var boxesCol in boxesWarehouseList)
            {
                foreach (var box in boxesCol)
                {
                    if (box == newBox)
                    {
                        positionNewBox(newBox, colCounter, rowCounter);
                    }
                    rowCounter++;
                }
                rowCounter = 0;
                colCounter++;
            }
        }

    }
    public void positionNewBox(ResourceBox box, int col, int row)
    {
        Vector3 newPos=Vector3.zero;
        switch (row)
        {
            case 0:
                if (col > 0)
                {
                    newPos = boxesWarehouseList[col - 1][row].transform.position;
                    newPos.z += (boxesWarehouseList[col - 1][row].GetComponent<Renderer>().bounds.size.x);
                }
                else {
                    newPos = box.transform.position;
                }
                break;
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                newPos = boxesWarehouseList[col][row - 1].transform.position;
                newPos.y += (boxesWarehouseList[col][row - 1].GetComponent<Renderer>().bounds.size.y);
                break;
        }
        box.transform.position = newPos;
    }
}
