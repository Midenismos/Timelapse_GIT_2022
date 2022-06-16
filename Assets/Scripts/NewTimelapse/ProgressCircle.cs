using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ProgressCircle : MonoBehaviour
{
    private int _filledEntryNumber = 0;
    [SerializeField] GameObject _circle = null;
    private OptionData optionData;

    private string maxNumber = "/12";

    private void Awake()
    {
        try
        {
            optionData = GameObject.Find("OptionsData").GetComponent<OptionData>();
        }
        catch
        {
            optionData = null;
        }
        if (optionData != null)
        {
            if (optionData.TutoActivated)
            {
                GetComponent<Slider>().maxValue = 15;
                maxNumber = "/15";
            }
            else
            {
                GetComponent<Slider>().maxValue = 12;
                maxNumber = "/12";
            }
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponentInChildren<TMP_Text>().text != _filledEntryNumber.ToString() + maxNumber)
            GetComponentInChildren<TMP_Text>().text = _filledEntryNumber.ToString() + maxNumber;

        if (GetComponent<Slider>().value != _filledEntryNumber)
            GetComponent<Slider>().value = _filledEntryNumber;

    }

    public void IncreaseEntryNumber()
    {
        _filledEntryNumber += 1;
        _circle.GetComponent<Animator>().Play("Rotate.rotateFeedback", 1);
    }
    public void DecreaseEntryNumber()
    {
        _filledEntryNumber -= 1;
    }
}
