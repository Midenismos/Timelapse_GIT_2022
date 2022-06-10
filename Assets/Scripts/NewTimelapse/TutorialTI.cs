using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTI : MonoBehaviour
{
    public bool TutorialActivated = true;
    [SerializeField] private GameObject[] WrittenButtonsPanels = null;
    [SerializeField] private GameObject _greyedOut = null;

    private void Awake()
    {
        if(!GameObject.Find("TutorialManager").GetComponent<Tutorial>().activateTuto)
            TutorialActivated = false;
    }
    private void Update()
    {
        if (TutorialActivated == false)
        {
            foreach (GameObject ButtonAndPanel in WrittenButtonsPanels)
                ButtonAndPanel.SetActive(false);
            _greyedOut.SetActive(true);
        }

    }

    public void EndTITuto()
    {
        TutorialActivated = false;
    }
}
