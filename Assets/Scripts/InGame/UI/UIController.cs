using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryWindow;

    public void ActiveInven()
    {
        _inventoryWindow.SetActive(true);
    }
}
