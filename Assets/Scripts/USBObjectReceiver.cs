using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class USBObjectReceiver : ObjectReceiver
{
    [SerializeField] private CamScreenScript _camscreen = null;
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
            USB usb = held.GetComponent<USB>();
            //_camscreen.Movie.clip = usb.Video;
        }
        else
        {
            //_camscreen.Movie.clip = null;
        }
    }
    public override void Interact(GameObject pickup, PlayerController player)
    {
        // place l'item dans le receiver 
        if (pickup && pickup.CompareTag(objectTagRequired))
        {
            if (held == null)
            {
                held = pickup;

                pickup.transform.SetParent(receivedObjectMarker.transform, true);
                pickup.transform.position = receivedObjectMarker.position;
                pickup.transform.rotation = receivedObjectMarker.rotation;

                player.ReleaseHeldObject();

            }
        }

        // retire l'item du receiver 
        else if (held)
        {
            held = null;
            //_camscreen.Movie.Stop();
        }
    }
}
