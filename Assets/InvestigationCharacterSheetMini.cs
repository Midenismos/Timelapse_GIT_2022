using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InvestigationCharacterSheetMini : MonoBehaviour
{
    [SerializeField] private InvestigationWidget _widget;

    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _profession;
    [SerializeField] private Image _portrait;
    [SerializeField] private Image[] _icons;

    [Header("Icons")]
    [SerializeField] private Sprite _murderedSprite = null;
    [SerializeField] private Sprite _crystalFriendlySprite = null;
    [SerializeField] private Sprite _notCrystalFriendlySprite = null;
    [SerializeField] private Sprite _crystalCorruptedSprite = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _name.text = string.Format("{0} {1}", _widget.data.name != "" && _widget.data.name != null ? _widget.data.name : "X", _widget.data.firstname != "" && _widget.data.firstname != null ? _widget.data.firstname : "X");
        _profession.text = _widget.data.job != "" && _widget.data.job != null ? _widget.data.job : "profession inconnue";
        _portrait.sprite = _widget.data.portrait;

        for (int i = 0; i <= _icons.Length - 1; i++)
        {
            switch (_widget.data.IconTypes[i])
            {
                case InvestigationIconType.NONE:
                    _icons[i].sprite = null;
                    _icons[i].gameObject.SetActive(false);
                    break;
                case InvestigationIconType.MURDERED:
                    _icons[i].sprite = _murderedSprite;
                    _icons[i].gameObject.SetActive(true);
                    break;
                case InvestigationIconType.CRYSTALFRIENDLY:
                    _icons[i].sprite = _crystalFriendlySprite;
                    _icons[i].gameObject.SetActive(true);
                    break;
                case InvestigationIconType.NOTCRYSTALFRIENDLY:
                    _icons[i].sprite = _notCrystalFriendlySprite;
                    _icons[i].gameObject.SetActive(true);
                    break;
                case InvestigationIconType.CRYSTALCORRUPTED:
                    _icons[i].sprite = _crystalCorruptedSprite;
                    _icons[i].gameObject.SetActive(true);
                    break;
            }
        }
    }
}
