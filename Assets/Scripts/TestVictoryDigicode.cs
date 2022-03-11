using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVictoryDigicode : MonoBehaviour
{

    public void Activate()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void DeActivate()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
