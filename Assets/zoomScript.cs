using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class zoomScript : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform = null;
    private float x = 1;
    private float y = 1;


    // Update is called once per frame
    void Update()
    {
         x = Mathf.Clamp(x + Input.mouseScrollDelta.y * 0.1f, 0.2f, 2);
         y = Mathf.Clamp(y + Input.mouseScrollDelta.y * 0.1f, 0.2f, 2);
        rectTransform.localScale = new Vector2(x, y);
    }
}
