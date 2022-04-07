using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CamButton : MonoBehaviour
{
    public UnityEvent onClickedCam;
    public UnityEvent onClickedAudio;
    [SerializeField] private MeshRenderer _interactFeedBack;

    private void Awake()
    {
        _interactFeedBack = transform.GetChild(1).gameObject.GetComponent<MeshRenderer>();
    }
    private void OnMouseOver()
    {
        if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 5 ||GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 4)
        {
            _interactFeedBack.enabled = true;
            if (Input.GetMouseButtonDown(0))
                onClickedCam?.Invoke(); 
        }

        if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 1)
        {
            _interactFeedBack.enabled = true;
            if (Input.GetMouseButtonDown(0))
            {
                onClickedAudio?.Invoke();
            }
        }


    }
    private void OnMouseExit()
    {
        _interactFeedBack.enabled = false;
    }
}
