using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelBasketScript : MonoBehaviour
{
    [SerializeField] private GameObject[] _panels = new GameObject[4];
    [SerializeField] private GameObject _panelImage = null;
    [SerializeField] private GameObject _panelImageTape = null;
    public float _zoomLerp = 0;
    public float _zoomCountdown = 1;
    public float _zoomSpeed = 1;
    public bool _isLerping = false;
    public bool HasZoomed = false;
    private GameObject scannedItem = null;
    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _scanningColor;
    [SerializeField] private Color _glitchedColor;
    [SerializeField] private Color _glitchedScanningColor;
    private Material _mat = null;
    [SerializeField] private TIBellScript[] Bells;

    private Color _colorA;
    private Color _colorB;

    private void Awake()
    {
        _mat = GetComponent<MeshRenderer>().material;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<DragObjects>())
        {
            if (other.GetComponent<DragObjects>().IsDragged)
            {
                if (other.GetComponent<PanelImageData>() && !_isLerping)
                {
                    GameObject image;
                    if (other.tag == "Cam")
                    {
                        image = Instantiate(_panelImage, this.transform);
                        image.GetComponent<Image>().sprite = other.GetComponent<PanelImageData>().Image;
                        image.transform.SetParent(_panels[2].transform, false);
                        image.GetComponent<PanelTag>().ImageTag = "cam";
                        image.GetComponent<PanelTag>().IsCorrupted = GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>().Energy <= 0 ? true : false;

                        Bells[1].NewPanelImageNumber += 1;
                    }
                    if (other.tag == "Written")
                    {
                        image = Instantiate(_panelImage, this.transform);
                        image.GetComponent<Image>().sprite = other.GetComponent<PanelImageData>().Image;
                        image.transform.SetParent(_panels[1].transform, false);
                        image.GetComponent<PanelTag>().ImageTag = "written";
                        image.GetComponent<PanelTag>().Init(GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>().Energy <= 0 ? true : false);
                        Bells[0].NewPanelImageNumber += 1;
                    }
                    else if (other.tag == "Tape")
                    {
                        image = Instantiate(_panelImageTape, this.transform);
                        image.transform.GetChild(0).GetComponent<TMP_Text>().text = other.GetComponent<PanelImageData>().TMText.text;
                        image.transform.SetParent(_panels[0].transform, false);
                        image.GetComponent<PanelTag>().IsCorrupted = GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>().Energy <= 0 ? true : false;
                        Bells[2].NewPanelImageNumber += 1;
                    }
                    /*else if (other.tag == "Minimap")
                    {
                        image = Instantiate(_panelImageTape, this.transform);
                        image.GetComponent<Image>().sprite = other.GetComponent<PanelImageData>().Image;
                        image.transform.SetParent(_panels[3].transform, false);
                        image.GetComponent<PanelTag>().ImageTag = "map";
                        image.GetComponent<PanelTag>().IsCorrupted = GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>().Energy <= 0 ? true : false;
                    }*/
                  //  _colorA = _mat.GetColor("_EmissionColor");
                    if (GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>().Energy <= 0)
                        _colorB = _glitchedScanningColor;
                    else
                        _colorB = _scanningColor;


                    //other.transform.SetParent(this.transform, false);
                    other.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 90));
                    other.transform.position = GameObject.Find("start").transform.position;
                    other.GetComponent<DragObjects>().IsDragable = false;
                    if(other.GetComponent<ZoomScript>())
                        other.GetComponent<ZoomScript>().IsZoomable = false;
                    GetComponent<AudioSource>().Play();
                    _zoomCountdown = 1;
                    _zoomLerp = 0;
                    _isLerping = true;
                    scannedItem = other.gameObject;
                    GetComponent<Animation>().Play();
                }
            }
        }
       
    }

    private void Update()
    {
        if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 5)
        {
           GetComponent<BoxCollider>().size = new Vector3(0.05f, 0.4f, 0.4f);
           GetComponent<BoxCollider>().center = new Vector3(0, 0.07f, 0.04f);

        }
        else
        {
            GetComponent<BoxCollider>().center = new Vector3(0, 0.07f, -0.025f);
            GetComponent<BoxCollider>().size = new Vector3(0.05f, 0.4f, 0.1f);
        }


        if (_isLerping)
        {
            _zoomCountdown = Mathf.Clamp(_zoomCountdown - Time.unscaledDeltaTime * _zoomSpeed, 0f, 1f);
            if (scannedItem.gameObject.tag == "Cam")
                scannedItem.GetComponent<DragObjects>().enabled = false;
            if (_zoomCountdown == 0)
            {
                _isLerping = false;
                // _mat.SetColor("_EmissionColor", _colorA);
                if (scannedItem.gameObject.tag != "Cam")
                {
                    scannedItem.GetComponent<DragObjects>().IsDragable = true;
                    scannedItem.GetComponent<ZoomScript>().IsZoomable = true;
                    scannedItem.GetComponent<Rigidbody>().isKinematic = false;
                    scannedItem.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    //scannedItem.transform.SetParent(null, false);
                    scannedItem.transform.position = GameObject.Find("start").transform.position;
                    if (scannedItem.gameObject.tag == "Written")
                        scannedItem.GetComponent<Rigidbody>().AddForce((GameObject.Find("PosWrittenThrow").transform.position - GameObject.Find("start").transform.position) * 75);
                    else if (scannedItem.gameObject.tag == "Tape")
                        scannedItem.GetComponent<Rigidbody>().AddForce((GameObject.Find("PosTapeThrow").transform.position - GameObject.Find("start").transform.position) * 25);
                    scannedItem = null;
                }
                else
                {
                    Destroy(scannedItem);
                }

            }

            if (scannedItem)
                scannedItem.transform.position = Vector3.Lerp(GameObject.Find("start").transform.position, GameObject.Find("interior").transform.position, _zoomLerp);
            // _mat.SetColor("_EmissionColor", Color.Lerp(_colorA, _colorB, _zoomLerp));
            _zoomLerp = 1f - _zoomCountdown;
        }
        else
        {
            //if (GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>().Energy <= 0)
            //    _mat.SetColor("_EmissionColor", _glitchedColor);
            // else
            //    _mat.SetColor("_EmissionColor", _normalColor);
        }


    }

}
