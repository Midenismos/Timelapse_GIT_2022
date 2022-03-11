using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiegeticSound : MonoBehaviour
{
    [SerializeField] private float _newPitch;
    [SerializeField] private float _pitch;

    private bool _startedPlaying = false;
    private bool _isPaused = false;

    public bool _isAReversedSound = false;

    public bool StartedPlaying
    {
        get { return _startedPlaying; }
        set
        {
            if (_startedPlaying != value)
            {
                _startedPlaying = value;
            }
        }
    }

    public bool IsPaused
    {
        get { return _isPaused; }
        set
        {
            if (_isPaused != value)
            {
                _isPaused = value;
            }
        }
    }

    public float Pitch
    {
        get { return _pitch; }
        set
        {
            if (_pitch != value)
            {
                _pitch = value;
            }
            if(GetComponent<AudioLoreScript>())
                GameObject.Find("SoundManager").GetComponent<SoundManager>().SetLorePitch();

        }
    }
    public float NewPitch
    {
        get { return _newPitch; }
        set
        {
            if (_newPitch != value)
            {
                _newPitch = value;
            }
            if (GetComponent<AudioLoreScript>())
                GameObject.Find("SoundManager").GetComponent<SoundManager>().ChangeLorePitchWithChangeTimePeriodRewind(GetComponent<AudioSource>());
        }
    }

    public bool TimeManipulable = true;

    private void Awake()
    {
        //Pitch = GetComponent<AudioSource>().pitch;
        _newPitch = Pitch;
    }
    private void Start()
    {
        GetComponent<AudioSource>().pitch = Pitch;

    }


}
