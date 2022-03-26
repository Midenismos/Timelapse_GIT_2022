using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapeScript : MonoBehaviour
{
    [SerializeField]private AudioClip[] sounds = new AudioClip[4];
    public AudioClip CurrentSound = null;

    [SerializeField] private bool isAffectedByBlueGreen = false;
    private void Awake()
    {
        //Change l'audio en fonction des nébuleuse
        GameObject.Find("LoopManager").GetComponent<NewLoopManager>().ReactedToNebuleuse += delegate (NebuleuseType NebuleuseType)
        {
            switch (NebuleuseType)
            {
                case (NebuleuseType.PURPLE1):
                    CurrentSound = sounds[1];
                    break;
                case (NebuleuseType.PURPLE2):
                    CurrentSound = sounds[1];
                    break;
                case (NebuleuseType.BLUE):
                    if(isAffectedByBlueGreen)
                        CurrentSound = sounds[2];
                    else
                        CurrentSound = sounds[0];
                    break;
                case (NebuleuseType.GREEN):
                    if (isAffectedByBlueGreen)
                        CurrentSound = sounds[3];
                    else
                        CurrentSound = sounds[0];
                    break;
                default:
                    CurrentSound = sounds[0];
                    break;
            }
            if (GameObject.Find("Radio").GetComponent<TapeListener>().CurrentTape == this)
                GameObject.Find("Radio").GetComponent<TapeListener>().ChangeSound();
        };

        CurrentSound = sounds[1];
    }
    private void OnMouseDown()
    {
        AudioSource Radio = GameObject.Find("Radio").GetComponent<AudioSource>();

        if (Radio.clip == CurrentSound)
        {
            Radio.Stop();
            Radio.GetComponent<TapeListener>().CurrentTape = null;
            Radio.clip = null;
            GetComponent<ZoomScript>().enabled = true;
        }
    }


}
