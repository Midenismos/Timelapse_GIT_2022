using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TICHangeHourScript : MonoBehaviour
{
    [SerializeField] private string minute = "";
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Entry")
        {
            other.GetComponentInChildren<TIEntryScript>().ChangeHour(minute);
        }
    }
}
