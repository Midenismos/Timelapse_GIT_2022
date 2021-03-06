using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;


public class CamManager : MonoBehaviour
{

    [SerializeField] private VideoPlayer[] _cams;
    [SerializeField] private VideoPlayer _minimap;

    [SerializeField] private bool _isRewinding = false;
    [SerializeField] private Slider _slider;
    public bool IsSliderClicked = false;

    [SerializeField] private OnOffButton _camOnOffButton = null;
    [SerializeField] private OnOffButton _minimapOnOffButton = null;

    private RectTransform _sliderTransform = null;
    [SerializeField] private Vector3[] _Rotations;
    [SerializeField] private Vector3[] _Positions;
    private Vector3 _sliderCurrentRotation;
    private Vector3 _sliderCurrentPosition;
    private Vector3 _targetRotation;
    private Vector3 _targetPosition;
    [SerializeField] private float _moveLerp = 0;
    [SerializeField] private float _rotationCountdown = 1;
    [SerializeField] private float _speed = 0.2f;
    private bool _isLerping = false;
    private PlayerAxisScript player;
    [SerializeField] private float smooth;
    public bool isActivated = true;


    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerAxisScript>();
        _sliderTransform = _slider.transform.parent.gameObject.GetComponent<RectTransform>();
        _slider.maxValue = (float)_cams[0].clip.length;

        GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>().ReactedToEnergy += delegate ()
        {
            foreach (VideoPlayer cam in _cams)
            {
                cam.Stop();
                _minimap.Stop();
            }
            isActivated = false;
        };
    }
    private void Update()
    {
        //Rembobine les vidéos
        if(_isRewinding)
        {
            foreach(VideoPlayer cam in _cams)
                cam.time = _cams[0].time - 1;
            _minimap.time = _cams[0].time - 1;
        }
        if(!IsSliderClicked)
        {
            if(_camOnOffButton.IsActivated)
                _slider.value = (float)_cams[0].time;
            else if(_minimapOnOffButton.IsActivated)
                _slider.value = (float)_minimap.time;

        }

        if ((float)_cams[0].time <= _slider.value+0.1f && (float)_cams[0].time >= _slider.value - 0.1f)
            IsSliderClicked = false;

        //Gère le lerp des sons lors d'un changement temporel

        if (_isLerping)
        {
            _rotationCountdown = Mathf.Clamp(_rotationCountdown - Time.unscaledDeltaTime * _speed * smooth, 0f, 1f);

            if (_rotationCountdown == 0)
            {
                _isLerping = false;
            }
            _sliderTransform.position = Vector3.Lerp(_sliderCurrentPosition, _targetPosition, _moveLerp);
            _sliderTransform.rotation = Quaternion.Slerp(Quaternion.Euler(_sliderCurrentRotation), Quaternion.Euler(_targetRotation), _moveLerp);
            smooth = Mathf.Clamp(_rotationCountdown + 0.25f, 0.25f, 1f);
            _moveLerp = (1f - _rotationCountdown);

        }
    }
    public void Rewind()
    {
        _isRewinding = true;
        foreach (VideoPlayer cam in _cams)
            cam.playbackSpeed = 0;
        _minimap.playbackSpeed = 0;
    }
    public void Stop()
    {
        _isRewinding = false;
        foreach (VideoPlayer cam in _cams)
            cam.playbackSpeed = 0;
        _minimap.playbackSpeed = 0;
    }
    public void Normal()
    {
        _isRewinding = false;
        foreach (VideoPlayer cam in _cams)
            cam.playbackSpeed = 1;
        _minimap.playbackSpeed = 1;
    }
    public void Slow()
    {
        _isRewinding = false;
        foreach (VideoPlayer cam in _cams)
            cam.playbackSpeed = 0.5f;
        _minimap.playbackSpeed = 0.5f;
    }
    public void Accelerate()
    {
        _isRewinding = false;
        foreach (VideoPlayer cam in _cams)
            cam.playbackSpeed = 3;
        _minimap.playbackSpeed = 3;
    }

    public void ChangeTime()
    {
        foreach (VideoPlayer cam in _cams)
        {
            cam.time = _slider.value;
            _minimap.time = _slider.value;

            if (_camOnOffButton.IsActivated)
                cam.Play();
            if (_minimapOnOffButton.IsActivated)
                _minimap.Play();

        }

    }

    public void StopSlider()
    {
        print(player.IDCurrentAxis);
        if (player.IDCurrentAxis == 5 || player.IDCurrentAxis == 4)
        {
            foreach (VideoPlayer cam in _cams)
                cam.Pause();
            _minimap.Pause();
            IsSliderClicked = true;
        }


    }
    public void ReactivateSlider()
    {
        ChangeTime();
    }

    /*public void OnOff()
    {
        if(_camOnOffButton.IsActivated == false)
        {
            foreach (VideoPlayer cam in _cams)
            {
                cam.Stop();
            }
            GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>().HowManyMachineActivated -= 1;
        }
        else
        {
            foreach (VideoPlayer cam in _cams)
            {
                cam.time = _slider.value;
                cam.Play();
            }
            GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>().HowManyMachineActivated += 1;
        }

        if (_minimapOnOffButton.IsActivated == false)
        {
            _minimap.Stop();
            GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>().HowManyMachineActivated -= 1;
        }
        else
        {
            _minimap.time = _slider.value;
            _minimap.Play();
            GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>().HowManyMachineActivated += 1;
        }


    }*/

    //Déplacer le slider en fonction de si le joueur est face au cam ou la minimap
    public void StartSliderLerp()
    {
        if(player.IDCurrentAxis == 5)
        {
            _targetRotation = _Rotations[0];
            _targetPosition = _Positions[0];
        }
        else if (player.IDCurrentAxis == 4)
        {
            _targetRotation = _Rotations[1];
            _targetPosition = _Positions[1];
        }
        _rotationCountdown = 1;
        _moveLerp = 0;
        _isLerping = true;
        _sliderCurrentRotation = _sliderTransform.rotation.eulerAngles;
        _sliderCurrentPosition = _sliderTransform.position;
        smooth = 1;
    }
}

