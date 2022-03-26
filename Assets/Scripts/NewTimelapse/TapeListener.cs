using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapeListener : MonoBehaviour
{
    [SerializeField]private GameObject tapeReceiver = null;
    [SerializeField]private Slider _slider = null;
    private bool isActivated = true;
    public TapeScript CurrentTape = null;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tape")
        {
            if(GetComponent<AudioSource>().clip == null)
            {

                    other.GetComponent<DragObjects>().IsDragable = false;
                    other.GetComponent<Rigidbody>().isKinematic = true;
                    other.transform.position = tapeReceiver.transform.position;
                    other.transform.rotation = tapeReceiver.transform.rotation;
                if (isActivated)
                {
                    CurrentTape = other.GetComponent<TapeScript>();
                    GetComponent<AudioSource>().clip = CurrentTape.CurrentSound;
                    _slider.value = 1;
                    GetComponent<AudioSource>().time = 0;
                    GetComponent<AudioSource>().Play();
                    other.GetComponent<ZoomScript>().enabled = false;
                    GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>().HowManyMachineActivated += 1;
                }
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>().ReactedToEnergy += delegate ()
        {
            GetComponent<AudioSource>().clip = null;
            GetComponent<AudioSource>().Stop();
            isActivated = false;
        };
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<AudioSource>().pitch = _slider.value;
    }

    public void ChangeSound()
    {
        //Change l'audio en fonction des nébuleuse
        float time = GetComponent<AudioSource>().time;
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = CurrentTape.CurrentSound;
        GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().time = time;
    }
}
