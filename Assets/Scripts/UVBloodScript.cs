using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVBloodScript : MonoBehaviour
{
    [Header("Glisser ici tout les UVBloodCrystal du prefab")]
    [SerializeField] public GameObject[] Crystals;
    [Space]

    private TimeManager _timeManager;

    private bool _visible = false;
    private float _fadeCountdown = 1f;
    [Header("Change la vitesse d'apparition ou de disparition des cristaux")]
    [SerializeField] private float _fadeSpeed = 1;
    private float _fadeLerp;

    
    private Light _crystalLight = null;
    private float _lightIntensity = 0;

    [SerializeField]private Color _color;
    

    public bool Visible
    {
        get
        { return _visible; }
        set
        {
            if (value != _visible)
            {
                _visible = value;
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

    public float LightIntensity
    {
        get
        { return _lightIntensity; }
        set
        {
            if (value != _lightIntensity)
            {
                _lightIntensity = value;
            }
        }
    }
    public Color Color
    {
        get
        { return _color; }
        set
        {
            if (value != _color)
            {
                _color = value;
            }
        }
    }

    private void Awake()
    {
        _crystalLight = GetComponentInChildren<Light>();
        _timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        Color = Crystals[0].GetComponent<MeshRenderer>().material.color;
        // Abonne cet objet au système de phase de lumière du TimeManager et lui dit quoi faire en fonction de la phase
        _timeManager.ChangedToState += delegate (PhaseState isLight)
        {
            _visible = isLight == PhaseState.DARK ? true : false;
        };

    }

    // Update is called once per frame
    void Update()
    {

        _crystalLight.intensity = LightIntensity;
        //Gère le lerp
        if (!_timeManager.rewindManager.isRewinding)
        {
            if (_visible)
            {
                if (_timeManager.multiplier != 0)
                    _fadeCountdown = Mathf.Clamp(_fadeCountdown - Time.deltaTime * _fadeSpeed, 0f, 1f);
            }
            else
            {
                if (_timeManager.multiplier != 0)
                _fadeCountdown = Mathf.Clamp(_fadeCountdown + Time.deltaTime * _fadeSpeed, 0f, 1f);
            }
            LightIntensity = Mathf.Lerp(0, 5, _fadeLerp);
            Color = new Color(Color.r, Color.g, Color.b, Mathf.Lerp(0, 1f, _fadeLerp));
        }
        _fadeLerp = 1f - _fadeCountdown;
        foreach (GameObject crystal in Crystals)
        {
            crystal.GetComponent<MeshRenderer>().material.color = Color;
        }
    }
}
