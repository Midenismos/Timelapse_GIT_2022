using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOpenScript : MonoBehaviour
{
    [SerializeField] private GameObject buttonPannel;
    public void OpenClosePanel()
    {
        buttonPannel.SetActive(!buttonPannel.activeInHierarchy);
    }
}
