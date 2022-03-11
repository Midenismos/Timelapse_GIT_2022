using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvestigationPanel : MonoBehaviour
{
    [SerializeField] private Transform investigationWidgetsContainer = null;
    [SerializeField] private Transform investigationLinkContainer = null;
    [SerializeField] private InvestigationWidget investigationWidgetPrefab = null;
    [SerializeField] private InvestigationLink investigationLinkPrefab = null;

    [Header("Line Colors")]
    [SerializeField] private Color friendColor = Color.green;
    [SerializeField] private Color enemyColor = Color.red;
    [SerializeField] private Color neutralColor = new Color(94, 94, 94, 255);
    [SerializeField] private Color loveColor = new Color(255,119,233, 255);
    [SerializeField] private Color killedColor = Color.red;

    private InvestigationDataBase dataBase = null;

    private InvestigationLink heldLink = null;
    private InvestigationLinkType heldLinkType;

    private Dictionary<InvestigationWidgetData, InvestigationWidget> investigationWidgets = new Dictionary<InvestigationWidgetData, InvestigationWidget>();

    private void Start()
    {
        dataBase = FindObjectOfType<InvestigationDataBase>();

        if (dataBase)
        {
            InitializePanel();
        }
    }

    private void InitializePanel()
    {
        List<InvestigationCharacterData> characterData = dataBase.InvestigationItems;

        for (int i = 0; i < characterData.Count; i++)
        {
            CreateNewInvestigationWidget(characterData[i]);
        }

        List<InvestigationLinkData> linkData = dataBase.InvestigationLinks;

        for (int i = 0; i < linkData.Count; i++)
        {
            AddLink(linkData[i]);
        }
    }

    public void InvestigationWidgetLeftClicked(PointerEventData data, GameObject widget)
    {
        if(heldLink)
        {
            //Vérifie si le widget contient encore un espace disponible pour les liens, sinon on annule la création du lien
            if(heldLink.WidgetA)
            {
                if (widget.GetComponent<InvestigationWidget>().Available() == true && widget.GetComponent<InvestigationWidget>().data != heldLink.WidgetA.GetComponent<InvestigationWidget>().data)
                {
                    heldLink.WidgetB = widget.transform;

                    dataBase.InvestigationLinkCreated(
                        heldLink.WidgetA.GetComponent<InvestigationWidget>().data,
                        heldLink.WidgetB.GetComponent<InvestigationWidget>().data,
                        heldLinkType,
                        heldLink.gameObject
                        );
                    heldLink.WidgetBAncre = heldLink.WidgetB.GetComponent<InvestigationWidget>().CheckAvailableAncre();
                    heldLink = null;
                }
                else
                {
                    Destroy(heldLink.gameObject);
                    heldLink = null;
                }
            }
            else
            {
                if(widget.GetComponent<InvestigationWidget>().Available() == true)
                {
                    heldLink.WidgetA = widget.transform;
                    heldLink.WidgetAAncre = heldLink.WidgetA.GetComponent<InvestigationWidget>().CheckAvailableAncre();
                }
                else
                {
                    Destroy(heldLink.gameObject);
                    heldLink = null;
                }
            }
        }
    }

    public void CreateNewInvestigationWidget(PointerEventData eventData, GameObject board)
    {
        GameObject.Find("SoundManager").GetComponent<SoundManager>().Play("InvestigationClick", 0);
        InvestigationWidget widget = Instantiate(investigationWidgetPrefab, investigationWidgetsContainer, false);
        widget.GetComponent<UIPointerEvents>().OnClick.AddListener(InvestigationWidgetLeftClicked);

        widget.transform.localPosition = (Vector3)eventData.position - board.transform.position;
        
        widget.data = dataBase.CharacterWidgetCreated(widget.transform.localPosition);
        widget.data.linkedObject = widget.gameObject;

        investigationWidgets.Add(widget.data.widgetData, widget);

    }

    public void CreateNewInvestigationWidget(InvestigationCharacterData data)
    {
        InvestigationWidget widget = Instantiate(investigationWidgetPrefab, investigationWidgetsContainer, false);
        widget.GetComponent<UIPointerEvents>().OnClick.AddListener(InvestigationWidgetLeftClicked);

        widget.transform.localPosition = data.widgetData.position;
        widget.AssignData(data);
        widget.data.linkedObject = widget.gameObject;

        investigationWidgets.Add(widget.data.widgetData, widget);
    }

    

    public void CreateNewLink(int linkType)
    {
        if (heldLink)
        {
            heldLink.ChangeColor(GetCorrespondingColor(linkType));
            heldLinkType = (InvestigationLinkType)linkType;
            if (heldLinkType == InvestigationLinkType.KILLED)
                heldLink.GetDot();
        }
        else
        {

            heldLink = Instantiate(investigationLinkPrefab, investigationLinkContainer);
            heldLink.ChangeColor(GetCorrespondingColor(linkType));
            if (linkType == (int)InvestigationLinkType.KILLED)
                heldLink.GetDot();
            heldLinkType = (InvestigationLinkType)linkType;
        }
    }

    public void AddLink(InvestigationLinkData data)
    {
        InvestigationLink link = Instantiate(investigationLinkPrefab, investigationLinkContainer);
        link.ChangeColor(GetCorrespondingColor(data.linkType));
        if (data.linkType == InvestigationLinkType.KILLED)
            link.GetDot();


        link.WidgetA = investigationWidgets[data.widgetDataA].transform;
        link.WidgetB = investigationWidgets[data.widgetDataB].transform;
        link.WidgetAAncre = investigationWidgets[data.widgetDataA].GetComponent<InvestigationWidget>().CheckAvailableAncre();
        link.WidgetBAncre = investigationWidgets[data.widgetDataB].GetComponent<InvestigationWidget>().CheckAvailableAncre();
        data.linkObject = link.gameObject;
    }


    private Color GetCorrespondingColor(int linkType)
    {
        InvestigationLinkType type = (InvestigationLinkType)linkType;

        switch (type)
        {
            case InvestigationLinkType.FRIEND:
                return friendColor;            
            
            case InvestigationLinkType.ENEMY:
                return enemyColor;            
            
            case InvestigationLinkType.NEUTRAL:
                return neutralColor;

            case InvestigationLinkType.LOVE:
                return loveColor;

            case InvestigationLinkType.KILLED:
                return killedColor;
                
        }

        return friendColor;
    }

    private Color GetCorrespondingColor(InvestigationLinkType type)
    {
        switch (type)
        {
            case InvestigationLinkType.FRIEND:
                return friendColor;

            case InvestigationLinkType.ENEMY:
                return enemyColor;

            case InvestigationLinkType.NEUTRAL:
                return neutralColor;

            case InvestigationLinkType.LOVE:
                return loveColor;

            case InvestigationLinkType.KILLED:
                return killedColor;
        }

        return friendColor;
    }
}
