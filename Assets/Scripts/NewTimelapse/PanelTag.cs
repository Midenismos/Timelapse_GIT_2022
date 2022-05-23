using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelTag : MonoBehaviour
{
    public string ImageTag = "null";
    public Material corruptedMaterial = null;
    private bool _isCorrupted;
    //[SerializeField] private Image _glitchEffect = null;
    [SerializeField] private GameObject deleteButton = null;

    public bool IsCorrupted
    {
        get
        { return _isCorrupted; }
        set
        {
            if (value != _isCorrupted)
                _isCorrupted = value;
            /*if (_isCorrupted == true)
                _glitchEffect.enabled = true;
            else
                _glitchEffect.enabled = false;*/
        }
    }

    public void Init(bool isCorrupted)
    {
        IsCorrupted = isCorrupted;
        Debug.Log(IsCorrupted);
        if(IsCorrupted)
        {
            GetComponent<Image>().material = corruptedMaterial;
            Debug.Log(GetComponent<Image>().material = corruptedMaterial);
        }
    }

    private void Update()
    {
        //if (_glitchEffect.GetComponent<RectTransform>().sizeDelta != new Vector2(GetComponent<RectTransform>().sizeDelta.x, GetComponent<RectTransform>().sizeDelta.y))
           // _glitchEffect.GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, GetComponent<RectTransform>().sizeDelta.y);
        
        if (GameObject.Find("TI").GetComponent<TutorialTI>().TutorialActivated == false && ImageTag == "written")
        {
            deleteButton.SetActive(false);
            GetComponent<BoxCollider>().enabled = false;
            if (GetComponent<DragObjects>().EntrySlot == null)
                Destroy(gameObject);

        }
    }

    public void DeletePanelImage()
    {
        if(GetComponent<DragObjects>().EntrySlot)
        {
            GetComponent<DragObjects>().EntrySlot.GetComponent<SheetImageScript>().IsFilled = false;
            GetComponent<DragObjects>().EntrySlot = null;
        }
        GameObject.Find("TI").GetComponent<AudioSource>().clip = Resources.Load("Sound/Snd_Investigation/Snd_Delete") as AudioClip;
        GameObject.Find("TI").GetComponent<AudioSource>().Play();
        transform.parent = null;
        Destroy(gameObject);
    }

}
