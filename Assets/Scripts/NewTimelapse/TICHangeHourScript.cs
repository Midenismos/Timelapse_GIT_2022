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
}
