using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CamButton : MonoBehaviour
{
    public UnityEvent onClicked;
    [SerializeField] private MeshRenderer _interactFeedBack;

    private void Awake()
    {
        _interactFeedBack = transform.GetChild(1).gameObject.GetComponent<MeshRenderer>();
    }
    private void OnMouseOver()
    {
        if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 1)
        {
            _interactFeedBack.enabled = true;
            if (Input.GetMouseButtonDown(0))
                onClicked?.Invoke(); 
        }


    }
    private void OnMouseExit()
    {
        _interactFeedBack.enabled = false;
    }
}
