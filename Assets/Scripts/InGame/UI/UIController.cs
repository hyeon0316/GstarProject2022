using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour 
{
    [SerializeField] private GameObject _inventoryWindow;

    [SerializeField] private GameObject _questWindow;

    [SerializeField] private GameObject _storeWindow;


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

}
