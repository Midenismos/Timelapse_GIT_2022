﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SheetImageScript : MonoBehaviour
{
    [SerializeField] private string _imageTag = null;
    public bool IsFilled = false;
    private AudioClip _entryFilledFeedback = null;

    private void Awake()
    {
        _entryFilledFeedback = Resources.Load("Sound/Snd_Table/Snd_Table_Link") as AudioClip;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PanelImage")
        {
            if (transform.childCount == 0)
            {
                if( _imageTag == other.GetComponent<PanelTag>().ImageTag && other.GetComponent<DragObjects>().EntrySlot == null)
                {
                    other.transform.SetParent(this.transform, false);
                    other.GetComponent<RectTransform>().localScale = new Vector3(4, 4, 4);
                    other.transform.position = this.transform.position;
                    other.GetComponent<DragObjects>().IsDragable = false;
                    StartCoroutine(CheckIfChildren());
                    GameObject.Find("TI").GetComponent<AudioSource>().clip = _entryFilledFeedback;
                    GameObject.Find("TI").GetComponent<AudioSource>().Play();
                }
            }
        }
    }

    IEnumerator CheckIfChildren()
    {
        yield return new WaitForSeconds(0.2f);
        if (transform.childCount == 1)
        {
            transform.GetChild(0).GetComponent<DragObjects>().EntrySlot = gameObject;
            IsFilled = true;
        }

    }
}
