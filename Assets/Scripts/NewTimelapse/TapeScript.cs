using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapeScript : MonoBehaviour
{
    [Header("(si isAffectedByBlueGreen) 2: influence Bleue, 3: influence Verte")]
    [Header("0: Normal, 1: influence Violette")]
    [Header("Mettre les sons dans l'ordre")]
    [SerializeField]private AudioClip[] sounds = new AudioClip[4];
    [HideInInspector]public AudioClip CurrentSound = null;

    [Header("Cocher ici si le son est infuencé par les Nébuleuses bleues et vertes")]
    [SerializeField] private bool isAffectedByBlueGreen = false;

    private float timerFree = 0;
    private bool clicked = false;
    private float clickStart;
    private void Awake()
    {
        //Change l'audio en fonction des nébuleuse
        GameObject.Find("LoopManager").GetComponent<NewLoopManager>().ReactedToNebuleuse += delegate (NebuleuseType NebuleuseType)
        {
            switch (NebuleuseType)
            {
                case (NebuleuseType.PURPLE1):
                    if(sounds[1] != null)
                        CurrentSound = sounds[1];
                    break;
                case (NebuleuseType.PURPLE2):
                    if (sounds[1] != null)
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

        CurrentSound = sounds[0];
    }
    private void OnMouseDown()
    {
        AudioSource Radio = GameObject.Find("Radio").GetComponent<AudioSource>();

        if (Radio.clip == CurrentSound)
        {
            Radio.Stop();
            Radio.GetComponent<TapeListener>().CurrentTape = null;
            Radio.clip = null;
            GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>().HowManyMachineActivated -= 1;
        }
        clicked = true;
        clickStart = Time.time;
    }
    private void OnMouseUp()
    {
        clicked = false;
        if ((Time.time - clickStart) < 0.31f)
        {
            GetComponent<ZoomScript>().IsZoomable = true;
            clickStart = -1;
        }
    }

    private void Update()
    {
        AudioSource Radio = GameObject.Find("Radio").GetComponent<AudioSource>();
        if (Radio.clip == CurrentSound)
            GetComponent<ZoomScript>().IsZoomable = false;

    }

}
