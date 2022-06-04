using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    public bool Highlighted = false;
    public bool isHightlighting = false;
    private Color baseColor;
    // Start is called before the first frame update
    void Start()
    {
        baseColor = GetComponent<MeshRenderer>().material.color;
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
            GetComponent<MeshRenderer>().material.color = baseColor;
    }

    IEnumerator StartHighlight()
    {
        while (Highlighted)
        {
            GetComponent<MeshRenderer>().material.color = new Color(255, 255, 0);
            yield return new WaitForSeconds(0.5f);
            GetComponent<MeshRenderer>().material.color = baseColor;
            yield return new WaitForSeconds(0.5f);

        }
    }

    public void BeginHighlight()
    {
        Highlighted = true;
    }

    public void StopHighlight()
    {
        Highlighted = false;
        isHightlighting = false;
    }
}
