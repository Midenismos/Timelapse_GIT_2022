using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultScript : MonoBehaviour
{
    [SerializeField] private Animator _animator = null;
    [SerializeField] private BoxCollider[] _colOpen = null;
    [SerializeField] private BoxCollider _colClose = null;
    private bool isOpen = false;

    private void OnMouseDown()
    {
        if(GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>().Energy <=0 && GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 4)
        {
            if (isOpen)
            {
                CloseDoor();
            }
            else
            {
                _animator.Play("Door.VaultOpen", 0);
                foreach (BoxCollider col in _colOpen)
                    col.enabled = true;
                _colClose.enabled = false;
                isOpen = true;
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
        if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis != 4)
        {
            if (isOpen)
                CloseDoor();
        }
    }
}