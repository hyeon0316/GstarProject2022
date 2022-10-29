using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class QuestInGameUI : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public TextMeshProUGUI NeedObject;
    // Start is called before the first frame update
    
    public void UpdateUI(QuestData data)
    {
        Name.text = data.Name;
        if(data.type == QuestType.FindNpc)
        {
            NeedObject.text = data.Target + " 찾아가기 ";
        }
        else
        {
            NeedObject.text = data.Target + " 처치(" + data.collectObjectives.currentAmount + "/" + data.collectObjectives.amount + ")";
        }
    }
}
