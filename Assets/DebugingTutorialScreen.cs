using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugingTutorialScreen : MonoBehaviour
{
    private GameObject _player = null;
    [SerializeField] private GameObject _normalTutorialScreen = null;
    [SerializeField] private GameObject _investigationTutorialScreen = null;
    private void Awake()
    {
        _player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        bool investigationOpened = _player.GetComponent<PlayerController>().isInvestigationOpened;
        _normalTutorialScreen.SetActive(!investigationOpened);
        _investigationTutorialScreen.SetActive(investigationOpened);
    }
}
