using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFrameAnimations : MonoBehaviour
{
    [SerializeField] private int objectAxis;

    private Animator animator;
    void Start()
    {
        FindObjectOfType<PlayerAxisScript>().moveStarted += MoveStarted;
        FindObjectOfType<PlayerAxisScript>().moveEnded += MoveEnded;

        animator = GetComponent<Animator>();
    }

    private void MoveStarted(int axis)
    {
        if(axis != objectAxis)
        {
            animator.Play("frame_Disappear");
        }
    }

    private void MoveEnded(int axis)
    {
        if(axis == objectAxis)
        {
            animator.Play("frame_Appear");
        }
    }
}
