using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewLoopManager : MonoBehaviour
{
    public float currentLoopTime = 0;
    [Header("Loop Settings")]
    public float LoopDuration = 15;

    //TODO : REMETTRE LE SYSTEME DE NEBULEUSE ICI




    // Update is called once per frame
    void Update()
    {
        currentLoopTime += Time.deltaTime;

        if (currentLoopTime >= LoopDuration)
        {
            currentLoopTime = 0;
        }
    }
}
