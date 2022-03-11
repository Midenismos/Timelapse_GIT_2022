using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopCutscenePlayer : MonoBehaviour
{
    [SerializeField] private float playbackDuration = 5;
    [SerializeField] private OrbitingShipMaquette maquette = null;

    private List<TimeLoopKey> keys = new List<TimeLoopKey>();
    private float loopDuration = 60;

    public Action OnFinishedPlaying = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayRecordedLoop(List<TimeLoopKey> keys, float loopDuration)
    {
        this.keys = keys;
        this.loopDuration = loopDuration;

        maquette.StandartMode = false;
        StartCoroutine(PlayRecordedLoopCor());
    }

    private IEnumerator PlayRecordedLoopCor()
    {
        float timeCounter = 0;
        float playbackPercent = 0;
        float loopUnscaledTime = 0;
        float loopStartingUnscaledTime = keys[0].unscaledTime;
        float loopUnscaledDuration = keys[keys.Count - 1].unscaledTime - keys[0].unscaledTime;

        while (timeCounter < playbackDuration)
        {
            playbackPercent = timeCounter / playbackDuration;
            loopUnscaledTime = loopStartingUnscaledTime + loopUnscaledDuration * playbackPercent;
            maquette.ChangeShipPosition(FindLoopTimeOnInterval(FindInterval(loopUnscaledTime), loopUnscaledTime), loopDuration);

            yield return null;
            timeCounter += Time.unscaledDeltaTime;
        }

        OnFinishedPlaying?.Invoke();
    }

    private float FindLoopTimeOnInterval(int intervalStart, float unscaledTime)
    {
        float unscaledMin = keys[intervalStart].unscaledTime;
        float unscaledMax = keys[intervalStart + 1].unscaledTime;

        float min = keys[intervalStart].loopTime;
        float max = keys[intervalStart + 1].loopTime;

        return Mathf.Lerp(min, max, (unscaledTime - unscaledMin) / (unscaledMax - unscaledMin));


    }

    private int FindInterval(float unscaledTime)
    {
        for (int i = 0; i < keys.Count - 1; i++)
        {
            if (!(unscaledTime < keys[i].unscaledTime || unscaledTime > keys[i + 1].unscaledTime))
            {
                return i;
            }
        }

        return 0;
    }
}
