using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvestigationLink : MonoBehaviour
{
    [SerializeField] private Image image = null;

    [SerializeField] private Transform widgetA = null;
    [SerializeField] private Transform widgetB = null;
    [SerializeField] private InvestigationWidgetAncre widgetAAncre = null;
    [SerializeField] private InvestigationWidgetAncre widgetBAncre = null;
    [SerializeField] private bool initialized = false;
    [SerializeField] private Sprite dot = null;


    public Transform WidgetA { get => widgetA; set => widgetA = value; }
    public Transform WidgetB { get => widgetB; set => widgetB = value; }
    public InvestigationWidgetAncre WidgetAAncre { get => widgetAAncre; set => widgetAAncre = value; }
    public InvestigationWidgetAncre WidgetBAncre { get => widgetBAncre; set => widgetBAncre = value; }


    // Update is called once per frame
    void Update()
    {
        if (widgetA && widgetB)
        {
            Vector3 widgetAAncrePos = widgetAAncre.transform.position;
            Vector3 widgetBAncrePos = widgetBAncre.transform.position;
            transform.position = Vector3.Lerp(widgetAAncrePos, widgetBAncrePos, 0.5f);

            transform.right = widgetAAncrePos - widgetBAncrePos;

            RectTransform rt = (RectTransform)transform;

            rt.sizeDelta = new Vector2(Vector3.Distance(widgetAAncrePos, widgetBAncrePos), rt.sizeDelta.y);
            if (!initialized)
            {
                if(!GameObject.Find("SoundManager").GetComponent<SoundManager>().FindSoundSource("InvestigationOpen").isPlaying)
                    GameObject.Find("SoundManager").GetComponent<SoundManager>().Play("InvestigationLink", 0);
                initialized = true;
            }
        }
        
        if(widgetA == null || widgetB == null)
        {
            if(initialized)
                DeleteLink();
        }
    }

    public void Initialize(Transform widgetA, Transform widgetB, Color color)
    {
        this.widgetA = widgetA;
        this.widgetB = widgetB;
        widgetAAncre = widgetA.GetComponent<InvestigationWidget>().CheckAvailableAncre();
        widgetBAncre = widgetB.GetComponent<InvestigationWidget>().CheckAvailableAncre();

        image.color = color;
    }

    public void ChangeColor(Color color)
    {
        image.color = color;
    }

    public void GetDot()
    {
        GetComponent<Image>().sprite = dot;
    }

    public void DeleteLink()
    {
        GameObject.Find("SoundManager").GetComponent<SoundManager>().Play("InvestigationClose", 0);
        widgetAAncre.taken = false;
        widgetBAncre.taken = false;
        GameObject.Find("InvestigationDataBase").GetComponent<InvestigationDataBase>().DeleteLinkDatabase(gameObject.GetInstanceID());
        Destroy(gameObject);
    }
}
