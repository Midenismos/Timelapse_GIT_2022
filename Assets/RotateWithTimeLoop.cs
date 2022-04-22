using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithTimeLoop : MonoBehaviour
{
    private NewLoopManager timeManager = null;

    private float offset = 0;

    // Start is called before the first frame update
    void Start()
    {
        timeManager = FindObjectOfType<NewLoopManager>();
        if (!timeManager) Debug.LogError("RotateWithTimeLoop component needs a NewLoopManager in scene to function");
        offset = transform.eulerAngles.y;
    }

    private void Update()
    {
        if (timeManager)
        {
            transform.rotation = Quaternion.Euler(0, timeManager.CurrentLoopTime / timeManager.LoopDuration * -360f + offset, 0);
        }
    }
}
