using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SheetImageScript : MonoBehaviour
{
    [SerializeField] private string _imageTag = null;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PanelImage")
        {
            if (transform.childCount == 0)
            {
                if( _imageTag == other.GetComponent<PanelTag>().ImageTag)
                {
                    other.transform.SetParent(this.transform, false);
                    other.transform.position = this.transform.position;
                    other.GetComponent<DragObjects>().IsDragable = false;
                }

            }
        }
    }
}
