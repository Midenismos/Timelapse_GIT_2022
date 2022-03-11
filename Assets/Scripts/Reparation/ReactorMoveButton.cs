using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactorMoveButton : Button
{
    private ReactorManager ReacMan;
    private enum ButtonDirection
    {
        LEFT,
        RIGHT
    }

    [Header("Indiquer la direction où le réacteur est censé aller")]
    [SerializeField] private ButtonDirection directionMove = ButtonDirection.LEFT;
    public void Start()
    {
        ReacMan = GameObject.Find("ReactorManager").GetComponent<ReactorManager>();
    }
    public void Move()
    {
        if (ReacMan.IsWorking)
        {
            if (ReacMan.aligned == false)
            {
                if (ReacMan.reactorMoving == false)
                {
                    Reactor SelectedReactor = ReacMan.selectedReactor.GetComponent<Reactor>();
                    if (directionMove == ButtonDirection.LEFT)
                        SelectedReactor.curWayPointNumber = Mathf.Clamp(SelectedReactor.curWayPointNumber - 1, 0, SelectedReactor.WayPoints.Length - 1);
                    else if (directionMove == ButtonDirection.RIGHT)
                        SelectedReactor.curWayPointNumber = Mathf.Clamp(SelectedReactor.curWayPointNumber + 1, 0, SelectedReactor.WayPoints.Length - 1);
                }
            }
        }
    }
}
