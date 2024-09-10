using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

// bullet state
[System.Serializable]
public class BulletSate
{
    [SerializeField] int _bulletCount;    // 한번에 생성하는 총알 갯수
    [SerializeField] float _bulletSpeed;    // 총알 속도
    [SerializeField] float _bulletDamage;   // 총알 데미지 
    [SerializeField] float _bulletSize;     // 총알 크기 
    [SerializeField] int _bulletBounceCount;  // 총알 튕기는 횟수 

    // 프로퍼티 
    public int bulletCount { get => _bulletCount; set { _bulletCount = value; } }
    public float bulletSpeed { get => _bulletSpeed; set { _bulletSpeed = value; } }
    public float bulletDamage { get => _bulletDamage; set { _bulletDamage = value; } }
    public float bulletSize { get => _bulletSize; set { _bulletSize = value; } }
    public int bulletBounceCount { get => _bulletBounceCount; set { _bulletBounceCount = value; } }

    // 생성자
    public BulletSate(int v_cnt, float v_speed, float v_damage, float v_size, int _cnt)
    {
        this._bulletCount = v_cnt;
        this._bulletSpeed = v_speed;
        this._bulletDamage = v_damage;
        this._bulletSize = v_size;
        this._bulletBounceCount = _cnt;
    }

    public void F_SetField(BulletSate v_state) 
    {
        this._bulletCount = v_state._bulletCount;
        this._bulletSpeed = v_state._bulletSpeed;
        this._bulletDamage = v_state._bulletDamage;
        this._bulletSize = v_state._bulletSize;
        this._bulletBounceCount = v_state._bulletBounceCount;
    }
}

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
        _bulletSate = new BulletSate(1, 3f, 1f, 1f, 1);

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
        // ##TODO : 효과적용 코드 짜기 
    }
}