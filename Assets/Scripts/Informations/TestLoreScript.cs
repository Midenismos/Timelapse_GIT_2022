using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class TestLoreScript : MonoBehaviour, IInteractable
{

    public enum Type
    {
        SCREEN = 0,
        BOARD = 1,
        BOOK = 2,
        WHITE = 3,
    }
    private AudioSource source = null;
    private SoundManager _sndManager = null;
    private TimeManager _timeManager = null;

    public Type LoreType = Type.SCREEN;
    [SerializeField] private PaperPages BPage = null;

    private LoreScreenScript LoreScreen;
    [HideInInspector] public string LoreTextNormal = null;

    [SerializeField] private TMP_FontAsset Font;
    [SerializeField] private int Size = 0;

    private void Awake()
    {
        _sndManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        LoreScreen = GameObject.Find("HUD").GetComponent<LoreScreenScript>();
        _timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        source = this.GetComponent<AudioSource>();
    }
    public void PlayerHoverStart()
    {

    }

    public void PlayerHoverEnd()
    {
        LoreScreen.CloseScreen((int)LoreType);
    }

    public void Interact(GameObject pickup, PlayerController player)
    {
        // Lance l'audio et si une autre info est joué, la stoppe
        if (LoreType == Type.SCREEN || LoreType == Type.BOARD)
        {
            if(_timeManager.IsInNebuleuse == false)
            {
                _sndManager.CheckLoreSound();
                source.Play();
                LoreScreen.OpenScreen((int)LoreType, LoreTextNormal, Font, Size);
            }

        }
        if(_timeManager.Phase == PhaseState.LIGHT)
        {
            if (LoreType == Type.BOOK)
                LoreScreen.OpenPages((int)LoreType, BPage.BookTextLeftPage, BPage.BookTextRightPage, Font, Size);
            else if (LoreType == Type.WHITE)
                LoreScreen.OpenSheet((int)LoreType, BPage.TextSheet, Font, Size);
        }

    }

    public void InteractHolding(GameObject pickup, PlayerController player)
    {

    }

    public void StopInteractHolding(GameObject pickup, PlayerController player)
    {

    }
}



