using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    public QuestData[] quests;
    public Inventory _inventory;
    public QuestUI questUI;

    private int _mainId;
    // Start is called before the first frame update
    void Start()
    {
        questUI.SetQuest(quests);
        _mainId = 1;
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
                    if (quests[_mainId].IsCompleteObjectives)
                    {
                        //����Ʈ�Ϸ�
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
                //����Ʈ�Ϸ�
            }
        }
    }
}
