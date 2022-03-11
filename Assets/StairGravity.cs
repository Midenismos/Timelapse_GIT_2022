using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairGravity : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Rigidbody player = other.gameObject.GetComponent<Rigidbody>();
            player.drag = -5;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Rigidbody player = other.gameObject.GetComponent<Rigidbody>();
            player.drag = -0;
        }
    }
}
