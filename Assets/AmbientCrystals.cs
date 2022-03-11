using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientCrystals : MonoBehaviour
{

    [Header("Emission")]
    [SerializeField] private float minEmission = 1;
    [SerializeField] private float maxEmission = 100;
    [SerializeField] private AnimationCurve emissionCurve = null;

    [Header("References")]
    [SerializeField] private ParticleSystem particleSystemCrystal = null;

    private PlayerLoopManager playerLoopManager = null;


    // Start is called before the first frame update
    void Start()
    {
        playerLoopManager = FindObjectOfType<PlayerLoopManager>();

        if (!playerLoopManager)
        {
            Debug.LogError("WARNING AmbientCrystals needs a PlayerLoopManager in scene to function !");
        }
    }

    // Update is called once per frame
    void Update()
    {
        ParticleSystem.EmissionModule emissionModule = particleSystemCrystal.emission;
        emissionModule.rateOverTime = Mathf.Lerp(minEmission, maxEmission, emissionCurve.Evaluate(Mathf.InverseLerp(0, playerLoopManager.PlayerLoopDuration, playerLoopManager.currentPlayerLoopTime)));
        //Debug.Log(emissionCurve.Evaluate(Mathf.InverseLerp(0, playerLoopManager.PlayerLoopDuration, playerLoopManager.currentPlayerLoopTime)));
        //Debug.Log(Mathf.Lerp(minEmission, maxEmission, emissionCurve.Evaluate(Mathf.InverseLerp(0, playerLoopManager.PlayerLoopDuration, playerLoopManager.currentPlayerLoopTime))));
    }
}
