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
        if (_text.text != _filledEntryNumber.ToString() + maxNumber)
            _text.text = _filledEntryNumber.ToString() + maxNumber;

        if (GetComponent<Slider>().value != _filledEntryNumber)
            GetComponent<Slider>().value = _filledEntryNumber;

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
}
