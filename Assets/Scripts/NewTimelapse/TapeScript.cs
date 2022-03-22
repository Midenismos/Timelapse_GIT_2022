using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapeScript : MonoBehaviour
{

    public AudioClip sound = null;
    private void OnMouseDown()
    {
        AudioSource Radio = GameObject.Find("Radio").GetComponent<AudioSource>();
        if(Radio.clip == sound)
        {
            Radio.Stop();
            Radio.clip = null;
        }

    }

}
