using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class TimeWalkman : MonoBehaviour
{
    [SerializeField] private float vinylSpinSpeed = 180f;

    [SerializeField] private RectTransform track = null;
    [SerializeField] private RectTransform vinyl = null;
    [SerializeField] private ClockTimeButton rewind = null;
    [SerializeField] private ClockTimeButton stop = null;
    [SerializeField] private ClockTimeButton normal = null;
    [SerializeField] private ClockTimeButton slow = null;
    [SerializeField] private ClockTimeButton speed = null;

    private float rewindSpinMultiplier = -1f;
    private float stopSpinMultiplier = 0f;
    private float normalSpinMultiplier = 1f;
    private float slowSpinMultiplier = 0.5f;
    private float speedSpinMultiplier = 2f;

    private ClockTimeButton current = null;
    private float currentSpinMultiplier = 1;

    [SerializeField] private Image background = null;

    private float[] samples = null;

    private TimeManager timeManager = null;
    private float rmsValue;
    private int qSamples = 16000;

    private float minBrightness = 0.35f;
    private float maxBrightness = 0.8f;
    private int brightnessMultiplier = 15;

    public AudioSource ImportantSound = null;

    [SerializeField] private float _walkmanSoundLerp = 0;

    public float WalkmanSoundLerp
    {
        get { return _walkmanSoundLerp; }
        set
        {
            if (_walkmanSoundLerp != value)
                _walkmanSoundLerp = value;
        }
    }
    //Fonction récupéré sur un forum pour calculer l'intensité de la musique
    public void GetVolume()
    {
        GetComponent<AudioSource>().GetOutputData(samples, 0);  // fill array with samples
        int i;
        float sum = 0;
        for (i = 0; i < qSamples; i++)
        {
            sum += samples[i] * samples[i]; // sum squared samples
        }
        rmsValue = Mathf.Sqrt(sum / qSamples); // rms = square root of average

    }


    // Start is called before the first frame update
    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        if (!timeManager) Debug.LogError("PersonalWalkman needs a time manager in scene");
        else
        {
            timeManager.OnTimeChange += TimeChanged;
        }
        samples = new float[qSamples];

        ActivateButton(normal);

    }

    // Update is called once per frame
    void Update()
    {
        //Transitionne doucement le volume du walkman quand un son important se joue
        WalkmanSoundLerp = Mathf.Clamp(WalkmanSoundLerp + Time.unscaledDeltaTime, 0, 1);
        if (ImportantSound == null)
        {
            float volumeTransition = Mathf.Lerp(-40, -10, WalkmanSoundLerp);
            GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer.SetFloat("WalkmanVolume", volumeTransition);
        }
        else
        {
            float volumeTransition = Mathf.Lerp(-10, -40, WalkmanSoundLerp);
            GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer.SetFloat("WalkmanVolume", volumeTransition);
        }
        PositionVinyl();
        SpinVinyl();
        GetVolume();

        switch (timeManager.CurrentTimeChangeType)
        {
            case (TimeChangeType.NORMAL):
                brightnessMultiplier = 15;
                minBrightness = 0.35f;
                maxBrightness = 0.8f;
                break;
            case (TimeChangeType.REWIND):
                brightnessMultiplier = 15;
                minBrightness = 0.35f;
                maxBrightness = 0.8f;
                break;
            case (TimeChangeType.STOP):
                brightnessMultiplier = 15;
                minBrightness = 0.35f;
                maxBrightness = 0.8f;
                break;
            case (TimeChangeType.SLOW):
                brightnessMultiplier = 10;
                minBrightness = 0.25f;
                maxBrightness = 0.6f;
                break;
            case (TimeChangeType.SPEED):
                brightnessMultiplier = 17;
                minBrightness = 0.45f;
                maxBrightness = 0.9f;
                break;
        }
        background.color = new Color(background.color.r, background.color.g, background.color.b, Mathf.Clamp(rmsValue * brightnessMultiplier, minBrightness, maxBrightness));
    }

    private void PositionVinyl()
    {
        float percent = timeManager.currentLoopTime / timeManager.LoopDuration;

        float left = track.rect.xMin;
        float right = track.rect.xMax;

        vinyl.anchoredPosition = new Vector2(Mathf.Lerp(left, right, percent), 0);
    }

    private void SpinVinyl()
    {
        vinyl.Rotate(-Vector3.forward, vinylSpinSpeed * currentSpinMultiplier * Time.unscaledDeltaTime);
    }

    private void TimeChanged(TimeChangeType change)
    {
        switch(change)
        {
            case TimeChangeType.REWIND:
                ActivateButton(rewind);
                currentSpinMultiplier = rewindSpinMultiplier;
                break;

            case TimeChangeType.STOP:
                ActivateButton(stop);
                currentSpinMultiplier = stopSpinMultiplier;

                break;

            case TimeChangeType.NORMAL:
                ActivateButton(normal);
                currentSpinMultiplier = normalSpinMultiplier;

                break;

            case TimeChangeType.SLOW:
                ActivateButton(slow);
                currentSpinMultiplier = slowSpinMultiplier;

                break;

            case TimeChangeType.SPEED:
                ActivateButton(speed);
                currentSpinMultiplier = speedSpinMultiplier;

                break;
        }
    }

    private void ActivateButton(ClockTimeButton button)
    {
        button.Activate();
        if (current) current.Deactivate();
        current = button;
    }


}
