using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TIBellScript : MonoBehaviour
{
    [SerializeField] private TMP_Text _txt = null;
    [SerializeField] private Image _bellImage = null;
    public int NewPanelImageNumber = 0;
    [SerializeField] GameObject panel = null;
    // Start is called before the first frame update
    void Start()
    {
        _txt = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (NewPanelImageNumber > 0)
        {
            _bellImage.enabled = true;
            _txt.text = NewPanelImageNumber.ToString();
        }
        else
        {
            _bellImage.enabled = false;
            _txt.text = "";
        }

        if (panel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("border_Appear") && NewPanelImageNumber > 0)
            ResetNumber();
    }

    public void ResetNumber()
    {
        NewPanelImageNumber = 0;
    }
}
