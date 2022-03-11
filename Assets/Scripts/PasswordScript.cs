using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PasswordScript : MonoBehaviour
{
    private TMP_InputField inputField;
    [Header("Mot de passe requis pour accéder à l'info")]
    [SerializeField] private string _correctPassword = null;
    [Header("Canvas de l'écran contenant l'information recherchée")]
    [SerializeField] private GameObject _infoPanel = null;
    [Header("Canvas de l'écran de mot de passe")]
    [SerializeField] private GameObject _passwordPanel = null;
    [Header("Canvas de l'écran d'erreur lorsque le joueur se trompe de mdp")]
    [SerializeField] private GameObject _errorPanel = null;

    private bool _hasWrongPassword = false;
    private bool _hasCorrectPassword = false;

    private string _currentPassword = null;
    public string CurrentPassword
    {
        get
        { return _currentPassword; }
        set
        {
            if (value != _currentPassword)
            {
                _currentPassword = value;
            }
        }
    }
    public bool HasWrongPassword
    {
        get
        { return _hasWrongPassword; }
        set
        {
            if (value != _hasWrongPassword)
            {
                _hasWrongPassword = value;
            }
            // Change l'écran en fonction de si le joueur se trompe de mot de passe
            if(_hasWrongPassword)
            {
                _passwordPanel.SetActive(false);
                _errorPanel.SetActive(true);
            }
            else
            {
                if (!HasCorrectPassword)
                {
                    _passwordPanel.SetActive(true);
                    _infoPanel.SetActive(false);
                    _errorPanel.SetActive(false);
                }
            }
        }
    }

    public bool HasCorrectPassword
    {
        get
        { return _hasCorrectPassword; }
        set
        {
            if (value != _hasCorrectPassword)
            {
                _hasCorrectPassword = value;
            }
            // Change l'écran en fonction de si le joueur trouve le bon mot de passe
            if (_hasCorrectPassword)
            {
                _passwordPanel.SetActive(false);
                _infoPanel.SetActive(true);
            }
            else
            {
                if(!HasWrongPassword)
                {
                    _passwordPanel.SetActive(true);
                    _infoPanel.SetActive(false);
                    _errorPanel.SetActive(false);
                }

            }
        }
    }
    private void Awake()
    {
        inputField = GetComponentInChildren<TMP_InputField>();
    }
    public void CheckPassword()
    {
        if(CurrentPassword != "")
        {
            // Valide ou non le mot de passe du joueur
            if (CurrentPassword == _correctPassword)
            {
                HasCorrectPassword = true;
            }
            else
            {
                HasWrongPassword = true;
            }
            GameObject.Find("Player").GetComponent<PlayerController>().isInteractingWithScreen = false;
        }

    }

    private void Update()
    {
        if (transform.Find("Canvas/PasswordScreenCanvas/InputField (TMP)/Text Area/Caret"))
            Destroy(transform.Find("Canvas/PasswordScreenCanvas/InputField (TMP)/Text Area/Caret").gameObject);
        if(_passwordPanel.activeSelf)
        {
            if (GameObject.Find("Player").GetComponent<PlayerController>().isInteractingWithScreen == true)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    CheckPassword();
                }
            }
        }

        inputField.text = CurrentPassword;
    }

    public void changePasswordString()
    {
        CurrentPassword = inputField.text;
    }
}
