using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelTag : MonoBehaviour
{
    public string ImageTag = "null";
    private bool _isCorrupted;
    [SerializeField] private Image _glitchEffect = null;

    public bool IsCorrupted
    {
        get
        { return _isCorrupted; }
        set
        {
            if (value != _isCorrupted)
                _isCorrupted = value;
            if (_isCorrupted == true)
                _glitchEffect.enabled = true;
            else
                _glitchEffect.enabled = false;
        }
    }

    private void Update()
    {
        if (_glitchEffect.GetComponent<RectTransform>().sizeDelta != new Vector2(GetComponent<RectTransform>().sizeDelta.x, GetComponent<RectTransform>().sizeDelta.y))
            _glitchEffect.GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, GetComponent<RectTransform>().sizeDelta.y);

    }

    public void DeletePanelImage()
    {
        Destroy(gameObject);
    }

}
