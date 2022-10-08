using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class JoystickController : MonoBehaviour, IDragHandler, IPointerDownHandler,IPointerUpHandler
{
    [SerializeField] private RectTransform _background;

    [SerializeField] private RectTransform _joystick;

    [SerializeField] private Transform _cameraArm;
    
    
    private float _radius;
    private Vector3 _playerMoveAngle;
    private bool _isTouch;
    private float _moveDistance;//조이스틱 이동거리
    
    private void Start()
    {
        _radius = _background.rect.width / 2;
    }

    private void FixedUpdate()
    {
        if (_isTouch)
        {
            DataManager.Instance.CurPlayer.Move(_playerMoveAngle, _moveDistance);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isTouch = true;
        ControlJoystcik(eventData);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        ControlJoystcik(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _joystick.localPosition = Vector3.zero;
        _isTouch = false;
    }

    /// <summary>
    /// 조이스틱 이동에 따른 실제 플레이어 이동 처리
    /// </summary>
    private void ControlJoystcik(PointerEventData eventData)
    {
        
        Vector2 pos = eventData.position - (Vector2)_background.position;
        pos = Vector2.ClampMagnitude(pos, _radius);
        _joystick.localPosition = pos;
        
        _moveDistance = Vector2.Distance(_background.position, _joystick.position) / _radius;//조이스틱 기존위치에서 멀어진 거리
        
        pos = pos.normalized;
        Vector3 movePos = new Vector3(pos.x  , 0, pos.y);
        
        //기존 방향 + 갈려고 하는 방향 = 최종적으로 움직여야 할 각도
        Vector3 camAngle = _cameraArm.rotation.eulerAngles;
        Vector3 camDirAngle = Quaternion.LookRotation(movePos).eulerAngles;

        _playerMoveAngle = Vector3.up * (camAngle.y + camDirAngle.y); //y축 기준 회전
    }
}
