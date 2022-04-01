using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTIScript : MonoBehaviour
{
    [SerializeField] private GameObject _handle;

    [SerializeField] private GameObject _entryDownPrefab;
    [SerializeField] private GameObject _entryUpPrefab;


    public void CreateEntryDown()
    {
        GameObject entryDown = Instantiate(_entryDownPrefab, this.transform);
        entryDown.transform.position = _handle.transform.position;

    }
    public void CreateEntryUp()
    {
        GameObject entryUp = Instantiate(_entryUpPrefab, this.transform);
        entryUp.transform.position = _handle.transform.position;
    }

}
