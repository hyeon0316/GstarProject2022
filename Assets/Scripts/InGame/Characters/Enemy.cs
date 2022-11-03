using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum EnemyType
{
    Spider,
    FrightFly,
    ForestGolem,
    SpecialGolem
}

public abstract class Enemy : Creature
{
    [SerializeField] private Transform _spawnArea;
    
    [Header("스폰지점으로 돌아가기 전 까지 거리")]
    [SerializeField] private float _backDistance;
    
    /// <summary>
    /// 스폰 지점에서 나왔을때의 지점
    /// </summary>
    private Vector3 _outVector; 

    /// <summary>
    /// 스폰 지점에서 나왔는지
    /// </summary>
    private bool _isOutArea; 
    
    /// <summary>
    /// 되돌아 가는 중인지
    /// </summary>
    private bool _isGoBack; 
    
    public EnemyType _curEnemyType;
    protected Rigidbody _rigid;
    
    /// <summary>
    /// 플레이어를 추적해야 하는지에 대한 변수
    /// </summary>
    protected bool _isFollow; 
    
    protected bool _isAttack;

    /// <summary>
    /// 플레이어가 공격하지 않았을때 상태
    /// </summary>
    private bool _isWait; 

    private void OnEnable()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
        this.gameObject.layer = LayerMask.NameToLayer("Enemy");
        _isFollow = false;
        _isAttack = false;
        _isGoBack = false;
        _isOutArea = false;
        _isWait = true;
    }
    
    protected override void Awake()
    {
        base.Awake();
        Stat = new Stat(_curEnemyType);
        _rigid = GetComponent<Rigidbody>();
    }
    
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _targets.Add(DataManager.Instance.Player.transform);
        _animator.SetInteger(Global.EnemyStateInteger,0);
    }

    private void Update()
    {
        if (!IsDead)
        {
            if (_isOutArea)
            {
                if (_backDistance < Vector3.Distance(transform.position, _outVector)) //일정 거리 이상 스폰지역 밖으로 나왔을때
                {
                    StartCoroutine(BackToArea());
                }
                else //추적하던 적이 사라졌을때
                {
                    if (IsNullPlayer())
                    {
                        StartCoroutine(BackToArea());
                    }
                }
            }

            if (_isWait)
            {
                
            }
        }

    }

    private void FixedUpdate()
    {
        if (!IsDead)
        {
            DoPattern();
            FreezeVelocity();
        }
    }

    /// <summary>
    /// 스폰지역에서 멀어질때 OR 적이 없어질때 다시 제자리로 돌아감
    /// </summary>
    private IEnumerator BackToArea()
    {
        _isFollow = false;
        _isOutArea = false;
        _nav.SetDestination(_spawnArea.transform.position); //todo: 돌아가는 목적지에 랜덤성 부여
        _nav.isStopped = false;
        _isGoBack = true;
        while (true)
        {
            if (_nav.remainingDistance <= 0.5f) //도착했을때
            {
                if(_curEnemyType is EnemyType.ForestGolem or EnemyType.SpecialGolem)
                    _animator.SetInteger(Global.EnemyAttackInteger, -1);
                
                _animator.SetInteger(Global.EnemyStateInteger,0);
                _nav.isStopped = true;
                _isGoBack = false;
                Stat.Hp = Stat.MaxHp; //복귀시 체력 전체회복
                break;
            }

            if (IsDead) //도착하기 전에 죽게되는 경우
            {
                _nav.isStopped = true;
                break;
            }

            switch (_curEnemyType)
            {
                case EnemyType.Spider:
                    _animator.SetInteger(Global.EnemyStateInteger,4);
                    break;
                case EnemyType.FrightFly:
                    _animator.SetInteger(Global.EnemyStateInteger,0);
                    break;
                case EnemyType.ForestGolem:
                case EnemyType.SpecialGolem:
                    _animator.SetInteger(Global.EnemyStateInteger,1);
                    break;
            }
            
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
                    switch (_curEnemyType)
                    {
                        case EnemyType.Spider:
                            _animator.SetInteger(Global.EnemyStateInteger,2);
                            break;
                        case EnemyType.FrightFly:
                        case EnemyType.ForestGolem:
                        case EnemyType.SpecialGolem:
                            _animator.SetInteger(Global.EnemyStateInteger,1);
                            break;
                    }
                    _nav.isStopped = false;
                    _nav.SetDestination(_targets[0].transform.position);
                }
                else
                {
                    switch (_curEnemyType)
                    {
                        case EnemyType.Spider:
                            _animator.SetInteger(Global.EnemyStateInteger,1);
                            break;
                        case EnemyType.FrightFly:
                        case EnemyType.ForestGolem:
                        case EnemyType.SpecialGolem:
                            _animator.SetInteger(Global.EnemyStateInteger,0);
                            break;
                    }
                    
                    _nav.isStopped = true;
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
        QuestManager.Instance.CheckEnemyQuest(_curEnemyType);
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
