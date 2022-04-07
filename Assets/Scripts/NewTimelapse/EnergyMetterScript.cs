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
    [SerializeField] private float _maxEnergy;
    [SerializeField] private float _baseDecreaseValue;
    private float _yellowNebuleuseMultiplier = 1;
    [SerializeField] private float _decreaseValuePerMachine = 0.5f;
    public int HowManyMachineActivated = 0;
    private float _decreaseValueWithSpeed = 0.2f;

    [SerializeField] private GameObject ResetButton = null;

    private float _energy;
    public float Energy
    {
        get
        { return _energy; }
        set
        {
            if (value != _energy)
            {
                _energy = value;
                if(_energy >= _maxEnergy)
                {
                    if (ReactedToEnergy != null)
                        ReactedToEnergy();
                    ResetButton.SetActive(true);
                }

            }
        }
    }

    public delegate void ReactToEnergy();
    public event ReactToEnergy ReactedToEnergy;
    public delegate void ReactToEnergyReset();
    public event ReactToEnergyReset ReactedToEnergyReset;

    private void Awake()
    {
        //Accélére la perte d'énergie en cas de Nebuleuse Jaune.
        GameObject.Find("LoopManager").GetComponent<NewLoopManager>().ReactedToNebuleuse += delegate (NebuleuseType NebuleuseType)
        {
            if (NebuleuseType == NebuleuseType.YELLOW)
                _yellowNebuleuseMultiplier = 5;
            else
                _yellowNebuleuseMultiplier = 1;
        };
        StartCoroutine(DecreaseEnergy());


    }
    // Start is called before the first frame update
    void Start()
    {
        _slider.maxValue = _maxEnergy;
        player = GameObject.Find("Player").GetComponent<PlayerAxisScript>();
        _currentRotation = _Rotations[player.IDCurrentAxis];
        _currentPosition = _Positions[player.IDCurrentAxis];
    }

    // Update is called once per frame
    void Update()
    {
        _slider.value = Energy;

        if (Input.GetKeyDown("d") || Input.GetKeyDown("q"))
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
            transform.position = Vector3.Lerp(_currentPosition, _Positions[player.IDCurrentAxis], _moveLerp);
            transform.rotation = Quaternion.Slerp(Quaternion.Euler(_currentRotation), Quaternion.Euler(_Rotations[player.IDCurrentAxis]), _moveLerp);
            smooth = Mathf.Clamp(_rotationCountdown+0.25f, 0.25f, 1f);
            _moveLerp = (1f - _rotationCountdown);
            
        }
    }

    //Baisse la quantité d'energie en fonction de la présence de la Nébuleuse Jaunne, des machines et de la vitesse du vaisseau
    private IEnumerator DecreaseEnergy()
    {
        while (Energy < _maxEnergy)
        {
            Energy += _baseDecreaseValue * _yellowNebuleuseMultiplier + (_decreaseValuePerMachine * HowManyMachineActivated) +_decreaseValueWithSpeed;
            yield return new WaitForSeconds(1f);
        }
    }

    public void ResetEnergy()
    {
        if(Energy >= _maxEnergy)
        {
            Energy = 0;
            ReactedToEnergyReset();
            StartCoroutine(DecreaseEnergy());
            ResetButton.SetActive(false);
        }


    }
}
