using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TapeListener : MonoBehaviour
{
    [SerializeField]private GameObject tapeReceiver = null;
    //[SerializeField]private Slider _slider = null;
    [SerializeField] private bool isActivated = true;
    public OnOffButton _onOffButton = null;
    public TapeScript CurrentTape = null;
    private bool isAvailable = true;
    [SerializeField] private Slider sliderAudio = null;
    private bool IsSliderClicked = false;
    [SerializeField] private TMP_Text _tapeText = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tape" && isAvailable && other.GetComponent<DragObjects>().IsDragged)
        {
            if(GetComponent<AudioSource>().clip == null)
            {
                other.GetComponent<DragObjects>().IsDragable = false;
                other.GetComponent<Rigidbody>().isKinematic = true;
                other.transform.position = tapeReceiver.transform.position;
                other.transform.rotation = tapeReceiver.transform.rotation;
                CurrentTape = other.GetComponent<TapeScript>();
                other.GetComponent<ZoomScript>().IsZoomable = false;
                int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
                CurrentTape.gameObject.layer = LayerIgnoreRaycast;
                GetComponent<AudioSource>().clip = CurrentTape.CurrentSound;
                NormalAudio();
                GetComponent<AudioSource>().time = 0;
                sliderAudio.maxValue = GetComponent<AudioSource>().clip.length;
                if (isActivated && _onOffButton.IsActivated)
                    GetComponent<AudioSource>().Play();
            }
        }
    }
    private void Update()
    {
        if (!IsSliderClicked && CurrentTape != null)
        {
            sliderAudio.value = (float)GetComponent<AudioSource>().time;
        }

    }
    public void StopSlider()
    {
        IsSliderClicked = true;
    }
    public void ReactivateSlider()
    {
        IsSliderClicked = false;
        ChangeTime();
    }
    public void StartCooldown()
    {
        StartCoroutine(Cooldown());
    }
    public void RewindAudio()
    {
        GetComponent<AudioSource>().pitch = -1;
        _tapeText.text = "REWIND";
    }

    public void StopAudio()
    {
        GetComponent<AudioSource>().pitch = 0;
        _tapeText.text = "PAUSE";

    }
    public void AccelerateAudio()
    {
        GetComponent<AudioSource>().pitch = 2;
        _tapeText.text = "FAST";
    }
    public void SlowAudio()
    {
        GetComponent<AudioSource>().pitch = 0.5f;
        _tapeText.text = "SLOW";
    }
    public void NormalAudio()
    {
        GetComponent<AudioSource>().pitch = 1;
        _tapeText.text = "PLAY";
    }
    private void Awake()
    {
        GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>().ReactedToEnergy += delegate ()
        {
            GetComponent<AudioSource>().Stop();
            isActivated = false;
        };
        GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>().ReactedToEnergyReset += delegate ()
        {
            isActivated = true;
        };
    }

    public void ChangeTime()
    {
        GetComponent<AudioSource>().time = sliderAudio.value;
    }
    public void ChangeSound()
    {
        if(_onOffButton.IsActivated)
        {
            //Change l'audio en fonction des nébuleuse
            float time = GetComponent<AudioSource>().time;
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().clip = CurrentTape.CurrentSound;
            GetComponent<AudioSource>().Play();
            GetComponent<AudioSource>().time = time;
        }

    }

    public void OnOff()
    {
        if (_onOffButton.IsActivated == false)
        {
            GetComponent<AudioSource>().Stop();
            GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>().HowManyMachineActivated -= 1;
        }
        else
        {
            GetComponent<AudioSource>().Play();
            GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>().HowManyMachineActivated += 1;
        }
    }

    IEnumerator Cooldown()
    {
        isAvailable = false;
        yield return new WaitForSeconds(1f);
        isAvailable = true;
    }

    public void EjectTape()
    {
        if(CurrentTape != null)
        {
            AudioSource AudioSource = GetComponent<AudioSource>();
            AudioSource.Stop();
            int LayerIgnoreRaycast = LayerMask.NameToLayer("Tape");
            CurrentTape.gameObject.layer = LayerIgnoreRaycast;
            CurrentTape.GetComponent<Rigidbody>().isKinematic = false;
            CurrentTape.GetComponent<ZoomScript>().IsZoomable = true;
            CurrentTape.GetComponent<Rigidbody>().AddForce(1000, 100, 1000);
            CurrentTape.GetComponent<DragObjects>().IsDragable = false;
            CurrentTape = null;
            AudioSource.clip = null;
            StartCooldown();
        }

    }
}
