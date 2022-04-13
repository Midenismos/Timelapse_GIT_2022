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
                    }
                    if (other.tag == "Written")
                    {
                        image = Instantiate(_panelImage, this.transform);
                        image.GetComponent<Image>().sprite = other.GetComponent<PanelImageData>().Image;
                        image.transform.SetParent(_panels[1].transform, false);
                        image.GetComponent<PanelTag>().ImageTag = "written";
                    }
                    else if (other.tag == "Tape")
                    {
                        image = Instantiate(_panelImageTape, this.transform);
                        image.transform.GetChild(0).GetComponent<TMP_Text>().text = other.GetComponent<PanelImageData>().TMText.text;
                        image.transform.SetParent(_panels[0].transform, false);
                    }
                    else if (other.tag == "Minimap")
                    {
                        image = Instantiate(_panelImageTape, this.transform);
                        image.GetComponent<Image>().sprite = other.GetComponent<PanelImageData>().Image;
                        image.transform.SetParent(_panels[3].transform, false);
                        image.GetComponent<PanelTag>().ImageTag = "map";
                    }
                    other.transform.SetParent(this.transform, false);
                    other.transform.position = GameObject.Find("start").transform.position;
                    other.GetComponent<DragObjects>().IsDragable = false;
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
        if (_isLerping)
        {
            _zoomCountdown = Mathf.Clamp(_zoomCountdown - Time.unscaledDeltaTime * _zoomSpeed, 0f, 1f);

            if (_zoomCountdown == 0)
            {
                _isLerping = false;
                scannedItem.GetComponent<DragObjects>().IsDragable = true;
                scannedItem.GetComponent<ZoomScript>().IsZoomable = true;
                scannedItem.GetComponent<Rigidbody>().isKinematic = false;
                scannedItem.GetComponent<Rigidbody>().AddForce((GameObject.Find("start").transform.forward+ GameObject.Find("start").transform.up) * 200);
                scannedItem.transform.SetParent(null, false);
                scannedItem.transform.position = GameObject.Find("start").transform.position;
                scannedItem = null;
            }

            if(scannedItem)
                scannedItem.transform.position = Vector3.Lerp(GameObject.Find("start").transform.position, GameObject.Find("interior").transform.position, _zoomLerp);
            _zoomLerp = 1f - _zoomCountdown;
        }
    }
}
