using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapeListener : MonoBehaviour
{
    [SerializeField]private GameObject tapeReceiver = null;
    [SerializeField]private Slider _slider = null;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tape")
        {
            if(GetComponent<AudioSource>().clip == null)
            {
                other.GetComponent<DragObjects>().IsDragable = false;
                other.GetComponent<Rigidbody>().isKinematic = true;
                other.transform.position = tapeReceiver.transform.position;
                other.transform.rotation = tapeReceiver.transform.rotation;
                GetComponent<AudioSource>().clip = other.GetComponent<TapeScript>().sound;
                _slider.value = 1;
                GetComponent<AudioSource>().Play();
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<AudioSource>().pitch = _slider.value;
    }
}
