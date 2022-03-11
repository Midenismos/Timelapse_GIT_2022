using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class InvestigationWidget : MonoBehaviour
{
    [SerializeField] private GameObject _board = null;
    [SerializeField] private GameObject investigationCharacterSheet = null;
    [SerializeField] private GameObject investigationCharacterSheetMini = null;

    [Header("Input Fields")]
    [SerializeField] private InputField nickname = null;
    [SerializeField] private InputField lastname = null;
    [SerializeField] private InputField firstname = null;
    [SerializeField] private InputField age = null;
    [SerializeField] private InputField job = null;
    [SerializeField] private InputField height = null;
    [SerializeField] private InputField weight = null;
    [SerializeField] private InputField iD = null;
    [SerializeField] private InputField bloodGroup = null;
    [SerializeField] private InputField nationality = null;
    [SerializeField] private InvestigationWidgetIconButton[] icons = new InvestigationWidgetIconButton[3];


    public InvestigationCharacterData data = null;

    public RectTransform[] Ancres;
    [SerializeField] private RectTransform[] ancresPositionsMini = new RectTransform[3];
    [SerializeField] private RectTransform[] ancresPositionsSheet = new RectTransform[3];

    //Vérifie s'il reste des ancres disponibles, sinon anulle la création du lien
    public bool Available()
    {
        if (Ancres.Any(Ancre => Ancre.GetComponent<InvestigationWidgetAncre>().taken == false))
            return true;
        else
            return false;

    }

    private void Awake()
    {
        _board = GameObject.Find("InvestigationBoard");
    }
    public void AssignData(InvestigationCharacterData data)
    {
        this.data = data;
        nickname.text = data.nickname;
        firstname.text = data.firstname;
        lastname.text = data.name;
        age.text = data.age;
        job.text = data.job;
        height.text = data.height;
        weight.text = data.weight;
        iD.text = data.iD;
        bloodGroup.text = data.bloodGroup;
        nationality.text = data.nationality;

        //Restitue les icônes
        for (int i = 0; i <= icons.Length -1; i++)
        {
            if (data.IconTypes == null)
            {
                data.IconTypes = new InvestigationIconType[2];
                data.IconTypes[0] = InvestigationIconType.NONE;
                data.IconTypes[1] = InvestigationIconType.NONE;
            }
            else
            {
                switch (data.IconTypes[i])
                {
                    case InvestigationIconType.NONE:
                        icons[i].gameObject.GetComponent<Image>().sprite = null;
                        break;
                    case InvestigationIconType.MURDERED:
                        icons[i].gameObject.GetComponent<Image>().sprite = icons[i].MurderedSprite;
                        break;
                    case InvestigationIconType.CRYSTALFRIENDLY:
                        icons[i].gameObject.GetComponent<Image>().sprite = icons[i].CrystalFriendlySprite;
                        break;
                    case InvestigationIconType.CRYSTALCORRUPTED:
                        icons[i].gameObject.GetComponent<Image>().sprite = icons[i].CrystalCorruptedSprite;
                        break;
                    case InvestigationIconType.NOTCRYSTALFRIENDLY:
                        icons[i].gameObject.GetComponent<Image>().sprite = icons[i].NotCrystalFriendlySprite;
                        break;
                }
            }


        }
    }
    
    public void SwitchCharacterSheetVisibility()
    {
        GameObject.Find("SoundManager").GetComponent<SoundManager>().Play("InvestigationClick", 0);
        investigationCharacterSheet.SetActive(!investigationCharacterSheet.activeInHierarchy);
        investigationCharacterSheetMini.SetActive(!investigationCharacterSheetMini.activeInHierarchy);

        for (int i = 0; i<= Ancres.Length-1; i++)
        {
            Ancres[i].transform.position = investigationCharacterSheet.activeInHierarchy ? ancresPositionsSheet[i].position : ancresPositionsMini[i].position;
        }
    }

    public void EventDoubleClicked()
    {
        SwitchCharacterSheetVisibility();
    }

    public void NickNameChanged(string nickname)
    {
        data.nickname = nickname;
    }

    public void FirstNameChanged(string firstname)
    {
        data.firstname = firstname;
    }

    public void LastNameChanged(string name)
    {
        data.name = name;
    }

    public void JobChanged(string job)
    {
        data.job = job;
    }

    public void WeightChanged(string weight)
    {
        /*string value;
        bool isInt = int.TryParse(weight, out value);
        if (isInt)*/
        data.weight = weight;
    }

    public void HeightChanged(string height)
    {
        /*string value;
        bool isInt = int.TryParse(height, out value);
        if (isInt)*/
        data.height = height;
    }

    public void IDChanged(string iD)
    {
        /*string value;
        bool isFloat = float.TryParse(iD, out value);
        if (isFloat)*/
        data.iD = iD;
    }

    public void AgeChanged(string age)
    {
        /*string value;
        bool isInt = int.TryParse(age, out value);
        if (isInt)*/
        data.age = age;
    }

    public void BloodGroupChanged(string bloodGroup)
    {
        data.bloodGroup = bloodGroup;
    }

    public void NationalityChanged(string nationality)
    {
        data.nationality = nationality;
    }

    public void PositionChanged (Vector2 position)
    {
        data.widgetData.position = position;
    }

    public void IconChanged (InvestigationIconType IconType, int space)
    {
        data.IconTypes[space] = IconType;
        GameObject.Find("SoundManager").GetComponent<SoundManager>().Play("InvestigationClick", 0);
    }

    public void Delete()
    {
        GameObject.Find("InvestigationDataBase").GetComponent<InvestigationDataBase>().DeleteWidget(gameObject.GetInstanceID());
        GameObject.Find("SoundManager").GetComponent<SoundManager>().Play("InvestigationClose", 0);
    }


    //Retourne la première ancre disponible et l'attribue au nouveau lien
    public InvestigationWidgetAncre CheckAvailableAncre()
    {
        foreach (RectTransform Ancre in Ancres)
        {
            if (Ancre.GetComponent<InvestigationWidgetAncre>().taken == false)
            {
                Ancre.GetComponent<InvestigationWidgetAncre>().taken = true;
                return Ancre.GetComponent<InvestigationWidgetAncre>();
            }
        }
        // TODO Feedback si plus de place disponible
        return null;

    }

    //Permet de déplacer les widgets sans que le scroll rect n'interfère
    public void ActivateScrollRect()
    {
        _board.GetComponent<ScrollRect>().enabled = true;
    }

    public void DeactivateScrollRect()
    {
        _board.GetComponent<ScrollRect>().enabled = false;
    }

}
