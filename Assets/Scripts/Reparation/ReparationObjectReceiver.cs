using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReparationObjectReceiver : ObjectReceiver
{
    public enum ColorOR
    {
        YELLOW = 1,
        RED = 2,
        BLUE = 3,
        GREEN = 4
    }

    [SerializeField] private ColorOR CorrectColor = ColorOR.YELLOW;

    [SerializeField]
    private bool _isActivated = false;
    public bool IsActivated
    {
        get { return _isActivated; }
        set
        {
            if (value != _isActivated)
                _isActivated = value;
        }
    }

    private void Update()
    {
        if (held)
        {
            Fusible Fus = held.GetComponent<Fusible>();
            if ((int)Fus.FusColor == (int)CorrectColor)
            {
                if (IsActivated == false)
                {
                    OnObjectReceived?.Invoke();
                    IsActivated = true;
                }
            }
            else
            {
                IsActivated = false;
            }
        }
        else
        {
            IsActivated = false;
        }
    }
    public override void Interact(GameObject pickup, PlayerController player)
    {
        // place l'item dans le receiver et ajoute 1 au ReparationPlomb si c'est la bonne couleur
        if (pickup && pickup.CompareTag(objectTagRequired))
        {
            if (held == null)
            {
                //Fusible Fus = pickup.GetComponent<Fusible>();
                held = pickup;

                pickup.transform.SetParent(receivedObjectMarker.transform, true);
                pickup.transform.position = receivedObjectMarker.position;
                pickup.transform.rotation = receivedObjectMarker.rotation;

                player.ReleaseHeldObject();
                /*
                if ((int)Fus.FusColor == (int)CorrectColor)
                {
                    OnObjectReceived?.Invoke();
                }*/
            }
        }

        // retire l'item du receiver et retire 1 au ReparationPlomb si c'était la bonne couleur
        else if (held)
        {
            Fusible Fus = held.GetComponent<Fusible>();
            if ((int)Fus.FusColor == (int)CorrectColor)
            {
                //OnObjectRemoved?.Invoke();
                IsActivated = false;
            }
            held = null;
        }
    }
}
