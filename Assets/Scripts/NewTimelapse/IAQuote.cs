using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public struct Dialogue
{

    [Header("Nom du dialogue")]
    public string DialogueName;
    [Header("Ins�rer le texte de chaque r�plique � mettre dans la bulle de dialogue dans l'ordre")]
    public string[] DialogueTexts;
    [Header("Ins�rer les sons de chaque r�plique du dialogue dans l'ordre")]
    public AudioClip[] DialogueSounds;
    public Dialogue(string DialogueName, string[] DialogueTexts, AudioClip[] DialogueSounds)
    {
        this.DialogueName = DialogueName;
        this.DialogueTexts = DialogueTexts;
        this.DialogueSounds = DialogueSounds;
    }

}