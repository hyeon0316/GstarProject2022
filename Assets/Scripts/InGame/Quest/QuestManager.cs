using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    public QuestData[] quests;
    public Inventory _inventory;
    public QuestUI questUI;
    private QuestData _quset; 
    public QuestInGameUI questInGameUI;
    private int _mainId;
    // Start is called before the first frame update
    void Start()
    {
        
        _mainId = 0;
        Init();
    }
    private void Init()
    {
        questUI.SetQuest(quests);
        questInGameUI.UpdateUI(quests[0]);
    }
    public void InGameQuestUI()
    {
        _quset = quests[_mainId];
    }
    public void ExitButton()
    {
        questUI.gameObject.SetActive(false);
    }
    /// <summary> Äù½ºÆ® EnemyÃ¼Å© _id </summary>
    public void CheckEnemyQuest(EnemyType _id)
    {
        if (quests[_mainId].type == QuestType.KillEnemy)
        {
            for (int a=0; a < quests[_mainId].collectObjectives.EnemyID.Length; a++)
            {
                if (quests[_mainId].collectObjectives.EnemyID[a] == _id)
                {
                    quests[_mainId].collectObjectives.UpdateCount();

                    questInGameUI.UpdateUI(quests[_mainId]);
                    if (quests[_mainId].IsCompleteObjectives)
                    {
                        //Äù½ºÆ®¿Ï·á
                    }
                }
            }
        }
    }
    /// <summary> Äù½ºÆ® NPCÃ¼Å© </summary><param name="_id"></param>
    public void CheckNpcQuest(int _id)
    {
        if (quests[_mainId].type == QuestType.FindNpc)
        {
            if (quests[_mainId].collectObjectives.NpcId == _id)
            {
                //Äù½ºÆ®¿Ï·á
            }
        }
    }
}
