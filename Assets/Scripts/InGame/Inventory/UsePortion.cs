using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UsePortion : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI count;
    [SerializeField] private Inventory inventory;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetPortion(int i)
    {
        count.text = i.ToString();
    }

    public void BtnUse()
    {
        inventory.UsePortion();
    }
}
