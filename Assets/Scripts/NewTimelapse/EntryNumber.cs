using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EntryNumber : MonoBehaviour
{
    public int Number = 5;
    [SerializeField] private int EntryID = 0;
    [SerializeField] private TMP_Text text = null;
    [SerializeField] private GameObject button = null;

    public void CallEntry()
    {
        if (Number > 0)
        {
            GameObject.Find("TI").GetComponent<NewTIScript>().CreateEntry(EntryID);
            Number -= 1;
        }
    }

    private void Update()
    {
        if (Number > 0)
            button.GetComponent<Image>().color = Color.white;
        else
            button.GetComponent<Image>().color = Color.black;

        text.text = Number.ToString();
    }

}
