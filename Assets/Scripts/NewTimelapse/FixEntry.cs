using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixEntry : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Entry" && other.gameObject.layer == 17 && other.GetComponent<DragObjects>().IsFixedInTI == false)
        {
            other.transform.SetParent(this.transform, false);
            other.GetComponent<RectTransform>().localScale = new Vector3(0.75f, 0.75f, 0.75f);
            other.GetComponent<DragObjects>().IsFixedInTI = true;
            other.GetComponent<DragObjects>().OnMouseUp();
            other.GetComponent<DragObjects>().OnMouseDown();
            if (GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex == 23)
            {
                if (GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying)
                    GameObject.Find("TutorialManager").GetComponent<Tutorial>().DialogueFinished();
                GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex++;
                StartCoroutine(GameObject.Find("TutorialManager").GetComponent<Tutorial>().LaunchNextDialogue(4));
            }
        }
    }
}
