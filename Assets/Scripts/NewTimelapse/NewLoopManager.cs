using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpeedType
{
    SLOW,
    NORMAL,
    FAST
}
public enum NebuleuseType
{
    PURPLE1,
    PURPLE2,
    YELLOW,
    GREEN,
    BLUE,
    NONE,
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
    //Changer si besoin pour déterminer la période de changement de Phase Audio
    [SerializeField] public int PurpleNebuleuse1Start, PurpleNebuleuse1End, PurpleNebuleuse2Start, PurpleNebuleuse2End, YellowNebuleuseStart, YellowNebuleuseEnd, GreenNebuleuseStart, GreenNebuleuseEnd, BlueNebuleuseStart, BlueNebuleuseEnd;


    public NebuleuseType _currentNebuleusePhase;
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
        if (CurrentLoopTime > PurpleNebuleuse1Start && CurrentLoopTime <= PurpleNebuleuse1End)
            CurrentNebuleusePhase = NebuleuseType.PURPLE1;
        else if (CurrentLoopTime > PurpleNebuleuse2Start && CurrentLoopTime <= PurpleNebuleuse2End)
            CurrentNebuleusePhase = NebuleuseType.PURPLE2;
        else if (CurrentLoopTime > YellowNebuleuseStart && CurrentLoopTime <= YellowNebuleuseEnd)
            CurrentNebuleusePhase = NebuleuseType.YELLOW;
        else if (CurrentLoopTime > GreenNebuleuseStart && CurrentLoopTime <= GreenNebuleuseEnd)
            CurrentNebuleusePhase = NebuleuseType.GREEN;
        else if (CurrentLoopTime > BlueNebuleuseStart && CurrentLoopTime <= BlueNebuleuseStart)
            CurrentNebuleusePhase = NebuleuseType.BLUE;
        else
            CurrentNebuleusePhase = NebuleuseType.NONE;

    }
}
