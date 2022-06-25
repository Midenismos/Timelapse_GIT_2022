using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlHelps : MonoBehaviour
{
    [SerializeField] private GameObject sControlHelper = null;
    [SerializeField] private GameObject hControlHelper = null;
    [SerializeField] private GameObject fControlHelper = null;
    void Start()
    {
        FindObjectOfType<PanelBasketScript>().OnFirstScan += FirstScan;
    }

    private void FirstScan()
    {
        sControlHelper.SetActive(true);

        FindObjectOfType<PanelBasketScript>().OnFirstScan -= FirstScan;
        FindObjectOfType<PlayerAxisScript>().tiOpened += FirstTiOpened;
    }

    private void FirstTiOpened()
    {
        sControlHelper.SetActive(false);

        FindObjectOfType<PlayerAxisScript>().tiOpened -= FirstTiOpened;
    }

    public void SwitchOverheadHelper()
    {
        hControlHelper.SetActive(false);
        fControlHelper.SetActive(true);
    }
}
