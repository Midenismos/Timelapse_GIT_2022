using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SheetImageScript : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PanelImage")
        {
            other.transform.SetParent(this.transform, false);
            other.transform.position = this.transform.position;
            other.GetComponent<DragObjects>().IsDragable = false;
        }
    }
}
