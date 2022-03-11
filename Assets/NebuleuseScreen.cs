using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NebuleuseScreen : MonoBehaviour
{
    private TimeManager _timeManager = null;


    private void Awake()
    {
        _timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        _timeManager.GetComponent<TimeManager>().ReactedToNebuleuse += delegate (bool isInNebuleuse)
        {
            GetComponent<MeshRenderer>().enabled = isInNebuleuse ? true : false;
        };
    }
}
