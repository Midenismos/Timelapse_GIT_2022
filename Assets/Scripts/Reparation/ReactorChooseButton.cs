using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactorChooseButton : Button
{
    private ReactorManager reacMan;
    private enum reactorChooseDirection
    {
        UP,
        DOWN,
    }
    [Header("Indiquer si on choisit le réacteur du dessus ou du dessous")]
    [SerializeField] private reactorChooseDirection directionChoose = reactorChooseDirection.UP;
    public void Start()
    {
        reacMan = GameObject.Find("ReactorManager").GetComponent<ReactorManager>();
    }

    public void ChooseReactor()
    {
        if (reacMan.aligned == false)
        {
            if (reacMan.reactorMoving == false)
            {
                reacMan.Record();
                if (directionChoose == reactorChooseDirection.UP)
                {
                    reacMan.i = Mathf.Clamp(reacMan.i + 1, 0, reacMan.Reactors.Length - 1);
                }
                else if (directionChoose == reactorChooseDirection.DOWN)
                {
                    reacMan.i = Mathf.Clamp(reacMan.i - 1, 0, reacMan.Reactors.Length - 1);
                }
            }
        }
    }

    
}
