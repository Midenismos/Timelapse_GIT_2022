using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockTimeButton : MonoBehaviour
{
    [SerializeField] private Color activatedColor = Color.red;
    [SerializeField] private Color deactivatedColor = Color.white;

    [SerializeField] private float activatedScale = 1.7f;
    [SerializeField] private float deactivatedScale = 1;

    [Header("References")]
    [SerializeField] private Image image = null;

    public void Activate()
    {
        image.color = activatedColor;
        image.transform.localScale = Vector3.one * activatedScale;
    }

    public void Deactivate()
    {
        image.color = deactivatedColor;
        image.transform.localScale = Vector3.one * deactivatedScale;
    }

    private void OnValidate()
    {
        image.color = deactivatedColor;
    }
}
