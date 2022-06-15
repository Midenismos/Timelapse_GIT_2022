using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAxisScript : MonoBehaviour
{
    [SerializeField] private Vector3[] _axisRotation;
    [SerializeField] private Vector3[] _camRotation;
    [SerializeField] private Vector3[] _camPosition;
    [SerializeField] private Vector3[] _consolePosition;
    [SerializeField] private Vector3[] _panelBasketPosition;
    private Vector3 _currentAxisRotation;
    private Vector3 _currentCamRotation;
    private Vector3 _currentCamPosition;
    private Vector3 _currentConsolePosition;
    private Vector3 _currentPanelBasketPosition;

    private Vector3 _targetRotation;
    private Vector3 _targetPosition;
    private Vector3 _targetCamRotation;
    private Vector3 _targetConsolePosition;
    private Vector3 _targetPanelBasketPosition;


    public int IDCurrentAxis = 0;
    [SerializeField] private float _moveLerp = 0;
    [SerializeField] private float _rotationCountdown = 1;
    [SerializeField] private float _rotationSpeed = 0.2f;
    private bool _isLerping = false;
    public bool HasItem = false;
    public GameObject CurrentHoldItem = null;
    private GameObject cam;
    private GameObject console;
    public bool IsInTI = false;
    public bool IsDraging = false;
    public bool canMove = true;

    [SerializeField] private InterfaceAnimManager _TI = null;

    public bool isInTuto = true;
    public bool CanClick = false;
    public bool QEnabled = false;
    public bool DEnabled = false;
    public bool ZEnabled = false;
    public bool SEnabled = false;
    public bool MouseInConsole = false;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Camera");
        console = GameObject.Find("Console");
        Cursor.lockState = CursorLockMode.Confined;
        IDCurrentAxis = 0;
        _currentAxisRotation = _axisRotation[IDCurrentAxis];
        _currentCamRotation = _camRotation[IDCurrentAxis];
        _currentCamPosition = _camPosition[IDCurrentAxis];
        _currentConsolePosition = _consolePosition[IDCurrentAxis];
        _currentPanelBasketPosition = _panelBasketPosition[1];
    }

    // Update is called once per frame
    void Update()
    {
        if(isInTuto)
        {
            if (Input.GetKeyDown("d") && DEnabled)
            {
                if (!_isLerping && !IsInTI && !GameObject.Find("PanelBasket").GetComponent<PanelBasketScript>()._isLerping)
                {
                    IDCurrentAxis += 1;
                    if (IDCurrentAxis < 0)
                        IDCurrentAxis = _axisRotation.Length - 1;
                    else if (IDCurrentAxis > _axisRotation.Length - 1)
                        IDCurrentAxis = 0;
                    _rotationCountdown = 1;
                    _moveLerp = 0;
                    _targetRotation = _axisRotation[IDCurrentAxis];
                    _targetPosition = _camPosition[IDCurrentAxis];
                    _targetConsolePosition = _consolePosition[IDCurrentAxis];
                    _targetCamRotation = _camRotation[IDCurrentAxis];
                    _isLerping = true;
                    GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
                    GetComponent<AudioSource>().Play();
                    if (IDCurrentAxis == 3)
                        _targetPanelBasketPosition = _panelBasketPosition[1];
                    else
                        _targetPanelBasketPosition = _panelBasketPosition[0];
                }
            }
            if (Input.GetKeyDown("q") && QEnabled)
            {
                if (!_isLerping && !IsInTI && !GameObject.Find("PanelBasket").GetComponent<PanelBasketScript>()._isLerping)
                {
                    IDCurrentAxis -= 1;
                    if (IDCurrentAxis < 0)
                        IDCurrentAxis = _axisRotation.Length - 1;
                    else if (IDCurrentAxis > _axisRotation.Length - 1)
                        IDCurrentAxis = 0;
                    _rotationCountdown = 1;
                    _moveLerp = 0;
                    _targetRotation = _axisRotation[IDCurrentAxis];
                    _targetPosition = _camPosition[IDCurrentAxis];
                    _targetConsolePosition = _consolePosition[IDCurrentAxis];
                    _targetCamRotation = _camRotation[IDCurrentAxis];
                    _isLerping = true;
                    GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
                    GetComponent<AudioSource>().Play();

                    if (IDCurrentAxis == 3)
                        _targetPanelBasketPosition = _panelBasketPosition[1];
                    else
                        _targetPanelBasketPosition = _panelBasketPosition[0];

                }
            }
            if (Input.GetKeyDown("s") && SEnabled)
            {
                if (!_isLerping && !IsInTI && CurrentHoldItem == null)
                {
                    _rotationCountdown = 1;
                    _moveLerp = 0;
                    _targetConsolePosition = new Vector3(console.transform.localPosition.x - 3f, console.transform.localPosition.y, console.transform.localPosition.z);
                    _targetPosition = new Vector3(cam.transform.localPosition.x, 1, 4f);
                    _targetCamRotation = new Vector3(90, cam.transform.rotation.eulerAngles.y, cam.transform.rotation.eulerAngles.z);
                    _isLerping = true;
                    IsInTI = true;
                    _TI.startAppear();
                }
            }
            if (Input.GetKeyDown("z") && ZEnabled)
            {
                if (!_isLerping && IsInTI && GameObject.Find("EventSystem").GetComponent<EventSystem>().currentSelectedGameObject == null)
                {
                    _rotationCountdown = 1;
                    _moveLerp = 0;
                    _targetConsolePosition = _consolePosition[IDCurrentAxis];
                    _targetPosition = _camPosition[IDCurrentAxis];
                    _targetCamRotation = _camRotation[IDCurrentAxis];
                    _isLerping = true;
                    IsInTI = false;
                    _TI.startDisappear();
                }
            }
        }
        else if(!HasItem && !IsDraging && canMove && !isInTuto)
        {
            if (Input.GetKeyDown("d"))
            {
                if (!_isLerping && !IsInTI && !GameObject.Find("PanelBasket").GetComponent<PanelBasketScript>()._isLerping)
                {
                    IDCurrentAxis += 1;
                    if (IDCurrentAxis < 0)
                        IDCurrentAxis = _axisRotation.Length - 1;
                    else if (IDCurrentAxis > _axisRotation.Length - 1)
                        IDCurrentAxis = 0;
                    _rotationCountdown = 1;
                    _moveLerp = 0;
                    _targetRotation = _axisRotation[IDCurrentAxis];
                    _targetPosition = _camPosition[IDCurrentAxis];
                    _targetConsolePosition = _consolePosition[IDCurrentAxis];
                    _targetCamRotation = _camRotation[IDCurrentAxis];
                    _isLerping = true;
                    GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
                    GetComponent<AudioSource>().Play();
                    if (IDCurrentAxis == 3)
                        _targetPanelBasketPosition = _panelBasketPosition[1];
                    else
                        _targetPanelBasketPosition = _panelBasketPosition[0];
                }
            }
            if (Input.GetKeyDown("q"))
            {
                if (!_isLerping && !IsInTI && !GameObject.Find("PanelBasket").GetComponent<PanelBasketScript>()._isLerping)
                {
                    IDCurrentAxis -= 1;
                    if (IDCurrentAxis < 0)
                        IDCurrentAxis = _axisRotation.Length - 1;
                    else if (IDCurrentAxis > _axisRotation.Length - 1)
                        IDCurrentAxis = 0;
                    _rotationCountdown = 1;
                    _moveLerp = 0;
                    _targetRotation = _axisRotation[IDCurrentAxis];
                    _targetPosition = _camPosition[IDCurrentAxis];
                    _targetConsolePosition = _consolePosition[IDCurrentAxis];
                    _targetCamRotation = _camRotation[IDCurrentAxis];
                    _isLerping = true;
                    GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
                    GetComponent<AudioSource>().Play();
                    if (IDCurrentAxis == 3)
                        _targetPanelBasketPosition = _panelBasketPosition[1];
                    else
                        _targetPanelBasketPosition = _panelBasketPosition[0];
                }
            }
            if (Input.GetKeyDown("s"))
            {
                if (!_isLerping && !IsInTI && CurrentHoldItem == null)
                {
                    _rotationCountdown = 1;
                    _moveLerp = 0;
                    _targetConsolePosition = new Vector3(console.transform.localPosition.x-3f, console.transform.localPosition.y, console.transform.localPosition.z);
                    _targetPosition = new Vector3(cam.transform.localPosition.x, 1, 4f);
                    _targetCamRotation = new Vector3(90, cam.transform.rotation.eulerAngles.y, cam.transform.rotation.eulerAngles.z);
                    _isLerping = true;
                    IsInTI = true;
                    _TI.startAppear();
                }
            }
            if (Input.GetKeyDown("z"))
            {
                if (!_isLerping && IsInTI && GameObject.Find("EventSystem").GetComponent<EventSystem>().currentSelectedGameObject == null)
                {
                    _rotationCountdown = 1;
                    _moveLerp = 0;
                    _targetConsolePosition = _consolePosition[IDCurrentAxis];
                    _targetPosition = _camPosition[IDCurrentAxis];
                    _targetCamRotation = _camRotation[IDCurrentAxis];
                    _isLerping = true;
                    IsInTI = false;
                    _TI.startDisappear();
                }
            }
        }

        //Gère le lerp des sons lors d'un changement temporel

        if (_isLerping)
        {
            _rotationCountdown = Mathf.Clamp(_rotationCountdown - Time.unscaledDeltaTime * _rotationSpeed, 0f, 1f);

            if (_rotationCountdown == 0)
            {
                transform.rotation = Quaternion.Euler(_targetRotation);
                cam.transform.localPosition = _targetPosition;
                cam.transform.rotation = Quaternion.Euler(_targetCamRotation);
                GameObject.Find("PanelBasket").transform.localPosition = _targetPanelBasketPosition;
                _currentAxisRotation = transform.rotation.eulerAngles;
                _currentCamRotation = cam.transform.rotation.eulerAngles;
                _currentCamPosition = cam.transform.localPosition;
                _currentConsolePosition = console.transform.localPosition;
                _currentPanelBasketPosition = GameObject.Find("PanelBasket").transform.localPosition;
                _isLerping = false;
            }
            transform.rotation = Quaternion.Slerp( Quaternion.Euler(_currentAxisRotation), Quaternion.Euler(_targetRotation), _moveLerp);
            cam.transform.localPosition = Vector3.Lerp(_currentCamPosition, _targetPosition, _moveLerp);
            cam.transform.rotation = Quaternion.Slerp(Quaternion.Euler(_currentCamRotation), Quaternion.Euler(_targetCamRotation), _moveLerp);
            console.transform.localPosition = Vector3.Lerp(_currentConsolePosition, _targetConsolePosition, _moveLerp);
            GameObject.Find("PanelBasket").transform.localPosition = Vector3.Lerp(_currentPanelBasketPosition, _targetPanelBasketPosition, _moveLerp);
            _moveLerp = 1f - _rotationCountdown;
        }
    }

    public void RotateToAxis(int axisNumber)
    {
        IDCurrentAxis = axisNumber;

        _rotationCountdown = 1;
        _moveLerp = 0;
        _targetRotation = _axisRotation[IDCurrentAxis];
        _targetPosition = _camPosition[IDCurrentAxis];
        _targetConsolePosition = _consolePosition[IDCurrentAxis];
        _targetCamRotation = _camRotation[IDCurrentAxis];
        if (IDCurrentAxis == 3)
            _targetPanelBasketPosition = _panelBasketPosition[1];
        else
            _targetPanelBasketPosition = _panelBasketPosition[0];
        _isLerping = true;
    }

    public void PutConsoleDown()
    {
        _rotationCountdown = 1;
        _moveLerp = 0;
        _currentConsolePosition = _consolePosition[IDCurrentAxis];
        _targetConsolePosition = new Vector3(console.transform.localPosition.x, console.transform.localPosition.y-0.2f, console.transform.localPosition.z);
        _targetPosition = _camPosition[IDCurrentAxis];
        _targetCamRotation = _camRotation[IDCurrentAxis];
        _isLerping = true;
        IsInTI = false;
    }

    public void PutConsoleUp()
    {
        _rotationCountdown = 1;
        _moveLerp = 0;
        _currentConsolePosition = new Vector3(console.transform.localPosition.x, console.transform.localPosition.y - 0.2f, console.transform.localPosition.z);
        _targetConsolePosition = _consolePosition[IDCurrentAxis];
        _targetPosition = _camPosition[IDCurrentAxis];
        _targetCamRotation = _camRotation[IDCurrentAxis];
        _isLerping = true;
        IsInTI = false;
    }


    public void QTrue()
    {
        QEnabled = true;
    }
    public void QFalse()
    {
        QEnabled = false;
    }
    public void DTrue()
    {
        DEnabled = true;
    }
    public void DFalse()
    {
        DEnabled = false;
    }
    public void ZTrue()
    {
        ZEnabled = true;
    }
    public void ZFalse()
    {
        ZEnabled = false;
    }
    public void STrue()
    {
        SEnabled = true;
    }
    public void SFalse()
    {
        SEnabled = false;
    }
    public void ClickTrue()
    {
        CanClick = true;
    }
    public void ClickFalse()
    {
        CanClick = false;
    }
    public void ForceOutOfTI()
    {
        DragObjects[] dragables = FindObjectsOfType<DragObjects>();
        foreach (DragObjects dragable in dragables)
        {
            if(dragable.IsDragged)
            {
                dragable.IsDragable = false;
                dragable.IsDragged = false;
                dragable.OnMouseUp();
            }
        }

        IsDraging = false;
        _rotationCountdown = 1;
        _moveLerp = 0;
        _targetConsolePosition = _consolePosition[IDCurrentAxis];
        _targetPosition = _camPosition[IDCurrentAxis];
        _targetCamRotation = _camRotation[IDCurrentAxis];
        _isLerping = true;
        IsInTI = false;
        _TI.startDisappear();
    }




}
