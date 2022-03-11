using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NebuleuseEmission : MonoBehaviour
{
    private TimeManager _timeManager = null;
    private Material[] materials = null;
    [SerializeField] private int _materialID = 0;
    private Color _baseColor;
    [SerializeField] private Color _lightOffColor;


    private void Awake()
    {
        materials = GetComponent<Renderer>().materials;
        _baseColor = materials[_materialID].GetColor("_EmissionColor");
        _timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        _timeManager.GetComponent<TimeManager>().ReactedToNebuleuse += delegate (bool isInNebuleuse)
        {
            materials[_materialID].SetColor("_EmissionColor", isInNebuleuse ? _lightOffColor : _baseColor);
            GetComponent<Renderer>().materials = materials;
        };
    }
}
