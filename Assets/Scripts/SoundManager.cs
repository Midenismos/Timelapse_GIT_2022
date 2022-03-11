using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Linq;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;

    public Sound[] SpookyRandomSounds;

    public static SoundManager instance;

    [HideInInspector]
    public GameObject[] _loreWithAudios;

    private TimeManager _timeManager;

    public delegate void RegisterNonDiegeticSound(string sound, float timeSound);
    public event RegisterNonDiegeticSound RegisteredNonDiegeticSound;

    public delegate void RegisterDiegeticSound(AudioSource source, float timeSound);
    public event RegisterDiegeticSound RegisteredDiegeticSound;

    private bool _reversed = false;

    [SerializeField] private float _fadeCountdown = 1f;
    [SerializeField] private float _fadeSpeed = 0.2f;
    [SerializeField] private float _fadeLerp;
    [SerializeField] private bool _lerping = false;

    [SerializeField] private AudioSource[] _diegeticSounds = null;

    [SerializeField] private float _constantPitchChange = 0.5f;

    private GameObject _walkman = null;
    private AudioMixerGroup _walkmanMixer = null;

    private float _randomSpookySoundTimerMax = 0f;
    private float _randomSpookySoundTimer = 0f;

    public float ConstantPitchChange
    {
        get { return _constantPitchChange; }
        set
        {
            if (_constantPitchChange != value)
                _constantPitchChange = value;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        //Trouve le TimeManager
        _timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();

        // Liste toutes les informations à la fois écrits et audios.
        _loreWithAudios = GameObject.FindGameObjectsWithTag("LoreAudio");

        _walkman = GameObject.Find("WalkmanBackground");

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        foreach (Sound s in SpookyRandomSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        _timeManager.GetComponent<TimeManager>().ReactedToNebuleuse += delegate (bool isInNebuleuse)
        {
            if (isInNebuleuse)
                Play("Alerte", 0f);
            else
                if (_timeManager.currentLoopTime !=0)
                    Play("EndAlerte", 0f);
        };


        _diegeticSounds = FindObjectsOfType<AudioSource>().Where(f => f.gameObject.GetInstanceID() != this.gameObject.GetInstanceID()).ToArray(); ;

        _randomSpookySoundTimerMax = UnityEngine.Random.Range(30f, 90f);
    }

    // Joue le son
    public void Play(string name, float timeSound)
    {
        float startingTime;
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        s.source.Play();

        // Si on Rewind, le son se lance ou moment où il s'est arrêté en temps normal
        if (_timeManager.rewindManager.isRewinding)
            startingTime = timeSound;
        else
            startingTime = 0;
        s.source.time = startingTime;
        s.StartedPlaying = true;
    }

    public void PlaySpookySound(string name, float timeSound)
    {
        float startingTime;
        Sound s = Array.Find(SpookyRandomSounds, Sound => Sound.name == name);
        s.source.Play();

        // Si on Rewind, le son se lance ou moment où il s'est arrêté en temps normal
        if (_timeManager.rewindManager.isRewinding)
            startingTime = timeSound;
        else
            startingTime = 0;
        s.source.time = startingTime;
        s.StartedPlaying = true;
    }

    // Change le pitch du son au hasard
    public void RandomisePitch(string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        s.source.pitch = UnityEngine.Random.Range(s.pitch - 0.1f, s.pitch + 0.1f);
    }

    // Stoppe le son joué en loop
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        if(s.TimeManipulable)
            RegisteredNonDiegeticSound(name, s.source.time);

        s.StartedPlaying = false;
        s.source.Stop();
    }

    public void StopDiegeticSound(AudioSource sound)
    {
        // A utiliser absolument si on arrête manuellement un son diégétique
        RegisteredDiegeticSound(sound, sound.time);
        sound.Stop();
    }

    // Trouve le son si on a besoin de paramètres associés dans un autre script
    public AudioClip FindSoundClip(string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        return s.source.clip;
    }

    public AudioSource FindSoundSource(string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        return s.source;
    }

    public void CheckLoreSound()
    {
        foreach (GameObject withAudio in _loreWithAudios)
        {
            if (withAudio.GetComponent<AudioSource>().isPlaying)
                withAudio.GetComponent<AudioSource>().Stop();
        }
    }
    private void Update()
    {
        //Lance des sons flippants aléatoire à intervalle de temps aléatoire
        _randomSpookySoundTimer += Time.unscaledDeltaTime;
        if(_randomSpookySoundTimer >= _randomSpookySoundTimerMax)
        {
            //PlaySpookySound(SpookyRandomSounds[UnityEngine.Random.Range(0, SpookyRandomSounds.Length)].name, 0);
            _randomSpookySoundTimer = 0f;
            _randomSpookySoundTimerMax = UnityEngine.Random.Range(30f, 90f);
        }

        //Baisse le son du walkman lorsqu'il y a une Alarme à Nébuleuse
        if (FindSoundSource("Alerte").isPlaying)
        {
            if (_walkman.GetComponent<TimeWalkman>().ImportantSound == null)
            {
                _walkman.GetComponent<TimeWalkman>().WalkmanSoundLerp = 0;
                _walkman.GetComponent<TimeWalkman>().ImportantSound = FindSoundSource("Alerte");
            }
        }
        else if (FindSoundSource("EndAlerte").isPlaying)
        {
            if (_walkman.GetComponent<TimeWalkman>().ImportantSound == null)
            {
                _walkman.GetComponent<TimeWalkman>().WalkmanSoundLerp = 0;
                _walkman.GetComponent<TimeWalkman>().ImportantSound = FindSoundSource("EndAlerte");
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
            }
        }


        //Gère le lerp des sons lors d'un changement temporel
        if (!_timeManager.rewindManager.isRewinding)
        {
            if (_lerping)
            {
                if (_timeManager.multiplier != 0)
                    _fadeCountdown = Mathf.Clamp(_fadeCountdown - Time.unscaledDeltaTime * _fadeSpeed, 0f, 1f);

                if (_fadeCountdown == 0)
                    _lerping = false;

                foreach (Sound sound in sounds)
                    sound.source.pitch = Mathf.Lerp(sound.source.pitch, sound.newPitch, _fadeLerp);

                foreach (AudioSource diegeticSound in _diegeticSounds)
                {
                    if(diegeticSound.GetComponent<DiegeticSound>())
                    {
                        diegeticSound.pitch = Mathf.Lerp(diegeticSound.pitch, diegeticSound.GetComponent<DiegeticSound>().NewPitch, _fadeLerp);
                    }
                }
            }
        }

        _fadeLerp = 1f - _fadeCountdown;


        // Met le son à l'envers en cas de rewind
        if (_timeManager.rewindManager.isRewinding)
        {
            if(_reversed == false)
            {
                foreach (Sound sound in sounds)
                {
                    if (sound.TimeManipulable)
                        sound.source.pitch = -sound.pitch;
                }
                foreach (AudioSource diegeticSound in _diegeticSounds)
                {
                    if (diegeticSound.GetComponent<DiegeticSound>())
                    {
                        //Met le son à l'envers à l'exception de ceux utilisant  AudioLoreScript
                        if (!diegeticSound.GetComponent<AudioLoreScript>())
                            diegeticSound.pitch = -diegeticSound.GetComponent<DiegeticSound>().Pitch;

                        else if (diegeticSound.GetComponent<AudioLoreScript>() && !diegeticSound.GetComponent<AudioLoreScript>().HasChangeTimePeriod) // Fait le rewind en fonction des modifications apportés par les phases Audios
                            diegeticSound.pitch = -diegeticSound.GetComponent<DiegeticSound>().NewPitch;

                        else if (diegeticSound.GetComponent<AudioLoreScript>() && diegeticSound.GetComponent<AudioLoreScript>().HasChangeTimePeriod) // Variante pour les sons changeant de pitch durant la lecture comme certains Puzzle Audios
                            ChangeLorePitchWithChangeTimePeriodRewind(diegeticSound);
                    }

                }
                _reversed = true;
            }
        }
        else
        {
            if (_reversed == true)
            {
                foreach (Sound sound in sounds)
                {
                    if (sound.TimeManipulable)
                        sound.source.pitch = -sound.source.pitch;
                }
                foreach (AudioSource diegeticSound in _diegeticSounds)
                {
                    if (diegeticSound.GetComponent<DiegeticSound>())
                    {
                        //Met le son à l'envers à l'exception de ceux utilisant  AudioLoreScript
                        if (!diegeticSound.GetComponent<AudioLoreScript>())
                            diegeticSound.pitch = -diegeticSound.GetComponent<DiegeticSound>().Pitch;

                        else if (diegeticSound.GetComponent<AudioLoreScript>() && !diegeticSound.GetComponent<AudioLoreScript>().HasChangeTimePeriod) // Fait le rewind en fonction des modifications apportés par les phases Audios
                            diegeticSound.pitch = -diegeticSound.GetComponent<DiegeticSound>().NewPitch;

                        else if (diegeticSound.GetComponent<AudioLoreScript>() && diegeticSound.GetComponent<AudioLoreScript>().HasChangeTimePeriod) // Variante pour les sons changeant de pitch durant la lecture comme certains Puzzle Audios
                            ChangeLorePitchWithChangeTimePeriodRewind(diegeticSound);
                    }
                }
                _reversed = false;
            }
        }

        //Enregistre les sons ayant prit fin dans le rewind
        foreach (Sound sound in sounds)
        {
            if (sound.StartedPlaying == true)
            {
                if (!sound.source.isPlaying)
                {
                    if(!_timeManager.rewindManager.isRewinding && !sound.IsPaused && sound.source.time != 0)
                    {
                        if (sound.TimeManipulable)
                            RegisteredNonDiegeticSound(sound.name, sound.source.time);
                        sound.StartedPlaying = false;
                    }
                }
            }
        }

        foreach (AudioSource diegeticSound in _diegeticSounds)
        {
            if(diegeticSound.GetComponent<DiegeticSound>())
            {
                if (diegeticSound.isPlaying)
                    diegeticSound.GetComponent<DiegeticSound>().StartedPlaying = true;

                if (diegeticSound.GetComponent<DiegeticSound>().StartedPlaying == true)
                {
                    if (!diegeticSound.isPlaying)
                    {
                        if (!diegeticSound.GetComponent<DiegeticSound>()._isAReversedSound) // Enregistre différement le son si le son se joue déjà en sens inverse (comme les puzzles Rapports d'expérience)
                        {
                            if (!_timeManager.rewindManager.isRewinding && !diegeticSound.GetComponent<DiegeticSound>().IsPaused && diegeticSound.time != 0)
                            {

                                if (diegeticSound.GetComponent<DiegeticSound>().TimeManipulable)
                                    RegisteredDiegeticSound(diegeticSound, diegeticSound.time);

                                diegeticSound.GetComponent<DiegeticSound>().StartedPlaying = false;
                            }
                        }
                        else
                        {
                            if (!_timeManager.rewindManager.isRewinding && !diegeticSound.GetComponent<DiegeticSound>().IsPaused && diegeticSound.time == 0)
                            {
                                if (diegeticSound.GetComponent<DiegeticSound>().TimeManipulable)
                                    RegisteredDiegeticSound(diegeticSound, diegeticSound.time);

                                diegeticSound.GetComponent<DiegeticSound>().StartedPlaying = false;
                            }
                        }

                    }
                }
            }

        }
    }


    public void ChangePitch(TimeChangeType Type)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.TimeManipulable)
            {
                //Change le pitch en fonction des changements de temps
                switch (Type)
                {
                    case TimeChangeType.SLOW:
                        sound.newPitch = sound.pitch - ConstantPitchChange;
                        break;
                    case TimeChangeType.NORMAL:
                        sound.newPitch = sound.pitch;
                        break;
                    case TimeChangeType.SPEED:
                        sound.newPitch = sound.pitch + ConstantPitchChange;
                        break;
                    case TimeChangeType.REWIND:
                        sound.source.pitch = sound.pitch;
                        break;

                    default:
                        sound.newPitch = sound.pitch;
                        break;
                }
            }
            else
            {
                sound.newPitch = sound.pitch;
            }

        }

        foreach (AudioSource diegeticSound in _diegeticSounds)
        {
            if(diegeticSound.GetComponent<DiegeticSound>())
            {
                if (diegeticSound.GetComponent<DiegeticSound>().TimeManipulable)
                {
                    var diegeticSoundScript = diegeticSound.GetComponent<DiegeticSound>();
                    if (diegeticSound.GetComponent<AudioLoreScript>())
                    {
                        var audioLoreScript = diegeticSound.GetComponent<AudioLoreScript>();
                        if (diegeticSound.GetComponent<AudioLoreScript>().HasChangeTimePeriod)
                        {
                            //Change le pitch en fonction des changements de temps, du changement de pitch du puzzle et de la phase Audio pour les puzzles audio avec HasChangeTimePeriod cochée

                            switch (Type)
                            {
                                case TimeChangeType.SLOW:
                                    if (_timeManager.CurrentAudioPuzzlePhase == AudioPuzzlePhase.REWIND)
                                        diegeticSoundScript.NewPitch = audioLoreScript.PuzzleAudioPitch + ConstantPitchChange + audioLoreScript.PhasePitchChange;
                                    else
                                        diegeticSoundScript.NewPitch = audioLoreScript.PuzzleAudioPitch - ConstantPitchChange + audioLoreScript.PhasePitchChange;
                                    break;
                                case TimeChangeType.NORMAL:
                                    diegeticSoundScript.NewPitch = audioLoreScript.PuzzleAudioPitch + audioLoreScript.PhasePitchChange;
                                    break;
                                case TimeChangeType.SPEED:
                                    if (_timeManager.CurrentAudioPuzzlePhase == AudioPuzzlePhase.REWIND)
                                        diegeticSoundScript.NewPitch = audioLoreScript.PuzzleAudioPitch - ConstantPitchChange + audioLoreScript.PhasePitchChange;
                                    else
                                        diegeticSoundScript.NewPitch = audioLoreScript.PuzzleAudioPitch + ConstantPitchChange + audioLoreScript.PhasePitchChange;
                                    break;
                                case TimeChangeType.REWIND:
                                    diegeticSoundScript.NewPitch = audioLoreScript.PuzzleAudioPitch + audioLoreScript.PhasePitchChange; ;
                                    break;
                                default:
                                    diegeticSoundScript.NewPitch = audioLoreScript.PuzzleAudioPitch + audioLoreScript.PhasePitchChange;
                                    break;
                            }

                        }
                        else
                        {
                            //Change le pitch en fonction des changements de temps, et de la phase Audio pour les puzzles audio avec HasChangeTimePeriod décochée
                            switch (Type)
                            {
                                case TimeChangeType.SLOW:
                                    if (diegeticSoundScript._isAReversedSound)
                                    {
                                        if (_timeManager.CurrentAudioPuzzlePhase == AudioPuzzlePhase.REWIND)
                                            diegeticSoundScript.NewPitch = diegeticSoundScript.Pitch - ConstantPitchChange - audioLoreScript.PhasePitchChange;
                                        else
                                            diegeticSoundScript.NewPitch = diegeticSoundScript.Pitch + ConstantPitchChange - audioLoreScript.PhasePitchChange;
                                    }
                                    else
                                    {
                                        if (_timeManager.CurrentAudioPuzzlePhase == AudioPuzzlePhase.REWIND)
                                            diegeticSoundScript.NewPitch = diegeticSoundScript.Pitch + ConstantPitchChange + audioLoreScript.PhasePitchChange;
                                        else
                                            diegeticSoundScript.NewPitch = diegeticSoundScript.Pitch - ConstantPitchChange + audioLoreScript.PhasePitchChange;
                                    }
                                    break;
                                case TimeChangeType.NORMAL:
                                    if (diegeticSoundScript._isAReversedSound)
                                        diegeticSoundScript.NewPitch = diegeticSoundScript.Pitch - audioLoreScript.PhasePitchChange;
                                    else
                                        diegeticSoundScript.NewPitch = diegeticSoundScript.Pitch + audioLoreScript.PhasePitchChange;
                                    break;
                                case TimeChangeType.SPEED:
                                    if (diegeticSoundScript._isAReversedSound)
                                    {
                                        if (_timeManager.CurrentAudioPuzzlePhase == AudioPuzzlePhase.REWIND)
                                            diegeticSoundScript.NewPitch = diegeticSoundScript.Pitch + ConstantPitchChange - audioLoreScript.PhasePitchChange;
                                        else
                                            diegeticSoundScript.NewPitch = diegeticSoundScript.Pitch - ConstantPitchChange - audioLoreScript.PhasePitchChange;
                                    }
                                    else
                                    {
                                        if (_timeManager.CurrentAudioPuzzlePhase == AudioPuzzlePhase.REWIND)
                                            diegeticSoundScript.NewPitch = diegeticSoundScript.Pitch - ConstantPitchChange + audioLoreScript.PhasePitchChange;
                                        else
                                            diegeticSoundScript.NewPitch = diegeticSoundScript.Pitch + ConstantPitchChange + audioLoreScript.PhasePitchChange;
                                    }
                                    break;
                                case TimeChangeType.REWIND:
                                    if (diegeticSoundScript._isAReversedSound)
                                        diegeticSoundScript.NewPitch = diegeticSoundScript.Pitch - audioLoreScript.PhasePitchChange;
                                    else
                                        diegeticSoundScript.NewPitch = diegeticSoundScript.Pitch + audioLoreScript.PhasePitchChange;
                                    break;
                                default:
                                    if (diegeticSoundScript._isAReversedSound)
                                        diegeticSoundScript.NewPitch = diegeticSoundScript.Pitch - audioLoreScript.PhasePitchChange;
                                    else
                                        diegeticSoundScript.NewPitch = diegeticSoundScript.Pitch + audioLoreScript.PhasePitchChange;
                                    break;
                            }
                        }

                    }
                    else
                    {
                        //Change le pitch en fonction des changements de temps pour les sons diégétiques normaux
                        switch (Type)
                        {
                            case TimeChangeType.SLOW:
                                diegeticSound.GetComponent<DiegeticSound>().NewPitch = diegeticSound.GetComponent<DiegeticSound>().Pitch - ConstantPitchChange;
                                break;
                            case TimeChangeType.NORMAL:
                                diegeticSound.GetComponent<DiegeticSound>().NewPitch = diegeticSound.GetComponent<DiegeticSound>().Pitch;
                                break;
                            case TimeChangeType.SPEED:
                                diegeticSound.GetComponent<DiegeticSound>().NewPitch = diegeticSound.GetComponent<DiegeticSound>().Pitch + ConstantPitchChange;
                                break;
                            case TimeChangeType.REWIND:
                                diegeticSound.GetComponent<DiegeticSound>().NewPitch = diegeticSound.GetComponent<DiegeticSound>().Pitch;
                                break;
                            default:
                                diegeticSound.GetComponent<DiegeticSound>().NewPitch = diegeticSound.GetComponent<DiegeticSound>().Pitch;
                                break;
                        }
                    }
                }
                else
                {
                    diegeticSound.GetComponent<DiegeticSound>().NewPitch = diegeticSound.GetComponent<DiegeticSound>().Pitch;
                }
            }
        }
        //Active le lerping dans Update
        ActiveLerp();
    }

    // Demande directement au soundManager de changer le pitch
    public void SetLorePitch()
    {
        foreach (AudioSource diegeticSound in _diegeticSounds)
        {
            if (diegeticSound.GetComponent<DiegeticSound>())
            {
                if (diegeticSound.GetComponent<DiegeticSound>().TimeManipulable)
                {
                    if (diegeticSound.GetComponent<AudioLoreScript>())
                    {
                        ChangePitch(_timeManager.CurrentTimeChangeType);
                    }
                }
            }

        }
    }

    //Permet aux puzzles Audios de changer pendant le rewind
    public void ChangeLorePitchWithChangeTimePeriodRewind(AudioSource diegeticSound)
    {
        if (_timeManager.rewindManager.isRewinding)
        {
            if (diegeticSound.GetComponent<DiegeticSound>())
            {
                if (diegeticSound.GetComponent<AudioLoreScript>())
                {
                    if (diegeticSound.GetComponent<AudioLoreScript>().HasChangeTimePeriod)
                    {
                        diegeticSound.pitch = -diegeticSound.GetComponent<DiegeticSound>().NewPitch;
                    }
                }
            }

        }
    }



    public void PauseSound()
    {
        //Stoppe le son en cas de stop temps
        foreach (Sound sound in sounds)
        {
            if(sound.TimeManipulable)
            {
                sound.IsPaused = true;
                sound.source.Pause();
            }
        }
        foreach (AudioSource diegeticSound in _diegeticSounds)
        {
            if (diegeticSound.GetComponent<DiegeticSound>())
            {
                diegeticSound.GetComponent<DiegeticSound>().IsPaused = true;
                diegeticSound.Pause();
            }
        }
    }

    public void UnPauseSound()
    {
        //Remet le son en route cas de fin de stop temps
        foreach (Sound sound in sounds)
        {
            if (sound.TimeManipulable)
            {
                sound.IsPaused = false;
                if (sound.source.time > 0 && sound.source.time < sound.clip.length)
                    sound.source.UnPause();
            }
        }
        foreach (AudioSource diegeticSound in _diegeticSounds)
        {
            if (diegeticSound.GetComponent<DiegeticSound>())
            {
                diegeticSound.GetComponent<DiegeticSound>().IsPaused = false;
                if (diegeticSound.time > 0 && diegeticSound.time < diegeticSound.clip.length)
                    diegeticSound.UnPause();
            }
        }
    }

    public void ActiveLerp()
    {
        _lerping = true;
        _fadeLerp = 0;
        _fadeCountdown = 1;
    }
}
