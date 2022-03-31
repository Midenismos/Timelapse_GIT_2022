using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelBasketScript : MonoBehaviour
{
    [SerializeField] private GameObject[] _panels = new GameObject[3];
    [SerializeField] private GameObject _panelImage = null;
    [SerializeField] private GameObject _panelImageTape = null;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PanelImageData>())
        {
            GameObject image;
            if (other.tag == "Cam")
            {
                image = Instantiate(_panelImage, this.transform);
                image.GetComponent<Image>().sprite = other.GetComponent<PanelImageData>().Image;
                image.transform.SetParent(_panels[2].transform, false);
            }
            if (other.tag == "Written")
            {
                image = Instantiate(_panelImage, this.transform);
                image.GetComponent<Image>().sprite = other.GetComponent<PanelImageData>().Image;
                image.transform.SetParent(_panels[1].transform, false);
            }
            else if (other.tag == "Tape")
            {
                image = Instantiate(_panelImageTape, this.transform);
                image.transform.GetChild(0).GetComponent<TMP_Text>().text = other.GetComponent<PanelImageData>().TMText.text;
                image.transform.SetParent(_panels[0].transform, false);

            }
        }
    }
}
