using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public enum TimeChangeType
{
    REWIND,
    STOP,
    SLOW,
    SPEED,
    NORMAL
};

[Serializable]
public struct TimeChange
{
    public TimeChangeType type;
    public float duration;
    public float speed;
}

public enum PhaseState
{
    LIGHT,
    DARK
}

public enum AudioPuzzlePhase
{
    SLOW,
    SPEED,
    NORMAL,
    REWIND
}



public class TimeManager : MonoBehaviour
{
    [Header("Loop Settings")]
    [SerializeField] private float loopDuration = 10;

    [Header("Timescale Settings")]
    [SerializeField] private float startingMultiplier = 1;
    //[SerializeField] private float timeLerpDuration = 1;
    [SerializeField] private int maximumActiveTimeChangers = 2;

    [Header("Time Manipulation Settings")]
    [SerializeField] private float stopTimeMultiplier = 0;
    [SerializeField] private float slowTimeMultiplier = 0.5f;
    [SerializeField] private float normalTimeMultiplier = 1;
    [SerializeField] private float speedTimeMultiplier = 2;
    [SerializeField] private float rewindSlowTimeMultiplier = 0.5f;
    [SerializeField] private float rewindTimeMultiplier = 1;
    [SerializeField] private float rewindSpeedTimeMultiplier = 2;

    [Header("Debug Settings")]
    [SerializeField] private bool PlayLoopCutscene = false;

    [Header("References")]
    [SerializeField] public RewindManager rewindManager = null;
    [SerializeField] public PlayerLoopManager playerLoopManager = null;

    [Header("Angle Sun Light")]
    [SerializeField] private float LightPhaseAngle = 120;
    [SerializeField] private float DarkPhaseAngle = 190;

    [Header("Light and Dark Phases")]
    [SerializeField] private float lightPhaseIntensity = 1.3f;
    [SerializeField] private float darkPhaseIntensity = 0.3f;

    public event Action<TimeChangeType> OnTimeChange = null;
    public event Action OnLoopFinished = null;

    [Header("Others")]
    public float multiplier = 1;
    public float timeChangeTimer = 0f;

    public float currentLoopTime = 0;
    private bool hasTimeChange = false;
    private TimeChange currentTimeChange;

    private bool hasStandartTimeChange = false;

    //private bool mustResumeCurrentTimeChange = false;

    //private int currentlyActiveTimechangers = 0;

    //private Coroutine TimeLerpCoroutine = null;

    private List<ITimeStoppable> timeStoppables = new List<ITimeStoppable>();

    public bool IsTimeStopped { get => multiplier == 0; }


    private TimeChangeType currentTimeChangeType = TimeChangeType.NORMAL;

    public TimeChangeType CurrentTimeChangeType
    {
        get
        {
            return currentTimeChangeType;
        }
    }

    private PhaseState _phase;

    //Appelle tout les objets devant changer en fonction de la phase lumineuse
    public PhaseState Phase
    {
        get
        { return _phase; }
        set
        {
            if (value != _phase)
            {
                _phase = value;
                //Debug.Log(RenderSettings.ambientIntensity);
                //Debug.Log(RenderSettings.ambientSkyColor);

                switch (_phase)
                {
                    case (PhaseState.LIGHT):
                        //GameObject.Find("Sun").transform.rotation = Quaternion.Euler(120, 0, 0); //Simule le jour et la nuit
                        //GameObject.Find("UVLight").transform.rotation = Quaternion.Euler(-90, 0, 0); //Simule le jour et la nuit
                        RenderSettings.ambientIntensity = lightPhaseIntensity;
                        //RenderSettings.reflectionIntensity = 0.6f;
                        //RenderSettings.ambientSkyColor = new Color(111, 111, 111);
                        //DynamicGI.UpdateEnvironment();
                        //Debug.Log("LIGHT");
                        break;
                    case (PhaseState.DARK):
                        //GameObject.Find("Sun").transform.rotation = Quaternion.Euler(190, 0, 0); //Simule le jour et la nuit
                        //GameObject.Find("UVLight").transform.rotation = Quaternion.Euler(120, 0, 0); //Simule le jour et la nuit
                        RenderSettings.ambientIntensity = darkPhaseIntensity;
                        //RenderSettings.reflectionIntensity = 0.2f;
                        //RenderSettings.ambientSkyColor = new Color(39, 39, 39);
                        //DynamicGI.UpdateEnvironment();
                        //Debug.Log("DARK");
                        break;
                }

                /*if (GameObject.Find("Sun") && GameObject.Find("UVLight"))
                {
                    switch (_phase)
                    {
                        case (PhaseState.LIGHT):
                            //GameObject.Find("Sun").transform.rotation = Quaternion.Euler(120, 0, 0); //Simule le jour et la nuit
                            //GameObject.Find("UVLight").transform.rotation = Quaternion.Euler(-90, 0, 0); //Simule le jour et la nuit
                            RenderSettings.ambientIntensity = 0.6f;
                            break;
                        case (PhaseState.DARK):
                            //GameObject.Find("Sun").transform.rotation = Quaternion.Euler(190, 0, 0); //Simule le jour et la nuit
                            //GameObject.Find("UVLight").transform.rotation = Quaternion.Euler(120, 0, 0); //Simule le jour et la nuit
                            RenderSettings.ambientIntensity = 0.2f;
                            break;
                    }
                }*/

                if (ChangedToState != null)
                    ChangedToState(_phase);
            }
        }
    }

    private AudioPuzzlePhase _currentAudioPuzzlePhase;
    public AudioPuzzlePhase CurrentAudioPuzzlePhase
    {
        get
        { return _currentAudioPuzzlePhase; }
        set
        {
            if (value != _currentAudioPuzzlePhase)
            {
                _currentAudioPuzzlePhase = value;
                if (ReactedToAudioPhaseChange != null)
                    ReactedToAudioPhaseChange(_currentAudioPuzzlePhase);
            }
        }
    }

    public delegate void ChangeToStateLight(PhaseState isLight);
    public event ChangeToStateLight ChangedToState;

    public delegate void ReactToNebuleuse(bool isInNebuleuse);
    public event ReactToNebuleuse ReactedToNebuleuse;

    public delegate void ReactToAudioPhaseChange(AudioPuzzlePhase audioPhase);
    public event ReactToAudioPhaseChange ReactedToAudioPhaseChange;

    public delegate void PlayRadio(RadioSound sound);
    public event PlayRadio PlayedRadio;

    public delegate void KillIenumerator();
    public event KillIenumerator KilledIenumerator;

    //Changer si besoin pour déterminer la période de la phase lumineuse
    [SerializeField] public int _lightPhaseMinSecond = 150, _lightPhaseMaxSecond = 510;

    //Changer si besoin pour déterminer la période d'effet des deux nébuleuses
    [SerializeField] public int _nebuleuse1MinSecond = 270, _nebuleuse1MaxSecond = 420, _nebuleuse2MinSecond = 720, _nebuleuse2MaxSecond = 870;


    //Changer si besoin pour déterminer la période de changement de Phase Audio
    [SerializeField] public int _audioPhaseChange1 = 225, _audioPhaseChange2 = 450, _audioPhaseChange3 = 675;

    [SerializeField] private int[] _radioPlayTime;
    [SerializeField] private RadioSound[] _radioSounds;

    [SerializeField]
    private bool _isInNebuleuse;

    //Appelle tout les objets devant changer en fonction de présence de nébuleuse
    public bool IsInNebuleuse
    {
        get
        { return _isInNebuleuse; }
        set
        {
            if (value != _isInNebuleuse)
            {
                _isInNebuleuse = value;
                if (ReactedToNebuleuse != null)
                    ReactedToNebuleuse(_isInNebuleuse);
            }
        }
    }




    // Start is called before the first frame update
    void Start()
    {
        multiplier = startingMultiplier;
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
        //rewindManager.OnRewindStopped += RewindStopped;

        //Find other way to gather timestoppables
        //timeStoppables = FindObjectsOfType<UnityEngine.Object>().OfType<ITimeStoppable>().ToList();

        ReactedToNebuleuse(_isInNebuleuse);
    }

    // Update is called once per frame
    void Update()
    {

        if (!rewindManager.isRewinding)
        {
            if(!IsTimeStopped)
            currentLoopTime += Time.deltaTime;

            if(currentLoopTime >= loopDuration)
            {
                RestartLoop();
            }
        }
        if(hasStandartTimeChange)
        {
            /*timeChangeTimer -= Time.unscaledDeltaTime;

            if(timeChangeTimer <= 0)
            {
                EndStandartTimeChange();
            }*/
        }

        // Change la phase Nébuleuse en fonction du temps fixés
        if (currentLoopTime >= 0)
        {
            if (currentLoopTime >= _nebuleuse1MinSecond && currentLoopTime <= _nebuleuse1MaxSecond)
            {
                IsInNebuleuse = true;
            }
            else if (currentLoopTime >= _nebuleuse2MinSecond && currentLoopTime <= _nebuleuse2MaxSecond)
            {
                IsInNebuleuse = true;
            }
            else
            {
                IsInNebuleuse = false;
            }
        }
        else // Si on est avant le reveil du joueur on inverse tout
        {
            if (currentLoopTime <= -_nebuleuse1MinSecond && currentLoopTime >= -_nebuleuse1MaxSecond)
            {
                IsInNebuleuse = true;
            }
            else if (currentLoopTime <= -_nebuleuse2MinSecond && currentLoopTime >= -_nebuleuse2MaxSecond)
            {
                IsInNebuleuse = true;
            }
            else
            {
                IsInNebuleuse = false;
            }
        }


        //Change la phase Audio en fonction des périodes de temps fixés
        
        if(currentLoopTime >= 0)
        {
            if (currentLoopTime <= _audioPhaseChange1)
                CurrentAudioPuzzlePhase = AudioPuzzlePhase.REWIND;
            else if (currentLoopTime > _audioPhaseChange1 && currentLoopTime <= _audioPhaseChange2)
                CurrentAudioPuzzlePhase = AudioPuzzlePhase.NORMAL;
            else if (currentLoopTime > _audioPhaseChange2 && currentLoopTime <= _audioPhaseChange3)
                CurrentAudioPuzzlePhase = AudioPuzzlePhase.SLOW;
            else if (currentLoopTime > _audioPhaseChange3)
                CurrentAudioPuzzlePhase = AudioPuzzlePhase.SPEED;
        }
        else // Si on est avant le reveil du joueur on inverse tout
        {
            if (currentLoopTime >= -_audioPhaseChange1)
                CurrentAudioPuzzlePhase = AudioPuzzlePhase.SPEED;
            else if (currentLoopTime < -_audioPhaseChange1 && currentLoopTime >= -_audioPhaseChange2)
                CurrentAudioPuzzlePhase = AudioPuzzlePhase.SLOW;
            else if (currentLoopTime < -_audioPhaseChange2 && currentLoopTime >= -_audioPhaseChange3)
                CurrentAudioPuzzlePhase = AudioPuzzlePhase.NORMAL;
            else if (currentLoopTime < -_audioPhaseChange3)
                CurrentAudioPuzzlePhase = AudioPuzzlePhase.REWIND;
        }


        //Change la phase de lumière en fonction de la période de temps qu'on a fixé
        if(currentLoopTime >= 0)
        {
            if (currentLoopTime >= _lightPhaseMinSecond && currentLoopTime <= _lightPhaseMaxSecond)
            {
                Phase = PhaseState.LIGHT;
            }
            else
            {
                Phase = PhaseState.DARK;
            }
        }
        else
        {
            if (currentLoopTime <= -_lightPhaseMinSecond && currentLoopTime >= -_lightPhaseMaxSecond)
            {
                Phase = PhaseState.LIGHT;
            }
            else
            {
                Phase = PhaseState.DARK;
            }
        }


        //Stoppe toutes les animations en cas de STOP
        if (multiplier == 0)
        {
            if(rewindManager.isRewinding)
            {
                foreach (Animator animator in FindObjectsOfType<Animator>())
                {
                    animator.speed = 1;
                }
            } else
            {
                foreach (Animator animator in FindObjectsOfType<Animator>())
                {
                    animator.speed = 0;
                }
            }
            
        }
        else
        {
            foreach (Animator animator in FindObjectsOfType<Animator>())
            {
                animator.speed = 1;
            }
        }

        //Demande à toutes les radios de jouer le son correspondant au bon moment
        for (int i= 0; i <= _radioPlayTime.Length-1; i++)
        {
            if (currentLoopTime >= _radioPlayTime[i]-0.1f && currentLoopTime <= _radioPlayTime[i] + 0.1f)
            {
                if (!rewindManager.isRewinding)
                    PlayedRadio(_radioSounds[i]);
            }
        }


    }

    public void RegisterTimeStoppable(ITimeStoppable timeStoppable)
    {
        timeStoppables.Add(timeStoppable);
    }

    public void UnRegisterTimeStoppable(ITimeStoppable timeStoppable)
    {
        timeStoppables.Remove(timeStoppable);
    }


    //Commence le changement de temps (appellé par un TimeChanger)
    /*public void StartTimeChange(TimeChange timeChange, float toleranceCost = 0)
    {
        PayToleranceCost(toleranceCost);

        if (!hasTimeChange)
        {
            if (timeChange.type == TimeChangeType.REWIND)
            {
                StartRewind(timeChange.speed, timeChange.duration);
            }
            else
            {
                StartStandartTimeChange(timeChange.speed, timeChange.duration);
            }
            currentTimeChange = timeChange;
        }
        else
        {
            if (currentTimeChange.type == TimeChangeType.SLOW)
            {
                if (timeChange.type == TimeChangeType.REWIND)
                {
                    EndStandartTimeChange(false);
                    StartRewind(timeChange.speed, timeChangeTimer + timeChange.duration);
                }
                else if (timeChange.type == TimeChangeType.STOP)
                {
                    PauseCurrentTimeChange();
                    StartStandartTimeChange(timeChange.speed, timeChange.duration);
                }
                else if (timeChange.type == TimeChangeType.ACCELERATE)
                {
                    EndStandartTimeChange();
                }
                else if (timeChange.type == TimeChangeType.SLOW)
                {
                    timeChangeTimer += timeChange.duration;
                }
            }
            else if (currentTimeChange.type == TimeChangeType.ACCELERATE)
            {
                if (timeChange.type == TimeChangeType.REWIND)
                {
                    EndStandartTimeChange(false);
                    StartRewind(timeChange.speed, timeChangeTimer + timeChange.duration);
                }
                else if (timeChange.type == TimeChangeType.STOP)
                {
                    PauseCurrentTimeChange();
                    StartStandartTimeChange(timeChange.speed, timeChange.duration);
                }
                else if (timeChange.type == TimeChangeType.SLOW)
                {
                    EndStandartTimeChange();
                }
                else if (timeChange.type == TimeChangeType.ACCELERATE)
                {
                    timeChangeTimer += timeChange.duration;
                }
            } else if(currentTimeChange.type == TimeChangeType.REWIND)
            {
                if(timeChange.type == TimeChangeType.REWIND)
                {
                    rewindManager.AddDuration(timeChange.duration);
                } else if(timeChange.type == TimeChangeType.STOP)
                {
                    PauseCurrentTimeChange();
                    StartStandartTimeChange(timeChange.speed, timeChange.duration);
                } else if(timeChange.type == TimeChangeType.SLOW || timeChange.type == TimeChangeType.ACCELERATE)
                {
                    rewindManager.AddDuration(timeChange.duration);
                    rewindManager.ChangeSpeed(timeChange.speed);
                }
            } else if(currentTimeChange.type == TimeChangeType.STOP)
            {
                if (timeChange.type == TimeChangeType.REWIND)
                {
                    currentTimeChange = timeChange;
                    mustResumeCurrentTimeChange = true;
                } else if(timeChange.type == TimeChangeType.STOP)
                {
                    timeChangeTimer += timeChange.duration;
                } else if(timeChange.type == TimeChangeType.SLOW || timeChange.type == TimeChangeType.ACCELERATE)
                {
                    currentTimeChange = timeChange;
                    mustResumeCurrentTimeChange = true;
                }
            }
        }

        IncrementCurrentlyActiveTimeChangers();
    }*/
    public void StopTimePressed()
    {
        if (currentTimeChangeType != TimeChangeType.STOP)
        {
            StartStandartTimeChange(stopTimeMultiplier);
            currentTimeChangeType = TimeChangeType.STOP;
            OnTimeChange?.Invoke(currentTimeChangeType);
            GameObject.Find("SoundManager").GetComponent<SoundManager>().PauseSound();
        }
    }

    public void SlowTimePressed()
    {
        if (currentTimeChangeType != TimeChangeType.SLOW)
        {
            StartStandartTimeChange(slowTimeMultiplier);
            currentTimeChangeType = TimeChangeType.SLOW;
            OnTimeChange?.Invoke(currentTimeChangeType);
            GameObject.Find("SoundManager").GetComponent<SoundManager>().UnPauseSound();
            GameObject.Find("SoundManager").GetComponent<SoundManager>().ChangePitch(TimeChangeType.SLOW);
        }
    }

    public void NormalTimePressed()
    {
        if (currentTimeChangeType != TimeChangeType.NORMAL)
        {
            StartStandartTimeChange(normalTimeMultiplier);
            currentTimeChangeType = TimeChangeType.NORMAL;
            OnTimeChange?.Invoke(currentTimeChangeType);
            GameObject.Find("SoundManager").GetComponent<SoundManager>().UnPauseSound();
            GameObject.Find("SoundManager").GetComponent<SoundManager>().ChangePitch(TimeChangeType.NORMAL);
        }
    }

    public void SpeedTimePressed()
    {
        if (currentTimeChangeType != TimeChangeType.SPEED)
        {
            StartStandartTimeChange(speedTimeMultiplier);
            currentTimeChangeType = TimeChangeType.SPEED;
            OnTimeChange?.Invoke(currentTimeChangeType);
            GameObject.Find("SoundManager").GetComponent<SoundManager>().UnPauseSound();
            GameObject.Find("SoundManager").GetComponent<SoundManager>().ChangePitch(TimeChangeType.SPEED);
        }
    }

    public void RewindTimePressed()
    {
        if (currentTimeChangeType != TimeChangeType.REWIND)
        {
            StartRewind(rewindTimeMultiplier);
            currentTimeChangeType = TimeChangeType.REWIND;
            OnTimeChange?.Invoke(currentTimeChangeType);
            GameObject.Find("SoundManager").GetComponent<SoundManager>().UnPauseSound();
            GameObject.Find("SoundManager").GetComponent<SoundManager>().SetLorePitch();

        }
    }

    private void StartStandartTimeChange(float speed)
    {
        if (speed != normalTimeMultiplier)
        { playerLoopManager.AddMinutesToTimer();}

        if (rewindManager.isRewinding)
        {
            rewindManager.EndRewind();
        }

        hasStandartTimeChange = true;

        //StartTimeLerp(speed);
        ChangeTimeScale(speed);
    }

    public void EndStandartTimeChange(bool executeTimeLerp = true)
    {
        hasStandartTimeChange = false;

        /*if(mustResumeCurrentTimeChange)
        {
            if(currentTimeChange.type == TimeChangeType.SLOW || currentTimeChange.type == TimeChangeType.ACCELERATE)
            {
                StartStandartTimeChange(currentTimeChange.speed, currentTimeChange.duration);
            } else
            {
                StartRewind(currentTimeChange.speed, currentTimeChange.duration);
            }

            mustResumeCurrentTimeChange = false;
            currentlyActiveTimechangers--;
            return;
        }

        currentlyActiveTimechangers = 0;*/

        if(executeTimeLerp)
        {
            //StartTimeLerp(1);
            ChangeTimeScale(1);
        }
    }

    private void StartRewind(float speed)
    {
        playerLoopManager.AddMinutesToTimer();
        if (!rewindManager.isRewinding)
        {
            ChangeTimeScale(0);
            hasTimeChange = true;
        }
        rewindManager.StartRewind(speed);
    }

    private void RewindStopped()
    {
        ChangeTimeScale(1);
    }


    /*private void StartTimeLerp(float speed)
    {
        if (TimeLerpCoroutine != null)
        {
            StopCoroutine(TimeLerpCoroutine);
        }
        TimeLerpCoroutine = StartCoroutine(TimeLerp(multiplier, speed, timeLerpDuration));
    }

    private IEnumerator TimeLerp(float oldMultiplier, float newMultiplier, float duration)
    {
        float timeCounter = 0;

        while (timeCounter < duration)
        {
            timeCounter += Time.unscaledDeltaTime;
            
            ChangeTimeScale(Mathf.Lerp(oldMultiplier, newMultiplier, timeCounter / duration));
            yield return null;
        }
        
        ChangeTimeScale(newMultiplier);
    }*/

    private void PauseCurrentTimeChange()
    {
        if(currentTimeChange.type == TimeChangeType.SLOW  || currentTimeChange.type == TimeChangeType.SPEED)
        {
            EndStandartTimeChange(false);
            currentTimeChange.duration = timeChangeTimer;
        } else if (currentTimeChange.type == TimeChangeType.REWIND)
        {
            currentTimeChange.duration = rewindManager.EndRewind();
        }
        //mustResumeCurrentTimeChange = true;

    }

    /*private void IncrementCurrentlyActiveTimeChangers()
    {
        currentlyActiveTimechangers++;

        if(currentlyActiveTimechangers > maximumActiveTimeChangers)
        {
            RestartLoop();
        }
    }*/

    public void RestartLoop()
    {
        currentTimeChangeType = TimeChangeType.NORMAL;
        ChangedToState = null;
        ReactedToNebuleuse = null;
        ReactedToAudioPhaseChange = null;
        PlayedRadio = null;
        if (KilledIenumerator != null)
        {
            KilledIenumerator();
        }
        KilledIenumerator = null;
        GameObject.Find("InvestigationDataBase").GetComponent<InvestigationDataBase>().CompareToGoodClues();

        if(PlayLoopCutscene)
            OnLoopFinished?.Invoke();
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /*private void PayToleranceCost (float toleranceCost)
    {
        currentTolerance += toleranceCost * loopDuration;

        if (currentTolerance >= loopDuration)
        {
            RestartLoop();
        }
    }*/

    private void ChangeTimeScale (float newTimeScale)
    {
        float oldMultiplier = multiplier;
        multiplier = newTimeScale;
        if(multiplier != 0)
        {
            Time.timeScale = newTimeScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            if(oldMultiplier == 0)
            {
                ResumeTime();
            }
        } else {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;

            if(oldMultiplier != 0)
            {
                StopTime();
            }
        }
    }

    private void StopTime()
    {
        for (int i = 0; i < timeStoppables.Count; i++)
        {
            timeStoppables[i].StartTimeStop();
        }
    }

    private void ResumeTime()
    {
        for (int i = 0; i < timeStoppables.Count; i++)
        {
            timeStoppables[i].EndTimeStop();
        }
    }

    public float LoopDuration { get { return loopDuration; } }
}
