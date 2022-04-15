using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TIEntryScript : MonoBehaviour
{
    private float _clickStart = 0;
    [SerializeField] private GameObject _fullSheet = null;
    [SerializeField] private AudioSource _openSound = null;
    [SerializeField] private AudioSource _closeSound = null;
    public void OnMouseUp()
    {
        if ((Time.time - _clickStart) < 0.3f)
        {
            _fullSheet.SetActive(!_fullSheet.activeInHierarchy);
            if (_fullSheet.activeInHierarchy)
                _openSound.Play();
            else
                _closeSound.Play();

           _clickStart = -1;
        }
    }

    public void OnMouseDown()
    {
        _clickStart = Time.time;
    }


}
