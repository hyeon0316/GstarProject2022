using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightningLine : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _drawingSpeed;
    [Range(1, 64)]
    [SerializeField] private int _rows = 8;
    
    [Range(1, 64)]
    [SerializeField] private int _columns = 1;
    
    private LineRenderer _lineRenderer;

    public List<Transform> _targets = new List<Transform>();
    private Vector2 _textureSize;
    private Vector2[] _offsets;
    private int _animationOffsetIndex;
    private int _animationPingPongDirection = 1;

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
            
            for (int i = 0; i < _targets.Count; i++)
            {
                if (_targets[i] != null)
                {
                    _lineRenderer.SetPosition(i, _targets[i].transform.position + Vector3.up);
                }
                else
                {
                    _targets.RemoveAt(i);
                    _lineRenderer.positionCount--;
                }
            }
            SelectOffset();
        }
    }

    private IEnumerator TakeLightningDamage()
    {
        int count = 0;
        while (_timer < _keepTime)
        {
            if (IsAllDead())
                break;
            
            foreach (var target in _targets)
            {
                if (target != null)
                    target.GetComponent<Creature>().TakeDamage(_damage);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private bool IsAllDead()
    {
        foreach (var target in _targets)
        {
            if (target != null)
            {
                return false; 
            }
        }
        return true;
    }

    public void CreateLine()
    {
        _lineRenderer.enabled = true;
        SetFromMaterialChange();
        StartCoroutine(CreateLineCo());
    }

    /// <summary>
    /// 라인을 서서히 그려줌
    /// </summary>
    private IEnumerator CreateLineCo()
    {
        _lineRenderer.SetPosition(0, _targets[0].transform.position + Vector3.up);

        int index = 1;
        int duration = 1;
        float time = 0;
        while (true)
        {
            if (time >= duration)
            {
                index++;
                time = 0;
                _lineRenderer.positionCount++;
                if (index == _targets.Count)
                {
                    _lineRenderer.positionCount--;
                    break;
                }
            }
            time += Time.deltaTime * _drawingSpeed;
            _lineRenderer.SetPosition(index, Vector3.Lerp(_targets[index - 1].transform.position + Vector3.up, _targets[index].transform.position + Vector3.up, Mathf.Clamp01(time)));
            SelectOffset();
            yield return null;
        }
        _isDone = true;
        StartCoroutine(TakeLightningDamage());
    }
    
    
    private void CloseLine()
    {
        _isDone = false;
        _targets.Clear();
        _lineRenderer.positionCount = 2;
        _lineRenderer.enabled = false;
        _timer = 0;
    }

    

    /// <summary>
    /// 전기 텍스쳐 초기 설정
    /// </summary>
    private void SetFromMaterialChange()
    {
        _textureSize = new Vector2(1.0f / (float)_columns, 1.0f / (float)_rows);
        _lineRenderer.material.mainTextureScale = _textureSize;
        _offsets = new Vector2[_rows * _columns];
        for (int y = 0; y < _rows; y++)
        {
            for (int x = 0; x < _columns; x++)
            {
                _offsets[x + (y * _columns)] = new Vector2((float)x / _columns, (float)y / _rows);
            }
        }
    }

    /// <summary>
    /// 텍스쳐를 수시로 바꿔서 전기가 흐르는 것을 표현
    /// </summary>
    private void SelectOffset()
    {
        int index;

        index = _animationOffsetIndex;
        _animationOffsetIndex += _animationPingPongDirection;
        if (_animationOffsetIndex >= _offsets.Length)
        {
            _animationOffsetIndex = _offsets.Length - 2;
            _animationPingPongDirection = -1;
        }
        else if (_animationOffsetIndex < 0)
        {
            _animationOffsetIndex = 1;
            _animationPingPongDirection = 1;
        }

        if (index >= 0 && index < _offsets.Length)
        {
            _lineRenderer.material.mainTextureOffset = _offsets[index];
        }
        else
        {
            _lineRenderer.material.mainTextureOffset = _offsets[0];
        }
    }
}
