using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAccelerateButton : MonoBehaviour
{
    [SerializeField] private MeshRenderer _interactFeedBack;

    [SerializeField] private Material[] _mats = new Material[2];
    [SerializeField] private MeshRenderer _plane = null;
    [SerializeField] private bool _isActivated = false;
    [SerializeField] private AudioSource _iaVoiceManager = null;
    [SerializeField] private float acceleratePitchNumber = 2;
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
                {
                    _iaVoiceManager.pitch = 1;
                    _plane.material = _mats[0];
                }
                else
                {
                    _iaVoiceManager.pitch = acceleratePitchNumber; 
                    _plane.material = _mats[1];
                }
            }
        }
    }
    private void OnMouseOver()
    {
        _interactFeedBack.enabled = true;
        if (Input.GetMouseButtonDown(0))
        {
            IsActivated = !IsActivated;
        }
    }
    private void OnMouseExit()
    {
        _interactFeedBack.enabled = false;
    }
}
