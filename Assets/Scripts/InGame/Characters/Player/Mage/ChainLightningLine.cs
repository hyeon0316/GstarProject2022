using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public class ChainLightningLine : SkillAttack
{
    [SerializeField] private float _drawingSpeed;
    
    private LineRenderer _lineRenderer;

    private bool _isDone;
    private float _timer = 0;


    [Header("스킬 유지 시간")] [SerializeField] private float _keepTime;

    [Header("연결 범위")] [SerializeField] private float _chainRange;

    private List<Transform> _chainTargets = new List<Transform>();
    
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        _lineRenderer.enabled = false;
    }

    private void Update()
    {
        if (_isDone)
        {
            _timer += Time.deltaTime;
            if(_timer >= _keepTime || IsAllDead())
                CloseLine();
            
            _lineRenderer.SetPosition(0, transform.position + Vector3.up);
            for (int i = 0; i < _chainTargets.Count; i++)
            {
                _lineRenderer.SetPosition(i + 1, _chainTargets[i].transform.position + Vector3.up);
            }
        }
    }

    private IEnumerator TakeLightningDamage()
    {
        int count = 0;
        Stat playerStat = DataManager.Instance.Player.Stat;
        while (_timer < _keepTime)
        {
            if (IsAllDead())
                break;

            List<Transform> tempTargets = new List<Transform>();
            foreach (var t in _chainTargets) //깊은복사, foreach로 탐색 중 target이 사라졌을때 오류발생에 대한 방지
            {
                tempTargets.Add(t);
            }
            
            foreach (var target in tempTargets)
            {
                if (target.TryGetComponent(out Creature enemy))
                {
                    enemy.TryGetDamage(DataManager.Instance.Player.Stat, this);
                }
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private bool IsAllDead()
    {
        if (_chainTargets.Count != 0)
            return false;
        
        return true;
    }

    public void CreateLine()
    {
        _lineRenderer.enabled = true;
        _chainTargets.Add(DataManager.Instance.Player.Targets[0]);
        CheckRange(3);
        
        StartCoroutine(CreateLineCo());
    }

    /// <summary>
    /// 라인을 서서히 그려줌
    /// </summary>
    private IEnumerator CreateLineCo()
    {
        _lineRenderer.SetPosition(0, transform.position + Vector3.up);
        
        int index = 0;
        int duration = 1;
        float time = 0;
        while (true)
        {
            if (time >= duration)
            {
                index++;
                time = 0;
                _lineRenderer.positionCount++;
                if (index == _chainTargets.Count)
                {
                    _lineRenderer.positionCount--;
                    break;
                }
            }
            time += Time.deltaTime * _drawingSpeed;
            if (index == 0)
            {
                _lineRenderer.SetPosition(index + 1, Vector3.Lerp(
                    transform.position + Vector3.up, 
                    _chainTargets[index].transform.position + Vector3.up, Mathf.Clamp01(time)));
            }
            else if(index == 1)
            {
                _lineRenderer.SetPosition(index + 1, Vector3.Lerp(
                    _chainTargets[index - 1].transform.position + Vector3.up,
                    _chainTargets[index].transform.position + Vector3.up, Mathf.Clamp01(time)));
            }
            yield return null;
        }
        _isDone = true;
        StartCoroutine(TakeLightningDamage());
    }

    private void CheckRange(int searchCount)
    {
        Collider[] colliders = Physics.OverlapSphere(_chainTargets[0].transform.position, _chainRange, LayerMask.GetMask("Enemy"));

        var searchList = colliders.OrderBy(col => Vector3.Distance(_chainTargets[0].transform.position, col.transform.position))
            .ToList();

        if (searchList.Count != 1) //자기자신만 검출될 경우를 제외하고
        {
            for (int i = 1; i < searchCount; i++)
            {
                if (i == searchList.Count) //찾고자 하는 타겟 수가 실제 존재하는 타겟 수 보다 적을 경우
                    break;

                _chainTargets.Add(searchList[i].transform);
            }
        }
    }
    
    private void CloseLine()
    {
        _isDone = false;
        DataManager.Instance.Player.Targets.Clear();
        _chainTargets.Clear();
        _lineRenderer.positionCount = 2;
        _lineRenderer.enabled = false;
        _timer = 0;
    }
}
