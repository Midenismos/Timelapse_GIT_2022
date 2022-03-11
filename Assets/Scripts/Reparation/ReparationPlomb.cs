using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[HelpURL("https://docs.google.com/document/d/1W8cTLWeFWzMCgIkSpMI-Pg_JY2nzQXZRee-YED1FzGs/edit?usp=sharing")]

public class ReparationPlomb : MonoBehaviour
{

    [SerializeField]
    private ReparationObjectReceiver[] _receiver;

    [SerializeField]
    private bool _isRepaired;

    [Header("Si vous avez du mal à paramétrer ce script, cliquez sur le point d'interrogation")]
    [Header("Préciser ici ce qui doit se passer lorsque le joueur a réparé les plombs")]
    public UnityEvent onEveryPlombDone;
    [Header("Préciser ici ce qui doit se passer lorsque la réparation est annulée par le rewind")]

    public UnityEvent onPlombUndone;

    private TimeManager _timeManager;
    

    //Vérifie si tous les ObjectReceiver sont branchés aux bons fusibles
    private bool AllReceiverActivated()
    {
        for (int i = 0; i < _receiver.Length; ++i)
        {
            if (_receiver[i].IsActivated == false)
            {
                return false;
            }
        }
        return true;
    }

    private void Awake()
    {
        _timeManager = FindObjectOfType<TimeManager>().GetComponent<TimeManager>();
    }
    public void Update()
    {

        if (AllReceiverActivated())
        {
            if (_isRepaired == false)
            {
                onEveryPlombDone?.Invoke();
                if(!_timeManager.rewindManager.isRewinding && !_timeManager.IsTimeStopped)
                {
                    GetComponent<AudioSource>().Play();
                    GetComponent<AudioSource>().time = 0;
                }
                _isRepaired = true;
            }
        }
        else
        {
            if (_isRepaired == true)
            {
                onPlombUndone?.Invoke();
                if(GetComponent<AudioSource>().isPlaying)
                    GameObject.Find("SoundManager").GetComponent<SoundManager>().StopDiegeticSound(GetComponent<AudioSource>());
                _isRepaired = false;
            }
        }



        /*//Fait le rewind de ce prefab
        if (manager.GetComponent<RewindManager>().isRewinding)
        {
            if (ObjectReceiver.Count != 0)
            {
                if (manager.currentLoopTime <= ObjectReceiver[0].timeStamp)
                {
                    ObjectReceiver[0].OR.RewindObject();
                    plombNumber = ObjectReceiver[0].plombNv;
                    ObjectReceiver.RemoveAt(0);
                    CheckPlomb();
                }
            }
        }*/
    }
    /*public void AddPlomb(ObjectReceiver OR)
    {
        ObjectReceiver.Insert(0, new TimeStampedObjectReceiver(manager.currentLoopTime, OR, plombNumber));
        plombNumber += 1;
        CheckPlomb();
    }

    private void CheckPlomb()
    {

        if (plombNumber >= requiredNumber)
        {
            onEveryPlombDone?.Invoke();
        }
        if (plombNumber < requiredNumber)
        {
            onPlombUndone?.Invoke();
        }
    }

    public void RemovePlomb()
    {
        plombNumber -= 1;
        CheckPlomb();
    }*/

}
