using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocReposScript : MonoBehaviour
{
    public float _zoomLerp = 0;
    public float _zoomCountdown = 1;
    public float _zoomSpeed = 1;
    public bool _isLerping = false;
    public bool HasZoomed = false;
    private GameObject Item = null;


    public bool hasDone = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DragObjects>() && hasDone)
        {

            if (other.GetComponent<DragObjects>().IsDragged)
            {

                if (other.GetComponent<ZoomScript>()._isFixedButDragable && !_isLerping)
                {
                    other.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 90));
                    other.transform.position = GameObject.Find("startRepos").transform.position;
                    print(GameObject.Find("startRepos").transform.position);
                    other.GetComponent<DragObjects>().IsDragable = false;
                    if (other.GetComponent<ZoomScript>())
                        other.GetComponent<ZoomScript>().IsZoomable = false;
                    GetComponent<AudioSource>().Play();
                    _zoomCountdown = 1;
                    _zoomLerp = 0;
                    _isLerping = true;
                    Item = other.gameObject;
                    GetComponent<Animation>().Play();


                }
            }
        }

    }

    private void Update()
    {

        if (_isLerping)
        {
            hasDone = false;
            _zoomCountdown = Mathf.Clamp(_zoomCountdown - Time.unscaledDeltaTime * _zoomSpeed, 0f, 1f);
            if (_zoomCountdown == 0)
            {
                _isLerping = false;
                Item.GetComponent<DragObjects>().IsDragable = true;
                Item.GetComponent<ZoomScript>().IsZoomable = true;
                Item.GetComponent<Rigidbody>().isKinematic = false;
                Item.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                Item.transform.position = GameObject.Find("startRepos").transform.position;
                if (Item.gameObject.tag == "Written")
                    Item.GetComponent<Rigidbody>().AddForce((Item.GetComponent<ZoomScript>()._fixedPosition - GameObject.Find("startRepos").transform.position) * 200);
                StartCoroutine(CooldownFix());
            }

            if (Item)
                Item.transform.position = Vector3.Lerp(GameObject.Find("startRepos").transform.position, GameObject.Find("interiorRepos").transform.position, _zoomLerp);
            _zoomLerp = 1f - _zoomCountdown;
        }
    }


    IEnumerator CooldownFix()
    {
        yield return new WaitForSeconds(0.3f);
        Item.GetComponent<ZoomScript>().PutBackFixedWrittenDoc();
        Item.GetComponent<DragObjects>().IsDragable = false;
        Item.GetComponent<Rigidbody>().isKinematic = true;
        Item = null;
        hasDone = true;

    }
}
