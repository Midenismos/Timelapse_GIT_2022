using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BatteryScript : MonoBehaviour
{
    [SerializeField]private Slider _slider = null;
    private float _multiplier = 0;
    public bool isPluged = false;
    public bool isInBox = false;
    public float Energy = 0;
    private void Awake()
    {
        _slider.maxValue = GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>().MaxEnergy;
        GameObject.Find("LoopManager").GetComponent<NewLoopManager>().ReactedToNebuleuse += delegate (NebuleuseType NebuleuseType)
        {
            if (NebuleuseType == NebuleuseType.YELLOW)
                _multiplier = 2f;
            else
                _multiplier = -1f;
        };
        _multiplier = -1;
    }
    private void Start()
    {
        if (isPluged)
        {
            GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>().OnTriggerEnter(this.GetComponent<BoxCollider>());
        }
    }
    private void OnMouseDown()
    {
        EnergyMetterScript EnergyMetter = GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>();
        if (EnergyMetter.CurrentBattery == this && GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 3)
        {
            EnergyMetter.CurrentBattery = null;
            EnergyMetter.Energy = 0;
            EnergyMetter.StopCoroutine(EnergyMetter.co);
            isPluged = false;
            EnergyMetter.StartCooldown();
        }

        BatteryBoxScript Box = GameObject.Find("Box").GetComponent<BatteryBoxScript>();
        if (Box.CurrentBattery == this.gameObject && GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 3)
        {
            Box.CurrentBattery = null;
            isInBox = false;
            Box.StartCooldown();
        }
    }

    private void Update()
    {
        if (!isPluged && !isInBox)
            Energy = Mathf.Clamp(Energy + _multiplier * 0.01f, 0, 100) ;
        _slider.value = Energy;
    }
}
