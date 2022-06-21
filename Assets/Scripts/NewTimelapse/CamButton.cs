using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CamButton : MonoBehaviour
{
    public UnityEvent onClickedCam;
    public UnityEvent onClickedAudio;
    public UnityEvent onClickedVault;
    [SerializeField] private Material _activatedMat = null;
    private Material _deactivatedMat = null;
    [SerializeField] private MeshRenderer _interactFeedBack;

    private void Awake()
    {
        _deactivatedMat = GetComponent<MeshRenderer>().material;
        _interactFeedBack = transform.GetChild(1).gameObject.GetComponent<MeshRenderer>();
    }
    private void OnMouseOver()
    {
        if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 5)
        {
            _interactFeedBack.enabled = true;
            if (Input.GetMouseButtonDown(0))
            {
                onClickedCam?.Invoke();
                StartCoroutine(ButtonFeedback());
                if(GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex == 5)
                {
                    if (GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying)
                        GameObject.Find("TutorialManager").GetComponent<Tutorial>().DialogueFinished();
                    GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex++;
                    StartCoroutine(GameObject.Find("TutorialManager").GetComponent<Tutorial>().LaunchNextDialogue(2));
                }
                GetComponent<Highlight>().StopHighlightChildren();

            }
        }

        if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 1)
        {
            _interactFeedBack.enabled = true;
            if (Input.GetMouseButtonDown(0))
            {
                if (GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex == 8)
                {
                    if (GameObject.Find("IAVoiceManager").GetComponent<AudioSource>().isPlaying)
                        GameObject.Find("TutorialManager").GetComponent<Tutorial>().DialogueFinished();
                    GameObject.Find("TutorialManager").GetComponent<Tutorial>().dialogueIndex++;
                    StartCoroutine(GameObject.Find("TutorialManager").GetComponent<Tutorial>().LaunchNextDialogue(2));
                }
                onClickedAudio?.Invoke();
                StartCoroutine(ButtonFeedback());
                GetComponent<Highlight>().StopHighlightChildren();

            }
        }
        if (GameObject.Find("Player").GetComponent<PlayerAxisScript>().IDCurrentAxis == 4)
        {
            _interactFeedBack.enabled = true;
            if (Input.GetMouseButtonDown(0))
            {
                onClickedVault?.Invoke();
                StartCoroutine(ButtonFeedback());
                GetComponent<Highlight>().StopHighlightChildren();

            }
        }



    }
    private void OnMouseExit()
    {
        _interactFeedBack.enabled = false;
    }


    IEnumerator ButtonFeedback()
    {
        GetComponent<MeshRenderer>().material = _activatedMat;
        yield return new WaitForSeconds(0.5f);
        GetComponent<MeshRenderer>().material = _deactivatedMat;
        yield return null;
    }
}
