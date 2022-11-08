using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BulletRainMissile : MonoBehaviour
{
    private Vector3[] _bezierPoints = new Vector3[4];

    private float _speed;
    private float _endTime = 0;
    private float _curTime = 0;
    private int _percentDamage;
    
    /// <summary>
    /// 미사일의 초기값 설정
    /// </summary>
    public void Init(int percentDamage, Transform startTr,Transform endTr, float speed, float distanceFromStart, float distanceFromEnd)
    {
        _curTime = 0;
        _percentDamage = percentDamage;
        _speed = speed;
        
        _endTime = Random.Range(0.8f, 1.0f); //도착 시간을 랜덤으로 설정

        _bezierPoints[0] = startTr.position; //시작 지점

        //시작 지점을 기준으로 랜덤 포인트 지정
        _bezierPoints[1] = startTr.position +
                           (distanceFromStart * Random.Range(-1.0f, 1.0f) * startTr.right) +  // X (좌, 우 전체)
                           (distanceFromStart * Random.Range(-0.15f, 1.0f) * startTr.up) + // Y (아래쪽 조금, 위쪽 전체)
                           (distanceFromStart * Random.Range(-1.0f, -0.8f) * startTr.forward); // Z (뒤 쪽만)
        
        // 도착 지점을 기준으로 랜덤 포인트 지정.
        _bezierPoints[2] = endTr.position +
                           (distanceFromEnd * Random.Range(-1.0f, 1.0f) * endTr.right) + // X (좌, 우 전체)
                           (distanceFromEnd * Random.Range(-1.0f, 1.0f) * endTr.up) + // Y (위, 아래 전체)
                           (distanceFromEnd * Random.Range(0.8f, 1.0f) * endTr.forward); // Z (앞 쪽만)
        
        _bezierPoints[3] = endTr.position; // 도착 지점

        transform.position = _bezierPoints[0];
    }

    private void Update()
    {
        if (_curTime > _endTime)
        {
            DisableMissile();
            return;
        }

        _curTime += Time.deltaTime * _speed;

        transform.position = new Vector3(
            CubicBezierCurve(_bezierPoints[0].x, _bezierPoints[1].x, _bezierPoints[2].x, _bezierPoints[3].x),
            CubicBezierCurve(_bezierPoints[0].y, _bezierPoints[1].y, _bezierPoints[2].y, _bezierPoints[3].y),
            CubicBezierCurve(_bezierPoints[0].z, _bezierPoints[1].z, _bezierPoints[2].z, _bezierPoints[3].z));
    }

    /// <summary>
    /// 3차 베지어 곡선.
    /// </summary>
    /// <param name="a">시작 위치</param>
    /// <param name="b">시작 위치에서 얼마나 꺾일 지 정하는 위치</param>
    /// <param name="c">도착 위치까지 얼마나 꺾일 지 정하는 위치</param>
    /// <param name="d">도착 위치</param>
    /// <returns></returns>
    private float CubicBezierCurve(float a, float b, float c, float d)
    {
        float time = _curTime / _endTime;

        //방정식 표현
        float ab = Mathf.Lerp(a, b, time);
        float bc = Mathf.Lerp(b, c, time);
        float cd = Mathf.Lerp(c, d, time);

        float abbc = Mathf.Lerp(ab, bc, time);
        float bccd = Mathf.Lerp(bc, cd, time);

        return Mathf.Lerp(abbc, bccd, time);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Stat playerStat = DataManager.Instance.Player.Stat;
            float resultDamage = playerStat.Attack * _percentDamage / 100 * playerStat.SkillDamage / 100 *
                playerStat.AllDamge / 100 * Random.Range(0.8f, 1f);
            other.transform.GetComponent<Creature>().TakeDamage((int)resultDamage, playerStat.Attack);
            CreateMissileEffect();
            DisableMissile();
        }
    }

    private void CreateMissileEffect()
    {
        GameObject effect = ObjectPoolManager.Instance.GetObject(PoolType.BulletRainEffect);
        effect.transform.position = transform.position;
        effect.GetComponent<BulletRainEffect>().DelayDisable();
    }
    private void DisableMissile()
    {
        ObjectPoolManager.Instance.ReturnObject(PoolType.BulletRainMissile, this.gameObject);
    }
}
