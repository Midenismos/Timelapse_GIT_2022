using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIDeleteScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Entry" || other.tag == "PanelImage")
        {
            Destroy(other.gameObject);
            GameObject.Find("Player").GetComponent<PlayerAxisScript>().IsDraging = false;
        }
    }
}
