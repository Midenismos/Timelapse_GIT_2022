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

    private AudioClip _plugSound;
    private AudioClip _unPlugSound;
    private AudioClip _EnergyDownSound;
    public BatteryScript CurrentBattery = null;
    private bool waitBeforeTriggerSounds = false;
    private float timerBeforeTriggerSounds = 0;

    public bool Activated = false;
    public float Energy
    {
        get
        { return _energy; }
        set
        {
            if (value != _energy)
            {
                _energy = value;
                if(_energy <= 62.5f && !GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying)
                {
                    if(GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex == 33 && GameObject.Find("TutorialManager").GetComponent<Tutorial>().Dialogue33Fini)
                    {
                        GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex = 34;
                        StartCoroutine(GameObject.Find("TutorialManager").GetComponent<Tutorial>().LaunchNextDialogue(0));
                    }
                }

                if(_energy <= 0)
                {
                    GetComponent<AudioSource>().clip = _EnergyDownSound;
                    GetComponent<AudioSource>().Play();
                    GetComponent<Animator>().Play("Base Layer.EnergyMetterDown", 0);
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
        if (!GameObject.Find("TutorialManager").GetComponent<Tutorial>().activateTuto)
            Activated = true;
        _unPlugSound = Resources.Load("Sound/Snd_UnPlugBattery") as AudioClip;
        _plugSound = Resources.Load("Sound/Snd_PlugBattery") as AudioClip;
        _EnergyDownSound = Resources.Load("Sound/Snd_EnergyDown") as AudioClip;
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
        // Gère la couleur de la barre d'énergie
        if (Energy > 75 && Energy <= 250)
            _sliderImage.color = Color.Lerp(Color.yellow, Color.green, (Energy - 75) / 75);
        else if (Energy >= 0 && Energy <= 75)
            _sliderImage.color = Color.Lerp(Color.red, Color.yellow, Energy / 75);

        if (timerBeforeTriggerSounds < 5)
            timerBeforeTriggerSounds += Time.deltaTime;
        else
            waitBeforeTriggerSounds = true;
        if (CurrentBattery)
        {
            //CurrentBattery.transform.position = GameObject.Find("BatteryPosition").transform.position;
             //CurrentBattery.transform.rotation = GameObject.Find("BatteryPosition").transform.rotation;
            Energy = CurrentBattery.Energy;
        }
        _slider.value = Energy;

        if (Input.GetKeyDown("d") && player.DEnabled == true && !GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().IsTalkingTutorial)
            StartLerp();
        if (Input.GetKeyDown("q") && player.QEnabled == true && !GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().IsTalkingTutorial)
            StartLerp();
        if (Input.GetKeyDown("z") && player.ZEnabled == true && !GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().IsTalkingTutorial)
            StartLerp();

        if (Input.GetKeyDown("s") && player.SEnabled == true && !GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().IsTalkingTutorial && FindObjectOfType<PanelBasketScript>().hasScanned)
        {
            _rotationCountdown = 1;
            _moveLerp = 0;
            _isLerping = true;
            _currentRotation = transform.rotation.eulerAngles;
            _currentPosition = transform.position;
            smooth = 1;
            if (Energy != 0)
                GetComponent<Animator>().Play("Base Layer.EnergyMetterTI", 0);
        }

        //Change la quantité d'énergie consommée en fonctionne de la vitesse.
        if (GameObject.Find("LoopManager").GetComponent<NewLoopManager>().Speed == SpeedType.FAST || GameObject.Find("LoopManager").GetComponent<NewLoopManager>().Speed == SpeedType.BACKWARDFAST)
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

    public void StartLerp()
    {
        _rotationCountdown = 1;
        _moveLerp = 0;
        _isLerping = true;
        _currentRotation = transform.rotation.eulerAngles;
        _currentPosition = transform.position;
        smooth = 1;
        if (Energy != 0)
            GetComponent<Animator>().Play("Base Layer.EnergyMetter", 0);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Battery" && CurrentBattery == null && isAvailable)
        {
            HowManyMachineActivated = 0;
            other.transform.parent = GameObject.Find("BatteryPosition").transform;
            other.transform.position = GameObject.Find("BatteryPosition").transform.position;
            other.transform.rotation = GameObject.Find("BatteryPosition").transform.rotation;
            other.GetComponent<BatteryScript>().isPluged = true;
            other.GetComponent<DragObjects>().IsDragable = false;
            other.GetComponent<Rigidbody>().isKinematic = true;
            CurrentBattery = other.GetComponent<BatteryScript>();
            Energy = CurrentBattery.Energy;

            if(CurrentBattery.Energy>0)
            {
                if (waitBeforeTriggerSounds == true)
                {
                    GetComponent<AudioSource>().clip = _plugSound;
                    GetComponent<AudioSource>().Play();
                }
                GetComponent<Animator>().Play("Base Layer.EnergyMetterUp", 0);
                ReactedToEnergyReset();

                if(Activated)
                    co = StartCoroutine(DecreaseEnergy());

                _sliderImage.enabled = true;
            }
            if (GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex == 36)
            {
                if (GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying)
                    GameObject.Find("TutorialManager").GetComponent<Tutorial>().DialogueFinished();
                GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex++;
                StartCoroutine(GameObject.Find("TutorialManager").GetComponent<Tutorial>().LaunchNextDialogue(0));
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

    public void PlayUnPlugSound()
    {
        GetComponent<AudioSource>().clip = _unPlugSound;
        GetComponent<AudioSource>().Play();
    }

    public void Activate()
    {
        Activated = true;
        co = StartCoroutine(DecreaseEnergy());
    }
}
