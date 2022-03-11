using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum RadioSound
{
    ALIEN,
    STATIONMERE,
    LUNE,
    NONE,
};

[RequireComponent(typeof(AudioSource))]
public class AudioLoreScript : MonoBehaviour, IInteractable
{
    public enum SoundSpeedType
    {
        SLOW,
        SPEED,
        NORMAL
    };



    [SerializeField]private AudioSource _source = null;
    private SoundManager _sndManager = null;
    private DiegeticSound _sndScript = null;
    private TimeManager _timeManager = null;

    [SerializeField] private bool _hasChangeTimePeriod = false;
    [HideInInspector] public float ChangeTimePeriod1 = 0f;
    [HideInInspector] public float ChangeTimePeriod2 = 0f;
    [Range(1,2)]public int HowManyTimePeriod = 1;

    [HideInInspector] public SoundSpeedType SoundSpeedBeforeTimePeriod = SoundSpeedType.NORMAL;
    [HideInInspector] public SoundSpeedType SoundSpeedBetweenTimePeriods = SoundSpeedType.NORMAL;
    [HideInInspector] public SoundSpeedType SoundSpeedAfterTimePeriod = SoundSpeedType.NORMAL;

    private SoundSpeedType _soundSpeed = SoundSpeedType.NORMAL;
    [SerializeField] private RadioSound _radioSound;


    private float _puzzleAudioPitch = 0f;
    private float _phasePitchChange = 0f;

    private GameObject _walkman = null;

    private GameObject _player = null;


    public float PuzzleAudioPitch
    {
        get
        {
            return _puzzleAudioPitch;
        }
        set
        {
            if (_puzzleAudioPitch != value)
            {
                _puzzleAudioPitch = value;

                _sndManager.ChangeLorePitchWithChangeTimePeriodRewind(_source);
            }
        }
    }
    public float PhasePitchChange
    {
        get
        {
            return _phasePitchChange;
        }
        set
        {
            if (_phasePitchChange != value)
                _phasePitchChange = value;
        }
    }
    public bool HasChangeTimePeriod
    {
        get
        {
            return _hasChangeTimePeriod;
        }
        set
        {
            if (_hasChangeTimePeriod != value)
                _hasChangeTimePeriod = value;
        }
    }
    public SoundSpeedType SoundSpeed
    {
        get
        {
            return _soundSpeed;
        }
        set
        {
            if (_soundSpeed != value)
            {
                _soundSpeed = value;
                //Paramètre le changement de son
                if(_hasChangeTimePeriod)
                {
                    if(_timeManager.CurrentAudioPuzzlePhase == AudioPuzzlePhase.REWIND)
                    {
                        switch (_soundSpeed)
                        {
                            case SoundSpeedType.NORMAL:
                                PuzzleAudioPitch = _sndScript.Pitch;
                                break;
                            case SoundSpeedType.SPEED:
                                PuzzleAudioPitch = _sndScript.Pitch - _sndManager.ConstantPitchChange;
                                break;
                            case SoundSpeedType.SLOW:
                                PuzzleAudioPitch = _sndScript.Pitch + _sndManager.ConstantPitchChange;
                                break;
                            default:
                                PuzzleAudioPitch = _sndScript.Pitch;
                                break;
                        }
                    }
                    else
                    {
                        switch (_soundSpeed)
                        {
                            case SoundSpeedType.NORMAL:
                                PuzzleAudioPitch = _sndScript.Pitch;
                                break;
                            case SoundSpeedType.SPEED:
                                PuzzleAudioPitch = _sndScript.Pitch + _sndManager.ConstantPitchChange;
                                break;
                            case SoundSpeedType.SLOW:
                                PuzzleAudioPitch = _sndScript.Pitch - _sndManager.ConstantPitchChange;
                                break;
                            default:
                                PuzzleAudioPitch = _sndScript.Pitch;
                                break;
                        }
                    }

                    //Demande au SoundManager de changer le pitch de ce son
                    _sndManager.SetLorePitch();
                }
            }
        }
    }



    private void Awake()
    {
        _player = GameObject.Find("Player");
        _timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        _sndScript = GetComponent<DiegeticSound>();
        _sndManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        _source = GetComponent<AudioSource>();
        _walkman = GameObject.Find("WalkmanBackground");
        _timeManager.PlayedRadio += delegate (RadioSound radioSound)
        {
            if (radioSound == _radioSound)
            {
                if(!_source.isPlaying)
                    Interact(this.gameObject, GameObject.Find("Player").GetComponent<PlayerController>());
            }
        };


        _timeManager.ReactedToAudioPhaseChange += delegate (AudioPuzzlePhase audioPhase)
        {
            //Change la variable PhasePitchChange

            if (_timeManager.rewindManager.isRewinding)
            {
                switch(audioPhase)
                {
                    case AudioPuzzlePhase.NORMAL:
                        PhasePitchChange = 0f;
                        break;
                    case AudioPuzzlePhase.REWIND:
                        PhasePitchChange = 0f;
                        break;
                    case AudioPuzzlePhase.SLOW:
                        PhasePitchChange = +_sndManager.ConstantPitchChange;
                        break;
                    case AudioPuzzlePhase.SPEED:
                        PhasePitchChange = -_sndManager.ConstantPitchChange;
                        break;
                }

            }
            else
            {
                switch (audioPhase)
                {
                    case AudioPuzzlePhase.NORMAL:
                        PhasePitchChange = 0f;
                        break;
                    case AudioPuzzlePhase.REWIND:
                        PhasePitchChange = 0f;
                        break;
                    case AudioPuzzlePhase.SLOW:
                        PhasePitchChange = -_sndManager.ConstantPitchChange;
                        break;
                    case AudioPuzzlePhase.SPEED:
                        PhasePitchChange = +_sndManager.ConstantPitchChange;
                        break;
                }
            }
            switch(audioPhase)
            {
                //Inverse le pitch en fonction de la phase Audio
                case AudioPuzzlePhase.NORMAL:
                    if (_sndScript.Pitch < 0 && !_sndScript._isAReversedSound)
                        _sndScript.Pitch = -_sndScript.Pitch;
                    else if (_sndScript.Pitch >= 0 && _sndScript._isAReversedSound)
                        _sndScript.Pitch = -_sndScript.Pitch;
                    break;
                case AudioPuzzlePhase.REWIND:
                    PhasePitchChange = 0f;
                    if (_sndScript.Pitch >= 0 && !_sndScript._isAReversedSound)
                        _sndScript.Pitch = -_sndScript.Pitch;
                    else if (_sndScript.Pitch < 0 && _sndScript._isAReversedSound)
                        _sndScript.Pitch = -_sndScript.Pitch;
                    break;
                case AudioPuzzlePhase.SLOW:
                    if (_sndScript.Pitch < 0 && !_sndScript._isAReversedSound)
                        _sndScript.Pitch = -_sndScript.Pitch;
                    else if (_sndScript.Pitch >= 0 && _sndScript._isAReversedSound)
                        _sndScript.Pitch = -_sndScript.Pitch;
                    break;
                case AudioPuzzlePhase.SPEED:
                    if (_sndScript.Pitch < 0 && !_sndScript._isAReversedSound)
                        _sndScript.Pitch = -_sndScript.Pitch;
                    else if (_sndScript.Pitch >= 0 && _sndScript._isAReversedSound)
                        _sndScript.Pitch = -_sndScript.Pitch;
                    break;
            }

            if(HasChangeTimePeriod)
            {
                if (audioPhase == AudioPuzzlePhase.REWIND)
                {
                    switch (_soundSpeed)
                    {
                        case SoundSpeedType.NORMAL:
                            PuzzleAudioPitch = _sndScript.Pitch;
                            break;
                        case SoundSpeedType.SPEED:
                            PuzzleAudioPitch = _sndScript.Pitch - _sndManager.ConstantPitchChange;
                            break;
                        case SoundSpeedType.SLOW:
                            PuzzleAudioPitch = _sndScript.Pitch + _sndManager.ConstantPitchChange;
                            break;
                        default:
                            PuzzleAudioPitch = _sndScript.Pitch;
                            break;
                    }
                }
                else
                {
                    switch (_soundSpeed)
                    {
                        case SoundSpeedType.NORMAL:
                            PuzzleAudioPitch = _sndScript.Pitch;
                            break;
                        case SoundSpeedType.SPEED:
                            PuzzleAudioPitch = _sndScript.Pitch + _sndManager.ConstantPitchChange;
                            break;
                        case SoundSpeedType.SLOW:
                            PuzzleAudioPitch = _sndScript.Pitch - _sndManager.ConstantPitchChange;
                            break;
                        default:
                            PuzzleAudioPitch = _sndScript.Pitch;
                            break;
                    }
                }
            }
            _sndManager.SetLorePitch();
        };
    }


    private void Update()
    {

        //print(Vector3.Distance(transform.position, _player.transform.position));
        //Baisse le volume de la musique lorsque ce son est joué à proximité du joueur
        if (_source.isPlaying)
        {
            if (Vector3.Distance(transform.position, _player.transform.position) <= 20)
            {
                if(_walkman.GetComponent<TimeWalkman>().ImportantSound == null)
                {
                    _walkman.GetComponent<TimeWalkman>().WalkmanSoundLerp = 0;
                    _walkman.GetComponent<TimeWalkman>().ImportantSound = _source;
                }
            }
        }
        else 
        {
            if (_walkman.GetComponent<TimeWalkman>().ImportantSound != null)
            {
                if (!_walkman.GetComponent<TimeWalkman>().ImportantSound.isPlaying)
                {
                    _walkman.GetComponent<TimeWalkman>().WalkmanSoundLerp = 0;
                    _walkman.GetComponent<TimeWalkman>().ImportantSound = null;
                }
                if (_walkman.GetComponent<TimeWalkman>().ImportantSound != _sndManager.FindSoundSource("Alerte") && _walkman.GetComponent<TimeWalkman>().ImportantSound != _sndManager.FindSoundSource("EndAlerte") && _walkman.GetComponent<TimeWalkman>().ImportantSound != null)
                {
                    if ((Vector3.Distance(_walkman.GetComponent<TimeWalkman>().ImportantSound.transform.position, _player.transform.position) > 20))
                    {
                        _walkman.GetComponent<TimeWalkman>().WalkmanSoundLerp = 0;
                        _walkman.GetComponent<TimeWalkman>().ImportantSound = null;
                    }
                }
            }
        }


        if (_hasChangeTimePeriod == true && _source.isPlaying)
        {
            if(HowManyTimePeriod == 1)
            {
                if (_source.time <= ChangeTimePeriod1)
                    SoundSpeed = SoundSpeedBeforeTimePeriod;
                else
                    SoundSpeed = SoundSpeedAfterTimePeriod;
            }
            else if (HowManyTimePeriod == 2)
            {
                if (_source.time <= ChangeTimePeriod1)
                    SoundSpeed = SoundSpeedBeforeTimePeriod;
                else if (_source.time > ChangeTimePeriod1 && _source.time <= ChangeTimePeriod2)
                    SoundSpeed = SoundSpeedBetweenTimePeriods;
                if (_source.time > ChangeTimePeriod2)
                    SoundSpeed = SoundSpeedAfterTimePeriod;
            }

        }

    }

    public void PlayerHoverStart()
    {

    }

    // Est appellé par le RewindSoundManager régler le début de la piste de sons en fonction de si le son est inversé de base et si la période de son est le Rewind
    public void StartDuringRewind()
    {
        if(GetComponent<DiegeticSound>()._isAReversedSound)
        {
            if (_timeManager.CurrentAudioPuzzlePhase == AudioPuzzlePhase.REWIND)
                _source.time = _source.clip.length - 0.01f;
            else
                _source.time = _source.clip.length - _source.clip.length;
        }
        else
        {
            if (_timeManager.CurrentAudioPuzzlePhase == AudioPuzzlePhase.REWIND)
                _source.time = _source.clip.length - _source.clip.length;
            else
                _source.time = _source.clip.length - 0.01f;
        }
      

        _source.Play();
    }

    public void PlayerHoverEnd()
    {

    }

    public void Interact(GameObject pickup, PlayerController player)
    {
        _source.pitch = _sndScript.Pitch;
        _sndScript.NewPitch = _sndScript.Pitch + PhasePitchChange;
        _sndManager.ChangePitch(_timeManager.CurrentTimeChangeType);
        _sndManager.CheckLoreSound(); // Coupe les autres sons d'informations.

        if (_source.pitch < 0)
        {
            _source.time = _source.clip.length - 0.01f; // Fait en sorte que le son se joue à partir de la fin s'il est inversé de base comme les puzzles rapports d'expérience
        }
        else
        {
            _source.time = _source.clip.length - _source.clip.length;
        }
        if (!_timeManager.rewindManager.isRewinding && _timeManager.multiplier != 0)
            _source.Play();
    }

    public void InteractHolding(GameObject pickup, PlayerController player)
    {

    }

    public void StopInteractHolding(GameObject pickup, PlayerController player)
    {

    }
}
