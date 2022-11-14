using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour 
{
    [SerializeField] private GameObject _inventoryWindow;

    [SerializeField] private GameObject _questWindow;

    [SerializeField] private GameObject _storeWindow;

    [SerializeField] private GameObject _mapWindow;

    [SerializeField] private GameObject _townTeleportWindow;

    public void ActiveInven()
    {
        _inventoryWindow.SetActive(true);
    }

    public void ActiveQuest()
    {
        _questWindow.SetActive(true);
    }
    public void ActiveStore()
    {
        _storeWindow.SetActive(true);
    }

    public void ActiveTownTeleportWindow(bool isActive)
    {
        _townTeleportWindow.SetActive(isActive);
    }

    public void ActiveMap()
    {
        if (_mapWindow == null)
        {
            _mapWindow = GameObject.Find("Map").transform.Find("MapWindow").gameObject;
            Debug.Log(_mapWindow);
        }
        _mapWindow.SetActive(true);
    }
    public void ActiveDungeon()
    {
        MapManager.Instance.dun.SetActive(true);
    }
}
