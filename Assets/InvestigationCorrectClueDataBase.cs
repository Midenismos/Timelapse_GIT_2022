using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigationCorrectClueDataBase : MonoBehaviour
{
    [SerializeField] private List<InvestigationCharacterData> investigationCorrectClues = new List<InvestigationCharacterData>();
    [SerializeField] private List<InvestigationLinkData> investigationCorrectLinks = new List<InvestigationLinkData>();

    [SerializeField] public List<InvestigationCharacterData> InvestigationCorrectClues { get => investigationCorrectClues; }
    [SerializeField] public List<InvestigationLinkData> InvestigationCorrectLinks { get => investigationCorrectLinks; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
