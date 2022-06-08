using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IADialogue", menuName = "IAVoice/IADialogue")]
public class IADialogue : ScriptableObject
{
    [Header("Nom du dialogue")]
    public string DialogueName;
    [Header("Insérer le texte de chaque réplique à mettre dans la bulle de dialogue dans l'ordre")]
    public string[] DialogueTexts;
    [Header("Insérer les sons de chaque réplique du dialogue dans l'ordre")]
    public AudioClip[] DialogueSounds;

    public bool IsFolowedByAnotherDialogue;
    public bool notInTutorialDialogue;
}
