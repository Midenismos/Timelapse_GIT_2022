using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingBrokenScreen : MonoBehaviour
{
    [Header("Glisser ici les parties endommagées de l'écran")]
    [SerializeField] private GameObject[] _brokenParts = null;

    private DiegeticLoreScreenScript _mainScript = null;

    private bool _screenVisible = false;
    public bool ScreenVisible
    {
        get
        { return _screenVisible; }
        set
        {
            if (value != _screenVisible)
            {
                _screenVisible = value;
            }
        }
    }
    
    private float _timerScreenInvisible;
    public float TimerScreenInvisible
    {
        get
        { return _timerScreenInvisible; }
        set
        {
            if (value != _timerScreenInvisible)
            {
                _timerScreenInvisible = value;
            }
        }
    }


    // Le Level Designer peut changer cette variable afin de fixer une durée pour la lumière allumée
    [Header("Permet de régler la période pendant lequel l'écran est partiellement éteint ou allumé")]
    [SerializeField]
    private float _timerScreenInvisibleMax;
    
    private float _timerScreenVisible = 0;
    public float TimerScreenVisible
    {
        get
        { return _timerScreenVisible; }
        set
        {
            if (value != _timerScreenVisible)
            {
                _timerScreenVisible = value;
            }
        }
    }


    // Le Level Designer peut changer cette variable afin de fixer une durée pour la lumière éteinte
    [SerializeField]
    private float _timerScreenVisibleMax;

    private TimeManager _timeManager;

    private void Awake()
    {
        _mainScript = GetComponent<DiegeticLoreScreenScript>();
        _timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        TimerScreenInvisible = _timerScreenInvisibleMax;
    }

    // Update is called once per frame
    void Update()
    {

        if (_timeManager.multiplier != 0 && !_timeManager.rewindManager.isRewinding)
        {
            if (_mainScript.IsWorking)
            {
                //Gère l'écran allumé
                if (TimerScreenInvisible >= 0 && TimerScreenInvisible <= _timerScreenInvisibleMax)
                    TimerScreenInvisible -= Time.deltaTime;
                else
                    _screenVisible = false;

                //Gère l'écran éteint
                if (TimerScreenVisible >= 0 && TimerScreenVisible <= _timerScreenVisibleMax)
                    TimerScreenVisible -= Time.deltaTime;
                else
                    _screenVisible = true;

                //Reset les timers
                if (_screenVisible == false)
                    TimerScreenInvisible = _timerScreenInvisibleMax;
                else
                    TimerScreenVisible = _timerScreenVisibleMax;
            }
        }

        foreach (GameObject image in _brokenParts)
        {
            //Active l'écran et reset les timers en fonction de si l'écran doit être allumée ou éteinte
            if (_screenVisible == false)
                image.SetActive(false);
            else
                image.SetActive(true);
        }

    }
}
