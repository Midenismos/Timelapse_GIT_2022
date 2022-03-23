using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpeedType
{
    SLOW,
    NORMAL,
    FAST
}
public class NewLoopManager : MonoBehaviour
{
    public float CurrentLoopTime = 0;
    [Header("Loop Settings")]
    public float LoopDuration = 15;

    public float Multiplier = 1;

    public SpeedType Speed;

    //TODO : REMETTRE LE SYSTEME DE NEBULEUSE ICI




    // Update is called once per frame
    void Update()
    {
        CurrentLoopTime += Time.deltaTime * Multiplier;

        if (CurrentLoopTime >= LoopDuration)
        {
            CurrentLoopTime = 0;
        }

        switch(Speed)
        {
            case (SpeedType.SLOW):
                Multiplier = 0.5f;
                break;
            case (SpeedType.NORMAL):
                Multiplier = 1f;
                break;
            case (SpeedType.FAST):
                Multiplier = 2f;
                break;
        }
    }
}
