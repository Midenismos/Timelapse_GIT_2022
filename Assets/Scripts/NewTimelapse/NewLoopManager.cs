using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum SpeedType
{
    SLOW,
    NORMAL,
    FAST
}
public enum NebuleuseType
{
    PURPLE1 = 0,
    PURPLE2 = 1,
    YELLOW = 2,
    GREEN = 3,
    BLUE = 4,
}

[Serializable]
public struct Nebuleuse
{
    public NebuleuseType type;
    public float start;
    public float end;
    public bool isActive;
}
public class NewLoopManager : MonoBehaviour
{
    public float CurrentLoopTime = 0;
    [Header("Loop Settings")]
    public float LoopDuration = 900;

    public float Multiplier = 1;

    public SpeedType Speed;

    //TODO : REMETTRE LE SYSTEME DE NEBULEUSE ICI

    public delegate void ReactToNebuleuse(NebuleuseType NebuleuseType);
    public event ReactToNebuleuse ReactedToNebuleuse;
    //Changer si besoin pour déterminer la période de changement de Nebuleuse
    public Nebuleuse[] Nebuleuses = new Nebuleuse[6];
    

    private NebuleuseType _currentNebuleusePhase;
    public NebuleuseType CurrentNebuleusePhase
    {
        get
        { return _currentNebuleusePhase; }
        set
        {
            if (value != _currentNebuleusePhase)
            {
                _currentNebuleusePhase = value;
                if (ReactedToNebuleuse != null)
                    ReactedToNebuleuse(_currentNebuleusePhase);
            }
        }
    }


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
        //Quand est-ce que les Nébuleuses ont lieu

        for(int i = 0; i<= Nebuleuses.Length-1; i++) 
        {
            if (CurrentLoopTime > Nebuleuses[i].start && CurrentLoopTime <= Nebuleuses[i].end)
            {
                CurrentNebuleusePhase = Nebuleuses[i].type;
                Nebuleuses[i].isActive = true;
                break;
            }
            else
            {
                Nebuleuses[i].isActive = false;
            }
        }


    }
}
