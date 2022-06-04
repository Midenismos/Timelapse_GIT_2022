using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public class ZoomScript : MonoBehaviour
{
    public Vector3 _originalPosition;
    public Quaternion _originalRotation;

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
    }

    //Pour faire le double clic
    void OnMouseUp()
    {
        clicked = false;
        if ((Time.time - clickStart) < 0.15f)
        {
            this.OnSimpleClick();
            clickStart = -1;
        }


    }

    void OnSimpleClick()
    {
        //Rapproche le doc du joueur ou le remet à sa place en cliquant dessus
        if (AxisScript.IDCurrentAxis == itemAxis && !AxisScript.IsInTI)
        {
            if (!_isLerping)
            {
                if (AxisScript.HasItem && AxisScript.CurrentHoldItem.GetComponent<ZoomScript>() != this)
                {
                    DezoomCurrentItem();

                }
                if (ZoomPos.GetComponent<ZoomPoint>().IsEmpty == true)
                {
                    if ( GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex == 3 && GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying)
                    {
                        GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().Pause();
                    }
                    if (GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex == 6 && !GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying)
                    {
                        GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex++;
                        StartCoroutine(GameObject.Find("TutorialManager").GetComponent<Tutorial>().LaunchNextDialogue(4));
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
                    if(GetComponent<AudioSource>())
                    {
                        GetComponent<AudioSource>().clip = _pickupSound;
                        GetComponent<AudioSource>().Play();
                    }
                    if( tag == "Written")
                        AxisScript.PutConsoleDown();

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
    }

    private void OnMouseDown()
    {
        if (IsZoomable)
        {
            clicked = true;
            clickStart = Time.time;
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
            if (AxisScript.CurrentHoldItem != null && AxisScript.CurrentHoldItem.GetComponent<ZoomScript>().clicked != true)
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
            if (AxisScript.IDCurrentAxis != AxisScript.CurrentHoldItem.GetComponent<ZoomScript>().itemAxis)
            {
                if (ZoomPos.GetComponent<ZoomPoint>().IsEmpty == false)
                {
                    DezoomCurrentItem();
                }
            }
        }

        if (Input.GetKeyDown("s") && AxisScript.SEnabled == true)
        {
            if (AxisScript.CurrentHoldItem != null && AxisScript.CurrentHoldItem.gameObject.tag != "Tape")
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
            GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex++;
            StartCoroutine(GameObject.Find("TutorialManager").GetComponent<Tutorial>().LaunchNextDialogue(2));
        }

        currentItem.posB = currentItem._originalPosition;
        if (currentItem.GetComponent<AudioSource>() != null)
        {
            currentItem.GetComponent<AudioSource>().clip = currentItem.PutDownSound;
            currentItem.GetComponent<AudioSource>().Play();
        }
        if (currentItem.gameObject.CompareTag("Written"))
        {
            if(AxisScript.IDCurrentAxis == 0 && !currentItem.IsFixed)
            {
                currentItem.posB.y = -29.75f;
                currentItem.posB.z = Mathf.Clamp(currentItem.posB.z, -55, Mathf.Infinity);
            }
            AxisScript.PutConsoleUp();
        }
        else if (currentItem.gameObject.CompareTag("Tape"))
        {
            currentItem.posB.x = Mathf.Clamp(currentItem.posB.x, -27, Mathf.Infinity);
            currentItem.posB.y = -30;
            currentItem.posB.z = Mathf.Clamp(currentItem.posB.z, -61, Mathf.Infinity);
        }

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
