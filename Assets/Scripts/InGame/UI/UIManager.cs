using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject _inventoryWindow;

    [SerializeField] private GameObject _questWindow;

    [SerializeField] private GameObject _storeWindow;

    [SerializeField] private GameObject _mapWindow;

    [SerializeField] private GameObject _townTeleportWindow;

    public void ActiveInven(bool isActive)
    {
        _inventoryWindow.SetActive(isActive);
    }

    public void ActiveQuest(bool isActive)
    {
        _questWindow.SetActive(isActive);
    }
    public void ActiveStore(bool isActive)
    {
        _storeWindow.SetActive(isActive);
    }

    public void ActiveTownTeleportWindow(bool isActive)
    {
        _townTeleportWindow.SetActive(isActive);
    }

    public void ActiveMap(bool isActive)
    {
        if (_mapWindow == null)
        {
            _mapWindow = GameObject.Find("Map").transform.Find("MapWindow").gameObject;
            Debug.Log(_mapWindow);
        }
        _mapWindow.SetActive(isActive);
    }
    public void ActiveDungeon(bool isActive)
    {
        MapManager.Instance.dun.SetActive(isActive);
    }

    /// <summary>
    /// 모든 창을 꺼준다.
    /// </summary>
    public void CloseAll()
    {
        ActiveDungeon(false);
        ActiveInven(false);
        ActiveMap(false);
        ActiveQuest(false);
        ActiveMap(false);
        ActiveTownTeleportWindow(false);
    }
}
