using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryBoxScript : MonoBehaviour
{
    [SerializeField] private Animator _animator = null;
    [SerializeField] private BoxCollider _colOpen = null;
    [SerializeField] private BoxCollider _colClose = null;
    [SerializeField] private AudioSource _colOpenSound = null;
    [SerializeField] private AudioSource _colCloseSound = null;
    [SerializeField] private AudioSource _putBatterySound = null;
    private bool isOpen = false;
    public GameObject CurrentBattery = null;
    private bool isAvailable = true;

    private void OnMouseDown()
    {
        if(isOpen)
        {
            _animator.Play("Ceilling.CloseCeilling", 0);
            _colOpen.enabled = true;
            _colClose.enabled = false;
            isOpen = false;
            _colCloseSound.Play();
        }
        else
        {
            _animator.Play("Ceilling.OpenCeilling", 0);
            _colOpen.enabled = false;
            _colClose.enabled = true;
            isOpen = true;
            _colOpenSound.Play();
        }

    }
    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void StartCooldown()
    {
        StartCoroutine(Cooldown());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Battery" && CurrentBattery == null && isOpen && isAvailable)
        {
            other.transform.SetParent(GameObject.Find("BoxBatteryPosition").transform, true);
            other.GetComponent<DragObjects>().IsDragable = false;
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.transform.position = GameObject.Find("BoxBatteryPosition").transform.position;
            other.transform.rotation = GameObject.Find("BoxBatteryPosition").transform.rotation;
            CurrentBattery = other.gameObject;
            _putBatterySound.Play();
        }
    }
    private void Update()
    {
        if (isOpen)
        {
            if (CurrentBattery)
                CurrentBattery.GetComponent<BatteryScript>().isInBox = false;
        }
        else
        {
            if (CurrentBattery)
                CurrentBattery.GetComponent<BatteryScript>().isInBox = true;
        }
    }

    IEnumerator Cooldown()
    {
        isAvailable = false;
            yield return new WaitForSeconds(1f);
        isAvailable = true;
    }


}
