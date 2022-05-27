using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;


public class IAVoiceManager : MonoBehaviour
{
   /* public List<GameObject> Petales = new List<GameObject>();
    public string[] DialogueTexts;
    public AudioClip[] DialogueSounds;

    public Dialogue[] DialogueList = new Dialogue[]
{
        new Dialogue
        ( "Test",
         new string[]
            {
                "Début de la mission INV0071-ND12-TAS.",
                "Nous sommes le 7 septembre 2246",
                "Bonjour enquêtrice 854-186-420.",
                "Je suis la nouvelle intelligence artificielle missionnée pour les investigations." ,
                "Mon nom usuel est I A M I.",
                "Veuillez vous installer rapidement à votre poste pour commencer votre mission."
            },
         new AudioClip[]
            {
                Resources.Load("Sound/Snd_CameraClose/1.Contexte/1") as AudioClip,
                Resources.Load("Sound/Snd_CameraClose/1.Contexte/2") as AudioClip,
                Resources.Load("Sound/Snd_CameraClose/1.Contexte/3") as AudioClip,
                Resources.Load("Sound/Snd_CameraClose/1.Contexte/4") as AudioClip,
                Resources.Load("Sound/Snd_CameraClose/1.Contexte/5") as AudioClip,
                Resources.Load("Sound/Snd_CameraClose/1.Contexte/6") as AudioClip
            }
        )

};
    [SerializeField] int i = 0;

    private bool dialogueHappening = false;

    private AudioSource source;

    private TMP_Text txt = null;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        txt = GameObject.Find("IASubtitle").GetComponent<TMP_Text>();
    }

    public void LaunchDialogue(string dialogueName)
    {
        print(DialogueList);
        DialogueTexts = Array.Find(DialogueList, Dialogue => Dialogue.DialogueName == dialogueName).DialogueTexts;
        DialogueSounds = Array.Find(DialogueList, Dialogue => Dialogue.DialogueName == dialogueName).DialogueSounds;
        dialogueHappening = true;
        i = 0;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
            LaunchDialogue("Test");
        if (dialogueHappening == true)
        {
            if (!source.isPlaying)
            {
                if (source.clip != null)
                {
                    if (source.time >= source.clip.length - 0.5f)
                    {
                        i += 1;
                        if (i >= DialogueTexts.Length)
                        {
                            DialogueTexts = null;
                            DialogueSounds = null;
                            dialogueHappening = false;
                            txt.text = "";
                        }
                        if(i< DialogueTexts.Length)
                        {
                            source.clip = DialogueSounds[i];
                            source.Play();
                            txt.text = DialogueTexts[i];
                        }
                    }
                }
            }
        }
    }*/



}
