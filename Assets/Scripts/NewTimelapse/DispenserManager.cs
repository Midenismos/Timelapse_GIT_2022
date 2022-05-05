using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserManager : MonoBehaviour
{
    [SerializeField] private EntryNumber _nb = null;
    public DragObjects CurrentDrag = null;
    [SerializeField] GameObject EntryType = null;
    private void Awake()
    {
        GameObject newDrag = Instantiate(EntryType, this.transform);
        newDrag.transform.position = this.transform.position;
        newDrag.GetComponent<DragObjects>().EntryDispenser = this.gameObject;
        newDrag.GetComponent<TIEntryScript>().Manager = this;
        newDrag.GetComponent<RectTransform>().localScale = new Vector3(0.125f, 0.125f, 1);
        CurrentDrag = newDrag.GetComponent<DragObjects>();
    }

    // Update is called once per frame
    void Update()
    {

        if(CurrentDrag ==  null && _nb.Number > 0)
        {
            _nb.Number -= 1;
            if (_nb.Number > 0)
            {
                GameObject newDrag = Instantiate(EntryType, this.transform);
                newDrag.transform.position = this.transform.position;
                newDrag.GetComponent<RectTransform>().localScale = new Vector3(0.125f, 0.125f, 1);
                newDrag.GetComponent<DragObjects>().EntryDispenser = this.gameObject;
                newDrag.GetComponent<TIEntryScript>().Manager = this;
                CurrentDrag = newDrag.GetComponent<DragObjects>();
            }
        }
    }

    public void IncreaseNumber()
    {
        if(_nb.Number == 0)
            _nb.Number += 2;
        else
            _nb.Number += 1;
    }
}
