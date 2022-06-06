using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IABubble : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private TMP_Text txt;
    // Update is called once per frame
    void Update()
    {
        if (txt.text == "")
            background.SetActive(false);
        else
            background.SetActive(true);
    }
}
