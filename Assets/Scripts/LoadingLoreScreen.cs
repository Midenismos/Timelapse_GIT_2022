using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class LoadingLoreScreen : MonoBehaviour
{
    private TMP_InputField inputField;
    [Header("Mot clé requis pour accéder à l'info")]
    [SerializeField] private string _correctResearchWord = null;

    [Header("Canvas de l'écran contenant les informations recherchées")]
    [SerializeField] private GameObject _infoPanel = null;
    [Header("Canvas de l'écran contenant la barre de recherche")]
    [SerializeField] private GameObject _researchPanel = null;
    [Header("Canvas de l'écran de chargement")]
    [SerializeField] private GameObject _loadingPanel = null;
    [Header("GameObject TMP s'affichant si le joueur tappe une mauvaise recherche")]
    [SerializeField] private GameObject _errorText = null;
    [Header("Canvas de l'écran de résultat de recherche")]
    [SerializeField] private GameObject _resultPanel = null;
    [Header("GameObject TMP de l'infoPanel")]
    [SerializeField] private TMP_Text _infoText = null;

    private float _timerLoading = 0;

    [Header("Inscrire le temps de chargement en minutes")]
    [SerializeField] private float _loadingDuration = 5;

    [Header("Placer ici la barre de chargement (un Slider)")]
    [SerializeField] private Slider _slider = null;

    private TimeManager _timeManager = null;

    private bool _loaded = false;
    private bool _isOnAResult = false;
    private bool _hasEnteredResearch = false;
    private bool _hasWrongResearch = false;

    private string _currentResearch = null;

    [Header("Inscrire la durée d'apparition du texte d'erreur de recherche")]
    [SerializeField] private float _researchFailScreenDuration = 1f;

    [Header("Ecrire ici les 2 informations contenues dans ce puzzle ainsi que 2 fausses recherches")]
    [TextArea]
    [SerializeField] private string[] _infoTxtChoice = new string[4];

    private int _infoTxtNumber = 0;

    [Header("Placer ici les 2 GameObject TMP servant de fausses recherches")]
    [SerializeField] private TMP_Text[] _falseLinks;

    public int InfoTxtNumber
    {
        get
        { return _infoTxtNumber; }
        set
        {
            if (value != _infoTxtNumber)
            {
                _infoTxtNumber = value;
            }
            _infoText.text = _infoTxtChoice[_infoTxtNumber];
        }
    }

    public string CurrentResearch
    {
        get
        { return _currentResearch; }
        set
        {
            if (value != _currentResearch)
            {
                _currentResearch = value;
            }
        }
    }

    public bool HasWrongResearch
    {
        get
        { return _hasWrongResearch; }
        set
        {
            if (value != _hasWrongResearch)
            {
                _hasWrongResearch = value;
            }

            if (_hasWrongResearch)
                _errorText.SetActive(true);
            else
                _errorText.SetActive(false);
        }
    }

    public bool Loaded
    {
        get
        {
            return _loaded;
        }
        set
        {
            if (_loaded != value)
                _loaded = value;

            if (_loaded && !IsOnAResult)
            {
                _resultPanel.SetActive(true);
                _loadingPanel.SetActive(false);
                _researchPanel.SetActive(false);
            }
            else
            {
                if(!HasEnteredResearch && !IsOnAResult)
                {
                    _resultPanel.SetActive(false);
                    _loadingPanel.SetActive(false);
                    _researchPanel.SetActive(false);
                }
            }
        }
    }
    public bool IsOnAResult
    {
        get
        {
            return _isOnAResult;
        }
        set
        {
            if (_isOnAResult != value)
                _isOnAResult = value;

            if (_isOnAResult)
            {
                _infoPanel.SetActive(true);
                _resultPanel.SetActive(false);
                _loadingPanel.SetActive(false);
                _researchPanel.SetActive(false);
            }
            else
            {
                if (HasEnteredResearch && _loaded)
                {
                    _resultPanel.SetActive(true);
                    _loadingPanel.SetActive(false);
                    _researchPanel.SetActive(false);
                    _infoPanel.SetActive(false);
                }
                if (HasEnteredResearch && !_loaded)
                {
                    _resultPanel.SetActive(false);
                    _loadingPanel.SetActive(true);
                    _researchPanel.SetActive(false);
                    _infoPanel.SetActive(false);
                }
            }
        }
    }

    public bool HasEnteredResearch
    {
        get
        {
            return _hasEnteredResearch;
        }
        set
        {
            if (_hasEnteredResearch != value)
                _hasEnteredResearch = value;

            if (_hasEnteredResearch )
            {
                _researchPanel.SetActive(false);
                _resultPanel.SetActive(false);
                _loadingPanel.SetActive(true);
            }
            else 
            {
                if (!Loaded)
                {
                    _researchPanel.SetActive(true);
                    _resultPanel.SetActive(false);
                    _loadingPanel.SetActive(false);
                }

            }
        }
    }

    public float TimerLoading
    {
        get
        {
            return _timerLoading;
        }
        set
        {
            if (_timerLoading != value)
                _timerLoading = value;

            if (_timerLoading >= _loadingDuration)
                Loaded = true;
        }
    }

    private void Awake()
    {
        transform.Find("Canvas").GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        inputField = GetComponentInChildren<TMP_InputField>();
        _loadingDuration *= 60; 
        _slider.maxValue = _loadingDuration;

        //Trouve le TimeManager
        _timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();

        _timeManager.GetComponent<TimeManager>().KilledIenumerator += delegate ()
        {
            StopAllCoroutines();
        };
    }

    // Update is called once per frame
    void Update()
    {
        //Supprime le Caret
        if (transform.Find("Canvas/ResearchScreenCanvas/InputField (TMP)/Text Area/Caret"))
            Destroy(transform.Find("Canvas/ResearchScreenCanvas/InputField (TMP)/Text Area/Caret").gameObject);

        // Fait avancer la barre de chargement
        if (TimerLoading < _loadingDuration && _timeManager.multiplier !=0 && !_timeManager.rewindManager.isRewinding && GetComponent<DiegeticLoreScreenScript>().IsWorking && HasEnteredResearch)
            TimerLoading += Time.deltaTime;

        inputField.text = CurrentResearch;


        _slider.value = _timerLoading;
        if (_researchPanel.activeSelf)
        {
            if (GameObject.Find("Player").GetComponent<PlayerController>().isInteractingWithScreen == true)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    CheckResearch();
                }
            }
        }
    }


    public void ChangeResearchString()
    {
        CurrentResearch = inputField.text;
    }

    public void CheckResearch()
    {
        if(CurrentResearch != "")
        {
            // Valide ou non le mot de passe du joueur
            if (CurrentResearch == _correctResearchWord)
            {
                HasEnteredResearch = true;

                //Gère les deux faux textes
                _infoTxtChoice[2] = string.Format(_infoTxtChoice[2], CurrentResearch);
                _infoTxtChoice[3] = string.Format(_infoTxtChoice[3], CurrentResearch);
                if(_falseLinks.Length != 0)
                {
                    _falseLinks[0].text = _infoTxtChoice[2];
                    _falseLinks[1].text = _infoTxtChoice[3];
                }

            }
            else
            {
                StartCoroutine(Error());
            }
            GameObject.Find("Player").GetComponent<PlayerController>().isInteractingWithScreen = false;
        }
    }

    IEnumerator Error()
    {
        HasWrongResearch = true;
        yield return new WaitForSeconds(_researchFailScreenDuration);
        HasWrongResearch = false;
    }

    public void OpenWindow(int textID)
    {
        IsOnAResult = true;
        InfoTxtNumber = textID;
    }

    public void ReturnButton()
    {
        IsOnAResult = false;
    }
}
