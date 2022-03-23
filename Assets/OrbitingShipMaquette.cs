using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingShipMaquette : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] [Range(0, 1)] private float shipProgress = 0;
    [SerializeField] [Range(0, 1)] private float startOffset = 0;
    [SerializeField] private bool standartMode = true;

    public bool StandartMode { get { return standartMode; } set { standartMode = value; } }

    [Header("Ellipse Parameters")]
    [SerializeField] private float ellipseMinRadius = 2;
    [SerializeField] private float ellipseMaxRadius = 3;

    [Header("References")]
    [SerializeField] private EllipseRenderer ellipse = null;
    [SerializeField] private Transform ship = null;

    private NewLoopManager timeManager = null;

    // Start is called before the first frame update
    void Start()
    {
        if (standartMode)
        {
            timeManager = FindObjectOfType<NewLoopManager>();
            if (!timeManager) Debug.LogError("OrbitingShipMaquette needs a timeManager to function");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(timeManager && standartMode)
        {
            ChangeShipPosition(timeManager.CurrentLoopTime, timeManager.LoopDuration);
        }
    }

    private void OnValidate()
    {
        PositionShip(shipProgress);
        UpdateEllipse();
    }

    public void ChangeShipPosition(float currentLoopTime, float loopDuration)
    {
        shipProgress = currentLoopTime / loopDuration;
        PositionShip(shipProgress);
    }

    private void PositionShip(float progress)
    {
        float angle = Mathf.Deg2Rad * (progress + startOffset) * 360f;
        ship.localPosition = new Vector3(Mathf.Sin(angle) * ellipseMinRadius, 0, Mathf.Cos(angle) * ellipseMaxRadius);
    }

    private void UpdateEllipse()
    {
        ellipse.UpdateParameters(ellipseMinRadius, ellipseMaxRadius);
    }
}
