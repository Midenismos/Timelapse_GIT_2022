using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleClickHandler : MonoBehaviour
{
    [SerializeField] private Color hoverColor;
    [SerializeField] private MeshRenderer meshRenderer = null;
    public UnityEvent OnClick;

    private MaterialPropertyBlock propBlock = null;

    private void Start()
    {
        propBlock = new MaterialPropertyBlock();
        meshRenderer.GetPropertyBlock(propBlock);
    }
    private void OnMouseDown()
    {
        OnClick?.Invoke();
    }

    private void OnMouseEnter()
    {
        propBlock.SetColor("_BaseColor", hoverColor);
        meshRenderer.SetPropertyBlock(propBlock);
    }

    private void OnMouseExit()
    {
        propBlock.SetColor("_BaseColor", Color.white);
        meshRenderer.SetPropertyBlock(propBlock);
    }
}
