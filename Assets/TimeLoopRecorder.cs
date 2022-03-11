using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TimeLoopKey
{
    public float unscaledTime;
    public float loopTime;

    public TimeLoopKey(float unscaledTime, float loopTime)
    {
        this.unscaledTime = unscaledTime;
        this.loopTime = loopTime;
    }
}

public class TimeLoopRecorder : MonoBehaviour
{
    private List<TimeLoopKey> keys = new List<TimeLoopKey>();

    private TimeManager timeManager = null;

    
    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        if (!timeManager) Debug.LogError("TimeLoopRecorder needs a TimeManager in scene to function");
        else
        {
            timeManager.OnTimeChange += TimeChanged;
        }

        keys.Add(new TimeLoopKey(Time.unscaledTime, timeManager.currentLoopTime));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<TimeLoopKey> EndLoop()
    {
        timeManager.OnTimeChange -= TimeChanged;

        keys.Add(new TimeLoopKey(Time.unscaledTime, timeManager.currentLoopTime));

        return keys;
    }

    //dont forget to add last key

    private void TimeChanged(TimeChangeType type)
    {
        keys.Add(new TimeLoopKey(Time.unscaledTime, timeManager.currentLoopTime));
    }

    private void OnDestroy()
    {
        if (timeManager) timeManager.OnTimeChange -= TimeChanged;
    }
}
