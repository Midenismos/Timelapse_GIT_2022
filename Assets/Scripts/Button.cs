using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Button : MonoBehaviour, IInteractable, ITimeStoppable
{
    private enum HowManyTimeClickable
    {
        ONCE,
        MULTIPLE,
        HOLDABLE,
    }
    [Header("Préciser si on est censé n'appuyer qu'une fois, plusieurs fois ou maintenir le bouton")]
    [SerializeField] private HowManyTimeClickable TimeClickable = HowManyTimeClickable.ONCE;

    [Header("Placer ici InteractFeedback (enfant de ce bouton)")]
    [SerializeField] private MeshRenderer interactMesh = null;

    //[HideInInspector]
    public bool clicked = false;
    //[HideInInspector]
    //public float timerSinceClicked = 0.0f;
    [HideInInspector]
    public GameObject TimeManager;
    [HideInInspector]
    public float multiplier = 1f;

    [Header("Placer ici les matériaux du bouton activé et désactivé")]
    public Material buttonActivatedMaterial = null;
    public Material buttonDeactivatedMaterial = null;
    
    [HideInInspector]
    public bool isRewinding = false;
    private bool isStopped = false;

    [Header("Indiquer ce qui doit se passer en cas de Rewind")]
    public UnityEvent executedRewindedFunction;

    private bool isPressed = false;

    private bool soundPlayed = false;

    [Header("Indiquer ce qui doit se passer si le joueur appuie sur le bouton")]
    public UnityEvent ButtonFunction = null;
    
    private bool _isWorking;
    public bool IsWorking
    {
        get { return _isWorking; }
    }

    [SerializeField] private bool _isNotCaringAboutNebuleuse = false;

    private void Awake()
    {
        TimeManager = GameObject.Find("TimeManager");
        if (TimeManager)
            TimeManager.GetComponent<TimeManager>().RegisterTimeStoppable(this);

        if (_isNotCaringAboutNebuleuse == false)
        {
            //Désactive les interactions avec cet objet si on passe dans une Nébuleuse
            TimeManager.GetComponent<TimeManager>().ReactedToNebuleuse += delegate (bool isInNebuleuse)
            {
                _isWorking = isInNebuleuse ? false : true;
            };
        }
        else
        {
            _isWorking = true;
        }


    }

    // Update is called once per frame
    public virtual void Update()
    {
        multiplier = TimeManager.GetComponent<TimeManager>().multiplier;
        //Stoppe le bouton en fonction du multiplier
        if (multiplier == 0)
            isStopped = true;
        else isStopped = false;

        if(TimeClickable == HowManyTimeClickable.ONCE)
        {
            /*if (clicked)
            {
                timerSinceClicked += Time.deltaTime * multiplier;
            }
            if (timerSinceClicked < 0)
            {
                clicked = false;
                executedRewindedFunction?.Invoke();
                timerSinceClicked = 0;

                GetComponent<MeshRenderer>().material = buttonDeactivatedMaterial;
            }*/

            //Change le material du bouton en fonction de son activation
            if (this.GetComponent<DigicodeButton>().CheckedButton == false)
            {
                GetComponent<MeshRenderer>().material = buttonDeactivatedMaterial;
            }
            else
            {
                GetComponent<MeshRenderer>().material = buttonActivatedMaterial;
            }
        }

        if(TimeClickable == HowManyTimeClickable.HOLDABLE)
        {
            if (isPressed == true)
            {
                ButtonFunction?.Invoke();
            }
        }
    }

    public void PlayerHoverStart()
    {
        interactMesh.enabled = true;
    }

    public void PlayerHoverEnd()
    {
        interactMesh.enabled = false;
        isPressed = false;
        soundPlayed = false;
    }

    public void Interact(GameObject pickup, PlayerController player)
    {

        if (_isWorking == true) // Empêche le joueur d'appuyer sur les boutons si une Nebuleuse est présente
        {
            //Empêche le joueur d'appuyer sur les boutons si le rewind ou le stop a lieu
            if (!isRewinding && !isStopped)
            {
                // Interragit avec le bouton
                //FindObjectOfType<SoundManager>().ChangePitch("Button");
                //FindObjectOfType<SoundManager>().Play("Button");

                if (TimeClickable == HowManyTimeClickable.MULTIPLE)
                {
                    ButtonFunction?.Invoke();
                }
                else if (TimeClickable == HowManyTimeClickable.ONCE)
                {
                    clicked = true;
                    GetComponent<MeshRenderer>().material = buttonActivatedMaterial;
                }

            }
        }

    }

    public void InteractHolding(GameObject pickup, PlayerController player)
    {
        if (!isRewinding && !isStopped)
        {
            if (soundPlayed == false)
            {
                //FindObjectOfType<SoundManager>().ChangePitch("Button");
                //FindObjectOfType<SoundManager>().Play("Button");
            }
            isPressed = true;
            soundPlayed = true;
        }
    }
    public void StopInteractHolding(GameObject pickup, PlayerController player)
    {
        if (!isRewinding && !isStopped)
        {
            isPressed = false;
            soundPlayed = false;
        }
    }


    public void StartTimeStop()
    {
        //TODO
    }

    public void EndTimeStop()
    {
        //TODO
    }

    private void OnDestroy()
    {
        if (TimeManager)
            TimeManager.GetComponent<TimeManager>().UnRegisterTimeStoppable(this);
    }
}
