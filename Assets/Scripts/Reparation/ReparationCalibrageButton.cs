using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[HelpURL("https://docs.google.com/document/d/1ybLDsyvFSfyHyQXm3qOMYwDXoBWAWYOS2rm46UGMBqg/edit?usp=sharing")]

public class ReparationCalibrageButton : Button
{
    [Header("Insérer ici le numéro du button")]
    public int buttonNumber = 0;
    private ReparationCalibrageScript cal;
    private TextMeshPro butText = null;

    /*[HideInInspector]
    public List<float> timeCalled = new List<float>();*/
    [HideInInspector]
    public List<string> codeMemory = new List<string>();
    public void Start()
    {
        butText = GetComponentInChildren<TextMeshPro>();
        if (buttonNumber < 10)
            butText.text = buttonNumber.ToString();
        cal = GameObject.Find("ReparationCalibrage").GetComponent<ReparationCalibrageScript>();
    }

    public override void Update()
    {
        base.Update();
        // Rewind des Boutons de la réparation Callibrage
        /*if (TimeManager.GetComponent<RewindManager>().isRewinding)
        {
            if (this.GetComponent<ReparationCalibrageButton>() == true)
            {
                if (timeCalled.Count != 0)
                {
                    if (TimeManager.GetComponent<TimeManager>().currentLoopTime <= timeCalled[0])
                    {
                        executedRewindedFunction?.Invoke();
                        timeCalled.RemoveAt(0);
                    }
                }
            }
        }*/
    }
    public void SendNumber()
    {
        if (cal.repaired == false)
        {
            //Enregistre le moment ou chaque bouton a été appuyé
            //timeCalled.Insert(0, TimeManager.GetComponent<TimeManager>().currentLoopTime);
            if (buttonNumber < 10)
            {
                //Ajoute le numéro sur le code
                if (cal.CurrentCode.Length <= cal.requiredCode.Length - 1)
                {
                    cal.CurrentCode += buttonNumber.ToString();
                }
            }
            else if (buttonNumber == 10)
            {
                //Enregistre les codes précédemment effacés pour les réintroduire en cas de rewind
                codeMemory.Insert(0, cal.CurrentCode);
                cal.CurrentCode = "";
            }
            else if (buttonNumber == 11)
            {
                //Enregistre les parties de codes précédemment effacés pour les réintroduire en cas de rewind
                codeMemory.Insert(0, cal.CurrentCode);
                if(cal.CurrentCode.Length > 0)
                    cal.CurrentCode = cal.CurrentCode.Substring(0, cal.CurrentCode.Length - 1);
            }
        }
    }
        
}
