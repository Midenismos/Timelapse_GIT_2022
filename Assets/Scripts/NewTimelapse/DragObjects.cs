using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragObjects : MonoBehaviour, IClickable
{

    private Vector3 mOffset;

    private float mZCoord;

    public bool IsDragable = true;
    public bool IsDragged = false;
    public bool Is3D = true;
    public bool IsFixedInTI = false;
    [SerializeField] private MeshRenderer _interactFeedBack;
    [SerializeField] private int _axisID = 0;
    public GameObject EntryDispenser = null;
    public GameObject EntrySlot = null;

    public bool isTutoTI1 = false;
    public bool isTutoTI2 = false;
    public bool hasBeenTutoScaned = false;
    public SheetImageScript[] SheetImages;
    public bool isFixable = false;

    private bool isClickable = true;
    private Action OnClicked;
    public Action GetOnClicked
    {
        get { return OnClicked; }
        set { OnClicked = value; }
    }


    private void Awake()
    {
        try
        {
            _interactFeedBack = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        }
        catch
        {
            _interactFeedBack = null;
        }

        if (tag == "Entry")
        {
            IsDragable = false;      
            StartCoroutine(EntryCoolDown());

        }
        //if (GetComponent<TapeScript>() && !GameObject.Find("TutorialManager").GetComponent<Tutorial>().activateTuto)
        //ActivateRaycast("Tape");
    }
    public void StartDrag() { StartCoroutine(SyntheticDrag()); }
    private void Start()
    {
        if (Input.GetMouseButtonDown(0) && tag == "Cam")
        {
            StartCoroutine(SyntheticDrag());
        }
    }
    public void OnMouseDown()
    {
        if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().CanClick && !GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().IsTalkingTutorial)
        {
            if(!isClickable)
            {
                OnClicked?.Invoke();
                return;
            }
            isFixable = false;
            StartCoroutine(FixableCooldown());
            if (tag == "PanelImage")
                GetComponent<AudioSource>().Play();
            if (tag == "Entry")
            {
                if (EntryDispenser != null)
                {
                    EntryDispenser.GetComponent<DispenserManager>().CurrentDrag = null;
                    EntryDispenser = null;
                    transform.SetParent(GameObject.Find("TI").transform, true);
                }
                if (GetComponent<Highlight>())
                    GetComponent<Highlight>().StopHighlightChildren();
            }
            if (tag == "PanelImage" || tag == "Entry" || GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == _axisID)
            {
                if (isTutoTI1 && GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex == 14)
                {
                    GetComponent<Highlight>().StopHighlight();
                    if (!GameObject.Find("PanelBasket").GetComponent<Highlight>().Highlighted)
                        GameObject.Find("PanelBasket").GetComponent<Highlight>().BeginHighlight();
                }
                if (GetComponent<ZoomScript>())
                {
                    if (!GetComponent<ZoomScript>().HasZoomed)
                        GetComponent<DragObjects>().IsDragable = true;
                }
                else
                {
                    GetComponent<DragObjects>().IsDragable = true;
                    if (tag == "Battery")
                        transform.SetParent(null, true);
                    else if (tag == "PanelImage")
                    {
                        if (EntrySlot != null)
                        {
                            EntrySlot.GetComponent<SheetImageScript>().IsFilled = false;
                            EntrySlot.GetComponent<SheetImageScript>().ID = "";
                            EntrySlot.GetComponent<SheetImageScript>().isGlitched = false;
                            EntrySlot = null;
                            GameObject.Find("TI").GetComponent<TIPanelImageData>().PanelImageList.Add(GetComponent<PanelTag>());
                        }
                        transform.SetParent(GameObject.Find("TI").transform, true);

                    }

                }

                if (IsDragable)
                {
                    if (GetComponent<Rigidbody>() && tag != "Battery" && tag != "Written" && tag != "Tape")
                        GetComponent<Rigidbody>().isKinematic = true;
                    else if (tag == "Battery" || tag == "Written" || tag == "Tape")
                        GetComponent<Rigidbody>().isKinematic = false;

                    mZCoord = GameObject.Find("Camera").GetComponent<Camera>().WorldToScreenPoint(gameObject.transform.position).z;
                    mOffset = gameObject.transform.position - GetMouseWorldPos();
                }
            }
        }
       


    }

    private void StartCoroutine(IEnumerable enumerable)
    {
        throw new NotImplementedException();
    }

    public Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;

        mousePoint.z = mZCoord;
        /*if (Is3D)
            mousePoint.y = Mathf.Clamp(mousePoint.y, 150, 5000);*/
       if (IsFixedInTI)
            mousePoint.y = 0;




        return GameObject.Find("Camera").GetComponent<Camera>().ScreenToWorldPoint(mousePoint);
    }
    public void OnMouseDrag()
    {
        if(GameObject.Find("Player").GetComponent<PlayerAxisScript>().CanClick && !GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().IsTalkingTutorial)
        {
            if(!isClickable)
            {
                OnClicked?.Invoke();
                return;
            }
            if (tag == "PanelImage")
            {
                if(transform.parent.name != "Content")
                    GetComponent<RectTransform>().localScale = new Vector3(6.5f, 6.5f, 4);
            }
            if (tag == "PanelImage" || tag == "Entry" || GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == _axisID)
            {
                if (IsDragable)
                {
                    if (tag == "Entry" && GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex == 26 && !GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying)
                    {
                        if (SheetImages[0].IsFilled)
                        {
                            GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex++;
                            StartCoroutine(GameObject.Find("TutorialManager").GetComponent<Tutorial>().LaunchNextDialogue(2));
                        }
                    }

                    /*if (tag == "Minimap" || tag == "Cam")
                        transform.parent.transform.position = GetMouseWorldPos() + mOffset;
                    else
                        transform.position = GetMouseWorldPos() + mOffset;*/

                    IsDragged = true;
                    GameObject.Find("Player").GetComponent<PlayerAxisScript>().IsDraging = true;
                    if (GetComponent<ZoomScript>())
                        GetComponent<ZoomScript>().enabled = true;

                    if (isTutoTI1)
                    {
                        GetComponent<Highlight>().StopHighlight();
                        if (!GameObject.Find("PanelBasket").GetComponent<Highlight>().Highlighted && GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex == 14)
                            GameObject.Find("PanelBasket").GetComponent<Highlight>().BeginHighlight();
                    }

                    if (Is3D && tag != "Cam" && tag != "Battery")
                    {
                        float planeY = -29.5f;
                        Transform draggingObject = transform;

                        Plane plane = new Plane(Vector3.up, Vector3.up * planeY); // ground plane

                        Ray ray = GameObject.Find("Camera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

                        float distance; // the distance from the ray origin to the ray intersection of the plane
                        if (plane.Raycast(ray, out distance))
                        {
                            if(tag != "Written")
                                draggingObject.GetComponent<Rigidbody>().velocity = (new Vector3(ray.GetPoint(Mathf.Clamp(distance, 3, 6)).x, -29.5f, ray.GetPoint(Mathf.Clamp(distance, 3, 4)).z) - draggingObject.transform.position) * 20; // distance along the ray
                            else
                                draggingObject.GetComponent<Rigidbody>().velocity = (new Vector3(ray.GetPoint(Mathf.Clamp(distance, 3.75f, 6)).x, -29.5f, ray.GetPoint(Mathf.Clamp(distance, 4.5f, 4)).z) - draggingObject.transform.position) * 20; // distance along the ray


                        }
                    }
                    else
                    {
                        if (tag == "Battery")
                        {
                            Rigidbody r = GetComponent<Rigidbody>();
                            r.velocity = (GetMouseWorldPos() + mOffset - transform.position) * 20;
                            transform.position = new Vector3(transform.position.x, transform.position.y, -74f);
                        }
                        //GetComponent<Rigidbody>().position = new Vector3(GetMouseWorldPos().x + mOffset.x, GetMouseWorldPos().y + mOffset.y, -74f); 
                        else
                        {
                            if (IsFixedInTI)
                                transform.position = new Vector3(GetMouseWorldPos().x + mOffset.x, transform.position.y, transform.position.z);
                            else
                                transform.position = GetMouseWorldPos() + mOffset;
                        }
                    }
                }
            }
        }
        
    }

    public void OnMouseUp()
    {
        if(GameObject.Find("Player").GetComponent<PlayerAxisScript>().CanClick && !GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().IsTalkingTutorial)
        {
            if (IsDragable)
            {
                if (GetComponent<Rigidbody>())
                    GetComponent<Rigidbody>().isKinematic = false;
            }
            IsDragged = false;
            GameObject.Find("Player").GetComponent<PlayerAxisScript>().IsDraging = false;

            if (tag == "Cam")
            {
                if (IsDragable)
                    Destroy(this.gameObject);
            }
            isFixable = false;
        }
    }

    private void OnMouseExit()
    {
        if(GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == _axisID && GameObject.Find("Player").GetComponent<PlayerAxisScript>().CanClick && !GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().IsTalkingTutorial)
        {
            if (_interactFeedBack)
                _interactFeedBack.enabled = false;
        }

    }
    private void OnMouseEnter()
    {
        if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == _axisID && GameObject.Find("Player").GetComponent<PlayerAxisScript>().CanClick && !GameObject.Find("IAVoiceManager").GetComponent<IAVoiceManager>().IsTalkingTutorial)
        {
            if (_interactFeedBack)
                _interactFeedBack.enabled = true;
        }
    }

    IEnumerator SyntheticDrag()
    {
        // Process the start of our drag, and then wait a frame.
        OnMouseDown();
        yield return null;

        // Keep updating our dragged position each frame
        // until the mouse button is released.
        while (Input.GetMouseButton(0))
        {
            OnMouseDrag();
            yield return null;
        }
        OnMouseUp();
        yield return null;
    }

    private void Update()
    {
        if(transform.position.y <= -30.5f && Is3D && tag != "Battery")
        {
            transform.position = GameObject.Find("SpittingPoint").transform.position;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            if(gameObject.CompareTag("Written"))
                GetComponent<Rigidbody>().AddForce((GameObject.Find("PosWrittenThrow").transform.position - transform.position) *50);
            if (gameObject.CompareTag("Tape"))
                GetComponent<Rigidbody>().AddForce((GameObject.Find("PosTapeThrow").transform.position - transform.position) *50);
        }
        /*if (IsFixedInTI && !IsDragged && !GetComponent<TIEntryScript>().IsTuto)
            transform.localPosition = new Vector3(GameObject.Find("TI").GetComponent<TutorialTI>().TutorialActivated ? Mathf.Clamp(transform.localPosition.x, -300, 300) : Mathf.Clamp(transform.localPosition.x, -210, 300), -7f, 0);
        if (IsFixedInTI && IsDragged && !GetComponent<TIEntryScript>().IsTuto)
            transform.localPosition = new Vector3(GameObject.Find("TI").GetComponent<TutorialTI>().TutorialActivated ? Mathf.Clamp(transform.localPosition.x, -300, 300) : Mathf.Clamp(transform.localPosition.x, -210, 300), -7f, 0);
        if (IsFixedInTI && !IsDragged && GetComponent<TIEntryScript>().IsTuto)
            transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -300, -210) , -7f, 0);
        if (IsFixedInTI && IsDragged && GetComponent<TIEntryScript>().IsTuto)
            transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -300, -210), -7f, 0);*/
        
        /*if (IsFixedInTI && !IsDragged && !GetComponent<TIEntryScript>().IsTuto)
            transform.localPosition =*/


    }

    IEnumerator EntryCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        IsDragable = true;
        
    }
    IEnumerator FixableCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        isFixable = true;
    }

    public void ActivateRaycast(string layer)
    {
        int LayerIgnoreRaycast = LayerMask.NameToLayer(layer);
        gameObject.layer = LayerIgnoreRaycast;
    }

    public void SetClickable(bool isClickable)
    {
        this.isClickable = isClickable;
    }
}
