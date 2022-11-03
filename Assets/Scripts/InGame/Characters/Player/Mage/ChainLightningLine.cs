using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightningLine : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _drawingSpeed;
    
    private LineRenderer _lineRenderer;

    private bool _isDone;
    private float _timer = 0;


    [Header("스킬 유지 시간")]
    [SerializeField] private float _keepTime;
    
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
            for (int i = 0; i < DataManager.Instance.Player.Targets.Count; i++)
            {
                _lineRenderer.SetPosition(i + 1, DataManager.Instance.Player.Targets[i].transform.position + Vector3.up);
            }
        }
    }

    private IEnumerator TakeLightningDamage()
    {
        int count = 0;
        while (_timer < _keepTime)
        {
            if (IsAllDead())
                break;

            List<Transform> tempTargets = new List<Transform>();
            foreach (var t in DataManager.Instance.Player.Targets) //깊은복사, foreach로 탐색 중 target이 사라졌을때 오류발생에 대한 방지
            {
                tempTargets.Add(t);
            }
            
            foreach (var target in tempTargets)
            {
                if (target.TryGetComponent(out Creature enemy))
                    enemy.TakeDamage(_damage);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private bool IsAllDead()
    {
        if (DataManager.Instance.Player.Targets.Count != 0)
            return false;
        
        return true;
    }

    public void CreateLine()
    {
        if (DataManager.Instance.Player.Targets.Count != 0)
        {
            _lineRenderer.enabled = true;
            StartCoroutine(CreateLineCo());
        }
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
                if (index == DataManager.Instance.Player.Targets.Count)
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
                    DataManager.Instance.Player.Targets[index].transform.position + Vector3.up, Mathf.Clamp01(time)));
            }
            else
            {
                _lineRenderer.SetPosition(index + 1, Vector3.Lerp(
                    DataManager.Instance.Player.Targets[index - 1].transform.position + Vector3.up,
                    DataManager.Instance.Player.Targets[index].transform.position + Vector3.up, Mathf.Clamp01(time)));
            }

            yield return null;
        }
        _isDone = true;
        StartCoroutine(TakeLightningDamage());
    }
    
    
    private void CloseLine()
    {
        _isDone = false;
        DataManager.Instance.Player.Targets.Clear();
        _lineRenderer.positionCount = 2;
        _lineRenderer.enabled = false;
        _timer = 0;
    }
}
