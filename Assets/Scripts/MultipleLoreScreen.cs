using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class MultipleLoreScreen : MonoBehaviour
{
    [Header("Canvas de l'écran contenant les informations recherchées")]
    [SerializeField] private GameObject _infoPanel = null;
    [Header("Canvas de l'écran listant les informations")]
    [SerializeField] private GameObject _infoListPanel = null;
    [Header("GameObject TMP de l'infoPanel")]
    [SerializeField] private TMP_Text _infoText = null;


    private TimeManager _timeManager = null;

    private bool _isOnAResult = false;

    [Header("Ecrire ici les informations contenues dans cet écran")]
    [TextArea]
    [SerializeField] private string[] _infoTxtChoice;

    private int _infoTxtNumber = 0;

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
                _infoListPanel.SetActive(false);
            }
            else
            {
                _infoListPanel.SetActive(true);
                _infoPanel.SetActive(false);
            }
        }
    }



    private void Awake()
    {
        transform.Find("Canvas").GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

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
