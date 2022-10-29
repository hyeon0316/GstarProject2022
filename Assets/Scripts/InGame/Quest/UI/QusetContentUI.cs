using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QusetContentUI : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Content;
    public TextMeshProUGUI NeedObject;
    public TextMeshProUGUI Reward1;
    public TextMeshProUGUI Reward2;

    public void UpdateUI(QuestData data)
    {
        Name.text = data.Name;
        Content.text = data.Content;
        if (data.type == QuestType.KillEnemy)
        {
            NeedObject.text = "@@���� óġ(" + data.collectObjectives.currentAmount + "/" + data.collectObjectives.amount + ")";
        }
        else
        {
            NeedObject.text = "@@@ã�ư���";
        }
        if (data.rewards[0].ItemReward == null)
            Reward1.text = "";
        else
        {
            Reward1.text = "������ ������";
        }
        int rewardresult=0;
        for (int a = 0; a < data.rewards.Length; a++)
        {
            rewardresult += data.rewards[a].EXPReward;
        }
        Reward2.text = "EXP : " + rewardresult.ToString();
    }
}
