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
    [SerializeField] private Transform[] _nebuleuses;
    [SerializeField] private float[] MiddleValue = new float[5];

    [SerializeField] private NewLoopManager timeManager = null;
    private float SceneWidth;
    private Vector3 PressPoint;
    private Quaternion StartRotation;


    // Start is called before the first frame update
    void Start()
    {
        SceneWidth = Screen.width;

        /*if (standartMode)
        {
            timeManager = FindObjectOfType<NewLoopManager>();
            if (!timeManager) Debug.LogError("OrbitingShipMaquette needs a timeManager to function");
        }*/
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

        // TODO better
        MiddleValue[0] = (timeManager.PurpleNebuleuse1End - (timeManager.PurpleNebuleuse1End - timeManager.PurpleNebuleuse1Start) / 2)/ timeManager.LoopDuration;
        MiddleValue[1] = (timeManager.PurpleNebuleuse2End - ( timeManager.PurpleNebuleuse2End - timeManager.PurpleNebuleuse2Start) / 2) / timeManager.LoopDuration; 
        MiddleValue[2] = (timeManager.YellowNebuleuseEnd - (timeManager.YellowNebuleuseEnd - timeManager.YellowNebuleuseStart) / 2) / timeManager.LoopDuration; 
        MiddleValue[3] = (timeManager.GreenNebuleuseEnd - (timeManager.GreenNebuleuseEnd - timeManager.GreenNebuleuseStart) / 2) / timeManager.LoopDuration; 
        MiddleValue[4] = (timeManager.BlueNebuleuseEnd - (timeManager.BlueNebuleuseEnd - timeManager.BlueNebuleuseStart) / 2) / timeManager.LoopDuration; 

        for (int i = 0; i <= 4; i++)
        {
            PositionNebuleuse(_nebuleuses[i], MiddleValue[i]);
        }
        

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

    private void PositionNebuleuse(Transform nebuleuse, float Position)
    {
        float angle = Mathf.Deg2Rad * (Position + startOffset) * 360f;
        nebuleuse.localPosition = new Vector3(Mathf.Sin(angle) * ellipseMinRadius, 0, Mathf.Cos(angle) * ellipseMaxRadius);
    }

    private void UpdateEllipse()
    {
        ellipse.UpdateParameters(ellipseMinRadius, ellipseMaxRadius);
    }


    // Gère la rotation de la planète
    private void OnMouseDown()
    {
        if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 2)
        {
            PressPoint = Input.mousePosition;
            StartRotation = transform.rotation;
        }

    }

    private void OnMouseDrag()
    {
        if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 2)
        {
            float CurrentDistanceBetweenMousePositions = (Input.mousePosition - PressPoint).x;
            transform.rotation = StartRotation * Quaternion.Euler(Vector3.up * (CurrentDistanceBetweenMousePositions / SceneWidth) * 360);
        }
    }
}
