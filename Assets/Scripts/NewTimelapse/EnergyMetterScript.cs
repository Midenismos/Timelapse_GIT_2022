using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyMetterScript : MonoBehaviour
{
    [SerializeField] private Vector3[] _Rotations;
    [SerializeField] private Vector3[] _Positions;
    private Vector3 _currentRotation;
    private Vector3 _currentPosition;
    [SerializeField] private float _moveLerp = 0;
    [SerializeField] private float _rotationCountdown = 1;
    [SerializeField] private float _speed = 0.2f;
    private bool _isLerping = false;
    private PlayerAxisScript player;
    [SerializeField] private float smooth;

    [SerializeField] private Slider _slider;
    [SerializeField] private Image _sliderImage;
    [SerializeField] public float MaxEnergy;
    [SerializeField] private float _baseDecreaseValue;
    [SerializeField] private float _decreaseValuePerMachine = 0.5f;
    public int HowManyMachineActivated = 0;
    private float _decreaseValueWithSpeed = 0.2f;
    private float _energy;
    private bool isAvailable = true;

    public BatteryScript CurrentBattery = null;
    public float Energy
    {
        get
        { return _energy; }
        set
        {
            if (value != _energy)
            {
                _energy = value;
                if(_energy <= 0)
                {
                    if (ReactedToEnergy != null)
                        ReactedToEnergy();
                    _sliderImage.enabled = false;
                }

            }
        }
    }

    public delegate void ReactToEnergy();
    public event ReactToEnergy ReactedToEnergy;
    public delegate void ReactToEnergyReset();
    public event ReactToEnergyReset ReactedToEnergyReset;
    public Coroutine co = null;

    [SerializeField] GameObject TIBarPosition = null;

    // Start is called before the first frame update
    void Start()
    {
        TIBarPosition = GameObject.Find("TIEnergyBarPosition");
        _slider.maxValue = MaxEnergy;
        player = GameObject.Find("Player").GetComponent<PlayerAxisScript>();
        _currentRotation = _Rotations[player.IDCurrentAxis];
        _currentPosition = _Positions[player.IDCurrentAxis];
        HowManyMachineActivated = 0;
        //Energy = 0;
        //ReactedToEnergy();
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentBattery)
        {
            CurrentBattery.transform.position = GameObject.Find("BatteryPosition").transform.position;
            CurrentBattery.transform.rotation = GameObject.Find("BatteryPosition").transform.rotation;
            Energy = CurrentBattery.Energy;
        }
        _slider.value = Energy;

        if (Input.GetKeyDown("d") || Input.GetKeyDown("q") || Input.GetKeyDown("z"))
        {
            _rotationCountdown = 1;
            _moveLerp = 0;
            _isLerping = true;
            _currentRotation = transform.rotation.eulerAngles;
            _currentPosition = transform.position;
            smooth = 1;
        }
        if(Input.GetKeyDown("s"))
        {
            _rotationCountdown = 1;
            _moveLerp = 0;
            _isLerping = true;
            _currentRotation = transform.rotation.eulerAngles;
            _currentPosition = transform.position;
            smooth = 1;
        }

        //Change la quantité d'énergie consommée en fonctionne de la vitesse.
        if (GameObject.Find("LoopManager").GetComponent<NewLoopManager>().Speed == SpeedType.FAST)
            _decreaseValueWithSpeed = 0.5f;
        else
            _decreaseValueWithSpeed = 0.2f;


        //Gère le lerp des sons lors d'un changement temporel

        if (_isLerping)
        {
            _rotationCountdown = Mathf.Clamp(_rotationCountdown - Time.unscaledDeltaTime * _speed * smooth, 0f, 1f);

            if (_rotationCountdown == 0)
            {
                _isLerping = false;
            }
            if(!player.IsInTI)
            {
                transform.position = Vector3.Lerp(_currentPosition, _Positions[player.IDCurrentAxis], _moveLerp);
                transform.rotation = Quaternion.Slerp(Quaternion.Euler(_currentRotation), Quaternion.Euler(_Rotations[player.IDCurrentAxis]), _moveLerp);
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1,1,1), _moveLerp);
            }
            else
            {
                transform.position = Vector3.Lerp(_currentPosition, TIBarPosition.transform.position, _moveLerp);
                transform.rotation = Quaternion.Slerp(Quaternion.Euler(_currentRotation), TIBarPosition.transform.rotation, _moveLerp);
                transform.localScale = Vector3.Lerp(transform.localScale, TIBarPosition.transform.localScale, _moveLerp);
            }
            smooth = Mathf.Clamp(_rotationCountdown+0.25f, 0.25f, 1f);
            _moveLerp = (1f - _rotationCountdown);
            
        }
    }

    //Baisse la quantité d'energie en fonction de la présence de la Nébuleuse Jaunne, des machines et de la vitesse du vaisseau
    private IEnumerator DecreaseEnergy()
    {
        while (Energy > 0)
        {
            if(CurrentBattery)
                CurrentBattery.Energy -= _baseDecreaseValue + (_decreaseValuePerMachine * HowManyMachineActivated) +_decreaseValueWithSpeed;
            /*print("Energy =" + Energy);
            print("_baseDecreaseValue =" + _baseDecreaseValue);
            print("_yellowNebuleuseMultiplier =" + _yellowNebuleuseMultiplier);
            print("_decreaseValuePerMachine =" + _decreaseValuePerMachine);
            print("HowManyMachineActivated =" + HowManyMachineActivated);
            print("_decreaseValueWithSpeed =" + _decreaseValueWithSpeed);*/

            yield return new WaitForSeconds(1f);
        }
        if (Energy <= 0)
            StopCoroutine(co);
    }

    /*public void ResetEnergy()
    {
        if(Energy >= MaxEnergy)
        {
            HowManyMachineActivated = 0;
            Energy = 0;
            ReactedToEnergyReset();
            StopCoroutine(co);
            co = StartCoroutine(DecreaseEnergy());
            ResetButton.SetActive(false);
        }
    }*/

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Battery" && CurrentBattery == null && isAvailable)
        {
            HowManyMachineActivated = 0;
            other.GetComponent<BatteryScript>().isPluged = true;
            other.GetComponent<DragObjects>().IsDragable = false;
            other.GetComponent<Rigidbody>().isKinematic = true;
            CurrentBattery = other.GetComponent<BatteryScript>();
            Energy = CurrentBattery.Energy;
            if(CurrentBattery.Energy>0)
            {
                ReactedToEnergyReset();
                co = StartCoroutine(DecreaseEnergy());
                _sliderImage.enabled = true;
            }



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
        StartCoroutine(Cooldown());
    }
}
