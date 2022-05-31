using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialActionAppear : MonoBehaviour
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

    public void OnTutoStart()
    {
        for (int i = 0; i < objectsToAppear.Length; i++)
        {
            objectsToAppear[i].SetActive(false);
        }
    }

    public void ExecuteAction()
    {
        for (int i = 0; i < objectsToAppear.Length; i++)
        {
            objectsToAppear[i].SetActive(true);
        }
    }
}
