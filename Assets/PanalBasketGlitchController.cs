using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanalBasketGlitchController : MonoBehaviour
{
    [SerializeField] private float glitchOnCenterLength = 0;
    [SerializeField] private float glitchOnAxisLength = 0.18f;

    [SerializeField] private float glitchOffCenterLength = 3;
    [SerializeField] private float glitchOffAxisLength = 0;

    private Material targetMaterial = null;

    private bool isGlitched = false;
    // Start is called before the first frame update
    void Start()
    {
        targetMaterial = GetComponent<Renderer>().material;

        isGlitched = GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>().Energy <= 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGlitchEffect();
    }

    private void UpdateGlitchEffect()
    {

        if(GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>().Energy <= 0 && !isGlitched)
        {
            targetMaterial.SetFloat("_BandCenterLength", glitchOnCenterLength);
            targetMaterial.SetFloat("_BandAxisLength", glitchOnAxisLength);

            isGlitched = true;


        }
        else if (GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>().Energy > 0 && isGlitched)
        {
            targetMaterial.SetFloat("_BandCenterLength", glitchOffCenterLength);
            targetMaterial.SetFloat("_BandAxisLength", glitchOffAxisLength);

            isGlitched = false;


        }
    }
}
