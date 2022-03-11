using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerHandler : MonoBehaviour
{
    public UnityEvent TriggerEnter;
    public UnityEvent TriggerExit;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
            TriggerEnter?.Invoke();
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
            TriggerExit?.Invoke();
    }
}
