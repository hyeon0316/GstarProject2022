using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour 
{
    [SerializeField] private GameObject _inventoryWindow;


    public void ActiveInven()
    {
        _inventoryWindow.SetActive(true);
    }

 
}
