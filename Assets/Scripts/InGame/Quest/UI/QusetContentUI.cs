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
            NeedObject.text = "@@몬스터 처치(" + data.collectObjectives.currentAmount + "/" + data.collectObjectives.amount + ")";
        }
        else
        {
            NeedObject.text = "@@@찾아가기";
        }
        if (data.rewards[0].ItemReward == null)
            Reward1.text = "";
        else
        {
            Reward1.text = "수상한 아이템";
        }
        int rewardresult=0;
        for (int a = 0; a < data.rewards.Length; a++)
        {
            rewardresult += data.rewards[a].EXPReward;
        }
        Reward2.text = "EXP : " + rewardresult.ToString();
    }
}
