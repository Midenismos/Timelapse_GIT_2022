using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : Door
{
    private bool isPlayerNearby;

    [SerializeField] private GameObject interactableDoorMessage = null;
    public bool IsPlayerNearby
    {
        get { return isPlayerNearby; }
        set
        {
            if (isPlayerNearby != value)
                isPlayerNearby = value;

            interactableDoorMessage.SetActive(isPlayerNearby);
        }
    }
    public void OnPlayerNearby()
    {
        IsPlayerNearby = true;
    }
    public void OnPlayerFar()
    {
        IsPlayerNearby = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if(GameObject.Find("TimeManager").GetComponent<TimeManager>().CurrentTimeChangeType != TimeChangeType.STOP && GameObject.Find("TimeManager").GetComponent<TimeManager>().CurrentTimeChangeType != TimeChangeType.REWIND)
            if (IsPlayerNearby)
            {
                if (IsOpen)
                    CloseDoor();
                else
                    OpenDoor();
            }

        }
    }
}
