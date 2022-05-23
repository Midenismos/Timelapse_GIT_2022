using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ProgressCircle : MonoBehaviour
{
    private int _filledEntryNumber = 0;
    [SerializeField] GameObject _circle = null;

    // Update is called once per frame
    void Update()
    {
        if (GetComponentInChildren<TMP_Text>().text != _filledEntryNumber.ToString() + "/15")
            GetComponentInChildren<TMP_Text>().text = _filledEntryNumber.ToString() + "/15";

        if (GetComponent<Slider>().value != _filledEntryNumber)
            GetComponent<Slider>().value = _filledEntryNumber;

        //if(_filledEntryNumber == 15)
        //TODO FAIRE CE QU'IL SE PASSE A LA FIN DU JEU;
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
