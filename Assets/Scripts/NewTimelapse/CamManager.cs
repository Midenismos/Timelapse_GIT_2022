using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;


public class CamManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer[] _cams;
    [SerializeField] private bool _isRewinding = false;
    [SerializeField] private Slider _slider;
    public bool IsSliderClicked = false;

    private void Awake()
    {
        _slider.maxValue = (float)_cams[0].clip.length;
    }
    private void Update()
    {
        //Rembobine les vidéos
        if(_isRewinding)
        {
            foreach(VideoPlayer cam in _cams)
                cam.time = _cams[0].time - 1;
        }
        if(!IsSliderClicked)
        {
            _slider.value = (float)_cams[0].time;
        }


        if ((float)_cams[0].time <= _slider.value+0.1f && (float)_cams[0].time >= _slider.value - 0.1f)
            IsSliderClicked = false;
    }
    public void Rewind()
    {
        _isRewinding = true;
        foreach (VideoPlayer cam in _cams)
            cam.playbackSpeed = 0;
    }
    public void Stop()
    {
        _isRewinding = false;
        foreach (VideoPlayer cam in _cams)
            cam.playbackSpeed = 0;
    }
    public void Normal()
    {
        _isRewinding = false;
        foreach (VideoPlayer cam in _cams)
            cam.playbackSpeed = 1;
    }
    public void Slow()
    {
        _isRewinding = false;
        foreach (VideoPlayer cam in _cams)
            cam.playbackSpeed = 0.5f;
    }
    public void Accelerate()
    {
        _isRewinding = false;
        foreach (VideoPlayer cam in _cams)
            cam.playbackSpeed = 3;
    }

    public void ChangeTime()
    {
        foreach (VideoPlayer cam in _cams)
        {
            cam.time = _slider.value;
            cam.Play();
        }

    }

    public void StopSlider()
    {
        if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 1)
        {
            foreach (VideoPlayer cam in _cams)
                cam.Pause();
            IsSliderClicked = true;
        }


    }
    public void ReactivateSlider()
    {
        ChangeTime();
    }
}

