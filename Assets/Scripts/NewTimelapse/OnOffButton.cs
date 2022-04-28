using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnOffButton : MonoBehaviour
{
    public UnityEvent onClicked;
    [SerializeField] private MeshRenderer _interactFeedBack;

    [SerializeField] private Material[] _mats = new Material[2];
    private MeshRenderer _plane = null;
    [SerializeField] private bool _isActivated = false;
    public bool IsActivated
    {
        get
        { return _isActivated; }
        set
        {
            if (value != _isActivated)
            {
                _isActivated = value;
                if (_isActivated == false)
                    _plane.material = _mats[0];
                else
                    _plane.material = _mats[1];
            }
        }
    }
    private void Awake()
    {
        _plane = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        _interactFeedBack = transform.GetChild(1).gameObject.GetComponent<MeshRenderer>();
        GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>().ReactedToEnergy += delegate ()
        {
            if(tag != "ResetEnergy")
                IsActivated = false;
        };
    }
    private void OnMouseOver()
    {
        if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 5 || GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 4 || GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 1 || tag == "ResetEnergy")
        {
            _interactFeedBack.enabled = true;
            if (Input.GetMouseButtonDown(0))
            {
                if (GameObject.Find("Console").GetComponent<ConsoleManager>().isActivated || tag == "ResetEnergy" || tag == "VaultButton")
                {
                    IsActivated = !IsActivated;
                    onClicked?.Invoke();
                }

            }
        }


    }
    private void OnMouseExit()
    {
        _interactFeedBack.enabled = false;
    }
}
