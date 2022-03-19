using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class CamManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer[] _cams;
    [SerializeField] private bool _isRewinding = false;

    private void Update()
    {
        //Rembobine les vidéos
        if(_isRewinding)
        {
            foreach(VideoPlayer cam in _cams)
                cam.time = _cams[0].time - 1;
        }
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
}

