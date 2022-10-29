using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum EnemyType
{
    Enemy1,
    Enemy2,
    Enemy3,
    Spider
}

public abstract class Enemy : Creature
{
    protected enum PublicAnimState
    {
        //todo: 적마다 공통적인 애니메이션 상태가 있을 경우 추가하여 매직넘버 대신 사용
    }
    
    [SerializeField] private Transform _spawnArea;
    
    [Header("스폰지점으로 돌아가기 전 까지 거리")]
    [SerializeField] private float _backDistance;
    private Vector3 _outVector; //스폰 지점에서 나왔을때의 지점

    private bool _isOutArea; //스폰 지점에서 나왔는지
    private bool _isGoBack; //되돌아 가는 중인지
    [SerializeField]
    public EnemyType _enemyType;
    protected Rigidbody _rigid;
    protected bool _isFollow; //플레이어를 쫓아야 하는지에 대한 변수
    protected bool _isAttack;

    private void OnEnable()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Enemy");
        _isFollow = false;
        _isAttack = false;
        _isGoBack = false;
        _isOutArea = false;
    }
    
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _targets.Add(DataManager.Instance.Player.transform);
        _animator.SetInteger(Global.EnemyStateInteger,0);
    }

    private void Update()
    {
        if (_isOutArea)
        {
            if (_backDistance < Vector3.Distance(transform.position, _outVector)) //일정 거리 이상 스폰지역 밖으로 나왔을때
            {
                StartCoroutine(BackToArea());
            }
            else //추적하던 적이 사라졌을때
            {
                if(IsNullPlayer())
                {
                    StartCoroutine(BackToArea());
                }
            }
        }
        
    }

    private void FixedUpdate()
    {
        DoPattern();
        FreezeVelocity();
    }

    /// <summary>
    /// 스폰지역에서 멀어질때 OR 적이 없어질때 다시 제자리로 돌아감
    /// </summary>
    private IEnumerator BackToArea()
    {
        _isFollow = false;
        _isOutArea = false;
        while (true)
        {
            if (_nav.remainingDistance <= 0.5f) //도착했을때
            {
                _animator.SetInteger(Global.EnemyStateInteger,0);
                _nav.isStopped = true;
                _isGoBack = false;
                //todo: 체력 전체 회복
                break;
            }
            _animator.SetInteger(Global.EnemyStateInteger,4);
            _nav.SetDestination(_spawnArea.transform.position);
            _nav.isStopped = false;
            _isGoBack = true;
            yield return null;
        }
        
    }
    
    /// <summary>
    /// Rigidbody의 Speed나 다른 값의 변화 방지
    /// </summary>
    private void FreezeVelocity()
    {
        _rigid.angularVelocity = Vector3.zero;
        _rigid.velocity = Vector3.zero;
    }

    private void DoPattern()
    {
        if (!_isAttack) //공격중이 아닐때에만 행동
        {
            if (_isFollow)
            {
                transform.LookAt(_targets[0]);
                if (_attackRadius < Vector3.Distance(transform.position, _targets[0].position)) //타겟이 공격사거리 밖에있을때
                {
                    _nav.isStopped = false;
                    _nav.SetDestination(_targets[0].transform.position);
                    _animator.SetInteger(Global.EnemyStateInteger,2);
                }
                else
                {
                    _nav.isStopped = true;
                    _animator.SetInteger(Global.EnemyStateInteger,1);
                    Attack();
                }
                
                if(IsNullPlayer())
                {
                    _isFollow = false;
                    _animator.SetInteger(Global.EnemyStateInteger,0);
                }
            }
        }
    }

    /// <summary>
    /// 플레이어가 없는지 체크
    /// </summary>
    protected bool IsNullPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _attackRadius * 3, LayerMask.GetMask("Player"));
        if (colliders.Length == 0)
        {
            return true;
        }

        return false;
    }
    protected abstract void Attack();
  

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        if (!_isFollow && !_isGoBack) //피격 당했을 때, 되돌아 가는 중이 아닐 때 추적 시작
        {
            _isFollow = true;
            _animator.SetInteger(Global.EnemyStateInteger,1);
        }
    }

    protected override void Die()
    {
        base.Die();
        _animator.SetTrigger(Global.EnemyDeadTrigger);
        QuestManager.Instance.CheckEnemyQuest(_enemyType);
        Invoke("DestroyObject",1.5f);
        DataManager.Instance.Player.Targets.Remove(this.transform);
    }

    private void DestroyObject()
    {
        Destroy(this.gameObject);//todo: 나중에는 오브젝트풀링으로 관리
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("SpawnArea")) //스폰 지역에서 벗어났을때
        {
            _isOutArea = true;
            _outVector = transform.position;
        }
    }

    
}
