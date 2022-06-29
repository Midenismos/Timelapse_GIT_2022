using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class TIEntryScript : MonoBehaviour
{
    private float _clickStart = 0;
    [SerializeField] private GameObject _fullSheet = null;
    private AudioClip _openSound = null;
    private AudioClip _closeSound = null;
    private AudioClip _clickSound = null;
    [SerializeField] private Sprite minimiseImage = null;
    [SerializeField] private Sprite maximiseImage = null;
    [SerializeField] private Image circleImage = null;
    public DispenserManager Manager = null;
    public TMP_Text text = null;

    private AudioClip _entryCompletedFeedback = null;

    [SerializeField] public SheetImageScript[] Slots;
    private bool entryFilled = false;

    public GameObject DeleteButton = null;
    public bool IsTuto = false;
    public string Date;
    public GameObject CurrentStandingPoint = null;

    private void Awake()
    {
        DeleteButton.SetActive(false);
        _openSound = Resources.Load("Sound/Snd_Investigation/Snd_Open") as AudioClip;
        _closeSound = Resources.Load("Sound/Snd_Investigation/Snd_Close") as AudioClip;
        _clickSound = Resources.Load("Sound/MMSequencingClick") as AudioClip;
        _entryCompletedFeedback = Resources.Load("Sound/Snd_Table/Snd_Table_Open") as AudioClip;
    }
    public void OnMouseUp()
    {
        if(GameObject.Find("Player").GetComponent<PlayerAxisScript>().CanClick && !GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().IsTalkingTutorial)
        {
            if ((Time.time - _clickStart) < 0.3f)
            {
                _fullSheet.SetActive(!_fullSheet.activeInHierarchy);
                circleImage.sprite = _fullSheet.activeInHierarchy ? minimiseImage : maximiseImage;
                if (_fullSheet.activeInHierarchy)
                {
                    GetComponent<AudioSource>().clip = _openSound;
                    GetComponent<AudioSource>().Play();
                }
                else
                {
                    GetComponent<AudioSource>().clip = _closeSound;
                    GetComponent<AudioSource>().Play();
                }
                _clickStart = -1;
            }
        }

    }

    public void OnMouseDown()
    {
        if(GameObject.Find("Player").GetComponent<PlayerAxisScript>().CanClick && !GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().IsTalkingTutorial)
        {
            _clickStart = Time.time;
            GetComponent<AudioSource>().clip = _clickSound;
            GetComponent<AudioSource>().Play();
        }
    }

    public void DeleteEntry()
    {
        if (Slots.All(Slot => Slot.IsFilled == true))
            GameObject.Find("ProgressCircle").GetComponent<ProgressCircle>().DecreaseEntryNumber();

        Manager.IncreaseNumber();
        GameObject.Find("TI").GetComponent<AudioSource>().clip = Resources.Load("Sound/Snd_Investigation/Snd_Delete") as AudioClip;
        GameObject.Find("TI").GetComponent<AudioSource>().Play();
        Destroy(gameObject);
    }

    public void ChangeHour(string minute, bool isTutoZone)
    {
        if (!isTutoZone)
            text.text = "15 : " + minute;
        else
            text.text = minute;
        Date = minute;
    }

    private void Update()
    {
        if(Slots.All(Slot => Slot.IsFilled == true))
        {
            if (entryFilled == false)
            {
                GameObject.Find("ProgressCircle").GetComponent<ProgressCircle>().IncreaseEntryNumber();
                GameObject.Find("TI").GetComponent<AudioSource>().clip = _entryCompletedFeedback; 
                GameObject.Find("TI").GetComponent<AudioSource>().Play();
                entryFilled = true;
            }
        }
        else
        {
            if (entryFilled == true)
            {
                GameObject.Find("ProgressCircle").GetComponent<ProgressCircle>().DecreaseEntryNumber();
                entryFilled = false;
            }
        }

        if (GameObject.Find("TI").GetComponent<TutorialTI>().TutorialActivated == false && IsTuto)
        {
            DeleteButton.SetActive(false);
            GetComponent<BoxCollider>().enabled = false;
        }
        if(GameObject.Find("TutorialManager").GetComponent<Tutorial>().activateTuto && GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex >= 30 && !IsTuto)
            DeleteButton.SetActive(true);

    }

    public void MoveLeft()
    {

    }

    public void MoveRight()
    {

    }
}