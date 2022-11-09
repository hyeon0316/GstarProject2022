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
    public TalkUI TalkUI;
    public TalkData TalkData;
    private int _mainId;

    // Start is called before the first frame update
    public void NextQuest()
    {
        _mainId++;
        questInGameUI.UpdateUI(quests[_mainId]);

    }
    void Start()
    {
        _mainId = 0;
        Init();
        for(int i=0;i< quests.Length;i++)
        {
            quests[i].collectObjectives.currentAmount = 0;
        }
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
    /// <summary> ����Ʈ Enemyüũ _id </summary>
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
                        FinishQuest();
                    }
                }
            }
        }
    }
    /// <summary> ����Ʈ NPCüũ </summary><param name="_id"></param>
    public void CheckNpcQuest(int _id)
    {
        if (quests[_mainId].type == QuestType.FindNpc)
        {
            if (quests[_mainId].collectObjectives.NpcId == _id)
            {
                quests[_mainId].collectObjectives.UpdateCount();
                questInGameUI.UpdateUI(quests[_mainId]);
                TalkUI.gameObject.SetActive(true);
                TalkUI.SetText(TalkData.GetStr(_id));
                FinishQuest();
            }
        }
    }

    private void FinishQuest()
    {
        foreach (var qu in quests[_mainId].rewards)
        {
            qu.Reward();
        }
    }
    public void SetInventoryReference(Inventory inventory)
    {
        Debug.Log(inventory.name);
        _inventory = inventory;
    }
}
