using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkData : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    // Start is called before the first frame update
    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }
    void GenerateData()
    {
        talkData.Add(1, new string[] { "����1","���⿡�� �ʹ� ��� ���δ�.", 
            "���⿡�� �ʹ� ��� ���δ�2.", "���⿡�� �ʹ� ��� ���δ�3." });
        talkData.Add(2, new string[] { "���2","���⿡�� �ʹ� ���2 ���δ�.",
            "���⿡�� �ʹ� ��� ����3��2.", "���⿡�� ��4�� ��� ���δ�3." });
        talkData.Add(3, new string[] { "���3","���⿡�� �ʹ� ���2 ���δ�.",
            "���⿡�� �ʹ� ��� ����3��2.", "���⿡�� ��4�� ��� ���δ�3." });
        talkData.Add(4, new string[] { "���3","���⿡�� �ʹ� ���2 ���δ�.",
            "���⿡�� �ʹ� ��� ����3��2.", "���⿡�� ��4�� ��� ���δ�3." });
        talkData.Add(101, new string[] { "���3","���⿡�� �ʹ� ���2 ���δ�.",
            "���⿡�� �ʹ� ��� ����3��2.", "���⿡�� ��4�� ��� ���δ�3." });


    }
    public string[] GetStr(int _s)
    {
        return talkData[_s];
    }
    /*
    public string GetTalk(int id, int talkIndex)
    {
        Debug.Log(id + "," + talkIndex);
        if (!talkData.ContainsKey(id))
        {
            //�ش� ����Ʈ ������ ��簡 ������ �������
            //����Ʈ ��ó�� ��� �������
            if (talkData.ContainsKey(id - id % 10))
            {
                return GetTalk(id - id % 10, talkIndex);
            }
            else
            {
                //����Ʈ ��ó�� ���
                return GetTalk(id - id % 100, talkIndex);
            }
        }
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
    */
}
