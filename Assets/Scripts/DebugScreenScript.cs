using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DebugScreenScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _txtMinuteSeconde, _txtSeconde, _txtNebuleuse, _txtPhaseLumineuse, _txtBoucleJoueur, _txtPuzzleAudioPhase;

    private TimeManager _timeManager;
    private PlayerLoopManager _playerLoop;
    private bool _isInNebuleuse;
    public bool IsInNebuleuse
    {
        get { return _isInNebuleuse; }
        set
        {
            if (_isInNebuleuse != value)
                _isInNebuleuse = value;
            switch (_isInNebuleuse)
            {
                case true:
                    _txtNebuleuse.color = Color.green;
                    break;
                case false:
                    _txtNebuleuse.color = Color.red;
                    break;
            }
        }
    }

    private string _phaseLumineuse;
    public string PhaseLumineuse
    {
        get { return _phaseLumineuse; }
        set
        {
            if(_phaseLumineuse != value)
                _phaseLumineuse = value;
            switch (_phaseLumineuse)
            {
                case "Jour":
                    _txtPhaseLumineuse.color = Color.yellow;
                    break;
                case "Nuit":
                    _txtPhaseLumineuse.color = Color.magenta;
                    break;
                default:
                    _txtPhaseLumineuse.color = Color.white;
                    break;
            }
        }
    }

    private AudioPuzzlePhase _audioPuzzlePhase;
    public AudioPuzzlePhase AudioPuzzlePhase
    {
        get { return _audioPuzzlePhase; }
        set
        {
            if (_audioPuzzlePhase != value)
                _audioPuzzlePhase = value;
            switch (_audioPuzzlePhase)
            {
                case AudioPuzzlePhase.REWIND:
                    _txtPuzzleAudioPhase.color = Color.blue;
                    break;
                case AudioPuzzlePhase.NORMAL:
                    _txtPuzzleAudioPhase.color = Color.white;
                    break;
                case AudioPuzzlePhase.SLOW:
                    _txtPuzzleAudioPhase.color = Color.yellow;
                    break;
                case AudioPuzzlePhase.SPEED:
                    _txtPuzzleAudioPhase.color = Color.red;
                    break;
                default:
                    _txtPuzzleAudioPhase.color = Color.white;
                    break;
            }
        }
    }

    private void Awake()
    {
        //Trouve le TimeManager
        _timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        _playerLoop = GameObject.Find("PlayerLoopManager").GetComponent<PlayerLoopManager>();

        _timeManager.ReactedToNebuleuse += delegate (bool isInNebuleuse)
        {
            IsInNebuleuse = isInNebuleuse ? true : false;
        };

        _timeManager.ChangedToState += delegate (PhaseState isLight)
        {
            PhaseLumineuse = isLight == PhaseState.LIGHT ? "Jour" : "Nuit";
        };

        _timeManager.ReactedToAudioPhaseChange += delegate (AudioPuzzlePhase audioPhase)
        {
            AudioPuzzlePhase = audioPhase;
        };
    }
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float timeInMinute = _timeManager.currentLoopTime / 60;
        _txtSeconde.text = "Secondes Totales : " + Math.Round(_timeManager.currentLoopTime, 2);
        _txtMinuteSeconde.text = string.Format("Temps réel : {0} Minutes {1} Secondes", Math.Truncate(timeInMinute), Math.Round((timeInMinute - (int)timeInMinute) * 60, 2));
        _txtNebuleuse.text = "Nébuleuse : " + IsInNebuleuse;
        _txtPhaseLumineuse.text = "Phase lumineuse : "+ PhaseLumineuse;
        _txtBoucleJoueur.text = "Temps restant du joueur : "+_playerLoop.currentPlayerLoopTime + "/1800";
        _txtPuzzleAudioPhase.text = "Phase Puzzle Audio : " + _timeManager.CurrentAudioPuzzlePhase;
    }
}
