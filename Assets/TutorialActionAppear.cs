using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialActionAppear : TutorialAction
{
    [SerializeField] public GameObject[] objectsToAppear;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnTutoStart()
    {
        for (int i = 0; i < objectsToAppear.Length; i++)
        {
            objectsToAppear[i].SetActive(false);
        }
    }

    public override void ExecuteAction()
    {
        for (int i = 0; i < objectsToAppear.Length; i++)
        {
            objectsToAppear[i].SetActive(true);
        }
    }
}
