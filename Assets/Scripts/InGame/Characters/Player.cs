using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Player : Creature
{
    /// <summary>
    /// 공격을 하고 있는 상태인지에 대한 bool값(연속터치 방지)
    /// </summary>
    public bool IsAttack { get; set; }
    
    public bool IsAutoMode { get; set; }

    private int _comboCount;
    
    [Header("적 탐색 범위")]
    [SerializeField] protected float _searchRadius;

    [Header("오토모드일때 탐색 범위에 곱해지는 값")]
    [SerializeField] protected float _autoModeSearch;

    [Header("캐릭터가 이동할때마다 발자국을 남길 오브젝트")]
    [SerializeField] private FootPrinter[] _foots;

    /// <summary>
    /// 다음단계의 기본공격이 가능한지에 대한 bool값
    /// </summary>
    private bool _canNextNormalAttack;

    /// <summary>
    /// 기본공격 콤보가 끊어지는 시간
    /// </summary>
    private float _normalAttackCancelDelay;

    /// <summary>
    /// 어떤 코루틴을 stop할것인지 알기 위한 할당 변수
    /// </summary>
    protected IEnumerator _moveCo;

    protected delegate void UseAttackType();

    protected override void Awake()
    {
        base.Awake();
        Stat = new Stat(DataManager.Instance.SelectJobType);
        DataManager.Instance.Player = this;
    }

    private void Start()
    {
        _nav.enabled = false;
    }

    protected virtual void Update()
    {
        if (_canNextNormalAttack)
        {
            CheckInitCombo(); //코루틴 대신 사용
        }

        TouchGetTarget();
    }
    
    /// <summary>
    /// 버튼을 눌렀을때 실행될 오토모드 셋팅
    /// </summary>
    public void SetAutoMode()
    {
        _searchRadius *= _autoModeSearch;
        IsAutoMode = true;
    }

    public void CancelAutoMode()
    {
        _searchRadius /= _autoModeSearch;
        IsAutoMode = false;
    }

    /// <summary>
    /// 직접 선택하여 타겟지정
    /// </summary>
    private void TouchGetTarget()
    {
        if (!IsAutoMode)
        {
            if (Input.GetMouseButtonDown(0)) //PC
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000, LayerMask.GetMask("Enemy")))
                {
                    if (_searchRadius > Vector3.Distance(transform.position, hit.transform.position))
                    {
                        _targets.Clear();
                        _targets.Add(hit.transform);
                        Debug.Log(hit.transform.gameObject.name);
                    }
                }
            }
            else if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began) //Mobile
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

                    if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000, LayerMask.GetMask("Enemy")))
                    {
                        if (_searchRadius > Vector3.Distance(transform.position, hit.transform.position))
                        {
                            _targets.Clear();
                            _targets.Add(hit.transform);
                            Debug.Log(hit.transform.gameObject.name);
                        }
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, _searchRadius);
    }
    
    /// <summary>
    /// 공격할 우선순위 타겟들을(searchCount 수 만큼) 지정
    /// </summary>
    protected void CheckAttackRange(int searchCount, UseAttackType useAttackType)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _searchRadius, LayerMask.GetMask("Enemy"));

        _targets.Clear();

        if (colliders.Length != 0)
        {
            var searchList = colliders.OrderBy(col => Vector3.Distance(transform.position, col.transform.position))
                .ToList(); //가까운 순으로 정렬

            for (int i = 0; i < searchCount; i++)
            {
                if (i == searchList.Count) //찾고자 하는 타겟 수가 실제 존재하는 타겟 수 보다 적을 경우
                    break;

                _targets.Add(searchList[i].transform);
            }
            AttackFromDistance(useAttackType);
        }
        
    }
    

    /// <summary>
    /// 기본 공격 사용
    /// </summary>
    public void UseNormalAttack()
    {
        if (!IsAttack && !IsDead)
        {
            if (_targets.Count != 0)
            {
                AttackFromDistance(NormalAttack);
            }
            else
            {
                CheckAttackRange(1, NormalAttack);
            }
        }
    }

    /// <summary>
    /// 적과의 거리에 따른 행동패턴
    /// </summary>
    protected void AttackFromDistance(UseAttackType useAttackType)
    {
        if (_attackRadius < Vector3.Distance(transform.position, _targets[0].position)) //타겟이 공격사거리 밖에있을때
        {
            if (_moveCo == null) //버튼이 여러번 눌렸을때 코루틴 중복 방지
            {
                _moveCo = MoveTowardEnemyCo(useAttackType);
                StartCoroutine(_moveCo);
            }
        }
        else
        {
            useAttackType();
        }
    }


    public void StopMoveCo()
    {
        if (_moveCo != null)
        {
            StopCoroutine(_moveCo);
            _moveCo = null;
        }
    }

    /// <summary>
    /// 탐색 사거리 안의 적이 공격사거리 보다 클 때 공격 사거리 안에 들어올때까지 해당 적에게 이동
    /// </summary>
    protected IEnumerator MoveTowardEnemyCo(UseAttackType useAttackType)
    {
        while (true)
        {
            if (_attackRadius >= Vector3.Distance(transform.position, _targets[0].position)) //공격 사거리 안에 들어왔을때
            {
                useAttackType();
                Move(transform.rotation.eulerAngles, 0);
                _moveCo = null;
                break;
            }

            transform.LookAt(new Vector3(_targets[0].position.x, transform.position.y, _targets[0].position.z));
            Move(transform.rotation.eulerAngles, 1);
            yield return new WaitForFixedUpdate();
        }
    }


    /// <summary>
    /// 기본공격을 사용될때 실제로 동작되는 기능
    /// </summary>
    private void NormalAttack()
    {
        transform.LookAt(new Vector3(_targets[0].position.x, transform.position.y, _targets[0].position.z));
        _animator.SetInteger(Global.NormalAttackInteger, _comboCount++ % Global.MaxComboAttack);
        IsAttack = true;
        _canNextNormalAttack = false;
    }


    /// <summary>
    /// 기본 공격의 애니메이션 단계를 처음으로 초기화
    /// </summary>
    public void InitNormalAttack()
    {
        _animator.SetInteger(Global.NormalAttackInteger, Global.InitAttackCount);
        InitAttack();

        _normalAttackCancelDelay = 1f;
        _canNextNormalAttack = true;
    }

    public void InitAttack()
    {
        IsAttack = false;
    }

    /// <summary>
    /// 콤보를 초기화
    /// </summary>
    private void CheckInitCombo()
    {
        _normalAttackCancelDelay -= Time.deltaTime;
        if (_normalAttackCancelDelay <= 0)
        {
            _comboCount = 0;
            _canNextNormalAttack = false;
        }
    }

    /// <summary>
    /// 플레이어 이동
    /// </summary>
    /// <param name="angle">이동방향각도</param>
    /// <param name="moveDistance">조이스틱 이동거리</param>
    public void Move(Vector3 angle, float moveDistance)
    {
        if (!IsDead)
        {
            transform.rotation = Quaternion.Euler(angle);
            transform.Translate(Vector3.forward * moveDistance * Stat.MoveSpeed * Time.fixedDeltaTime);
            _animator.SetFloat(Global.MoveBlend, moveDistance);
        }
    }

    public void ActiveFootPrinters(bool active)
    {
        foreach (var foot in _foots)
        {
            foot.ActiveFoot(active);
        }
    }

    protected override void Die()
    {
        //todo: 플레이어가 죽었을때 처리(페이드인 아웃 ->마을 부활, 체력 회복 등)
        base.Die();
        _animator.SetTrigger(Global.DeadTrigger);
    }
}