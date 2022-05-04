using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTIScript : MonoBehaviour
{
    [SerializeField] private GameObject _spawn;

    [SerializeField] private GameObject[] _entryTypes;
    [SerializeField] private GameObject _slider;

    
    //[SerializeField] private GameObject _entryUpPrefab;


    /*public void CreateEntryDown()
    {
        GameObject entryDown = Instantiate(_entryDownPrefab, this.transform);
        entryDown.transform.position = _handle.transform.position;

    }
    public void CreateEntryUp()
    {
        GameObject entryUp = Instantiate(_entryUpPrefab, this.transform);
        entryUp.transform.position = _handle.transform.position;
    }*/

    public void CreateEntry(int type)
    {
        GameObject entry = Instantiate(_entryTypes[type], this.transform);
        entry.transform.SetParent(_slider.transform, false);
        entry.transform.position = _spawn.transform.position;
    }

}
