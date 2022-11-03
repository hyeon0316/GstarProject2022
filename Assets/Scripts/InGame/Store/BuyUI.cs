using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyUI : MonoBehaviour
{
    public Image Icon;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI MaxCount;
    public TextMeshProUGUI Price;
    public TextMeshProUGUI BuyCountText;
    public ItemData Item;
    public int Gold;
    public int BuyCount=1;
    private int canPostion=1908;
    private Inventory inventory;

    public void SetBuyUI(StoreSlotUI _slot)
    {
        Gold = _slot.Gold;
       // canPostion = DataManager.Instance.Player.Stat.MaxPostion - _slot.HavePostion;
        MaxCount.text = canPostion.ToString();
        Item = _slot.Item;
        Icon.sprite = _slot.Icon.sprite;
        Name.text = _slot.Name.text;
        BuyCountText.text = BuyCount.ToString();
        int resultGold = BuyCount * Gold;
        Price.text = resultGold.ToString();
    }
    public void Button50()
    {
        if (BuyCount + 50 < canPostion)
            BuyCount += 50;
        else
            BuyCount = canPostion;
        BuyCountText.text = BuyCount.ToString();
        int resultGold = BuyCount * Gold;
        Price.text = resultGold.ToString();
    }
    public void Button100()
    {
        if (BuyCount + 100 < canPostion)
            BuyCount += 100;
        else
            BuyCount = canPostion;
        BuyCountText.text = BuyCount.ToString();
        int resultGold = BuyCount * Gold;
        Price.text = resultGold.ToString();
    }
    public void ButtonMAX()
    {
        BuyCount = canPostion;
        BuyCountText.text = BuyCount.ToString();
        int resultGold = BuyCount * Gold;
        Price.text = resultGold.ToString();
    }
    public void ButtonCancel()
    {
        BuyCount = 0;
        Price.text = 0.ToString();
        gameObject.SetActive(false);
    }
}
