using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DiegeticLoreScreenScript : MonoBehaviour, IInteractable
{
    public bool Test = false;
    [SerializeField] protected GameObject _canvas = null;
    protected AudioSource _source = null;
    private SoundManager _sndManager = null;
    protected GameObject _timeManager = null;
    protected bool _isWorking = true;
    [SerializeField] MeshRenderer interactMesh = null;

    public bool IsWorking
    {
        get
        { return _isWorking; }
        set
        {
            if (value != _isWorking)
            {
                _isWorking = value;
            }
        }
    }

    private void Awake()
    {
        _timeManager = GameObject.Find("TimeManager");
        //Désactive les interactions avec cet objet si on passe dans une Nébuleuse
        _timeManager.GetComponent<TimeManager>().ReactedToNebuleuse += delegate (bool isInNebuleuse)
        {
            _isWorking = isInNebuleuse ? false : true;
        };
        _sndManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        _source = this.GetComponent<AudioSource>();
        // Empêche le bug SerializedObject target has been destroyed de se produire en changeant de scène
        _timeManager.GetComponent<TimeManager>().KilledIenumerator += delegate ()
        {
            StopAllCoroutines();
        };

    }


    public void PlayerHoverStart()
    {
        interactMesh.enabled = true;
        // Active le mode test des lores sans puzzles
        if (Test == true)
        {
            if (_isWorking)
            {
                _canvas.SetActive(true);
                _sndManager.CheckLoreSound();
                _source.Play();
            }
        }

    }

    private void Update()
    {
        // Désactive l'écran en cas de Nébuleuse
        if (!_isWorking)
        {
            _canvas.SetActive(false);
            _source.Stop();
            if (GetComponentInChildren<TMP_InputField>())
            {
                StartCoroutine(DeactivatePasswordInputField());
            }
        }
        else
        {
            if (Test == false)
            {
                _canvas.SetActive(true);
            }
        }

        // Supprime le Caret qui, à mon avis, n'est pas utile et qui en plus fait bugguer l'inputField
        if (GetComponentInChildren<TMP_InputField>() != null)
        {
            
            // Empêche le joueur d'intéragir avec l'écran mot de passe si le temps est arrêté.
            if (_timeManager.GetComponent<TimeManager>().multiplier == 0)
                StartCoroutine(DeactivatePasswordInputField());
        }

    }
    public void PlayerHoverEnd()
    {
        interactMesh.enabled = false;
        GameObject.Find("Player").GetComponent<PlayerController>().isInteractingWithScreen = false;

        if (Test == true)
            _canvas.SetActive(false);

        // Si c'est le prefab PasswordScreen, désactive l'interraction avec l'inputField
        if (GetComponentInChildren<TMP_InputField>())
        {
            StartCoroutine(DeactivatePasswordInputField());
        }
    }

    public void Interact(GameObject pickup, PlayerController player)
    {
        GameObject.Find("Player").GetComponent<PlayerController>().isInteractingWithScreen = true;
        // S'il s'agit du prefab PasswordScreen, active l'interraction avec l'inputField
        if (GetComponentInChildren<TMP_InputField>())
        {
            if (_timeManager.GetComponent<TimeManager>().multiplier != 0 && !_timeManager.GetComponent<TimeManager>().rewindManager.isRewinding)
            {
                StartCoroutine(ActivatePasswordInputField());
            }
        }
    }

    public void InteractHolding(GameObject pickup, PlayerController player)
    {

    }

    public void StopInteractHolding(GameObject pickup, PlayerController player)
    {

    }

    // Permet d'intéragir avec l'écran de mot de passe si le joueur le regarde.
    private IEnumerator ActivatePasswordInputField()
    {
        GetComponentInChildren<TMP_InputField>().ActivateInputField();
        GetComponentInChildren<TMP_InputField>().Select();
        GetComponentInChildren<TMP_InputField>().text = null;
        if (GetComponent<PasswordScript>())
            GetComponent<PasswordScript>().CurrentPassword = null;
        yield return null;

    }

    // Permet d'arrêter l'intéraction avec l'écran de mot de passe si le joueur ne le regarde plus ou qu'un rewind/stop temps a lieu
    virtual protected IEnumerator DeactivatePasswordInputField()
    {
        GetComponentInChildren<TMP_InputField>().DeactivateInputField();
        if (_timeManager.GetComponent<TimeManager>().multiplier != 0 && !_timeManager.GetComponent<TimeManager>().rewindManager.isRewinding)
        {
            GetComponentInChildren<TMP_InputField>().text = null;
            if(GetComponent<PasswordScript>())
            GetComponent<PasswordScript>().CurrentPassword = null;
        }
        yield return null;
    }


}
