using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highlight : MonoBehaviour
{
    public bool Highlighted = false;
    public bool isHightlighting = false;
    private Color baseColor;
    private Color baseEmissiveColor;

    public Highlight[] HighlightedChildren;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<MeshRenderer>())
        {
            baseColor = GetComponent<MeshRenderer>().material.color;
            baseEmissiveColor = GetComponent<MeshRenderer>().material.GetColor("_EmissionColor");
        }
        if(GetComponent<Image>())
        {
            baseColor = GetComponent<Image>().color;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Highlighted && !isHightlighting)
        {
            StartCoroutine(StartHighlight());
            isHightlighting = true;
        }
        else if (!Highlighted)
        {
            if(GetComponent<MeshRenderer>())
            {
                GetComponent<MeshRenderer>().material.color = baseColor;
                GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", baseEmissiveColor);
            }
            else if(GetComponent<Image>())
            {
                GetComponent<Image>().color = baseColor;
            }

        }

    }

    IEnumerator StartHighlight()
    {
        while (Highlighted)
        {
            if (GetComponent<MeshRenderer>())
            {
                GetComponent<MeshRenderer>().material.color = Color.yellow;
                if (GetComponentInChildren<Image>())
                {
                    Image[] images = GetComponentsInChildren<Image>();
                    foreach (Image image in images)
                        image.color = Color.yellow;
                }
                GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.yellow);
            }
            else if (GetComponent<Image>())
            {
                GetComponent<Image>().color = Color.yellow;
            }

                yield return new WaitForSeconds(0.5f);
            if (GetComponent<MeshRenderer>())
            {
                GetComponent<MeshRenderer>().material.color = baseColor;
                if (GetComponentInChildren<Image>())
                {
                    Image[] images = GetComponentsInChildren<Image>();
                    foreach (Image image in images)
                        image.color = Color.white;
                }
                GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", baseEmissiveColor);
            }
            else if (GetComponent<Image>())
            {
                GetComponent<Image>().color = baseColor;
            }
            
            yield return new WaitForSeconds(0.5f);

        }
    }

    public void BeginHighlight()
    {
        Highlighted = true;
    }

    public void BeginHighlightChildren()
    {
        HighlightedChildren = GetComponentsInChildren<Highlight>();
        foreach (Highlight Object in HighlightedChildren)
        {
            Object.BeginHighlight();
        }
    }

    public void StopHighlight()
    {
        Highlighted = false;
        isHightlighting = false;
    }

    public void StopHighlightChildren()
    {
        foreach (Highlight Object in HighlightedChildren)
        {
            Object.StopHighlight();
        }
        HighlightedChildren = null;
    }
}
