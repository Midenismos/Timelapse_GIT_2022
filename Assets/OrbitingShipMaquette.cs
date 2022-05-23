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
    [SerializeField] private float[] MiddleValue = new float[6];

    [SerializeField] private NewLoopManager timeManager = null;
    private float SceneWidth;
    private Vector3 PressPoint;
    private Quaternion StartRotation;

    private MeshRenderer _interactFeedBack;


    // Start is called before the first frame update
    void Start()
    {
        try
        {
            _interactFeedBack = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        }
        catch
        {
            _interactFeedBack = null;
        }
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
        timeManager = GameObject.Find("LoopManager").GetComponent<NewLoopManager>();
        PositionShip(shipProgress);
        UpdateEllipse();

        for (int i = 0; i <= MiddleValue.Length-1; i++)
        { 
            MiddleValue[i] = (timeManager.Nebuleuses[i].end - (timeManager.Nebuleuses[i].end - timeManager.Nebuleuses[i].start) / 2) / timeManager.LoopDuration;
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
            GetComponent<AudioSource>().Play();
        }

    }
    private void OnMouseUp()
    {
        GetComponent<AudioSource>().Stop();
    }

    private void OnMouseDrag()
    {
        if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 2)
        {
            float Speed = Mathf.Clamp((Input.mousePosition - PressPoint).x, -200, 200);
            GetComponent<Rigidbody>().MoveRotation(GetComponent<Rigidbody>().rotation * Quaternion.Euler(Vector3.up * (Speed / SceneWidth)*10));
            GetComponent<AudioSource>().pitch = (Speed / 200) * 1.05f;
        }
    }

    private void OnMouseExit()
    {
        if (_interactFeedBack)
            _interactFeedBack.enabled = false;
    }
    private void OnMouseEnter()
    {
        if (_interactFeedBack)
            _interactFeedBack.enabled = true;
    }
}
