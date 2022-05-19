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
    [SerializeField] private AudioSource _openSound = null;
    [SerializeField] private AudioSource _closeSound = null;
    [SerializeField] private Sprite minimiseImage = null;
    [SerializeField] private Sprite maximiseImage = null;
    [SerializeField] private Image circleImage = null;
    public DispenserManager Manager = null;
    public TMP_Text text = null;

    [SerializeField] private AudioClip _entryCompletedFeedback = null;

    [SerializeField] private SheetImageScript[] Slots;
    private bool entryFilled = false;

    public GameObject DeleteButton = null;
    public bool IsTuto = false;

    public void OnMouseUp()
    {
        if ((Time.time - _clickStart) < 0.3f)
        {
            _fullSheet.SetActive(!_fullSheet.activeInHierarchy);
            circleImage.sprite = _fullSheet.activeInHierarchy ? minimiseImage : maximiseImage;
            if (_fullSheet.activeInHierarchy)
                _openSound.Play();
            else
                _closeSound.Play();

           _clickStart = -1;
        }
    }

    public void OnMouseDown()
    {
        _clickStart = Time.time;
    }

    public void DeleteEntry()
    {
        if (Slots.All(Slot => Slot.IsFilled == true))
            GameObject.Find("ProgressCircle").GetComponent<ProgressCircle>().DecreaseEntryNumber();

        Manager.IncreaseNumber();
        Destroy(gameObject);
    }

    public void ChangeHour(string minute, bool isTutoZone)
    {
        if (!isTutoZone)
            text.text = "15 : " + minute;
        else
            text.text = minute;
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
    }
}