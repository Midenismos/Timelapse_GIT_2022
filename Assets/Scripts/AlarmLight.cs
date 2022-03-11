using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmLight : MonoBehaviour
{
    private GameObject _lightBulb;
    public GameObject LightBulb
    {
        get
        { return _lightBulb; }
        set
        {
            if (value != _lightBulb)
            {
                _lightBulb = value;
            }
        }
    }

    
    private bool _isActivated = false;
    public bool IsActivated
    {
        get
        { return _isActivated; }
        set
        {
            if (value != _isActivated)
            {
                _isActivated = value;
            }
        }
    }
    [Header("Vitesse de rotation de la lumière")]
    [SerializeField] private float _speed = 2f;

    public float Speed
    {
        get
        { return _speed; }
        set
        {
            if (value != _speed)
            {
                _speed = value;
            }
        }
    }

    private TimeManager _timeManager;

    private Light[] _lights;

    [Header("Gérer l'intensité de la lumière ici directement")]
    [SerializeField] float _lightIntensity = 15f;

    private void Awake()
    {
        _lightBulb = transform.GetChild(0).gameObject;
        _lights = GetComponentsInChildren<Light>();
        //Trouve le TimeManager
        _timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();

        //Désactive les interactions avec cet objet si on passe dans une Nébuleuse
        _timeManager.GetComponent<TimeManager>().ReactedToNebuleuse += delegate (bool isInNebuleuse)
        {
            _isActivated = isInNebuleuse ? true : false;
        };
    }

    // Update is called once per frame
    void Update()
    {

        if (!_timeManager.rewindManager.isRewinding)
        {
            if (_isActivated)
            {
                if (_timeManager.multiplier != 0)
                    _lightBulb.transform.Rotate(0, _speed * Time.deltaTime, 0, 0);
            }
        }

        if (_isActivated)
        {
            foreach (Light light in _lights)
                light.intensity = _lightIntensity;
        }
        else
        {
            foreach (Light light in _lights)
                light.intensity = 0;
        }
    }
}
