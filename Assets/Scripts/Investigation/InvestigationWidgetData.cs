using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum InvestigationLinkType
{
    FRIEND,
    NEUTRAL,
    ENEMY,
    LOVE,
    KILLED
}
[System.Serializable]
public class InvestigationWidgetData
{
    public Vector2 position;
    
    public InvestigationWidgetData(Vector2 position)
    {
        this.position = position;
    }
}

[System.Serializable]
public class InvestigationLinkData
{
    public InvestigationCharacterData widgetA = null;
    public InvestigationCharacterData widgetB = null;
    public InvestigationWidgetData widgetDataA = null;
    public InvestigationWidgetData widgetDataB = null;
    public InvestigationLinkType linkType;
    public InvestigationLinkData linkedLink;
    public GameObject linkObject;

    public InvestigationLinkData(InvestigationCharacterData widgetA, InvestigationCharacterData widgetB, InvestigationLinkType type, GameObject linkObject)
    {
        this.widgetA = widgetA;
        this.widgetB = widgetB;
        this.widgetDataA = widgetA.widgetData;
        this.widgetDataB = widgetB.widgetData;
        this.linkType = type;
        this.linkObject = linkObject;
}
}
