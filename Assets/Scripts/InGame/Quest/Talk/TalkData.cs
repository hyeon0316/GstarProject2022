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
        string[] _s;
        _s = talkData[100];
        for(int i=0;i<_s.Length;i++)
        {
            Debug.Log(_s[i]);
        }    
    }
    void GenerateData()
    {
        talkData.Add(100, new string[] { "���⿡�� �ʹ� ��� ���δ�.", "���⿡�� �ʹ� ��� ���δ�2.", "���⿡�� �ʹ� ��� ���δ�3." });
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
