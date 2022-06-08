using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IADialogue", menuName = "IAVoice/IADialogue")]
public class IADialogue : ScriptableObject
{
    [Header("Nom du dialogue")]
    public string DialogueName;
    [Header("Ins�rer le texte de chaque r�plique � mettre dans la bulle de dialogue dans l'ordre")]
    public string[] DialogueTexts;
    [Header("Ins�rer les sons de chaque r�plique du dialogue dans l'ordre")]
    public AudioClip[] DialogueSounds;

    public bool IsFolowedByAnotherDialogue;
    public bool notInTutorialDialogue;
}
