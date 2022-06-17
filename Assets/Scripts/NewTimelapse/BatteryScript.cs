using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BatteryScript : MonoBehaviour
{
    [SerializeField]private Slider _slider = null;
    private float _multiplier = 0;
    public bool isPluged = false;
    public bool isVaultPluged = false;
    public bool isInBox = false;
    [SerializeField] private Image _sliderImage;
    public bool Activated = false;

    public float Energy
    {
        get
        { return _energy; }
        set
        {
            if (value != _energy)
                _energy = value;
        }
    }
    [SerializeField]private float _energy;
    private void Awake()
    {
        if (!GameObject.Find("TutorialManager").GetComponent<Tutorial>().activateTuto)
            Activated = true;
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

        if (isInBox)
        {
            GameObject.Find("NewBatteryBox").GetComponent<BatteryBoxScript>().OnTriggerEnter(this.GetComponent<BoxCollider>());
        }
    }
    private void OnMouseDown()
    {
        if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().CanClick && !GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().IsTalkingTutorial)
        {
            EnergyMetterScript EnergyMetter = GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>();
            if (EnergyMetter.CurrentBattery == this && GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 3)
            {
                EnergyMetter.CurrentBattery = null;
                EnergyMetter.Energy = 0;
                EnergyMetter.StopCoroutine(EnergyMetter.co);
                isPluged = false;
                gameObject.transform.parent = GameObject.Find("Batteries").transform;
                EnergyMetter.StartCooldown();
                EnergyMetter.PlayUnPlugSound();
                transform.localScale = new Vector3(0.2048901f, 0.12f, 0.28f);
            }

            BatteryBoxScript Box = GameObject.Find("NewBatteryBox").GetComponent<BatteryBoxScript>();
            if (Box.CurrentBattery == this.gameObject && GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 3)
            {
                Box.CurrentBattery = null;
                isInBox = false;
                Box.StartCooldown();
            }

            VaultPlug Plug = GameObject.Find("VaultPlug").GetComponent<VaultPlug>();
            if (Plug.CurrentBattery == this && GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 3)
            {
                Plug.CurrentBattery.isVaultPluged = false;
                Plug.CurrentBattery = null;
                Plug.StartCooldown();
            }
        }

    }

    private void Update()
    {
        if(Activated)
        {
            if (!isPluged && !isInBox && !isVaultPluged)
                Energy = Mathf.Clamp(Energy + _multiplier * 0.01f, 0, 250);
            else if (isVaultPluged)
                Energy = Mathf.Clamp(Energy - 0.01f, 0, 250);

            if (isPluged)
                _sliderImage.enabled = false;
            else
            {
                if (Energy > 0)
                    _sliderImage.enabled = true;
            }

            _slider.value = Energy;
        }

    }
    public void Activate()
    {
        Activated = true;
    }
}
