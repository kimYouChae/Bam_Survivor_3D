using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 카드 티어
public enum CardTier 
{
    Legendary,      // 빨강색
    Epic,           // 노랑색
    Rare,           // 보라색
    Common,         // 초록색
    Basic           // 회색
}

// 카드 능력치 
public enum CardAbility 
{
    Shield,             // 쉴드형
    PlayerState,        // 플레이어 스탯 형
    BulletShoot,        // 총알 발사
    BulletExplosion     // 총알 폭발 (unit에게 닿였을 때)
}

// Unit FSM
public enum UNIT_STATE
{
    Idle,               // 기본
    Tracking,           // 추적
    Attack,             // 공격
    Die                 // 사망 
}

// Unit Type
public enum Unt_Type
{
    MELLE,
    RANGED,
    MIDDLE,
    BOSS
}

// 쉴드 효과 ( csv의 _classSpritName과 같아야함 )
public enum Shield_Effect 
{
    Default,
    Rare_ShieldExpention,
    Epic_BloodSiphon,
    Legend_Supernova,
    Legend_BombShield
}

#region Shield State
[System.Serializable]
public class ShieldState 
{
    [SerializeField] private float _shieldSize;         // 쉴드 크기

    // 프로퍼티 
    public float shieldSize { get => _shieldSize; set { _shieldSize = value; } }

    // 생성자
    public ShieldState(float v_size) 
    {
        this._shieldSize = v_size;
    }

}


#endregion


#region Bullet

// bullet state
[System.Serializable]
public class BulletSate
{
    [SerializeField] private int _bulletCount;          // 한번에 생성하는 총알 갯수
    [SerializeField] private float _bulletSpeed;        // 총알 속도
    [SerializeField] private float _bulletDamage;       // 총알 데미지 
    [SerializeField] private float _bulletSize;         // 총알 크기 
    [SerializeField] private int _bulletBounceCount;    // 총알 튕기는 횟수 

    // 프로퍼티 
    public int bulletCount { get => _bulletCount; set { _bulletCount = value; } }
    public float bulletSpeed { get => _bulletSpeed; set { _bulletSpeed = value; } }
    public float bulletDamage { get => _bulletDamage; set { _bulletDamage = value; } }
    public float bulletSize { get => _bulletSize; set { _bulletSize = value; } }
    public int bulletBounceCount { get => _bulletBounceCount; set { _bulletBounceCount = value; } }

    // 생성자
    public BulletSate(int _bulletCnt, float _bulletSpeed, float _bulletDamage, float _bulletSize, int _bulletBounceCnt)
    {
        this._bulletCount       = _bulletCnt;
        this._bulletSpeed       = _bulletSpeed;
        this._bulletDamage      = _bulletDamage;
        this._bulletSize        = _bulletSize;
        this._bulletBounceCount = _bulletBounceCnt;
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
#endregion

#region Unit

public class UnitState 
{
    [SerializeField] private float _unitHp;           // hp
    [SerializeField] private float _unitSpeed;        // speed 
    [SerializeField] private float _unitAttackTime;   // 공격 지속시간
    [SerializeField] private float _unitTimeStamp;    // 공격 시 0으로 초기화                                                 
    [SerializeField] private float _searchRadious;    // 플레이어 감지 범위 

    // 프로퍼티 
    public float UnitHp { get { return _unitHp; } set { _unitHp = value; } }
    public float UnitSpeed { get { return _unitSpeed; } set { _unitSpeed = value; } }
    public float UnitAttackTime
    { get { return _unitAttackTime; } set { _unitAttackTime = value; } }
    public float UnitTimeStamp
    { get { return _unitTimeStamp; } set { _unitTimeStamp = value; } }

    public float searchRadious
    { get { return _searchRadious; } set { _searchRadious = value; } }

    // 생성자
    public UnitState(float h ,float s , float a , float t , float r) 
    {
        this._unitHp = h;
        this._unitSpeed = s;
        this._unitAttackTime = a;
        this._unitTimeStamp = t;
        this._searchRadious = r;
    }
}
#endregion

#region Player

[System.Serializable]
public class MarkerState
{
    // 스탯
    [SerializeField] private string _markerName;                // name
    [SerializeField] private float _markerHp;                   // marker Hp
    [SerializeField] private float _markerMaxHp;                // marker max hp
    [SerializeField] private float _markerMoveSpeed;            // marker speed
    [SerializeField] private float _defence;                    // 방어력
    [SerializeField] private float _naturalRecovery;            // 자연 회복량

    // 범위
    [SerializeField] private float _markerSearchRadious;        // unit 탐색 범위
    [SerializeField] private float _magnetSearchRadious;        // 자석 범위 
    
    // 쿨타임
    [SerializeField] private float _markerShieldCoolTime;       // marker 쉴드 쿨타임 
    [SerializeField] private float _markerBulletShootCoolTime;        // 총알 발사 쿨타임 
    [SerializeField] private float _recoveryCoolTime;           // 자연 회복량 쿨타임

    // 프로퍼티
    public float markerHp { get => _markerHp; set { _markerHp = value; } }
    public float markerMaxHp { get => _markerMaxHp; set { _markerMaxHp = value; } }
    public float markerMoveSpeed { get => _markerMoveSpeed; set { _markerMoveSpeed = value; } }
    public float defence { get => _defence; set { _defence = value; } }
    public float markerShieldCoolTime { get => _markerShieldCoolTime; set { _markerShieldCoolTime = value; } }
    public float markerBulletShootCoolTime { get => _markerBulletShootCoolTime; set { _markerBulletShootCoolTime = value; } }
    public float markerSearchRadious { get => _markerSearchRadious; set { _markerSearchRadious = value; } }
    public float naturalRecoery { get => _naturalRecovery; set { _naturalRecovery = value; } }
    public float recoveryCoolTime { get => _recoveryCoolTime; set { _recoveryCoolTime = value; } }
    public float magnetSearchRadious { get => _magnetSearchRadious; set { _magnetSearchRadious = value; } }

    // 생성자 
    public void F_SetMarkerState(string _name, float _hp, float _maxHp, float _speed, float _defence , float _search , float _magnet
        , float _sCoolTime, float _bCoolTime, float _recovery , float _rCoolTime)
    {
        this._markerName            = _name;
        this._markerHp              = _hp;
        this._markerMaxHp           = _maxHp;
        this._markerMoveSpeed       = _speed;
        this._defence               = _defence;
        this._markerSearchRadious   = _search;
        this._markerShieldCoolTime  = _sCoolTime;
        this._markerBulletShootCoolTime   = _bCoolTime;
        this._naturalRecovery       = _recovery;
        this._recoveryCoolTime      = _rCoolTime;
    }
}
#endregion

#region A * 

public class Node
{
    public int x;
    public int y;
    public int distance;    // 현재까지의 거리
    public int weight;    // compare위한 가중치 d(거리) +  h (휴리스틱 )

    public Node(int v_x, int v_y, int v_dis, int v_we)
    {
        this.x          = v_x;
        this.y          = v_y;
        this.distance   = v_dis;
        this.weight     = v_we;
    }
}

public class Pos
{
    public int x, y;

    public Pos() { }

    public Pos(int y, int x)
    {
        this.y = y;
        this.x = x;
    }
}
#endregion