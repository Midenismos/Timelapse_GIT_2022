using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class InvestigationDataBase : MonoBehaviour
{   
    
    [SerializeField] private static InvestigationDataBase instance = null;

    [SerializeField] private List<InvestigationCharacterData> investigationItems = new List<InvestigationCharacterData>();
    [SerializeField] private List<InvestigationLinkData> investigationLinks = new List<InvestigationLinkData>();

    [SerializeField] public List<InvestigationCharacterData> InvestigationItems { get => investigationItems; }
    [SerializeField] public List<InvestigationLinkData> InvestigationLinks { get => investigationLinks; }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        if(instance)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
        }
    }

    public InvestigationCharacterData CharacterWidgetCreated(Vector2 position)
    {
        InvestigationCharacterData character = new InvestigationCharacterData(position);
        investigationItems.Add(character);

        return character;
    }

    public void InvestigationLinkCreated(InvestigationCharacterData widgetA, InvestigationCharacterData widgetB, InvestigationLinkType type, GameObject gameObject)
    {
        investigationLinks.Add(new InvestigationLinkData(widgetA, widgetB, type, gameObject));
    }


    


    //Supprime les liens en double cliquant
    public void DeleteLinkDatabase(int linkObjectID)
    {
        foreach (InvestigationLinkData link in InvestigationLinks)
        {
            if (link.linkObject.GetInstanceID() == linkObjectID)
            {
                if(link.linkedLink != null)
                    link.linkedLink.linkedLink = null;
                InvestigationLinks.Remove(link);
                Destroy(link.linkObject);
                break;
            }
        }
    }


    //Supprime les widget en cliquant sur la poubelle
    public void DeleteWidget(int WidgetID)
    {
        foreach (InvestigationCharacterData character in InvestigationItems)
        {
            if (character.linkedObject.GetInstanceID() == WidgetID)
            {
                if(character.linkedItem != null)
                    character.linkedItem.linkedItem = null;
                InvestigationItems.Remove(character);
                Destroy(character.linkedObject);
                break;
            }
        }
    }
    public void CompareToGoodClues()
    {
        int currentHighestFirstnameSimilarity = 0;

        int currentHighestNicknameSimilarity = 0;

        int currentHighestNameSimilarity = 0;

        int currentHighestJobSimilarity = 0;

        int currentHighestIDSimilarity = 0;

        int currentHighestHeightSimilarity = 0;

        int currentHighestWeightSimilarity = 0;

        int currentHighestAgeSimilarity = 0;

        int currentHighestBloodGroupeSimilarity = 0;

        int currentHighestNationalitySimilarity = 0;

        InvestigationCharacterData[] closestCorrectItems = new InvestigationCharacterData[12];


        foreach (InvestigationCharacterData Item in investigationItems)
        {
            foreach (InvestigationCharacterData CorrectItem in GetComponent<InvestigationCorrectClueDataBase>().InvestigationCorrectClues)
            {
                // Compare caractère par caractère ce qu'a écrit le joueur face aux bonnes réponses, la réponse correcte la plus proche est ajouté dans la liste des candidats potentiels à être lié avec cette fiche personnage 
                if (Item.linkedItem == null)
                {
                    if(Item.firstname != null)
                    {
                        int FirstnameSimilarity = 0;
                        for (int i = 0; i <= Item.firstname.Length - 1; i++)
                        {
                            if (i <= CorrectItem.firstname.Length - 1)
                            {
                                if (Item.firstname[i] == CorrectItem.firstname[i])
                                    FirstnameSimilarity += 1;
                            }
                            if (FirstnameSimilarity > currentHighestFirstnameSimilarity)
                            {
                                currentHighestFirstnameSimilarity = FirstnameSimilarity;
                                if (currentHighestFirstnameSimilarity > 2 || Item.firstname == CorrectItem.firstname)
                                    closestCorrectItems[0] = CorrectItem;
                            }
                        }
                    }



                    /*if (Item.firstname == CorrectItem.firstname)
                    {
                        if (CorrectItem.linkedItem == null)
                        {
                            CorrectItem.linkedItem = Item;
                            Item.linkedItem = CorrectItem;
                            break;
                        }
                    }*/

                    if (Item.nickname != null)
                    {
                        int NicknameSimilarity = 0;
                        for (int i = 0; i <= Item.nickname.Length - 1; i++)
                        {
                            if (i <= CorrectItem.nickname.Length - 1)
                            {
                                if (Item.nickname[i] == CorrectItem.nickname[i])
                                    NicknameSimilarity += 1;
                            }
                            if (NicknameSimilarity > currentHighestNicknameSimilarity)
                            {
                                currentHighestNicknameSimilarity = NicknameSimilarity;
                                if (currentHighestNicknameSimilarity > 2 || Item.nickname == CorrectItem.nickname)
                                    closestCorrectItems[1] = CorrectItem;
                            }
                        }
                    }

                    /* if (Item.nickname == CorrectItem.nickname)
                     {
                         if (CorrectItem.linkedItem == null)
                         {
                             CorrectItem.linkedItem = Item;
                             Item.linkedItem = CorrectItem;
                             break;
                         }
                     }*/

                    if (Item.name != null)
                    {
                        int NameSimilarity = 0;
                        for (int i = 0; i <= Item.name.Length - 1; i++)
                        {
                            if (i <= CorrectItem.name.Length - 1)
                            {
                                if (Item.name[i] == CorrectItem.name[i])
                                    NameSimilarity += 1;
                            }
                            if (NameSimilarity > currentHighestNameSimilarity)
                            {
                                currentHighestNameSimilarity = NameSimilarity;
                                if (currentHighestNameSimilarity > 2 || Item.name == CorrectItem.name)
                                    closestCorrectItems[2] = CorrectItem;
                            }
                        }
                    }

                    /*if (Item.name == CorrectItem.name)
                    {
                        if (CorrectItem.linkedItem == null)
                        {
                            CorrectItem.linkedItem = Item;
                            Item.linkedItem = CorrectItem;
                            break;
                        }
                    }*/

                    if (Item.job != null)
                    {
                        int JobSimilarity = 0;
                        for (int i = 0; i <= Item.job.Length - 1; i++)
                        {
                            if (i <= CorrectItem.job.Length - 1)
                            {

                                if (Item.job[i] == CorrectItem.job[i])
                                    JobSimilarity += 1;
                            }
                            if (JobSimilarity > currentHighestJobSimilarity)
                            {
                                currentHighestJobSimilarity = JobSimilarity;
                                if (currentHighestJobSimilarity > 2 || Item.job == CorrectItem.job)
                                    closestCorrectItems[3] = CorrectItem;
                            }
                        }
                    }

                    /*if (Item.job == CorrectItem.job)
                    {
                        if (CorrectItem.linkedItem == null)
                        {
                            CorrectItem.linkedItem = Item;
                            Item.linkedItem = CorrectItem;
                            break;
                        }
                    }*/

                    if (Item.iD != null)
                    {
                        int IDSimilarity = 0;
                        for (int i = 0; i <= Item.iD.Length - 1; i++)
                        {
                            if (i <= CorrectItem.iD.Length - 1)
                            {
                                if (Item.iD[i] == CorrectItem.iD[i])
                                    IDSimilarity += 1;
                            }
                            if (IDSimilarity > currentHighestIDSimilarity)
                            {
                                currentHighestIDSimilarity = IDSimilarity;
                                if (currentHighestIDSimilarity > 2 || Item.iD == CorrectItem.iD)
                                    closestCorrectItems[4] = CorrectItem;
                            }
                        }
                    }

                    /*if (Item.iD == CorrectItem.iD)
                    {
                        if (CorrectItem.linkedItem == null)
                        {
                            CorrectItem.linkedItem = Item;
                            Item.linkedItem = CorrectItem;
                            break;
                        }
                    }*/

                    if (Item.height != null)
                    {
                        int HeightSimilarity = 0;
                        for (int i = 0; i <= Item.height.Length - 1; i++)
                        {
                            if (i <= CorrectItem.height.Length - 1)
                            {
                                if (Item.height[i] == CorrectItem.height[i])
                                    HeightSimilarity += 1;
                            }
                            if (HeightSimilarity > currentHighestHeightSimilarity)
                            {
                                currentHighestHeightSimilarity = HeightSimilarity;
                                if (currentHighestHeightSimilarity > 2 || Item.height == CorrectItem.height)
                                    closestCorrectItems[5] = CorrectItem;
                            }
                        }
                    }

                    /*if (Item.height == CorrectItem.height)
                    {
                        if (CorrectItem.linkedItem == null)
                        {
                            CorrectItem.linkedItem = Item;
                            Item.linkedItem = CorrectItem;
                            break;
                        }
                    }*/

                    if (Item.weight != null)
                    {
                        int WeightSimilarity = 0;
                        for (int i = 0; i <= Item.weight.Length - 1; i++)
                        {
                            if (i <= CorrectItem.weight.Length - 1)
                            {
                                if (Item.weight[i] == CorrectItem.weight[i])
                                    WeightSimilarity += 1;
                            }
                            if (WeightSimilarity > currentHighestWeightSimilarity)
                            {
                                currentHighestWeightSimilarity = WeightSimilarity;
                                if (currentHighestWeightSimilarity > 2 || Item.weight == CorrectItem.weight)
                                    closestCorrectItems[6] = CorrectItem;
                            }
                        }
                    }

                    /*if (Item.weight == CorrectItem.weight)
                    {
                        if (CorrectItem.linkedItem == null)
                        {
                            CorrectItem.linkedItem = Item;
                            Item.linkedItem = CorrectItem;
                            break;
                        }
                    }*/

                    if (Item.age != null)
                    {
                        int AgeSimilarity = 0;
                        for (int i = 0; i <= Item.age.Length - 1; i++)
                        {
                            if (i <= CorrectItem.age.Length - 1)
                            {
                                if (Item.age[i] == CorrectItem.age[i])
                                    AgeSimilarity += 1;
                            }
                            if (AgeSimilarity > currentHighestAgeSimilarity)
                            {
                                currentHighestAgeSimilarity = AgeSimilarity;
                                if (currentHighestAgeSimilarity > 2 || Item.age == CorrectItem.age)
                                    closestCorrectItems[7] = CorrectItem;
                            }
                        }
                    }

                    /*if (Item.age == CorrectItem.age)
                    {
                        if (CorrectItem.linkedItem == null)
                        {
                            CorrectItem.linkedItem = Item;
                            Item.linkedItem = CorrectItem;
                            break;
                        }
                    }*/

                    if (Item.bloodGroup != null)
                    {
                        int BloodGroupeSimilarity = 0;
                        for (int i = 0; i <= Item.bloodGroup.Length - 1; i++)
                        {
                            if (i <= CorrectItem.bloodGroup.Length - 1)
                            {
                                if (Item.bloodGroup[i] == CorrectItem.bloodGroup[i])
                                    BloodGroupeSimilarity += 1;
                            }
                            if (BloodGroupeSimilarity > currentHighestBloodGroupeSimilarity)
                            {
                                currentHighestBloodGroupeSimilarity = BloodGroupeSimilarity;
                                if (currentHighestBloodGroupeSimilarity > 2 || Item.bloodGroup == CorrectItem.bloodGroup)
                                    closestCorrectItems[8] = CorrectItem;
                            }
                        }
                    }

                    /*if (Item.bloodGroup == CorrectItem.bloodGroup)
                    {
                        if (CorrectItem.linkedItem == null)
                        {
                            CorrectItem.linkedItem = Item;
                            Item.linkedItem = CorrectItem;
                            break;
                        }
                    }*/

                    if (Item.nationality != null)
                    {
                        int NationalitySimilarity = 0;
                        for (int i = 0; i <= Item.nationality.Length - 1; i++)
                        {
                            if (i <= CorrectItem.nationality.Length - 1)
                            {
                                if (Item.nationality[i] == CorrectItem.nationality[i])
                                    NationalitySimilarity += 1;
                            }
                            if (NationalitySimilarity > currentHighestNationalitySimilarity)
                            {
                                currentHighestNationalitySimilarity = NationalitySimilarity;
                                if (currentHighestNationalitySimilarity > 2 || Item.nationality == CorrectItem.nationality)
                                    closestCorrectItems[9] = CorrectItem;
                            }
                        }

                    }

                    /*if (Item.nationality == CorrectItem.nationality)
                    {
                        if (CorrectItem.linkedItem == null)
                        {
                            CorrectItem.linkedItem = Item;
                            Item.linkedItem = CorrectItem;
                            break;
                        }
                    }*/
                    if (Item.portrait != null && Item.portrait == CorrectItem.portrait)
                    {
                        closestCorrectItems[10] = CorrectItem;
                    }
                    foreach (InvestigationIconType CorrectIconType in CorrectItem.IconTypes)
                    {
                        if(CorrectIconType != InvestigationIconType.NONE && Item.IconTypes.Contains(CorrectIconType))
                        {
                            closestCorrectItems[11] = CorrectItem;
                        }
                    }
                }

                
                /*foreach(InvestigationCharacterData chosenItem in CorrectItemsToCompare)
                {
                    int TimeAppearance = 0;
                    foreach(InvestigationCharacterData data in closestCorrectItems)
                    {
                        if (data == chosenItem)
                            TimeAppearance += 1;
                    }
                }*/

            }

            //Parmis tous les candidats potentiels à être lié avec la fiche personnage, prend celle qui apparaît le plus souvent dans la liste et la lie

            List<InvestigationCharacterData> closestItemtoGroup = new List<InvestigationCharacterData>();

            foreach (InvestigationCharacterData i in closestCorrectItems.Where(n => n != null))
            {
                if (i != null)
                {
                    closestItemtoGroup.Add(i);
                }
            }

            if(closestItemtoGroup.Count != 0)
            {
                InvestigationCharacterData ClosestItem = closestItemtoGroup.GroupBy(v => v != null ? v : null).OrderByDescending(g => g.Count()).First().Key;
                if (ClosestItem.linkedItem == null)
                {
                    ClosestItem.linkedItem = Item;
                    Item.linkedItem = ClosestItem;
                }


            }

            // si l'Item n'est lié à aucune bonne fiche, efface toutes ses valeurs
            if (Item.linkedItem == null)
            {
                Item.firstname = null;
                Item.nickname = null;
                Item.name = null;
                Item.job = null;
                Item.iD = null;
                Item.weight = null;
                Item.height = null;
                Item.age = null;
                Item.bloodGroup = null;
                Item.nationality = null;
                Item.portrait = null;
                Item.IconTypes = null;
            }

        }

        //si l'item a déjà une fiche liée, compare toutes les variables de l'item a cette fiche et efface les valeurs fausses

        foreach (InvestigationCharacterData Item in investigationItems)
        {
            if (Item.linkedItem != null)
            {
                if (Item.firstname != "")
                {
                    if (Item.firstname.Length > Item.linkedItem.firstname.Length)
                    {
                        Item.firstname = Item.firstname.Substring(0, Item.linkedItem.firstname.Length);
                    }
                    for (int i = 0; i <= Item.firstname.Length - 1; i++)
                    {
                        if (Item.firstname[i] != Item.linkedItem.firstname[i])
                        {
                            Item.firstname = Item.firstname.Remove(i, 1);
                            Item.firstname = Item.firstname.Insert(i, "_");
                        }
                    }
                }


                /*if (Item.firstname != Item.linkedItem.firstname)
                {
                    Item.firstname = null;
                }*/
                if (Item.nickname != "")
                {
                    if (Item.nickname.Length > Item.linkedItem.nickname.Length)
                    {
                        Item.nickname = Item.nickname.Substring(0, Item.linkedItem.nickname.Length);
                    }
                    for (int i = 0; i <= Item.nickname.Length - 1; i++)
                    {
                        if (Item.nickname[i] != Item.linkedItem.nickname[i])
                        {
                            Item.nickname = Item.nickname.Remove(i, 1);
                            Item.nickname = Item.nickname.Insert(i, "_");
                        }
                    }
                }

                /*if (Item.nickname != Item.linkedItem.nickname)
                {
                    Item.nickname = null;
                }*/

                if (Item.name != "")
                {
                    if (Item.name.Length > Item.linkedItem.name.Length)
                    {
                        Item.name = Item.name.Substring(0, Item.linkedItem.name.Length);
                    }
                    for (int i = 0; i <= Item.name.Length - 1; i++)
                    {
                        if (Item.name[i] != Item.linkedItem.name[i])
                        {
                            Item.name = Item.name.Remove(i, 1);
                            Item.name = Item.name.Insert(i, "_");
                        }
                    }
                }

                /*if (Item.name != Item.linkedItem.name)
                {
                    Item.name = null;
                }*/
                if (Item.job != "")
                {
                    if (Item.job.Length > Item.linkedItem.job.Length)
                    {
                        Item.job = Item.job.Substring(0, Item.linkedItem.job.Length);
                    }
                    for (int i = 0; i <= Item.job.Length - 1; i++)
                    {
                        if (Item.job[i] != Item.linkedItem.job[i])
                        {
                            Item.job = Item.job.Remove(i, 1);
                            Item.job = Item.job.Insert(i, "_");
                        }
                    }
                }

                /*if (Item.job != Item.linkedItem.job)
                {
                    Item.job = null;
                }*/
                if (Item.iD != "")
                {
                    if (Item.iD.Length > Item.linkedItem.iD.Length)
                    {
                        Item.iD = Item.iD.Substring(0, Item.linkedItem.iD.Length);
                    }
                    for (int i = 0; i <= Item.iD.Length - 1; i++)
                    {
                        if (Item.iD[i] != Item.linkedItem.iD[i])
                        {
                            Item.iD = Item.iD.Remove(i, 1);
                            Item.iD = Item.iD.Insert(i, "_");
                        }
                    }
                }


                /*if (Item.iD != Item.linkedItem.iD)
                {
                    Item.iD = 0;
                }*/
                if (Item.height != "")
                {
                    if (Item.height.Length > Item.linkedItem.height.Length)
                    {
                        Item.height = Item.height.Substring(0, Item.linkedItem.height.Length);
                    }
                    for (int i = 0; i <= Item.height.Length - 1; i++)
                    {
                        if (Item.height[i] != Item.linkedItem.height[i])
                        {
                            Item.height = Item.height.Remove(i, 1);
                            Item.height = Item.height.Insert(i, "_");
                        }
                    }
                }

                /*if (Item.height != Item.linkedItem.height)
                {
                    Item.height = 0;
                }*/
                if (Item.weight != "")
                {
                    if (Item.weight.Length > Item.linkedItem.weight.Length)
                    {
                        Item.weight = Item.weight.Substring(0, Item.linkedItem.weight.Length);
                    }
                    for (int i = 0; i <= Item.weight.Length - 1; i++)
                    {
                        if (Item.weight[i] != Item.linkedItem.weight[i])
                        {
                            Item.weight = Item.weight.Remove(i, 1);
                            Item.weight = Item.weight.Insert(i, "_");
                        }
                    }
                }

                /*if (Item.weight != Item.linkedItem.weight)
                {
                    Item.weight = 0;
                }*/
                if (Item.age != "")
                {
                    if (Item.age.Length > Item.linkedItem.age.Length)
                    {
                        Item.age = Item.age.Substring(0, Item.linkedItem.age.Length);
                    }
                    for (int i = 0; i <= Item.age.Length - 1; i++)
                    {
                        if (Item.age[i] != Item.linkedItem.age[i])
                        {
                            Item.age = Item.age.Remove(i, 1);
                            Item.age = Item.age.Insert(i, "_");
                        }
                    }
                }

                /*if (Item.age != Item.linkedItem.age)
                {
                    Item.age = 0;
                }*/
                if (Item.bloodGroup != "")
                {
                    if (Item.bloodGroup.Length > Item.linkedItem.bloodGroup.Length)
                    {
                        Item.bloodGroup = Item.bloodGroup.Substring(0, Item.linkedItem.bloodGroup.Length);
                    }
                    for (int i = 0; i <= Item.bloodGroup.Length - 1; i++)
                    {
                        if (Item.bloodGroup[i] != Item.linkedItem.bloodGroup[i])
                        {
                            Item.bloodGroup = Item.bloodGroup.Remove(i, 1);
                            Item.bloodGroup = Item.bloodGroup.Insert(i, "_");
                        }
                    }
                }

                /*if (Item.bloodGroup != Item.linkedItem.bloodGroup)
                {
                    Item.bloodGroup = null;
                }*/
                if (Item.nationality != "")
                {
                    if (Item.nationality.Length > Item.linkedItem.nationality.Length)
                    {
                        Item.nationality = Item.nationality.Substring(0, Item.linkedItem.nationality.Length);
                    }
                    for (int i = 0; i <= Item.nationality.Length - 1; i++)
                    {
                        if (Item.nationality[i] != Item.linkedItem.nationality[i])
                        {
                            Item.nationality = Item.nationality.Remove(i, 1);
                            Item.nationality = Item.nationality.Insert(i, "_");
                        }
                    }
                }

                /*if (Item.nationality != Item.linkedItem.nationality)
                {
                    Item.nationality = null;
                }*/
                if (Item.portrait != null)
                {
                    if (Item.portrait != Item.linkedItem.portrait)
                    {
                        Item.portrait = null;
                    }
                    for (int i = 0; i <= Item.IconTypes.Length - 1; i++)
                    {
                        if (!Item.linkedItem.IconTypes.Contains(Item.IconTypes[i]))
                        {
                            Item.IconTypes[i] = InvestigationIconType.NONE;
                        }
                    }
                }


                // TODO : Mettre ça de façon à que le joueur n'ait pas besoin de terminer la boucle pour effacer la liaison avec la bonne réponse annulée
                // Si pour une raison ou pour une autre le joueur retire les valeurs d'un item ayant été lié à une bonne fiche, le délie
                if (Item.name == null && Item.firstname == null && Item.nickname == null && Item.job == null && Item.iD == "0" && Item.height == "0" && Item.weight == "0" && Item.age == "0" && Item.bloodGroup == null && Item.nationality == null && Item.portrait == null && Item.IconTypes.All(Icon => Icon == InvestigationIconType.NONE))
                {
                    Item.linkedItem.linkedItem = null;
                    Item.linkedItem = null;

                }
            }
        }
        // Compare tous les liens avec les bonnes liaisons et les lies si les liens sont corrects
        foreach (InvestigationLinkData Link in InvestigationLinks.ToList())
        {
            foreach (InvestigationLinkData CorrectLink in GetComponent<InvestigationCorrectClueDataBase>().InvestigationCorrectLinks)
            {
                if(Link.widgetA.linkedItem != null && Link.widgetB.linkedItem != null)
                {
                    //Si c'est un lien de meurtre, cherche exactement qui a tué qui
                    if (Link.linkType == InvestigationLinkType.KILLED)
                    {
                        if (Link.widgetA.linkedItem.name == CorrectLink.widgetA.name)
                        {
                            if (Link.widgetB.linkedItem.name == CorrectLink.widgetB.name)
                            {
                                if (Link.linkType == CorrectLink.linkType)
                                {
                                    if (CorrectLink.linkedLink == null)
                                    {
                                        CorrectLink.linkedLink = Link;
                                        Link.linkedLink = CorrectLink;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    //Sinon, se contente de cherche si les deux persos sont correct même si ce n'est pas dans le bon sens
                    else
                    {
                        if (Link.widgetA.linkedItem.name == CorrectLink.widgetA.name || Link.widgetA.linkedItem.name == CorrectLink.widgetB.name)
                        {
                            if (Link.widgetB.linkedItem.name == CorrectLink.widgetA.name || Link.widgetB.linkedItem.name == CorrectLink.widgetB.name)
                            {
                                if (Link.linkType == CorrectLink.linkType)
                                {
                                    if (CorrectLink.linkedLink == null)
                                    {
                                        CorrectLink.linkedLink = Link;
                                        Link.linkedLink = CorrectLink;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if(Link.linkedLink == null)
            {
                InvestigationLinks.Remove(Link);
            }
        }
    }
}
