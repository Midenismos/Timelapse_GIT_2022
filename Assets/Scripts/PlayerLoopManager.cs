using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoopManager : MonoBehaviour
{
    [Header("Durée de la boucle")]
    [SerializeField] private float playerLoopDuration = 1800;
    public float currentPlayerLoopTime = 0;

    private TimeManager timeManager = null;

    [SerializeField] private int minutesAddedPerChange = 5;


    IEnumerator IncrementLoop()
    {
        while (currentPlayerLoopTime < playerLoopDuration)
        {
            currentPlayerLoopTime += 1;
            yield return new WaitForSeconds(1f);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        minutesAddedPerChange *= 60;
        StartCoroutine(IncrementLoop());
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPlayerLoopTime >= playerLoopDuration)
        {
            StopCoroutine(IncrementLoop());
            timeManager.RestartLoop();
        }
    }

    public void AddMinutesToTimer()
    {
        currentPlayerLoopTime += minutesAddedPerChange;
    }

    public float PlayerLoopDuration { get { return playerLoopDuration; } }
}
