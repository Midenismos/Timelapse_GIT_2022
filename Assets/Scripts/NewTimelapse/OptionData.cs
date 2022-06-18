using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionData : MonoBehaviour
{


    public bool TutoActivated = true;

    public bool IAActivated = true;

    public float VolumeSliderValue = 1;

    static bool created = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (!created)
        {
            // this is the first instance - make it persist
            DontDestroyOnLoad(transform.gameObject);
            created = true;
        }
        else
        {
            // this must be a duplicate from a scene reload - DESTROY!
            Destroy(this.gameObject);
        }
    }



}
