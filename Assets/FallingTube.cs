using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTube : MonoBehaviour
{
    [SerializeField] private Transform StartPosition = null;
    [SerializeField] private Transform EndPosition = null;
    [SerializeField] private Rigidbody body = null;
    [SerializeField] private AnimationCurve FallCurve;
    [SerializeField] private float FallStartTime = 0;
    [SerializeField] private float FallEndTime = 0;

    private TimeManager timeManager = null;

    // Start is called before the first frame update
    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        if (!timeManager) Debug.LogError("FallingTube Needs a TimeManager in scene to function");
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        if (timeManager)
        {
            if (timeManager.currentLoopTime <= FallStartTime)
            {
                transform.position = StartPosition.position;
            }
            else if (timeManager.currentLoopTime >= FallEndTime)
            {
                transform.position = EndPosition.position;
            }
            else
            {
                body.velocity = (Vector3.Lerp(StartPosition.position, EndPosition.position, FallCurve.Evaluate(Mathf.InverseLerp(FallStartTime, FallEndTime, timeManager.currentLoopTime))) - transform.position) / Time.deltaTime;
            }
            //if(timeManager.currentLoopTime <= 0.55f && timeManager.currentLoopTime >=  0.5f)
               // FindObjectOfType<SoundManager>().Play("Fracas", 0);
        }
    }
}
