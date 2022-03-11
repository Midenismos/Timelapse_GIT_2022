using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum InvestigationIconType
{
    NONE,
    MURDERED,
    CRYSTALFRIENDLY,
    NOTCRYSTALFRIENDLY,
    CRYSTALCORRUPTED,
}

public class InvestigationWidgetIconButton : MonoBehaviour
{
    public InvestigationIconType IconType;

    [SerializeField] private GameObject _buttons;
    [SerializeField] private InvestigationWidget _widget;
    [SerializeField] private int _space;

    [Header("Icons")]
    [SerializeField] private Sprite _murderedSprite = null;
    [SerializeField] private Sprite _crystalFriendlySprite = null;
    [SerializeField] private Sprite _notCrystalFriendlySprite = null;
    [SerializeField] private Sprite _crystalCorruptedSprite = null;

    [SerializeField] public Sprite MurderedSprite { get => _murderedSprite; }
    [SerializeField] public Sprite CrystalFriendlySprite { get => _crystalFriendlySprite; }
    [SerializeField] public Sprite NotCrystalFriendlySprite { get => _notCrystalFriendlySprite; }
    [SerializeField] public Sprite CrystalCorruptedSprite { get => _crystalCorruptedSprite; }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeIcon(int type)
    {
        IconType = (InvestigationIconType)type;

        switch (type)
        {
            case 0:
                GetComponent<Image>().sprite = null;
                break;
            case 1:
                GetComponent<Image>().sprite = _murderedSprite;
                break;
            case 2:
                GetComponent<Image>().sprite = _crystalFriendlySprite;
                break;
            case 3:
                GetComponent<Image>().sprite = _notCrystalFriendlySprite;
                break;
            case 4:
                GetComponent<Image>().sprite = _crystalCorruptedSprite;
                break;
        }
        _widget.IconChanged((InvestigationIconType)type, _space);
    }

    public void ActivateButtons()
    {
        if(_buttons.activeSelf)
            _buttons.SetActive(false);
        else
            _buttons.SetActive(true);
    }
}
