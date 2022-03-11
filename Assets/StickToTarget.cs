using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToTarget : MonoBehaviour
{

    [SerializeField] private Transform target = null;

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position;
    }

    private void OnValidate()
    {
        if(target)
        transform.position = target.position;
    }
}
