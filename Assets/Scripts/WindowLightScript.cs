using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowLightScript : MonoBehaviour
{
    private TimeManager timeManager;

    [HideInInspector] public Light _light;

    private bool _on = false;
    private float _fadeCountdown = 1f;
    [Header("Change la vitesse d'apparition ou de disparition de la lumière")]
    [SerializeField] private float _fadeSpeed = 1;
    private float _fadeLerp;

    public bool On
    {
        get
        { return _on; }
        set
        {
            if (value != _on)
            {
                _on = value;
            }
        }
    }

    public float FadeCountdown
    {
        get
        { return _fadeCountdown; }
        set
        {
            if (value != _fadeCountdown)
            {
                _fadeCountdown = value;
            }
        }
    }
    public float FadeLerp
    {
        get
        { return _fadeLerp; }
        set
        {
            if (value != _fadeLerp)
            {
                _fadeLerp = value;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Trouve le TimeManager
        timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        _light = GetComponent<Light>();
        
        // Abonne cet objet au système de phase de lumière du TimeManager et lui dit quoi faire en fonction de la phase
        timeManager.ChangedToState += delegate (PhaseState isLight)
        {
            _on = isLight == PhaseState.LIGHT ? true : false;
        };
    }

    // Update is called once per frame
    void Update()
    {
        //Gère le lerp
        if (!timeManager.rewindManager.isRewinding)
        {
            if (_on)
            {
                if (timeManager.multiplier != 0)
                    _fadeCountdown = Mathf.Clamp(_fadeCountdown - Time.deltaTime * _fadeSpeed, 0f, 1f);
            }
            else
            {
                if (timeManager.multiplier != 0)
                    _fadeCountdown = Mathf.Clamp(_fadeCountdown + Time.deltaTime * _fadeSpeed, 0f, 1f);
            }
            _light.intensity = Mathf.Lerp(0, 20, _fadeLerp);
        }
        _fadeLerp = 1f - _fadeCountdown;
    }

}
