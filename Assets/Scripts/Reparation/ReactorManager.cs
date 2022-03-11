using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[HelpURL("https://docs.google.com/document/d/1gCRdgo1f-iseF0AYqhCa8gNB4puwS2icaRGdfcYhMqc/edit?usp=sharing")]

public class ReactorManager : MonoBehaviour, ITimeStoppable
{
    [Header("Placer les réacteurs dans l'ordre de bas en haut")]
    [Header("Si vous avez du mal à paramétrer ce script, cliquez sur le point d'interrogation")]
    public GameObject[] Reactors = null;

    [HideInInspector]
    public GameObject selectedReactor = null;
    [HideInInspector]
    public int i = 0;

    [Header("Préciser ici ce qui doit se passer lorsque le joueur a alligné les réacteurs")]
    public UnityEvent onReactorAligned;
    [Header("Préciser ici ce qui doit se passer lorsque la réparation est annulée par le rewind")]
    public UnityEvent onReactorRewinded;

    [Header("Plus cette valeur est élevée, moins le joueur a besoin de les aligner exactement ")]
    [SerializeField] private float tolerance = 1;
    [HideInInspector]
    public bool aligned = false;

    private GameObject timeManager;

    [Header("Insérer une vitesse de base")]
    [Space(10)]
    [SerializeField] private float baseSpeed = 1; 

    [HideInInspector]
    public float speed = 0;

    [HideInInspector]
    public bool reactorMoving = false;

    struct TimeStampedSelectedReactor
    {
        public float timeStamp;
        public int i;

        public TimeStampedSelectedReactor(float timeStamp, int i)
        {
            this.timeStamp = timeStamp;
            this.i = i;
        }
    }

    [SerializeField] private List<TimeStampedSelectedReactor> historySelectedReactor = new List<TimeStampedSelectedReactor>();

    private bool _isWorking;
    public bool IsWorking
    {
        get { return _isWorking; }
    }



    public void Record()
    {
        historySelectedReactor.Insert(0, new TimeStampedSelectedReactor(
            timeManager.GetComponent<TimeManager>().currentLoopTime,
            i));
    }

    public void Awake()
    {
        //Trouve le TimeManager
        timeManager = GameObject.Find("TimeManager");
        if (timeManager)
            timeManager.GetComponent<TimeManager>().RegisterTimeStoppable(this);

        //Désactive les interactions avec cet objet si on passe dans une Nébuleuse
        timeManager.GetComponent<TimeManager>().ReactedToNebuleuse += delegate (bool isInNebuleuse)
        {
            _isWorking = isInNebuleuse ? false : true;
        };
    }
    public void Update()
    {
        SetSpeed(Time.deltaTime);
        selectedReactor = Reactors[i];
        Move();

        // Enregistre le réacteur séléctionné en fonction du temps

        if (timeManager.GetComponent<RewindManager>().isRewinding)
        {
            if (historySelectedReactor.Count != 0)
            {
                if (timeManager.GetComponent<TimeManager>().currentLoopTime <= historySelectedReactor[0].timeStamp)
                {
                    i = historySelectedReactor[0].i;
                    historySelectedReactor.RemoveAt(0);
                }
            }
        }

        //Vérifie si les réacteurs sont alignés puis valide la réparation
        if (_isWorking)
        {
            Vector3 Reactor1Relative = Reactors[0].transform.localPosition;
            Vector3 Reactor2Relative = Reactors[1].transform.localPosition;
            Vector3 Reactor3Relative = Reactors[2].transform.localPosition;

            float ReactorToleranceMin = Reactor2Relative.z - tolerance;
            float ReactorToleranceMax = Reactor2Relative.z + tolerance;

            if (Reactor1Relative.z >= ReactorToleranceMin && Reactor1Relative.z <= ReactorToleranceMax)
            {
                if (Reactor3Relative.z >= ReactorToleranceMin && Reactor3Relative.z <= ReactorToleranceMax)
                {
                    Repair();
                }
                else
                {
                    UnRepair();
                }
            }
            else
            {
                UnRepair();
            }

            //Gère les lumières des réacteurs

            foreach (GameObject reactor in Reactors)
            {
                if (reactor == selectedReactor)
                {
                    reactor.GetComponent<Reactor>().GreenLight.enabled = true;
                    reactor.GetComponent<Reactor>().RedLight.enabled = false;
                }
                else
                {
                    reactor.GetComponent<Reactor>().GreenLight.enabled = false;
                    reactor.GetComponent<Reactor>().RedLight.enabled = true;
                }
            }
        }
        else
        {
            foreach (GameObject reactor in Reactors)
            {
                reactor.GetComponent<Reactor>().GreenLight.enabled = false;
                reactor.GetComponent<Reactor>().RedLight.enabled = false;
            }
            UnRepair();
        }
    }

    private void Repair()
    {
        if (aligned == false)
        {
            onReactorAligned?.Invoke();
            if (!timeManager.GetComponent<TimeManager>().rewindManager.isRewinding && !timeManager.GetComponent<TimeManager>().IsTimeStopped)
            {
                GetComponent<AudioSource>().Play();
                GetComponent<AudioSource>().time = 0;
            }
            aligned = true;
        }
    }

    private void UnRepair()
    {
        if (aligned == true)
        {
            onReactorRewinded?.Invoke();
            aligned = false;
        }
    }

    public void StartTimeStop()
    {

    }


    public void EndTimeStop()
    {

    }

    public void SetSpeed(float deltaTime)
    {
        speed = baseSpeed * deltaTime * 100;
    }


    private void Move()
    {
        //Gère le mouvement des réacteurs

        if(_isWorking)
        {
            Transform curWayPoint = selectedReactor.GetComponent<Reactor>().WayPoints[selectedReactor.GetComponent<Reactor>().curWayPointNumber].transform;
            float dist = Vector3.Distance(selectedReactor.transform.position, curWayPoint.position);
            if (timeManager.GetComponent<TimeManager>().multiplier != 0)
            {
                if (dist > 0.01)
                {
                    selectedReactor.transform.position = Vector3.MoveTowards(selectedReactor.transform.position, curWayPoint.position, Time.deltaTime * speed);
                    reactorMoving = true;
                }
                else
                {
                    reactorMoving = false;
                }

            }
        }

    }
}
