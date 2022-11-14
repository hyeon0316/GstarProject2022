using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPanel : MonoBehaviour
{
    [SerializeField] private GameObject _targetBoxPrefab;
    private Camera _camera;

    private Transform _target;

    private GameObject _targetBox;

    private IEnumerator _setTargetCo;
    
    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Start()
    {
        _targetBox = Instantiate(_targetBoxPrefab);
        _targetBox.transform.SetParent(this.transform);
        _targetBox.SetActive(false);
    }

    public void SetTargetBox(Transform target)
    {
        if (_setTargetCo != null)
        {
            StopCoroutine(_setTargetCo);
            _setTargetCo = null;
        }
        
        _target = target;
        _targetBox.SetActive(true);
        _setTargetCo = UpdateTargetCo();
        StartCoroutine(_setTargetCo);
    }

    private IEnumerator UpdateTargetCo()
    {
        WaitForEndOfFrame delay = new WaitForEndOfFrame();
        Enemy target = _target.GetComponent<Enemy>();
        while (true)
        {
            if (target.IsDead)
            {
                _targetBox.SetActive(false);
                break;
            }

            Vector3 screenPosition = GetScreenPosition(_camera, _target.transform.position);
            screenPosition.z = 0;
            _targetBox.transform.position = screenPosition + new Vector3(0,50,0);
            yield return delay;
        }
       
    }

    
    private Vector3 GetScreenPosition(Camera mainCamera, Vector3 targetPosition)
    {
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetPosition);
        return screenPosition;
    }
}
