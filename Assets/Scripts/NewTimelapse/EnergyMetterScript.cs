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

    private NewLoopManager loopManager = null;

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
                if(_energy >= loopManager.LoopDuration-1)
                {
                    if (ReactedToEnergy != null)
                        ReactedToEnergy();
                }

            }
        }
    }

    public delegate void ReactToEnergy();
    public event ReactToEnergy ReactedToEnergy;

    // Start is called before the first frame update
    void Start()
    {
        loopManager = FindObjectOfType<NewLoopManager>();
        if (!loopManager) Debug.LogError("EnergyMetter needs a loopManager to function");

        _slider.maxValue = loopManager.LoopDuration;
        player = GameObject.Find("Player").GetComponent<PlayerAxisScript>();
        _currentRotation = _Rotations[player.IDCurrentAxis];
        _currentPosition = _Positions[player.IDCurrentAxis];
    }

    // Update is called once per frame
    void Update()
    {
        Energy = loopManager.CurrentLoopTime;

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
}
