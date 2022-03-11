using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ellipse : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float ellipseMinRadius = 2;
    [SerializeField] private float ellipseMaxRadius = 3;

    [SerializeField] private float startOffset = 0.1f;

    [Header("Orbiters")]
    [SerializeField] private TimeManager timeManager = null;
    [SerializeField] private EllipseOrbiter nebula1 = null;
    [SerializeField] private EllipseOrbiter nebula2 = null;
    [SerializeField] private EllipseOrbiter audioPhase0 = null;
    [SerializeField] private EllipseOrbiter audioPhase1 = null;
    [SerializeField] private EllipseOrbiter audioPhase2 = null;
    [SerializeField] private EllipseOrbiter audioPhase3 = null;

    [Header("Reperes Nebulae")]
    [SerializeField] private EllipseOrbiter nebula1Start = null;
    [SerializeField] private EllipseOrbiter nebula1End = null;
    [SerializeField] private EllipseOrbiter nebula2Start = null;
    [SerializeField] private EllipseOrbiter nebula2End = null;

    [Header("Reperes AudioPhases")]
    [SerializeField] private EllipseOrbiter audioStart0 = null;
    [SerializeField] private EllipseOrbiter audioStart1 = null;
    [SerializeField] private EllipseOrbiter audioStart2 = null;
    [SerializeField] private EllipseOrbiter audioStart3 = null;

    [Header("Reperes LightPhases")]
    [SerializeField] private EllipseOrbiter lightPhaseStart = null;
    [SerializeField] private EllipseOrbiter lightPhaseEnd = null;





    public Vector3 GetLocalPositionOnEllipse(float progress)
    {
        float angle = Mathf.Deg2Rad * (progress + startOffset) * 360f;
        return new Vector3(Mathf.Sin(angle) * ellipseMinRadius, 0, Mathf.Cos(angle) * ellipseMaxRadius);
    }

    private void OnValidate()
    {
        if (timeManager)
        {
            nebula1.progress = GetProgressOfCenter(timeManager._nebuleuse1MinSecond, timeManager._nebuleuse1MaxSecond, timeManager.LoopDuration);
            nebula1.PlaceOnEllipse();
            nebula2.progress = GetProgressOfCenter(timeManager._nebuleuse2MinSecond, timeManager._nebuleuse2MaxSecond, timeManager.LoopDuration);
            nebula2.PlaceOnEllipse();

            audioPhase0.progress = GetProgressOfCenter(0, timeManager._audioPhaseChange1, timeManager.LoopDuration);
            audioPhase0.PlaceOnEllipse();
            audioPhase1.progress = GetProgressOfCenter(timeManager._audioPhaseChange1, timeManager._audioPhaseChange2, timeManager.LoopDuration);
            audioPhase1.PlaceOnEllipse();
            audioPhase2.progress = GetProgressOfCenter(timeManager._audioPhaseChange2, timeManager._audioPhaseChange3, timeManager.LoopDuration);
            audioPhase2.PlaceOnEllipse();
            audioPhase3.progress = GetProgressOfCenter(timeManager._audioPhaseChange3, timeManager.LoopDuration, timeManager.LoopDuration);
            audioPhase3.PlaceOnEllipse();

            nebula1Start.progress = timeManager._nebuleuse1MinSecond / timeManager.LoopDuration;
            nebula1Start.PlaceOnEllipse();
            nebula1End.progress = timeManager._nebuleuse1MaxSecond / timeManager.LoopDuration;
            nebula1End.PlaceOnEllipse();
            nebula2Start.progress = timeManager._nebuleuse2MinSecond / timeManager.LoopDuration;
            nebula2Start.PlaceOnEllipse();
            nebula2End.progress = timeManager._nebuleuse2MaxSecond / timeManager.LoopDuration;
            nebula2End.PlaceOnEllipse();

            audioStart0.progress = 0 / timeManager.LoopDuration;
            audioStart0.PlaceOnEllipse();
            audioStart1.progress = timeManager._audioPhaseChange1 / timeManager.LoopDuration;
            audioStart1.PlaceOnEllipse();
            audioStart2.progress = timeManager._audioPhaseChange2 / timeManager.LoopDuration;
            audioStart2.PlaceOnEllipse();
            audioStart3.progress = timeManager._audioPhaseChange3 / timeManager.LoopDuration;
            audioStart3.PlaceOnEllipse();

            lightPhaseStart.progress = timeManager._lightPhaseMinSecond / timeManager.LoopDuration;
            lightPhaseStart.PlaceOnEllipse();
            lightPhaseEnd.progress = timeManager._lightPhaseMaxSecond / timeManager.LoopDuration;
            lightPhaseEnd.PlaceOnEllipse();
        }
    }

    private float GetProgressOfCenter(float start, float end, float total)
    {
        return ((end - start) / 2 + start) / total;
    }
}
