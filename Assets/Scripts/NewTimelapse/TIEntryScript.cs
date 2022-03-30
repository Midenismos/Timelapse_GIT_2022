using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TIEntryScript : MonoBehaviour
{
    private float _clickStart = 0;
    [SerializeField] private GameObject _fullSheet = null;

    public void OnMouseUp()
    {
        if ((Time.time - _clickStart) < 0.3f)
        {
            _fullSheet.SetActive(!_fullSheet.activeInHierarchy);
            _clickStart = -1;
        }
    }

    public void OnMouseDown()
    {
        _clickStart = Time.time;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
