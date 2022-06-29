using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using TMPro;
public class ProgressCircle : MonoBehaviour
{
    private int _filledEntryNumber = 0;
    [SerializeField] GameObject _circle = null;
    [SerializeField] GameObject _center = null;
    [SerializeField] TMP_Text _text = null;

    private string maxNumber = "/12";
    private bool finished = false;
    private bool doneOnce = false;
    [SerializeField] private IADialogue dialogueCompletion;
    [SerializeField] private GameObject reportButton;

    private void Start()
    {

        /*if (GameObject.Find("TI").GetComponent<TutorialTI>().TutorialActivated)
        {
            GetComponent<Slider>().maxValue = 15;
            maxNumber = "/15";
        }
        else
        {
            GetComponent<Slider>().maxValue = 12;
            maxNumber = "/12";
        }*/
        _center.transform.localScale = new Vector3(0, 0, 0);



    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A))
        {
            TestButton();
        }
        if (Input.GetKey(KeyCode.E))
        {
            TestButton2();
        }
        if (_text.text != _filledEntryNumber.ToString() + maxNumber)
            _text.text = _filledEntryNumber.ToString() + maxNumber;

        if (GetComponent<Slider>().value != _filledEntryNumber)
            GetComponent<Slider>().value = _filledEntryNumber;

        if (_filledEntryNumber == 12 && !finished && !doneOnce)
        {
            StartCoroutine(SpawnRepportButton());
        }
        else if (_filledEntryNumber == 12 && !finished && doneOnce)
        {
            StartCoroutine(RespawnButton());

        }

        if (_filledEntryNumber != 12 && finished)
        {
            StartCoroutine(DespawnButton());
        }

    }

    IEnumerator RespawnButton()
    {
        yield return new WaitForSeconds(reportButton.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
        reportButton.gameObject.SetActive(true);
        reportButton.GetComponent<Animator>().Play("customBloc_Appear");
        GetComponent<Animator>().Play("customBloc_Disappear");
        finished = true;
        yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
    }
    IEnumerator SpawnRepportButton()
    {
        if(!GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().DialogueHappening)
            GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().LaunchDialogue(dialogueCompletion);
        yield return new WaitForSeconds(7);
        reportButton.gameObject.SetActive(true);
        reportButton.GetComponent<Animator>().Play("customBloc_Appear");
        GetComponent<Animator>().Play("customBloc_Disappear");
        doneOnce = true;
        finished = true;
        yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
    }

    IEnumerator DespawnButton()
    {
        finished = false;
        reportButton.GetComponent<Animator>().Play("customBloc_Disappear");
        GetComponent<Animator>().Play("customBloc_Appear");
        yield return new WaitForSeconds(reportButton.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
        reportButton.gameObject.SetActive(false);
        foreach (Transform child in transform)
            child.gameObject.SetActive(true);
    }
    public void IncreaseEntryNumber()
    {
        _filledEntryNumber += 1;
        _circle.GetComponent<Animator>().Play("Rotate.rotateFeedback", 1);
        float centerScale = _filledEntryNumber / 12f * 0.6f;
        _center.transform.localScale = new Vector3(centerScale, centerScale, centerScale);
    }
    public void DecreaseEntryNumber()
    {
        _filledEntryNumber -= 1;
        float centerScale = _filledEntryNumber / 12f * 0.6f;
        _center.transform.localScale = new Vector3(centerScale, centerScale, centerScale);
    }

    public void TestButton()
    {
        _filledEntryNumber = 12;
    }
    public void TestButton2()
    {
        _filledEntryNumber = 11;
    }
}
