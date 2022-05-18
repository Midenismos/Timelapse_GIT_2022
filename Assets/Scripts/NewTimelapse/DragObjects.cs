using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragObjects : MonoBehaviour
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
    }
    public void StartDrag() { StartCoroutine("SyntheticDrag"); }
    private void Start()
    {
        if (Input.GetMouseButtonDown(0) && tag == "Cam")
        {
            StartCoroutine(SyntheticDrag());
        }
    }
    public void OnMouseDown()
    {
        if (tag == "Entry")
        {
            if (EntryDispenser != null)
            {
                EntryDispenser.GetComponent<DispenserManager>().CurrentDrag = null;
                EntryDispenser = null;
                transform.SetParent(GameObject.Find("TI").transform, true);
            }
        }
        if (tag == "PanelImage" || tag == "Entry" || GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == _axisID)
        {
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
                else if (tag == "PanelImage" )
                {
                    if(EntrySlot != null)
                    {
                        EntrySlot.GetComponent<SheetImageScript>().IsFilled = false;
                        EntrySlot = null;
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
        if (tag == "PanelImage")
        {
            GetComponent<RectTransform>().localScale = new Vector3(4f, 4f, 4);
        }
        if (tag == "PanelImage" || tag == "Entry" || GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == _axisID)
        {
            if (IsDragable)
            {
                /*if (tag == "Minimap" || tag == "Cam")
                    transform.parent.transform.position = GetMouseWorldPos() + mOffset;
                else
                    transform.position = GetMouseWorldPos() + mOffset;*/

                IsDragged = true;
                GameObject.Find("Player").GetComponent<PlayerAxisScript>().IsDraging = true;
                if (GetComponent<ZoomScript>())
                    GetComponent<ZoomScript>().enabled = true;

                if (Is3D && tag != "Cam" && tag != "Battery")
                {
                    float planeY = -29.5f;
                    Transform draggingObject = transform;

                    Plane plane = new Plane(Vector3.up, Vector3.up * planeY); // ground plane

                    Ray ray = GameObject.Find("Camera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

                    float distance; // the distance from the ray origin to the ray intersection of the plane
                    if (plane.Raycast(ray, out distance))
                        draggingObject.GetComponent<Rigidbody>().velocity = (new Vector3(ray.GetPoint(Mathf.Clamp(distance, 3, 6)).x, -29.5f, ray.GetPoint(Mathf.Clamp(distance, 3, 4)).z) - draggingObject.transform.position) * 20; // distance along the ray
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

    public void OnMouseUp()
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
            if(IsDragable)
                Destroy(this.gameObject);
        }
    }

    private void OnMouseExit()
    {
        if(GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == _axisID)
        {
            if (_interactFeedBack)
                _interactFeedBack.enabled = false;
        }

    }
    private void OnMouseEnter()
    {
        if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == _axisID)
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
        if (IsFixedInTI && !IsDragged && !GetComponent<TIEntryScript>().IsTuto)
            transform.localPosition = new Vector3(GameObject.Find("TI").GetComponent<TutorialTI>().TutorialActivated ? Mathf.Clamp(transform.localPosition.x, -300, 300) : Mathf.Clamp(transform.localPosition.x, -200, 300), -7f, 0);
        if (IsFixedInTI && IsDragged && !GetComponent<TIEntryScript>().IsTuto)
            transform.localPosition = new Vector3(GameObject.Find("TI").GetComponent<TutorialTI>().TutorialActivated ? Mathf.Clamp(transform.localPosition.x, -300, 300) : Mathf.Clamp(transform.localPosition.x, -200, 300), -7f, 0);
        if (IsFixedInTI && !IsDragged && GetComponent<TIEntryScript>().IsTuto)
            transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -300, -200) , -7f, 0);
        if (IsFixedInTI && IsDragged && GetComponent<TIEntryScript>().IsTuto)
            transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -300, -200), -7f, 0);



    }

    IEnumerator EntryCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        IsDragable = true;
        
    }
}
