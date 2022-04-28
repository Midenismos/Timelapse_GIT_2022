using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultPlug : MonoBehaviour
{

    public BatteryScript CurrentBattery = null;

    private bool isAvailable = true;

    public bool isOn = false;

    [SerializeField] private GameObject[] cables = null;

    [SerializeField] private Material _activatedMaterial = null;
    [SerializeField] private Material _deactivatedMaterial = null;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Battery" && CurrentBattery == null && isAvailable)
        {
            other.GetComponent<BatteryScript>().isVaultPluged = true;
            other.GetComponent<DragObjects>().IsDragable = false;
            other.GetComponent<Rigidbody>().isKinematic = true;
            CurrentBattery = other.GetComponent<BatteryScript>();
            CurrentBattery.transform.position = GameObject.Find("VaultBatteryPosition").transform.position;
            CurrentBattery.transform.rotation = GameObject.Find("VaultBatteryPosition").transform.rotation;
            isOn = true;
            foreach(GameObject cable in cables)
                cable.GetComponent<MeshRenderer>().material = _activatedMaterial;
        }
    }

    IEnumerator Cooldown()
    {
        isAvailable = false;
        yield return new WaitForSeconds(1f);
        isAvailable = true;
    }
    public void StartCooldown()
    {
        isOn = false;
        StartCoroutine(Cooldown());
        foreach (GameObject cable in cables)
            cable.GetComponent<MeshRenderer>().material = _deactivatedMaterial;
    }
}
