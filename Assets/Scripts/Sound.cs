using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound 
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;
    public bool loop;


    [HideInInspector]
    public AudioSource source;

    public float newPitch;

    private bool _startedPlaying = false;
    private bool _isPaused = false;

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

    public bool TimeManipulable = true;
}
