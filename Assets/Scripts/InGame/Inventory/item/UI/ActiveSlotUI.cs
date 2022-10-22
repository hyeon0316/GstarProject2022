using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ActiveSlotUI : MonoBehaviour
{
    public Image Icon;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI MainStat;
    public TextMeshProUGUI SubStat;
    public TextMeshProUGUI EnforceStat;
    public ItemStat ItemStat;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetStat(ItemStat _stat)
    {
        ItemStat = _stat;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetName(string _str)
    {
        Name.text = _str;
    }
    public void SetMainStat(string _str)
    {
        MainStat.text = _str;
    }
    public void SetSubStat(string _str)
    {
        SubStat.text = _str;
    }
    public void SetEnforceStat(string _str)
    {
        EnforceStat.text = _str;
    }
}
