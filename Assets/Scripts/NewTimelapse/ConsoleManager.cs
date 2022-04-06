using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class ConsoleManager : MonoBehaviour
{

    [SerializeField] private VideoPlayer[] _cams;
    [SerializeField] private VideoPlayer _minimap;

    [SerializeField] private bool _isRewinding = false;
    [SerializeField] private Slider _slider;
    public bool IsSliderClicked = false;

    private RectTransform _sliderTransform = null;
    [SerializeField] private float _moveLerp = 0;
    [SerializeField] private float _rotationCountdown = 1;
    [SerializeField] private float _speed = 0.2f;
    [SerializeField] private float smooth;
    public bool isActivated = true;
    private VideoPlayer currentModelCam = null;

    [SerializeField] private AudioSource _tapeListener = null;
    [SerializeField] private TMP_Text _tapeText = null;

    private void Awake()
    {
        _sliderTransform = _slider.transform.parent.gameObject.GetComponent<RectTransform>();
        OnOff();
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
        for(int i = 0; i <= _cams.Length-1;i++)
        {
            if(_cams[i].isPlaying)
            {
                currentModelCam = _cams[i];
                break;
            }
        }
        if (!_cams.Any(cam => cam.isPlaying))
            currentModelCam = null;

        //Rembobine les vidéos
        if (_isRewinding)
        {
            if(!IsSliderClicked)
            {
                foreach (VideoPlayer cam in _cams)
                {
                    if (currentModelCam)
                        cam.time = currentModelCam.time - 1;
                }
                if (currentModelCam)
                    _minimap.time = currentModelCam.time - 1;
                else
                    _minimap.time -=1;
            }

        }
        if (!IsSliderClicked)
        {
            if (currentModelCam != null)
                _slider.value = (float)currentModelCam.time;
            else if (_minimap.isPlaying)
                _slider.value = (float)_minimap.time;
        }

        if(currentModelCam)
        {
            if ((float)currentModelCam.time <= _slider.value + 0.1f && (float)currentModelCam.time >= _slider.value - 0.1f)
                IsSliderClicked = false;
        }
        else if (_minimap.isPlaying)
            if ((float)_minimap.time <= _slider.value + 0.1f && (float)_minimap.time >= _slider.value - 0.1f)
                IsSliderClicked = false;


        //Gère le lerp des sons lors d'un changement temporel

        /*if (_isLerping)
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

        }*/
    }


    public void RewindAudio()
    {
        _tapeListener.pitch = -1;
        _tapeText.text = "REWIND";
    }

    public void StopAudio()
    {
        _tapeListener.pitch = 0;
        _tapeText.text = "PAUSE";

    }
    public void AccelerateAudio()
    {
        _tapeListener.pitch = 2;
        _tapeText.text = "FAST";
    }
    public void SlowAudio()
    {
        _tapeListener.pitch = 0.5f;
        _tapeText.text = "SLOW";
    }
    public void NormalAudio()
    {
        _tapeListener.pitch = 1;
        _tapeText.text = "PLAY";
    }

    public void RewindCam()
    {
        _isRewinding = true;
        foreach (VideoPlayer cam in _cams)
            cam.playbackSpeed = 0;
        _minimap.playbackSpeed = 0;
    }
    public void StopCam()
    {
        _isRewinding = false;
        foreach (VideoPlayer cam in _cams)
            cam.playbackSpeed = 0;
        _minimap.playbackSpeed = 0;
    }
    public void NormalCam()
    {
        _isRewinding = false;
        foreach (VideoPlayer cam in _cams)
            cam.playbackSpeed = 1;
        _minimap.playbackSpeed = 1;
    }
    public void SlowCam()
    {
        _isRewinding = false;
        foreach (VideoPlayer cam in _cams)
            cam.playbackSpeed = 0.5f;
        _minimap.playbackSpeed = 0.5f;
    }
    public void AccelerateCam()
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
            if(!_isRewinding)
            {
                cam.time = _slider.value;
                _minimap.time = _slider.value;
            }
            else
            {
                currentModelCam.time = _slider.value;
            }


            if (cam.GetComponent<CamScreenScript>()._onOffButton.IsActivated)
                cam.Play();
            if (_minimap.GetComponent<CamScreenScript>()._onOffButton.IsActivated)
                _minimap.Play();

        }

    }

    public void StopSlider()
    {
        foreach (VideoPlayer cam in _cams)
            cam.Pause();
        _minimap.Pause();
        IsSliderClicked = true;
    }
    public void ReactivateSlider()
    {
        ChangeTime();
    }

    public void OnOff()
    {
        /*if (_camOnOffButton.IsActivated == false)
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
        }*/


    }

    //Déplacer le slider en fonction de si le joueur est face au cam ou la minimap
    /*public void StartSliderLerp()
    {
        if (player.IDCurrentAxis == 5)
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
    }*/
}