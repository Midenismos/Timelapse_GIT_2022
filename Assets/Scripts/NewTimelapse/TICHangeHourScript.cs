using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TICHangeHourScript : MonoBehaviour
{
    [SerializeField] private string minute = "";
    [SerializeField] private bool isTutoZone = false;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "EntryChangeHour")
        {
            if(other.GetComponentInParent<TIEntryScript>().text.text != "15 : " + minute)
                other.GetComponentInParent<TIEntryScript>().ChangeHour(minute, isTutoZone);
        }
    }

    /*private void OnMouseEnter()
    {
        GameObject DraggedEntry = null;
        foreach (DragObjects Entry in GameObject.Find("TI").transform)
        {
            if (Entry.IsDragged)
            {
                DraggedEntry = Entry.gameObject;
                break;
            }
        }
        if (DraggedEntry != null)
        {
            if (DraggedEntry.tag == "Entry" && DraggedEntry.layer == 17)
            {
                DraggedEntry.transform.SetParent(this.transform.GetChild(0).transform, false);
                DraggedEntry.GetComponent<RectTransform>().localScale = new Vector3(0.75f, 0.75f, 0.75f);
                DraggedEntry.GetComponent<DragObjects>().IsFixedInTI = true;
                DraggedEntry.GetComponent<DragObjects>().OnMouseUp();
                DraggedEntry.GetComponent<DragObjects>().OnMouseDown();
            }
        }


    }*/
}
