using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InvestigationCharacterData : InvestigationItemData
{
    public string firstname = "";
    public string nickname = "";
    public string job = "";
    public string height = "";
    public string weight = "";
    public string iD = "";
    public string age = "";
    public string bloodGroup = "";
    public string nationality = "";
    public Sprite portrait = null;
    public InvestigationCharacterData linkedItem = null;
    public GameObject linkedObject = null;
    public InvestigationIconType[] IconTypes = new InvestigationIconType[2];

    public InvestigationCharacterData()
    {

    }

    public InvestigationCharacterData(Vector2 position)
    {
        this.widgetData = new InvestigationWidgetData(position);
    }

    public InvestigationCharacterData(string name, string firstname, string nickname, string job, string height, string weight, string iD, string age, string bloodGroup, string nationality, Sprite portrait, InvestigationCharacterData linkedItem, GameObject linkedObject, InvestigationIconType[] IconTypes)
    {
        this.name = name;
        this.firstname = firstname;
        this.nickname = nickname;
        this.job = job;
        this.height = height;
        this.weight = weight;
        this.iD = iD;
        this.age = age;
        this.bloodGroup = bloodGroup;
        this.nationality = nationality;
        this.portrait = portrait;
        this.linkedItem = linkedItem;
        this.linkedObject = linkedObject;
        this.IconTypes = IconTypes;
    }
}
