using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithTimeLoop : MonoBehaviour
{
    private TimeManager timeManager = null;
    // Start is called before the first frame update
    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        if (!timeManager) Debug.LogError("RotateWithTimeLoop component needs a TimeManger in scene to function");
    }

    private void Update()
    {
        if (timeManager)
        {
            transform.rotation = Quaternion.Euler(0, timeManager.currentLoopTime / timeManager.LoopDuration * -360f, 0);
        }
    }
}
