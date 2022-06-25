using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SheetImageScript : MonoBehaviour
{
    [SerializeField] private string _imageTag = null;
    public bool IsFilled = false;
    private AudioClip _entryFilledFeedback = null;
    public string ID;
    public bool isGlitched;

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
                    other.GetComponent<RectTransform>().localScale = new Vector3(6f, 6f, 6);
                    other.transform.position = this.transform.position;
                    other.GetComponent<DragObjects>().IsDragable = false;
                    StartCoroutine(CheckIfChildren());
                    GameObject.Find("TI").GetComponent<AudioSource>().clip = _entryFilledFeedback;
                    GameObject.Find("TI").GetComponent<AudioSource>().Play();
                    if (other.GetComponent<Image>().sprite.name == "Journal de bord 1" && GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex == 25)
                    {
                        if (GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying)
                            GameObject.Find("TutorialManager").GetComponent<Tutorial>().DialogueFinished();
                        GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex++;
                        StartCoroutine(GameObject.Find("TutorialManager").GetComponent<Tutorial>().LaunchNextDialogue(0));
                    }
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
            ID = transform.GetChild(0).GetComponent<PanelTag>().ID;
            isGlitched = transform.GetChild(0).GetComponent<PanelTag>().IsCorrupted;
            GameObject.Find("TI").GetComponent<TIPanelImageData>().PanelImageList.Remove(transform.GetChild(0).GetComponent<PanelTag>());

        }

    }
}
