using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocFixer : MonoBehaviour
{
    public void FixDoc( GameObject writtenDoc)
    {
        writtenDoc.GetComponent<ZoomScript>().PutBackFixedWrittenDoc();
        //writtenDoc.GetComponent<DragObjects>().OnMouseUp();
       // GameObject.Find("Player").GetComponent<PlayerAxisScript>().IsDraging = false;
        writtenDoc.GetComponent<DragObjects>().IsDragable = false;
        writtenDoc.GetComponent<Rigidbody>().isKinematic = true;
        //break;
        /*if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().IsDraging)
        {
            foreach (GameObject writtenDoc in GameObject.FindGameObjectsWithTag("Written"))
            {
                if (writtenDoc.GetComponent<DragObjects>())
                {
                    if (writtenDoc.GetComponent<DragObjects>().IsDragged && writtenDoc.GetComponent<DragObjects>().isFixable && writtenDoc.GetComponent<ZoomScript>()._isFixedButDragable)
                    {

                    }
                }
            }
        }*/

    }
}
