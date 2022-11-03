using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreUI : MonoBehaviour
{
    public Inventory inventory;
    public BuyUI BuyUI;
    [SerializeField]
    private StoreSlotUI[] _slots;
    // Start is called before the first frame update
    void Start()
    {
        BuyUI.gameObject.SetActive(false);
        for (int i=0;i<_slots.Length; i++)
        {
            _slots[i].SetCount(inventory.HavePostion());
        }
    }

    public void ButtonBuy()
    {
        inventory.Add(BuyUI.Item, BuyUI.BuyCount);
    }
    public void ButtonExit()
    {
        gameObject.SetActive(false);
    }
}
