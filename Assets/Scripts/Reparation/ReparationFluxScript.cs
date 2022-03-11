using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[HelpURL("https://docs.google.com/document/d/1ncpzfQki9V6oYez32jH0N4INCU8ts5bZV7bN9QsomC0/edit?usp=sharing")]
public class ReparationFluxScript : MonoBehaviour
{
    [Header("Placer tous les boutons ici")]
    [Header("Si vous avez du mal à paramétrer ce script, cliquez sur le point d'interrogation")]
    public GameObject[] buttons;

    [Header("Placer tous les PARENTS de groupes de flux DANS L'ORDRE dans lequel ils sont alimentés ici")]
    [Space(10)]
    public GameObject[] fluxes;
    private GameObject[] _activeFluxes;

    [Header("Placer les materials activés et désactivés des boutons et flux ici")]
    [Space(10)]
    [SerializeField]
    private Material activatedButtonMaterial = null;
    [SerializeField]
    private Material deactivatedButtonMaterial = null;
    [SerializeField]
    private Material activatedFluxMaterial = null;
    [SerializeField]
    private Material deactivatedFluxMaterial = null;

    private int maxButtonNumber = 0;

    private int currentButtonNumber = 0;
    public int CurrentButtonNumber
    {
        get
        {
            return currentButtonNumber;
        }
        set
        {
            if (currentButtonNumber != value)
                currentButtonNumber = value;
            //Gère la fin de la réparation
            if (currentButtonNumber == maxButtonNumber && isActivatedSoundPlayed == false)
            {
                if (!TimeManager.GetComponent<TimeManager>().rewindManager.isRewinding && !TimeManager.GetComponent<TimeManager>().IsTimeStopped)
                {
                    GetComponent<AudioSource>().Play();
                    GetComponent<AudioSource>().time = 0;
                }
                executedFunction?.Invoke();
                isActivatedSoundPlayed = true;
            }

            //Gère l'annulation de la réparation
            else if (currentButtonNumber == maxButtonNumber-1  && isActivatedSoundPlayed == true)
            {
                isActivatedSoundPlayed = false;
                executedRewindedFunction?.Invoke();
            }
        }
    }


    private int currentFluxNumber;
    public int CurrentFluxNumber
    {
        get
        {
            return currentFluxNumber;
        }
        set
        {
            if (currentFluxNumber != value)
                currentFluxNumber = value;
            //Eteint ou allume les flux en fonction de la variable CurrentFluxNumbber;
            if (currentFluxNumber >= 1)
            {
                for(int i = 0; i <= currentFluxNumber - 1; i++ )
                {
                    foreach (Transform child in fluxes[i].transform)
                    {

                        child.gameObject.GetComponent<MeshRenderer>().material = activatedFluxMaterial;

                    }
                }
            }
            if (currentFluxNumber < fluxes.Length)
            {
                foreach (Transform child in fluxes[currentFluxNumber].transform)
                {
                    child.gameObject.GetComponent<MeshRenderer>().material = deactivatedFluxMaterial;
                }
            }
        }
    }

    private int expectedButtonNumber = 0;

    [HideInInspector]
    public float multiplier = 1f;

    private GameObject TimeManager;
    [Space(10)]
    [Header("Préciser ici ce qui doit se passer lorsque le joueur a réparé le flux")]
    public UnityEvent executedFunction;
    [Header("Préciser ici ce qui doit se passer lorsque la réparation est annulé par le Rewind")]
    public UnityEvent executedRewindedFunction;

    [SerializeField]
    private bool isActivatedSoundPlayed = false;

    public bool _isWorking;
    public bool IsWorking
    {
        get { return _isWorking; }
    }

    private void Awake()
    {
        TimeManager = GameObject.Find("TimeManager");

        //Désactive les interactions avec cet objet si on passe dans une Nébuleuse
        TimeManager.GetComponent<TimeManager>().ReactedToNebuleuse += delegate (bool isInNebuleuse)
        {
            if (isInNebuleuse)
            {
                _isWorking = false;
                if (CurrentButtonNumber == maxButtonNumber)
                {
                    executedRewindedFunction?.Invoke();
                }
                Fail();
            }
            else
            {
                _isWorking = true;
            }
        };
    }
    // Start is called before the first frame update
    void Start()
    {
        maxButtonNumber = buttons.Length;

        //Connecte l'objet au TimeManager

        //Attribue les materials activés et désactivés aux boutons
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().buttonActivatedMaterial = activatedButtonMaterial;
            button.GetComponent<Button>().buttonDeactivatedMaterial = deactivatedButtonMaterial;
        }
    }

    // Update is called once per frame
    void Update()
    {
        multiplier = TimeManager.GetComponent<TimeManager>().multiplier;

        //Règle le nombre attendu par la machine
        expectedButtonNumber = CurrentButtonNumber + 1;

        if (_isWorking == true)
        {
            foreach (GameObject button in buttons)
            {
                if (button.GetComponent<Button>().clicked == true)
                {
                    if (button.GetComponent<DigicodeButton>().CheckedButton == false)
                    {
                        //Valide le bouton et change son apparence s'il est appuyé dans le bon ordre
                        if (button.GetComponent<DigicodeButton>().ButtonOrderNumber == expectedButtonNumber)
                        {
                            button.GetComponent<DigicodeButton>().CheckedButton = true;
                            CurrentButtonNumber += 1;
                            if (CurrentButtonNumber >= 2)
                            {
                                CurrentFluxNumber += 1;
                            }
                        }
                        //Reset le digicode si le joueur n'appuye pas de le bon ordre.
                        else
                        {
                            Fail();
                            FindObjectOfType<SoundManager>().Play("Fail", 0f);
                        }
                    }
                }
            }
        }
    }
    private void Fail()
    {
        //Reset le digicode en cas d'échec
        foreach (GameObject button in buttons)
        {
            button.GetComponent<DigicodeButton>().CheckedButton = false;
            button.GetComponent<Button>().clicked = false;
        }

        foreach(GameObject flux in fluxes)
        {
            foreach (Transform child in flux.transform)
            {
                child.gameObject.GetComponent<MeshRenderer>().material = deactivatedFluxMaterial;
            }
        }
        CurrentButtonNumber = 0;
        CurrentFluxNumber = 0;
        isActivatedSoundPlayed = false;
    }
}
