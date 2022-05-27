using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;


public class IAVoiceManager : MonoBehaviour
{
    public List<GameObject> Petales = new List<GameObject>();
    public string[] DialogueTexts;
    public AudioClip[] DialogueSounds;

    public Dialogue[] DialogueList;

    [SerializeField] int i = 0;

    private bool dialogueHappening = false;

    private AudioSource source;

    private TMP_Text txt = null;
    private bool _inCooldown = false;

    private void Awake()
    {
        DialogueList = new Dialogue[]
        {
        new Dialogue
        ("Test",
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
                Resources.Load("Sound/IA/1.Contexte/1") as AudioClip,
                Resources.Load("Sound/IA/1.Contexte/2") as AudioClip,
                Resources.Load("Sound/IA/1.Contexte/3") as AudioClip,
                Resources.Load("Sound/IA/1.Contexte/4") as AudioClip,
                Resources.Load("Sound/IA/1.Contexte/5") as AudioClip,
                Resources.Load("Sound/IA/1.Contexte/6") as AudioClip
            }
        ),
                new Dialogue
        ("Test",
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
                Resources.Load("Sound/IA/1.Contexte/1") as AudioClip,
                Resources.Load("Sound/IA/1.Contexte/2") as AudioClip,
                Resources.Load("Sound/IA/1.Contexte/3") as AudioClip,
                Resources.Load("Sound/IA/1.Contexte/4") as AudioClip,
                Resources.Load("Sound/IA/1.Contexte/5") as AudioClip,
                Resources.Load("Sound/IA/1.Contexte/6") as AudioClip
            }
        ),
        };
        source = GetComponent<AudioSource>();
        txt = GameObject.Find("IASubtitle").GetComponent<TMP_Text>();
    }

    public void LaunchDialogue(string dialogueName)
    {
        DialogueTexts = Array.Find(DialogueList, Dialogue => Dialogue.DialogueName == dialogueName).DialogueTexts;
        DialogueSounds = Array.Find(DialogueList, Dialogue => Dialogue.DialogueName == dialogueName).DialogueSounds;
        dialogueHappening = true;
        i = 0;
        Play();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
            LaunchDialogue("Test");
        if(dialogueHappening)
        {
            if (!source.isPlaying && !_inCooldown)
                StartCoroutine(Cooldown());
        }
    }

    public void Play()
    {
        print("hum");
        source.clip = DialogueSounds[i];
        source.Play();
        txt.text = DialogueTexts[i];
    }
    IEnumerator Cooldown()
    {
        _inCooldown = true;
        yield return new WaitForSeconds(1);
        print("hum");
        i += 1;
        if (i >= DialogueTexts.Length)
        {
            DialogueTexts = null;
            DialogueSounds = null;
            dialogueHappening = false;
            txt.text = "";
            print("hum");
        }
        else
        {
            Play();
        }
        _inCooldown = false;
    }


}
