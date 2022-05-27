using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultScript : MonoBehaviour
{
    [SerializeField] private Animator _animator = null;
    [SerializeField] private BoxCollider[] _colOpen = null;
    [SerializeField] private BoxCollider _colClose = null;
    [SerializeField] private AudioSource _vaultOpenSound = null;
    [SerializeField] private AudioSource _vaultCloseSound = null;
    private bool isOpen = false;
    private bool waitBeforeTriggerSounds = false;
    private float timerBeforeTriggerSounds = 0;

    private void OnMouseDown()
    {
        if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 4)
        { 
            if (GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>().Energy <= 0)
            {
                if (isOpen && !GameObject.Find("Player").GetComponent<PlayerAxisScript>().HasItem)
                    CloseDoor();
                else
                {
                    _vaultOpenSound.Play();
                    _animator.Play("Door.VaultOpen", 0);
                    foreach (BoxCollider col in _colOpen)
                        col.enabled = true;
                    _colClose.enabled = false;
                    isOpen = true;
                }
            }
            else
            {
                GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().LaunchRandomAntiCasierDialogue();
            }
        }



    }
    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Battery" && isOpen)
        {
            other.transform.SetParent(GameObject.Find("BoxBatteryPosition").transform, true);
            other.GetComponent<DragObjects>().IsDragable = false;
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.transform.position = GameObject.Find("BoxBatteryPosition").transform.position;
            other.transform.rotation = GameObject.Find("BoxBatteryPosition").transform.rotation;
        }
    }

    public void CloseDoor()
    {
        _animator.Play("Door.VaultClose", 0);
        foreach (BoxCollider col in _colOpen)
            col.enabled = false;

        _colClose.enabled = true;
        isOpen = false;
    }

    private void Update()
    {
        if (timerBeforeTriggerSounds < 5)
            timerBeforeTriggerSounds += Time.deltaTime;
        else
            waitBeforeTriggerSounds = true;
        if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis != 4)
        {
            if (isOpen)
                CloseDoor();
        }
    }

    public void PlayCloseDoorSound()
    {
        if(waitBeforeTriggerSounds == true)
            _vaultCloseSound.Play();
    }
}