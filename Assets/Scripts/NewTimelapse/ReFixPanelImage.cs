using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReFixPanelImage : MonoBehaviour
{
    public string Tag = null;

    private void OnTriggerEnter(Collider other)
    {
        print(other);
        if(other.GetComponent<PanelTag>().ImageTag == Tag && other.GetComponent<DragObjects>().IsDragged)
        {
            other.transform.SetParent(GetComponentInChildren<GridLayoutGroup>().transform, false);
            other.GetComponent<DragObjects>().IsDragable = false;
            other.transform.localScale = new Vector3(1, 1, 1);
            other.transform.localPosition = new Vector3(other.transform.localPosition.x, other.transform.localPosition.y, 0);
        }
    }
}
