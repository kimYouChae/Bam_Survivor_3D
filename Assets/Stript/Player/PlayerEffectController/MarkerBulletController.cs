using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


public class MarkerBulletController : MonoBehaviour
{
    /// <summary>
    ///  총알은 델리게이트 사용 안 해도 될 듯 
    ///  
    /// ** 특수 총알은 카드를 획득 할 때 , 특수타입이면? 
    /// 총알에 스크립트addComponent해서 충돌시 얼리고...독주고...등등 하면 되지 않을까 ?
    /// => 나중에 pooling 사용할 땐 프리팹 미리 만들어놓고 사용하면 될듯 
    /// 
    /// </summary>

    [Header("===Bullet Sate===")]
    [SerializeField] private BulletSate _bulletSate;

    [Header("===basic Bullet Object===")]
    [SerializeField]
    private GameObject _basicBulletObject;

    // 프로퍼티
    public BulletSate bulletSate => _bulletSate;

    private void Start()
    {
        // 총알 state 초기화 
        _bulletSate = new BulletSate(
            _bulletCnt          : 1, 
            _bulletSpeed        : 3f, 
            _bulletDamage       : 1f, 
            _bulletSize         : 1f, 
            _bulletBounceCnt    : 1);

        // 갯수 , 속도, 데미지, 크기 , 튕기는 횟수 

    }

    public void F_BasicBulletShoot(Transform v_muzzleTrs)  // 총구 위치 
    {
        Debug.Log("기본 총알 발사");

        // unit collider 탐색
        Transform _destination;

        //##TODO : 여기서 오류남 null 
        // unit만 콜라이더 검사 
        Collider[] _coll = Physics.OverlapSphere
            (v_muzzleTrs.position, PlayerManager.instance.markers[0].markerState.markerSearchRadious, UnitManager.Instance.unitLayer);
        
        // 검출된게 없으면 종료
        if (_coll.Length <= 0) 
            return;

        // 총알 발사 갯수 만큼 
        for (int i = 0; i < _bulletSate.bulletCount; i++)
        {
            // 검출된게 있음 도착지 설정
            _destination = _coll[0].transform;

            // bullet 생성 
            GameObject _bullet = Instantiate(_basicBulletObject, v_muzzleTrs.position, Quaternion.identity);

            // bullet에 도착지 정해주기
            // ##TODO : bullet 총알 상속으로 구조 바꾸기 지금은 basic으로  
            _bullet.GetComponent<BasicBullet>().bulletState.F_SetField(this.bulletSate);
            _bullet.GetComponent<BasicBullet>().bulletDestination = _destination.position;
            _bullet.GetComponent<BasicBullet>().bulletStartPosition = v_muzzleTrs.position;
        }

    }

    public void F_ApplyBulletEffect(SkillCard v_card) 
    {
        // skill card의 effect 추가 
        v_card.F_SkillcardEffect();

        // ##TODO : Ui 변경 
    }

    // Bullet State 업데이트 
    public void F_UpdateBulletState(int BulletCnt = 0 , float BulletSpeedPercent = 0 , float BulletDamagePercent = 0 , float BulletSizePercent = 0 , int BulletBounceCount = 0 ) 
    {
        bulletSate.bulletCount  += BulletCnt;
        bulletSate.bulletSpeed  += bulletSate.bulletSpeed * BulletSpeedPercent;
        bulletSate.bulletDamage += bulletSate.bulletDamage * BulletDamagePercent;
        bulletSate.bulletSize   += bulletSate.bulletSize * BulletSizePercent;
        bulletSate.bulletCount  += BulletBounceCount;
    }

}