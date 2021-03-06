using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class ZoomScript : MonoBehaviour, IClickable
{
    public Vector3 _originalPosition;
    public Quaternion _originalRotation;

    public Vector3 _fixedPosition;
    public Quaternion _fixedRotation;

    public float _zoomLerp = 0;
    public float _zoomCountdown = 1;
    public float _zoomSpeed = 0.2f;
    public bool _isLerping = false;
    public bool HasZoomed = false;

    public Vector3 posA, posB;

    public Quaternion rotA, rotB;

    float clickStart = 0;

    public int itemAxis = 0;

    public GameObject ZoomPos = null;

    private PlayerAxisScript AxisScript = null;

    public bool isCamera = false;

    private bool clicked = false;
    public bool IsZoomable = true;
    [SerializeField] private MeshRenderer _interactFeedBack;

    public bool IsFixed = false;

    public AudioClip PutDownSound;
    [SerializeField] private AudioClip _pickupSound;

    public bool TutoBriefing = false;
    public bool TutoCasette = false;
    public bool TutoCasetteZoomable = false;
    public bool _isFixedButDragable = false;

    private bool isClickable = true;
    private Action OnClicked;
    public Action GetOnClicked { get { return OnClicked; } set { OnClicked = value; } }


    private void Awake()
    {
        AxisScript = GameObject.Find("Player").GetComponent<PlayerAxisScript>();
        try
        {
            _interactFeedBack = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        }
        catch
        {
            _interactFeedBack = null;
        }

        if(isCamera)
        {
            _originalPosition = transform.parent.position;
            _originalRotation = transform.parent.rotation;
        }
        if(IsFixed)
        {
            _originalPosition = transform.position;
            _originalRotation = transform.rotation;
        }
        if(_isFixedButDragable)
        {
            _fixedPosition = transform.position;
            _fixedRotation = transform.rotation;
        }

        if(gameObject.name == "SchemaTI")
        {
            AxisScript.ReactedToChangePosition += delegate ()
            {
                _originalPosition = transform.position;
                _originalRotation = transform.rotation;
            };
        }
    }

    //Pour faire le double clic
    void OnMouseUp()
    {
        if(GameObject.Find("Player").GetComponent<PlayerAxisScript>().CanClick && !GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().IsTalkingTutorial)
        {
            clicked = false;
            if ((Time.time - clickStart) < 0.15f)
            {
                this.OnSimpleClick();
                clickStart = -1;
            }
        }
    }

    void OnSimpleClick()
    {
        //Rapproche le doc du joueur ou le remet à sa place en cliquant dessus
        if (AxisScript.IDCurrentAxis == itemAxis && !AxisScript.IsInTI && GameObject.Find("Player").GetComponent<PlayerAxisScript>().CanClick && !GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().IsTalkingTutorial)
        {
            if (!_isLerping)
            {
                if (AxisScript.HasItem && AxisScript.CurrentHoldItem.GetComponent<ZoomScript>() != this)
                {
                    DezoomCurrentItem();

                }
                if (ZoomPos.GetComponent<ZoomPoint>().IsEmpty == true)
                {
                    if (GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex == 3 && GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying)
                    {
                        //GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().Pause();
                    }
                    if (GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex == 6 && !GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying)
                    {
                        //GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex++;
                        //StartCoroutine(GameObject.Find("TutorialManager").GetComponent<Tutorial>().LaunchNextDialogue(3));
                    }
                    posA = _originalPosition;
                    posB = ZoomPos.transform.position;
                    rotA = _originalRotation;
                    rotB = ZoomPos.transform.rotation;
                    _zoomCountdown = 1;
                    _zoomLerp = 0;
                    ZoomPos.GetComponent<ZoomPoint>().IsEmpty = false;
                    _isLerping = true;
                    AxisScript.HasItem = true;
                    AxisScript.CurrentHoldItem = this.gameObject;
                    if (GetComponent<AudioSource>())
                    {
                        GetComponent<AudioSource>().clip = _pickupSound;
                        GetComponent<AudioSource>().Play();
                    }
                    //if( tag == "Written")
                    //AxisScript.PutConsoleDown();

                }
                /*else
                {
                    if (ZoomPos.GetComponent<ZoomPoint>().IsEmpty == false)
                    {
                        posB = _originalPosition;
                        posA = ZoomPos.transform.position;
                        rotB = _originalRotation;
                        rotA = ZoomPos.transform.rotation;
                        _zoomCountdown = 1;
                        _zoomLerp = 0;
                        ZoomPos.GetComponent<ZoomPoint>().IsEmpty = true;
                        _isLerping = true;
                        AxisScript.HasItem = false;
                    }
                }*/
            }

        }
        else if (gameObject.name == "SchemaTI" && GameObject.Find("Player").GetComponent<PlayerAxisScript>().CanClick && !GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().IsTalkingTutorial)
        {
            if (!_isLerping)
            {
                if (AxisScript.HasItem && AxisScript.CurrentHoldItem.GetComponent<ZoomScript>() != this)
                {
                    DezoomCurrentItem();
                }
                if (ZoomPos.GetComponent<ZoomPoint>().IsEmpty == true)
                {
                    posA = _originalPosition;
                    posB = ZoomPos.transform.position;
                    rotA = _originalRotation;
                    rotB = ZoomPos.transform.rotation;
                    _zoomCountdown = 1;
                    _zoomLerp = 0;
                    ZoomPos.GetComponent<ZoomPoint>().IsEmpty = false;
                    _isLerping = true;
                    AxisScript.HasItem = true;
                    AxisScript.CurrentHoldItem = this.gameObject;
                    if (GetComponent<AudioSource>())
                    {
                        GetComponent<AudioSource>().clip = _pickupSound;
                        GetComponent<AudioSource>().Play();
                    }
                }
            }
        }
    }

    private void OnMouseDown()
    {
        if (!isClickable)
        {
            OnClicked?.Invoke();
            return;
        }

        if (!GameObject.Find("TutorialManager").GetComponent<Tutorial>().activateTuto)
        {
            
            if (IsZoomable && GameObject.Find("Player").GetComponent<PlayerAxisScript>().CanClick && !GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().IsTalkingTutorial)
            {
                clicked = true;
                clickStart = Time.time;
            }
        }
        else 
        {
            if (TutoCasette)
            {
                if (IsZoomable && GameObject.Find("Player").GetComponent<PlayerAxisScript>().CanClick && !GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().IsTalkingTutorial && TutoCasetteZoomable)
                {
                    clicked = true;
                    clickStart = Time.time;
                }
            }
            else
            {
                if (IsZoomable && GameObject.Find("Player").GetComponent<PlayerAxisScript>().CanClick && !GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().IsTalkingTutorial)
                {
                    clicked = true;
                    clickStart = Time.time;
                }
            }

        }


    }
    private void Update()
    {
        if(GetComponentInChildren<TMP_InputField>())
        {
            if (AxisScript.CurrentHoldItem != this.gameObject)
                GetComponentInChildren<TMP_InputField>().DeactivateInputField();
            else
                GetComponentInChildren<TMP_InputField>().ActivateInputField();
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (AxisScript.CurrentHoldItem != null && AxisScript.CurrentHoldItem.GetComponent<ZoomScript>().clicked != true && GameObject.Find("Player").GetComponent<PlayerAxisScript>().CanClick && !GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().IsTalkingTutorial)
            {
                RaycastHit hit;
                Ray ray = GameObject.Find("Camera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.tag != "Button")
                        DezoomCurrentItem();
                }
            }
        }
        if (!HasZoomed && !_isLerping)
        {
            if(!isCamera && !IsFixed)
            {
                _originalPosition = transform.position;
                _originalRotation = transform.rotation;
            }


        }
        else
        {
            if (GetComponent<Rigidbody>() && !IsFixed)
                GetComponent<Rigidbody>().isKinematic = true;
            if (GetComponent<DragObjects>())
                GetComponent<DragObjects>().IsDragable = false;
        }
        //Lerp du papier
        if (_isLerping)
        {
            _zoomCountdown = Mathf.Clamp(_zoomCountdown - Time.unscaledDeltaTime * _zoomSpeed, 0f, 1f);

            if (_zoomCountdown == 0)
            {
                _isLerping = false;
                if(GameObject.Find("Player").GetComponent<PlayerAxisScript>().CurrentHoldItem == transform.gameObject)
                    HasZoomed = true;
                else
                    HasZoomed = false;
                if (gameObject.CompareTag("Tape") || gameObject.CompareTag("Written"))
                {
                    if(!IsFixed)
                        GetComponent<Rigidbody>().isKinematic = false;
                    if(GetComponent<DragObjects>())
                        GetComponent<DragObjects>().IsDragable = true;
                }
                if(GameObject.Find("Player").GetComponent<PlayerAxisScript>().CurrentHoldItem == transform.gameObject && tag != "Cam")
                {
                    transform.position = ZoomPos.transform.position;
                    transform.rotation = ZoomPos.transform.rotation;
                    if(gameObject.name == "SchemaTI" && !HasZoomed)
                    {
                        _originalPosition = ZoomPos.transform.position;
                        _originalRotation = ZoomPos.transform.rotation;
                    }
                }

            }

            if (!isCamera)
            {
                transform.position = Vector3.Lerp(posA, posB, _zoomLerp);
                transform.rotation = Quaternion.Slerp(rotA, rotB, _zoomLerp);
            }
            else
            {
                transform.parent.position = Vector3.Lerp(posA, posB, _zoomLerp);
            }


            _zoomLerp = 1f - _zoomCountdown;
        }

        //Remet l'objet à sa place si le joueur change d'axe
        if (AxisScript.CurrentHoldItem)
        { 
            if (AxisScript.IDCurrentAxis != AxisScript.CurrentHoldItem.GetComponent<ZoomScript>().itemAxis && AxisScript.CurrentHoldItem.name != "SchemaTI")
            {
                if (ZoomPos.GetComponent<ZoomPoint>().IsEmpty == false)
                {

                    DezoomCurrentItem();
                }
            }
        }

        if (Input.GetKeyDown("s") && AxisScript.SEnabled == true)
        {
            if (AxisScript.CurrentHoldItem != null && AxisScript.CurrentHoldItem.gameObject.tag != "Tape" && AxisScript.CurrentHoldItem.name != "SchemaTI")
            {
                if (ZoomPos.GetComponent<ZoomPoint>().IsEmpty == false)
                {
                    DezoomCurrentItem();
                }
            }
        }
        if (Input.GetKeyDown("z") && AxisScript.ZEnabled == true)
        {
            if (AxisScript.CurrentHoldItem != null && AxisScript.CurrentHoldItem.name == "SchemaTI")
            {
                if (ZoomPos.GetComponent<ZoomPoint>().IsEmpty == false)
                {
                    DezoomCurrentItem();
                }
            }
        }
    }

    public void DezoomCurrentItem()
    {
        ZoomScript currentItem = AxisScript.CurrentHoldItem.GetComponent<ZoomScript>();
        if (GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex == 3 && GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying)
        {
            GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().UnPause();
        }
        if (currentItem.TutoBriefing && GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex == 2 && !GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying)
        {
            GameObject.Find("TutorialManager").GetComponent<Tutorial>().DialogueFinished();
            GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex++;
            StartCoroutine(GameObject.Find("TutorialManager").GetComponent<Tutorial>().LaunchNextDialogue(2));
        }
        if (currentItem.TutoCasette && GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex == 9 && !GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying)
        {
            if(currentItem.GetComponentInChildren<TMP_Text>().text != "#8-TW​" && currentItem.GetComponentInChildren<TMP_Text>().text != "")
            {
                if (GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying)
                    GameObject.Find("TutorialManager").GetComponent<Tutorial>().DialogueFinished();

                GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex++;
                StartCoroutine(GameObject.Find("TutorialManager").GetComponent<Tutorial>().LaunchNextDialogue(2));
            }
        }
        
        if(!TutoCasette)
        {
            currentItem.posB = currentItem._originalPosition;
            if (currentItem.GetComponent<AudioSource>() != null)
            {
                currentItem.GetComponent<AudioSource>().clip = currentItem.PutDownSound;
                currentItem.GetComponent<AudioSource>().Play();
            }
            if (currentItem.gameObject.CompareTag("Written") && currentItem.gameObject.name != "SchemaTI")
            {
                if (AxisScript.IDCurrentAxis == 0 && !currentItem.IsFixed)
                {
                    print("hey");
                    currentItem.posB.x = Mathf.Clamp(currentItem.posB.x, -35.5f, -32f);
                    currentItem.posB.y = -29.5f;
                    currentItem.posB.z = Mathf.Clamp(currentItem.posB.z, -56.5f, -55.5f);
                }
                //AxisScript.PutConsoleUp();
            }
            else if (currentItem.gameObject.CompareTag("Tape"))
            {
                currentItem.posB.x = Mathf.Clamp(currentItem.posB.x, -27, -24.5f);
                currentItem.posB.y = -30;
                currentItem.posB.z = Mathf.Clamp(currentItem.posB.z, -62.5f, -61);
            }

            if (!currentItem.isCamera)
            {
                currentItem.posA = currentItem.ZoomPos.transform.position;
                currentItem.rotB = currentItem._originalRotation;
                currentItem.rotA = currentItem.ZoomPos.transform.rotation;
                currentItem._zoomCountdown = 1;
                currentItem._zoomLerp = 0;
                currentItem.ZoomPos.GetComponent<ZoomPoint>().IsEmpty = true;
                currentItem._isLerping = true;
                AxisScript.HasItem = false;
                AxisScript.CurrentHoldItem = null;
            }
            else if (!AxisScript.MouseInConsole)
            {
                currentItem.posA = currentItem.ZoomPos.transform.position;
                currentItem.rotB = currentItem._originalRotation;
                currentItem.rotA = currentItem.ZoomPos.transform.rotation;
                currentItem._zoomCountdown = 1;
                currentItem._zoomLerp = 0;
                currentItem.ZoomPos.GetComponent<ZoomPoint>().IsEmpty = true;
                currentItem._isLerping = true;
                AxisScript.HasItem = false;
                AxisScript.CurrentHoldItem = null;
            }
        }
        else if (TutoCasetteZoomable)
        {
            currentItem.posB = currentItem._originalPosition;
            if (currentItem.GetComponent<AudioSource>() != null)
            {
                currentItem.GetComponent<AudioSource>().clip = currentItem.PutDownSound;
                currentItem.GetComponent<AudioSource>().Play();
            }
            if (currentItem.gameObject.CompareTag("Written"))
            {
                if (AxisScript.IDCurrentAxis == 0 && !currentItem.IsFixed)
                {
                    currentItem.posB.x = Mathf.Clamp(currentItem.posB.x, -35.5f, -32f);
                    currentItem.posB.y = -29.5f;
                    currentItem.posB.z = Mathf.Clamp(currentItem.posB.z, -56.5f, -55.5f);
                }
                //AxisScript.PutConsoleUp();
            }
            else if (currentItem.gameObject.CompareTag("Tape"))
            {
                currentItem.posB.x = Mathf.Clamp(currentItem.posB.x, -27, -24.5f);
                currentItem.posB.y = -30;
                currentItem.posB.z = Mathf.Clamp(currentItem.posB.z, -62.5f, -61);
            }

            if (!currentItem.isCamera)
            {
                currentItem.posA = currentItem.ZoomPos.transform.position;
                currentItem.rotB = currentItem._originalRotation;
                currentItem.rotA = currentItem.ZoomPos.transform.rotation;
                currentItem._zoomCountdown = 1;
                currentItem._zoomLerp = 0;
                currentItem.ZoomPos.GetComponent<ZoomPoint>().IsEmpty = true;
                currentItem._isLerping = true;
                AxisScript.HasItem = false;
                AxisScript.CurrentHoldItem = null;
            }
            else if (!AxisScript.MouseInConsole)
            {
                currentItem.posA = currentItem.ZoomPos.transform.position;
                currentItem.rotB = currentItem._originalRotation;
                currentItem.rotA = currentItem.ZoomPos.transform.rotation;
                currentItem._zoomCountdown = 1;
                currentItem._zoomLerp = 0;
                currentItem.ZoomPos.GetComponent<ZoomPoint>().IsEmpty = true;
                currentItem._isLerping = true;
                AxisScript.HasItem = false;
                AxisScript.CurrentHoldItem = null;
            }
        }

        
    }


    private void OnMouseExit()
    {
        if (_interactFeedBack && GameObject.Find("Player").GetComponent<PlayerAxisScript>().CanClick && !GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().IsTalkingTutorial)
            _interactFeedBack.enabled = false;
    }
    private void OnMouseEnter()
    {
        if (_interactFeedBack && GameObject.Find("Player").GetComponent<PlayerAxisScript>().CanClick && !GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().IsTalkingTutorial)
            _interactFeedBack.enabled = true;
    }

    public void PutBackFixedWrittenDoc()
    {
        transform.position = _fixedPosition;
        transform.rotation = _fixedRotation;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<DragObjects>().IsDragged = false;
    }

    public void MakeZoomable()
    {
        TutoCasetteZoomable = true;
    }

    public void SetClickable(bool isClickable)
    {
        this.isClickable = isClickable;
    }
}
