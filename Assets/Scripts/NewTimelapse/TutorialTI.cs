using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTI : MonoBehaviour
{
    public bool TutorialActivated = true;
    [SerializeField] private GameObject[] WrittenButtonsPanels = null;

    private void Update()
    {
        if (TutorialActivated == false)
        {
            foreach (GameObject ButtonAndPanel in WrittenButtonsPanels)
                ButtonAndPanel.SetActive(false);
        }

    }
}
