using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : Rewindable, ITimeStoppable
{
    [SerializeField] private Animator animator = null;
    [SerializeField] private string cardName = null;


    public UnityEvent OnDoorOpened = null;


    private bool isOpen = false;

    public bool IsOpen
    {
        get { return isOpen; }
        set
        {
            if (value != isOpen)
                isOpen = value;

            if (isOpen)
            {
                animator.SetBool("character_nearby", true);
                animator.SetTrigger("Open");
                OnDoorOpened?.Invoke();
            }
            else
            {
                animator.SetBool("character_nearby", false);
            }
        }
    }

    public override void Start()
    {
        base.Start();
        //TimeManager timeManager = FindObjectOfType<TimeManager>();
        //if (timeManager)
        //    timeManager.RegisterTimeStoppable(this);
    }
    public void StartTimeStop()
    {
        animator.enabled = false;
    }

    public void EndTimeStop()
    {
        animator.enabled = true;
    }


    public override void StartRewind(float timestamp)
    {
        base.StartRewind(timestamp);
        //animator.SetBool("isRewinding", true);
    }

    public override void EndRewind()
    {
        base.EndRewind();
        //animator.SetBool("isRewinding", false);
    }

    private void OnDestroy()
    {
        TimeManager timeManager = FindObjectOfType<TimeManager>();
        if (timeManager)
            timeManager.RegisterTimeStoppable(this);
    }

    // ouvre et ferme la porte
    public void OpenDoor()
    {
        IsOpen = true;

    }
    public void CloseDoor()
    {
        IsOpen = false;
    }

    // vérifie que le joueur possède la bonne carte d'accès
    public void ScanCard()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (IsOpen == false)
        {
            if (player.pickup != null)
            {
                if (player.pickup.name == cardName)
                {
                    /*if (player.pickup.GetComponent<Card>().isBroken == false)
                    {

                        OpenDoor();
                        FindObjectOfType<SoundManager>().Play("AccessGranted", 0f);
                    }
                    else
                    {
                        FindObjectOfType<SoundManager>().Play("Fail", 0f);
                    }*/
                }
                else
                {
                    FindObjectOfType<SoundManager>().Play("Fail", 0f);
                }
            }
            else
            {
                FindObjectOfType<SoundManager>().Play("Fail", 0f);
            }
        }
    }
}
