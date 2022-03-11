using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindableSoundManager : Rewindable
{
    private SoundManager _manager = null;
        private TimeManager _timeManager = null;

    [System.Serializable]
    struct TimeStampedNonDiegeticSound
    {
        public float timeStamp;
        public string sound;
        public float timeSound;
        public TimeStampedNonDiegeticSound(float timeStamp, string sound, float timeSound)
        {
            this.timeStamp = timeStamp;
            this.sound = sound;
            this.timeSound = timeSound;
        }
    }

    [System.Serializable]
    struct TimeStampedDiegeticSound
    {
        public float timeStamp;
        public AudioSource source;
        public float timeSound;
        public TimeStampedDiegeticSound(float timeStamp, AudioSource source, float timeSound)
        {
            this.timeStamp = timeStamp;
            this.source = source;
            this.timeSound = timeSound;
        }
    }


    [SerializeField]
    private List<TimeStampedNonDiegeticSound> historyNonDiegetic = new List<TimeStampedNonDiegeticSound>();

    [SerializeField]
    private List<TimeStampedDiegeticSound> historyDiegetic = new List<TimeStampedDiegeticSound>();

    private void Awake()
    {
        _timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        _manager = GetComponent<SoundManager>();
        _manager.RegisteredNonDiegeticSound += delegate (string sound, float timeSound)
        {
            historyNonDiegetic.Insert(0, new TimeStampedNonDiegeticSound(
                _timeManager.currentLoopTime,
                sound,
                timeSound
                ));
        };

        _manager.RegisteredDiegeticSound += delegate (AudioSource source, float timeSound)
        {
            historyDiegetic.Insert(0, new TimeStampedDiegeticSound(
                _timeManager.currentLoopTime,
                source,
                timeSound
                ));
        };
    }
    public override void Rewind(float deltaGameTime, float totalTime)
    {
        base.Rewind(deltaGameTime, totalTime);
        if (historyNonDiegetic.Count != 0)
        {
            if (historyNonDiegetic[0].timeStamp >= totalTime)
            {
                _manager.Play(historyNonDiegetic[0].sound, historyNonDiegetic[0].timeSound - 0.01f);
                historyNonDiegetic.RemoveAt(0);
            }
        }

        if (historyDiegetic.Count != 0)
        {
            if (historyDiegetic[0].timeStamp >= totalTime)
            {
                historyDiegetic[0].source.Stop();
                historyDiegetic[0].source.time = Mathf.Min(historyDiegetic[0].timeSound, historyDiegetic[0].source.clip.length - 0.01f);
                if (!historyDiegetic[0].source.GetComponent<AudioLoreScript>())
                    historyDiegetic[0].source.Play();
                else
                    historyDiegetic[0].source.GetComponent<AudioLoreScript>().StartDuringRewind();
                historyDiegetic.RemoveAt(0);
            }
        }

    }
}
