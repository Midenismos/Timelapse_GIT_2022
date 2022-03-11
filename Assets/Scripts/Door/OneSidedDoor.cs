using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneSidedDoor : Door
{
    [SerializeField] private GameObject wrongSideTrigger = null;
    [SerializeField] private GameObject goodSideTrigger = null;
    [SerializeField] private GameObject wrongSideMessage = null;
    private bool isOnGoodSide = false;
    private bool isOnWrongSide = false;
    public bool IsOnWrongSide
    {
        get { return isOnWrongSide; }
        set
        {
            if (isOnWrongSide != value)
                isOnWrongSide = value;

            wrongSideMessage.SetActive(isOnWrongSide);
        }
    }

    public void SetGoodSideOn()
    {
        isOnGoodSide = true;
    }
    public void SetGoodSideOff()
    {
        isOnGoodSide = false;
    }
    public void SetWrongSideOn()
    {
        IsOnWrongSide = true;
    }
    public void SetWrongSideOff()
    {
        IsOnWrongSide = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (isOnGoodSide)
            {
                wrongSideTrigger.SetActive(false);
                goodSideTrigger.SetActive(false);
                OpenDoor();
            }
        }
    }
}
