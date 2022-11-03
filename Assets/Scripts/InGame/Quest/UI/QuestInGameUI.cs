using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class QuestInGameUI : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public TextMeshProUGUI NeedObject;
    public GameObject Fin;
    private int questNum;
    private bool questType;
    private EnemyType enemyType;
    // Start is called before the first frame update

    private void Start()
    {
        Fin.SetActive(false);
    }
    public void UpdateUI(QuestData data)
    {
        
        if (data.IsCompleteObjectives)
        {
            Name.text = "완료";
            Fin.SetActive(true);
        }
        Name.text = data.Name;
        if(data.type == QuestType.FindNpc)
        {
            questNum = data.collectObjectives.NpcId;
            NeedObject.text = data.Target + " 찾아가기 ";
            questType = true;
        }
        else
        {

            questNum = data.ID;
            NeedObject.text = data.Target + " 처치(" + data.collectObjectives.currentAmount + "/" + data.collectObjectives.amount + ")";
            questType = false;
        }
    }
    public void OnMoveSpawn()
    {

    }
    public void OnClickQuest()
    {
        Transform _tr;
        if(questType)
        {
            _tr = DataManager.Instance.GetNpcData(questNum);
            DataManager.Instance.Player.SetAutoQuest(_tr);
            DataManager.Instance.TargetNpc = _tr.GetComponent<NpcData>();
        }
        else
        {
            _tr = DataManager.Instance.GetEnemySpwan(questNum);
            DataManager.Instance.Player.SetAutoQuest(_tr);
        }
        
    }
    public void OnFin()
    {
        QuestManager.Instance.NextQuest();
        Fin.SetActive(false);
    }
}
