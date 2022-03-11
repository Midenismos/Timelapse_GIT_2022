using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoreScreenScript : MonoBehaviour
{
    [SerializeField] public GameObject[] LoreType = null;

    [SerializeField] private string[] BookTextLeftPage;
    [SerializeField] private string[] BookTextRightPage;
    [SerializeField] private string[] SheetTexts;
    [SerializeField] private int iPage = 0;
    private TimeManager _timeManager = null;

    private void Awake()
    {
        _timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();

        // Coupe l'interface si c'est la phase nébuleuse
        _timeManager.GetComponent<TimeManager>().ReactedToNebuleuse += delegate (bool isInNebuleuse)
        {
            if (isInNebuleuse == true)
            {
                if (LoreType[0].activeInHierarchy)
                {
                    CloseScreen(0);
                }
                else if (LoreType[1].activeInHierarchy)
                {
                    CloseScreen(1);
                }
            }
        };

        // Empêche le joueur de lire les docs écrits la nuit
        _timeManager.GetComponent<TimeManager>().ChangedToState += delegate (PhaseState phase)
        {
            if (phase == PhaseState.DARK)
            {
                if (LoreType[2].activeInHierarchy)
                {
                    CloseScreen(2);
                }
                else if (LoreType[3].activeInHierarchy)
                {
                    CloseScreen(3);
                }
            }
        };
    }
    // Update is called once per frame
    void Update()
    {
        //Tourne la Page
        if(LoreType[2].activeSelf)
        {
            if (Input.GetMouseButtonDown(1))
            {
                iPage = Mathf.Clamp(iPage + 1, 0, BookTextLeftPage.Length-1);
                LoreType[2].transform.Find("TextLeft").GetComponent<TextMeshProUGUI>().text = BookTextLeftPage[iPage];
                LoreType[2].transform.Find("TextRight").GetComponent<TextMeshProUGUI>().text = BookTextRightPage[iPage];
            }
            if (Input.GetMouseButtonDown(0))
            {
                iPage = Mathf.Clamp(iPage - 1, 0, BookTextLeftPage.Length-1);
                LoreType[2].transform.Find("TextLeft").GetComponent<TextMeshProUGUI>().text = BookTextLeftPage[iPage];
                LoreType[2].transform.Find("TextRight").GetComponent<TextMeshProUGUI>().text = BookTextRightPage[iPage];
            }
        }
        
        else if(LoreType[3].activeSelf)
        {
            if (Input.GetMouseButtonDown(1))
            {
                iPage = Mathf.Clamp(iPage + 1, 0, SheetTexts.Length - 1);
                LoreType[3].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = SheetTexts[iPage];
            }
            if (Input.GetMouseButtonDown(0))
            {
                iPage = Mathf.Clamp(iPage - 1, 0, SheetTexts.Length - 1);
                LoreType[3].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = SheetTexts[iPage];
            }
        }
    }

    // Pour les écrans et tableaux
    public void OpenScreen(int SelectedLoreType, string text, TMP_FontAsset font, int Size)
    {
        LoreType[SelectedLoreType].SetActive(true);
        LoreType[SelectedLoreType].GetComponentInChildren<TextMeshProUGUI>().font = font;
        LoreType[SelectedLoreType].GetComponentInChildren<TextMeshProUGUI>().fontSize = Size;
        LoreType[SelectedLoreType].GetComponentInChildren<TextMeshProUGUI>().text = text;
    }


    // Pour les livres avec plusieurs pages
    public void OpenPages(int SelectedLoreType, string[] textLeft, string[] textRight, TMP_FontAsset font, int Size)
    {
        iPage = 0;
        LoreType[SelectedLoreType].SetActive(true);
        LoreType[SelectedLoreType].transform.Find("TextLeft").GetComponent< TextMeshProUGUI>().font = font;
        LoreType[SelectedLoreType].transform.Find("TextRight").GetComponent<TextMeshProUGUI>().font = font;
        LoreType[SelectedLoreType].transform.Find("TextLeft").GetComponent<TextMeshProUGUI>().fontSize = Size;
        LoreType[SelectedLoreType].transform.Find("TextRight").GetComponent<TextMeshProUGUI>().fontSize = Size;

        BookTextLeftPage = textLeft;
        BookTextRightPage = textRight;
        LoreType[SelectedLoreType].transform.Find("TextLeft").GetComponent<TextMeshProUGUI>().text = BookTextLeftPage[0];
        LoreType[SelectedLoreType].transform.Find("TextRight").GetComponent<TextMeshProUGUI>().text = BookTextRightPage[0];
    }

    // Pour les feuilles recto verso
    public void OpenSheet(int SelectedLoreType, string[] Texts, TMP_FontAsset font, int Size)
    {
        iPage = 0;
        LoreType[SelectedLoreType].SetActive(true);
        LoreType[SelectedLoreType].transform.Find("Text").GetComponent<TextMeshProUGUI>().font = font;
        LoreType[SelectedLoreType].transform.Find("Text").GetComponent<TextMeshProUGUI>().fontSize = Size;

        SheetTexts = Texts;
        LoreType[SelectedLoreType].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = SheetTexts[0];    }

    public void CloseScreen(int SelectedLoreType)
    {
        LoreType[SelectedLoreType].SetActive(false);
        BookTextLeftPage = null;
        BookTextRightPage = null;
    }
}
