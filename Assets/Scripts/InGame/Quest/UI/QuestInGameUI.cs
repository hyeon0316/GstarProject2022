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
    // Start is called before the first frame update

    private void Start()
    {
        Fin.SetActive(false);
    }
    public void UpdateUI(QuestData data)
    {
        if (data.IsCompleteObjectives)
        {
            Name.text = "�Ϸ�";
            Fin.SetActive(true);
        }
        Name.text = data.Name;
        if(data.type == QuestType.FindNpc)
        {
            NeedObject.text = data.Target + " ã�ư��� ";
        }
        else
        {
            NeedObject.text = data.Target + " óġ(" + data.collectObjectives.currentAmount + "/" + data.collectObjectives.amount + ")";
        }
    }
    public void OnMoveSpawn()
    {

    }
    public void OnClickQuest()
    {
    }
    public void OnFin()
    {
        QuestManager.Instance.NextQuest();
        Fin.SetActive(false);
    }
}
