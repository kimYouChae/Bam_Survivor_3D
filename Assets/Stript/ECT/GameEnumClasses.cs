using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Interface
public interface AttackStrategy
{
    void IS_Attack(Unit _unit);
}

public interface ITrackingHandler 
{
    IEnumerator IE_TrackinCorutine();
    void TH_EvaluateStateTransition();
}

public interface IAttackHandler 
{
    void AH_AddAttackList(UnitAnimationType _aniType , AttackStrategy attack);
    void AH_AttackExcutor();
}

#endregion

#region Enum


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

// 적 Type
public enum Unit_Type
{
    MELLE,
    RANGED,
    BOSS
}

// Animal Type
public enum Unit_Animal_Type 
{
    Rooster,
    Pig,
    Sheep,
    Chick,
    Duck,
    Hen,
    Donkey,
    Cow,
    Buffalo,
    RoosterBoss,
    SheepBoss,
    BuffaloBoss
}

// 쉴드 효과 ( csv의 _classSpritName과 같아야함 )
public enum Shield_Effect 
{
    Default,
    Epic_BloodSiphon,
    Legend_Supernova,
    Legend_HealingField
}

// 총알 폭발 효과 ( csv의 _classSpriteName과 같아야함 )
public enum Explosion_Effect
{
    Default,
    Rare_PoisionBullet,
    Rare_IceBullet
}

// 총알 폭발 시 Particle System 실행
public enum ParticleType
{
    BasicExposionVFX,           // 기본 폭발 vfx
    BasicPoisonVFX,             // 기본 독 vfx
    BasicIceVFX,                // 기본 ice vfx
    ReinPosionVFX,              // 강화 독 vfx
    ShieldEndVFX,               // 쉴드 끝날 때 vfx
    SupernovaVFX,               // 수퍼노바 vfx
    HealingEndVFX,              // 힐 끝날때 vfx 
}

// 오브젝트가 현재 씬에서 어떤 상태인지 
public enum LifeCycle 
{
    InitInstance,       // 초기 1회 생성
    ExistingInstance    // 이미 생성된 
}

// 인게임 작물
public enum CropsType 
{
    Rice,               // 벼
    Tomato,             // 토마토
    Carrot              // 당근 
}

public enum GoodsType 
{
    Crystal,
    Gold
}

// Unit 애니메이션
public enum UnitAnimationType 
{
    Tracking, 
    BasicAttack,
    RushAttack,
    ProjectileAttack,
    Die,
    Exit
}

//Unit Bullet Type
public enum UnitBulletType 
{
    RedApple,
    YellowApple
}

public enum AnimalType 
{
    Beaver,
    Kingfisher,
    SnappingTurtle,
    Swan,
    Crocodile
}

#endregion

#region Stage
[System.Serializable]
public class Stage
{
    [SerializeField] int    _stageIndex;        // 스테이지 순서
    [SerializeField] float  _stageMinites;      // 스테이지 진행 minute
    [SerializeField] int    _generateCount;     // 유닛 생성 횟수
    [SerializeField] List<Unit_Animal_Type> _generateUnitList;      // 생성 유닛 리스트 
    public int StageIndex { get => _stageIndex; set => _stageIndex = value; }
    public float StageMinites { get => _stageMinites; set => _stageMinites = value; }
    public int GenerateCount { get => _generateCount; set => _generateCount = value; }
    public List<Unit_Animal_Type> GenerateUnitList { get => _generateUnitList; set => _generateUnitList = value; }

    public Stage(string[] _value) 
    {
        // [0] : 스테이지 인덱스
        // [1] : 스테이지 진행 minute
        // [2] : 생성 횟수
        // [3] : 생성 유닛 리스트 , '-'로 구분 

        this._stageIndex    = int.Parse(_value[0]);
        this._stageMinites  = float.Parse(_value[1]);
        this._generateCount = int.Parse(_value[2]);

        string[] _unitList = _value[3].Split('-');
        _generateUnitList = new List<Unit_Animal_Type>();
        for (int i = 0; i < _unitList.Length; i++) 
        {
            Unit_Animal_Type _type = (Unit_Animal_Type)Enum.Parse(typeof(Unit_Animal_Type), _unitList[i]);
            _generateUnitList.Add(_type);
        }
    }
}

#endregion

#region Explosion State
[System.Serializable]
public class ExplosionState
{
    [SerializeField] private float _explosionRadious;       // 폭발 검출 범위

    // 프로퍼티
    public float explosionRadious => _explosionRadious;

    // 생성자
    public ExplosionState(float v_radious) 
    { 
        this._explosionRadious = v_radious;
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

[System.Serializable]
public class UnitState 
{
    [SerializeField] private Unit_Type _unitType;                   // 유닛 타입
    [SerializeField] private Unit_Animal_Type _animalType;          // animal 타입
    [SerializeField] private String _unitName;                      // 이름 
    [SerializeField] private float _unitHp;                         // hp
    [SerializeField] private float _unitMaxHp;                      // max hp 
    [SerializeField] private float _unitSpeed;                      // speed 
    [SerializeField] private float _unitAttackTime;                 // 공격 지속시간
    [SerializeField] private float _unitTimeStamp;                  // 공격 시 0으로 초기화                                                 
    [SerializeField] private float _searchRadious;                  // 플레이어 감지 범위 
    [SerializeField] private float _defencePower;                   // 방어력
    [SerializeField] private float _unitDamage;                     // 데미지 

    #region 프로퍼티
    
    public Unit_Type UnitType { get => _unitType; set => _unitType = value; }
    public Unit_Animal_Type AnimalType { get => _animalType; set => _animalType = value; }
    public string UnitName { get => _unitName; set => _unitName = value; }
    public float UnitHp { get => _unitHp; set => _unitHp = value; }
    public float UnitMaxHp { get => _unitMaxHp; set => _unitMaxHp = value; }
    
    public float UnitSpeed { get => _unitSpeed; set => _unitSpeed = value; }
    public float UnitAttackTime { get => _unitAttackTime; set => _unitAttackTime = value; }
    public float UnitTimeStamp { get => _unitTimeStamp; set => _unitTimeStamp = value; }
    public float SearchRadious { get => _searchRadious; set => _searchRadious = value; }
    public float DefencePower { get => _defencePower; set => _defencePower = value; }
    public float UnitDamage { get => _unitDamage; set => _unitDamage = value; }
    #endregion

    // 생성자
    public UnitState(UnitState _state) 
    {
        this._unitType          = _state.UnitType;
        this._animalType        = _state.AnimalType;
        this._unitName          = _state.UnitName;
        this._unitHp            = _state.UnitHp;
        this._unitMaxHp         = _state.UnitMaxHp;
        this._unitSpeed         = _state.UnitSpeed;
        this._unitAttackTime    = _state.UnitAttackTime;
        this._unitTimeStamp     = _state.UnitTimeStamp;
        this._searchRadious     = _state.SearchRadious;
        this._defencePower      = _state.DefencePower;
        this._unitDamage        = _state.UnitDamage;
        
    }

    public UnitState(String[] str) 
    {
        // [0] : unit type
        // [1] : animal Type
        // [2] : name
        // [3] : hp
        // [4] : speed
        // [5] : attackTime
        // [6] : timeStamp
        // [7] : radious
        // [8] : defence Power

        this._unitType          = (Unit_Type)Enum.Parse(typeof(Unit_Type), str[0]);
        this._animalType        = (Unit_Animal_Type)Enum.Parse(typeof(Unit_Animal_Type) , str[1]);
        this.UnitName          = str[2];
        this._unitHp            = float.Parse(str[3]);
        this._unitMaxHp         = _unitHp;
        this._unitSpeed         = float.Parse(str[4]);
        this._unitAttackTime    = float.Parse(str[5]);
        this._unitTimeStamp     = float.Parse(str[6]);
        this._searchRadious     = float.Parse(str[7]);
        this._defencePower      = float.Parse(str[8]);
        this._unitDamage        = float.Parse(str[9]);
    }

    // stage에 따른 state 변화
    public void F_StateByStage(float _stage) 
    {
        // hp, max hp 
        _unitMaxHp      += _stage;
        _unitHp         = _unitMaxHp;

        // damage 증가
        _unitDamage     += _stage;

        // 방어력 증가
        _defencePower   += _stage;
    }

}
#endregion

#region Player

[System.Serializable]
public class PlayerAnimalState
{
    // 스탯
    [SerializeField] private AnimalType _type;              // player 타입
    [SerializeField] private string _name;                  // name
    [SerializeField] private float _hp;                     // marker Hp
    [SerializeField] private float _maxHp;                  // marker max hp
    [SerializeField] private float _moveSpeed;              // marker speed
    [SerializeField] private float _damage;                 // 데미지 
    [SerializeField] private float _defence;                // 방어력
    [SerializeField] private float _naturalRecovery;        // 자연 회복량

    // 범위
    [SerializeField] private float _searchRadious;          // unit 탐색 범위
    [SerializeField] private float _magnetSearchRadious;    // 자석 범위 
    
    // 쿨타임
    [SerializeField] private float _shieldCoolTime;             // marker 쉴드 쿨타임 
    [SerializeField] private float _shootCoolTime;              // 총알 발사 쿨타임 
    [SerializeField] private float _recoveryCoolTime;           // 자연 회복량 쿨타임

    // 추가 스탯
    [SerializeField] private int _revivalCount;                 // 부활 횟수
    [SerializeField] private float _bonusExperience;            // 경험치 배율 
    [SerializeField] private float _luck;                       // 운

    #region 프로퍼티
    public AnimalType markerPlayerType { get => _type; }
    public string markerName { get => _name; }
    public float markerHp { get => _hp; set { _hp = value; } }
    public float markerMaxHp { get => _maxHp; set { _maxHp = value; } }
    public float markerMoveSpeed { get => _moveSpeed; set { _moveSpeed = value; } }
    public float defence { get => _defence; set { _defence = value; } }
    public float markerShieldCoolTime { get => _shieldCoolTime; set { _shieldCoolTime = value; } }
    public float markerBulletShootCoolTime { get => _shootCoolTime; set { _shootCoolTime = value; } }
    public float markerSearchRadious { get => _searchRadious; set { _searchRadious = value; } }
    public float naturalRecoery { get => _naturalRecovery; set { _naturalRecovery = value; } }
    public float recoveryCoolTime { get => _recoveryCoolTime; set { _recoveryCoolTime = value; } }
    public float magnetSearchRadious { get => _magnetSearchRadious; set { _magnetSearchRadious = value; } }
    #endregion

    // 생성자
    public PlayerAnimalState(string[] _value) 
    {
        this._type                  = (AnimalType)Enum.Parse(typeof(AnimalType), _value[0]);
        this._name                  = _value[1];
        this._hp                    = float.Parse(_value[2]);
        this._maxHp                 = float.Parse(_value[3]);
        this._moveSpeed             = float.Parse(_value[4]);
        this._damage                = float.Parse(_value[5]);
        this._defence               = float.Parse(_value[6]);
        this._searchRadious         = float.Parse(_value[7]);
        this._magnetSearchRadious   = float.Parse(_value[8]);
        this._shieldCoolTime        = float.Parse(_value[9]);
        this._shootCoolTime         = float.Parse(_value[10]);
        this._naturalRecovery       = float.Parse(_value[11]);
        this._recoveryCoolTime      = float.Parse(_value[12]);
        this._revivalCount          = int.Parse(_value[13]);
        this._bonusExperience       = float.Parse(_value[14]);
        this._luck                  = float.Parse(_value[15]);
    }

    // ##TODO : csv importer하고 지워야함 !
    public void F_SetMarkerState(string _name, float _hp, float _maxHp, float _speed, float _defence , float _search , float _magnet
        , float _sCoolTime, float _bCoolTime, float _recovery , float _rCoolTime)
    {
        this._name            = _name;
        this._hp              = _hp;
        this._maxHp           = _maxHp;
        this._moveSpeed       = _speed;
        this._defence               = _defence;
        this._searchRadious   = _search;
        this._shieldCoolTime  = _sCoolTime;
        this._shootCoolTime   = _bCoolTime;
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