using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateLayer : MonoBehaviour
{
    private void Start()
    {
        if (!GameObject.Find("TutorialManager").GetComponent<Tutorial>().activateTuto)
            ActivateRaycast("GrabbableCam");
    }
    public void ActivateRaycast(string layer)
    {
        int LayerIgnoreRaycast = LayerMask.NameToLayer(layer);
        gameObject.layer = LayerIgnoreRaycast;
        foreach (Transform g in transform.GetComponentsInChildren<Transform>())
            g.gameObject.layer = LayerIgnoreRaycast;
    }
}
