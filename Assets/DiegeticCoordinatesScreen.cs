using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DiegeticCoordinatesScreen : DiegeticLoreScreenScript
{
    [SerializeField] private string earthCoordinates = "aaa";
    [SerializeField] private TMP_InputField inputField = null;
    [SerializeField] private string endingEarthScene = "EndingEarth";
    [SerializeField] private TMP_Text errorTooltipText = null;
    [SerializeField] private TMP_Text repairedTooltipText = null;
    [SerializeField] private TMP_Text fuseBrokenText = null;
    [SerializeField] private TMP_Text fluxBrokenText = null;
    [SerializeField] private TMP_Text reactorBrokenText = null;
    [SerializeField] private Color systemRepairedColor = Color.green;
    [SerializeField] private Color systemBrokenColor = Color.red;
    private bool fuseRepaired = false;
    private bool fluxRepaired = false;
    private bool reactorRepaired = false;
    private bool isFullyRepaired = false;

    private void Start()
    {
        Destroy(GetComponentInChildren<TMP_SelectionCaret>().gameObject);
    }

    private void Update()
    {
        if (transform.Find("Canvas/CoordinatesScreenCanvas/InputField (TMP)/Text Area/Caret"))
            Destroy(transform.Find("Canvas/CoordinatesScreenCanvas/InputField (TMP)/Text Area/Caret").gameObject);

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

        if (_timeManager.GetComponent<TimeManager>().multiplier == 0)
            StartCoroutine(DeactivatePasswordInputField());

        if (GameObject.Find("Player").GetComponent<PlayerController>().isInteractingWithScreen == true)
        {
            if (Input.GetKeyDown(KeyCode.Return) && isFullyRepaired)
            {
                CheckCoordinates();
            }
        }
    }

    private void CheckCoordinates()
    {
        if(inputField.text == earthCoordinates)
        {
            SceneManager.LoadScene(endingEarthScene);
        }

        
    }

    private void CheckFullRepaired()
    {
        if(fuseRepaired && fluxRepaired && reactorRepaired)
        {
            isFullyRepaired = true;
            errorTooltipText.gameObject.SetActive(false);
            repairedTooltipText.gameObject.SetActive(true);
            fuseBrokenText.gameObject.SetActive(false);
            fluxBrokenText.gameObject.SetActive(false);
            reactorBrokenText.gameObject.SetActive(false);

        } else
        {
            isFullyRepaired = false;
            errorTooltipText.gameObject.SetActive(true);
            repairedTooltipText.gameObject.SetActive(false);
            fuseBrokenText.gameObject.SetActive(true);
            fluxBrokenText.gameObject.SetActive(true);
            reactorBrokenText.gameObject.SetActive(true);

            fuseBrokenText.color = fuseRepaired ? systemRepairedColor : systemBrokenColor;
            fluxBrokenText.color = fluxRepaired ? systemRepairedColor : systemBrokenColor;
            reactorBrokenText.color = reactorRepaired ? systemRepairedColor : systemBrokenColor;
        }
    }

    public void SetFuseRepairedState(bool state)
    {
        fuseRepaired = state;
        CheckFullRepaired();
    }

    public void SetFluxRepairedState(bool state)
    {
        fluxRepaired = state;
        CheckFullRepaired();

    }
    public void SetReactorRepairedState(bool state)
    {
        reactorRepaired = state;
        CheckFullRepaired();

    }

    protected override IEnumerator DeactivatePasswordInputField()
    {
        inputField.DeactivateInputField();
        if (_timeManager.GetComponent<TimeManager>().multiplier != 0 && !_timeManager.GetComponent<TimeManager>().rewindManager.isRewinding)
        {
            inputField.text = null;
            if (GetComponent<PasswordScript>())
                GetComponent<PasswordScript>().CurrentPassword = null;
        }
        yield return null;
    }


}
