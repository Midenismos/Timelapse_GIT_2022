using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectReceiver : MonoBehaviour, IInteractable
{
    public Transform receivedObjectMarker = null;
    [SerializeField] private MeshRenderer interactMesh = null;
    public string objectTagRequired = null;

    public UnityEvent OnObjectReceived;
    public UnityEvent OnObjectRemoved;

    //[HideInInspector]
    public GameObject held = null;

    public void DepositObject(GameObject obj)
    {
        
    }

    public void PlayerHoverStart()
    {
        interactMesh.enabled = true;
    }

    public void PlayerHoverEnd()
    {
        interactMesh.enabled = false;
    }

    public virtual void Interact(GameObject pickup, PlayerController player)
    {
        // place l'item dans le receiver
        if(pickup && !held && pickup.CompareTag(objectTagRequired))
        {
            held = pickup;

            pickup.transform.SetParent(receivedObjectMarker.transform, true);
            pickup.transform.position = receivedObjectMarker.position;
            pickup.transform.rotation = receivedObjectMarker.rotation;

            player.ReleaseHeldObject();

            OnObjectReceived?.Invoke();
        }
        // permet au joueur de retirer l'item du receiver
        else if(!pickup && held)
        {
            held = null;
        }
        
    }
    
    public void InteractHolding(GameObject pickup, PlayerController player)
    {

    }

    public void StopInteractHolding(GameObject pickup, PlayerController player)
    {

    }

    public void RewindObject()
    {
        held = null;
        if(receivedObjectMarker.transform.childCount != 0)
        {
            receivedObjectMarker.transform.GetChild(0).transform.parent = null;
            OnObjectRemoved?.Invoke();
        }
        
    }
}
