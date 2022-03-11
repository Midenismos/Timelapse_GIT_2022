using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableLight : MonoBehaviour
{
    [SerializeField] private bool _isWriten = false;
    [SerializeField] private bool _isScreen = false;
    private TimeManager _timeManager = null;
    private Light _light = null;

    private void Awake()
    {
        _light = GetComponent<Light>();
        _timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        if(_isScreen == true)
        {
            _timeManager.ReactedToNebuleuse += delegate (bool isInNebuleuse)
            {
                _light.enabled = isInNebuleuse ? false : true;
            };
        }

        if (_isWriten == true)
        {
            _timeManager.ChangedToState += delegate (PhaseState phase)
            {
                switch(phase)
                {
                    case (PhaseState.DARK):
                        _light.enabled = false;
                        break;
                    case (PhaseState.LIGHT):
                        _light.enabled = true;
                        break;
                }
            };
        }

    }
}
