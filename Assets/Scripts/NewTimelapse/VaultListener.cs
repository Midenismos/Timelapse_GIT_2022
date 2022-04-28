using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultListener : MonoBehaviour
{
    private bool isActivated = false;
    [SerializeField] private OnOffButton _onOffButton = null;

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis != 4)
        {
            if (isActivated)
                StopSound();
        }
        VaultPlug plug = GameObject.Find("VaultPlug").GetComponent<VaultPlug>();
        if (!plug.isOn || plug.CurrentBattery.Energy <= 0)
        {
            _onOffButton.IsActivated = false;
            StopSound();
        }
    }

    public void SoundOnOff()
    {
        VaultPlug plug = GameObject.Find("VaultPlug").GetComponent<VaultPlug>();
        if (plug.isOn && plug.CurrentBattery.Energy >0)
        {
            if(_onOffButton.IsActivated)
            {
                isActivated = true;
                GetComponent<AudioSource>().Play();
            }
            else
                StopSound();
        }

    }

    public void StopSound()
    {
        GetComponent<AudioSource>().Stop();
        isActivated = false;
    }
}
