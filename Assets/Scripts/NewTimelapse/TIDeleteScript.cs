using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIDeleteScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Entry")
            Destroy(other.gameObject);
    }
}
