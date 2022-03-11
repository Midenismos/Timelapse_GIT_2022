using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceButton : MonoBehaviour, IInteractable
{
    public void PlayerHoverStart()
    {

    }

    public void PlayerHoverEnd()
    {

    }

    public void Interact(GameObject pickup, PlayerController player)
    {
        GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
    }

    public void InteractHolding(GameObject pickup, PlayerController player)
    {

    }

    public void StopInteractHolding(GameObject pickup, PlayerController player)
    {

    }


}
