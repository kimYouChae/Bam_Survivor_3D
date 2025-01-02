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


// ī�� Ƽ��
public enum CardTier 
{
    Legendary,      // ������
    Epic,           // �����
    Rare,           // �����
    Common,         // �ʷϻ�
    Basic           // ȸ��
}

// ī�� �ɷ�ġ 
public enum CardAbility 
{
    Shield,             // ������
    PlayerState,        // �÷��̾� ���� ��
    BulletShoot,        // �Ѿ� �߻�
    BulletExplosion     // �Ѿ� ���� (unit���� �꿴�� ��)
}

// Unit FSM
public enum UNIT_STATE
{
    Idle,               // �⺻
    Tracking,           // ����
    Attack,             // ����
    Die                 // ��� 
}

// �� Type
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

// ���� ȿ�� ( csv�� _classSpritName�� ���ƾ��� )
public enum Shield_Effect 
{
    Default,
    Epic_BloodSiphon,
    Legend_Supernova,
    Legend_HealingField
}

// �Ѿ� ���� ȿ�� ( csv�� _classSpriteName�� ���ƾ��� )
public enum Explosion_Effect
{
    Default,
    Rare_PoisionBullet,
    Rare_IceBullet
}

// �Ѿ� ���� �� Particle System ����
public enum ParticleState
{
    BasicExposionVFX,           // �⺻ ���� vfx
    BasicPoisonVFX,             // �⺻ �� vfx
    BasicIceVFX,                // �⺻ ice vfx
    ReinPosionVFX,              // ��ȭ �� vfx
    ShieldEndVFX,               // ���� ���� �� vfx
    SupernovaVFX,               // ���۳�� vfx
    HealingEndVFX,              // �� ������ vfx 
}

// ������Ʈ�� ���� ������ � �������� 
public enum LifeCycle 
{
    InitInstance,       // �ʱ� 1ȸ ����
    ExistingInstance    // �̹� ������ 
}

// �ΰ��� �۹�
public enum CropsType 
{
    Rice,               // ��
    Tomato,             // �丶��
    Carrot              // ��� 
}

public enum GoodsType 
{
    Crystal,
    Gold
}

// Unit �ִϸ��̼�
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
public enum UnitBullet 
{
    RedApple,
    YellowApple
}

#endregion

#region PropsBuilding
[System.Serializable]
public class Building 
{
    [SerializeField] private CropsType _propsType;
    [SerializeField] private string _buildingName;
    [SerializeField] private float _generateSecond;
    [SerializeField] private Sprite _propsSprite;

    public CropsType PropsType { get => _propsType; set => _propsType = value; }
    public string BuildingName { get => _buildingName; set => _buildingName = value; }
    public float GenerateSecond { get => _generateSecond; set => _generateSecond = value; }
    public Sprite PropsSprite { get => _propsSprite; set => _propsSprite = value; }

    public Building(string[] value) 
    {
        // [0] : propsType
        // [1] : buildingName
        // [2] : generateSecond
        // [3] : Sprite

        this._propsType = (CropsType)Enum.Parse(typeof(CropsType), value[0]);
        this._buildingName = value[1];
        this._generateSecond = float.Parse(value[2]);
        this._propsSprite = ResourceManager.Instance.propsSprite(_propsType);
    }
}

#endregion

#region Stage
[System.Serializable]
public class Stage
{
    [SerializeField] int    _stageIndex;        // �������� ����
    [SerializeField] float  _stageMinites;      // �������� ���� minute
    [SerializeField] int    _generateCount;     // ���� ���� Ƚ��
    [SerializeField] List<Unit_Animal_Type> _generateUnitList;      // ���� ���� ����Ʈ 
    public int StageIndex { get => _stageIndex; set => _stageIndex = value; }
    public float StageMinites { get => _stageMinites; set => _stageMinites = value; }
    public int GenerateCount { get => _generateCount; set => _generateCount = value; }
    public List<Unit_Animal_Type> GenerateUnitList { get => _generateUnitList; set => _generateUnitList = value; }

    public Stage(string[] _value) 
    {
        // [0] : �������� �ε���
        // [1] : �������� ���� minute
        // [2] : ���� Ƚ��
        // [3] : ���� ���� ����Ʈ , '-'�� ���� 

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
    [SerializeField] private float _explosionRadious;       // ���� ���� ����

    // ������Ƽ
    public float explosionRadious => _explosionRadious;

    // ������
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
    [SerializeField] private int _bulletCount;          // �ѹ��� �����ϴ� �Ѿ� ����
    [SerializeField] private float _bulletSpeed;        // �Ѿ� �ӵ�
    [SerializeField] private float _bulletDamage;       // �Ѿ� ������ 
    [SerializeField] private float _bulletSize;         // �Ѿ� ũ�� 
    [SerializeField] private int _bulletBounceCount;    // �Ѿ� ƨ��� Ƚ�� 

    // ������Ƽ 
    public int bulletCount { get => _bulletCount; set { _bulletCount = value; } }
    public float bulletSpeed { get => _bulletSpeed; set { _bulletSpeed = value; } }
    public float bulletDamage { get => _bulletDamage; set { _bulletDamage = value; } }
    public float bulletSize { get => _bulletSize; set { _bulletSize = value; } }
    public int bulletBounceCount { get => _bulletBounceCount; set { _bulletBounceCount = value; } }

    // ������
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
    [SerializeField] private Unit_Type _unitType;                   // ���� Ÿ��
    [SerializeField] private Unit_Animal_Type _animalType;          // animal Ÿ��
    [SerializeField] private String _unitName;                      // �̸� 
    [SerializeField] private float _unitHp;                         // hp
    [SerializeField] private float _unitMaxHp;                      // max hp 
    [SerializeField] private float _unitSpeed;                      // speed 
    [SerializeField] private float _unitAttackTime;                 // ���� ���ӽð�
    [SerializeField] private float _unitTimeStamp;                  // ���� �� 0���� �ʱ�ȭ                                                 
    [SerializeField] private float _searchRadious;                  // �÷��̾� ���� ���� 
    [SerializeField] private float _defencePower;                   // ����
    [SerializeField] private float _unitDamage;                     // ������ 

    // ������Ƽ 
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


    // ������
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

    // stage�� ���� state ��ȭ
    public void F_StateByStage(float _stage) 
    {
        // hp, max hp 
        _unitMaxHp      += _stage;
        _unitHp         = _unitMaxHp;

        // damage ����
        _unitDamage     += _stage;

        // ���� ����
        _defencePower   += _stage;
    }

}
#endregion

#region Player

[System.Serializable]
public class MarkerState
{
    // ����
    [SerializeField] private string _markerName;                // name
    [SerializeField] private float _markerHp;                   // marker Hp
    [SerializeField] private float _markerMaxHp;                // marker max hp
    [SerializeField] private float _markerMoveSpeed;            // marker speed
    [SerializeField] private float _defence;                    // ����
    [SerializeField] private float _naturalRecovery;            // �ڿ� ȸ����

    // ����
    [SerializeField] private float _markerSearchRadious;        // unit Ž�� ����
    [SerializeField] private float _magnetSearchRadious;        // �ڼ� ���� 
    
    // ��Ÿ��
    [SerializeField] private float _markerShieldCoolTime;       // marker ���� ��Ÿ�� 
    [SerializeField] private float _markerBulletShootCoolTime;  // �Ѿ� �߻� ��Ÿ�� 
    [SerializeField] private float _recoveryCoolTime;           // �ڿ� ȸ���� ��Ÿ��

    // ������Ƽ
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

    // ������ 
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
    public int distance;    // ��������� �Ÿ�
    public int weight;    // compare���� ����ġ d(�Ÿ�) +  h (�޸���ƽ )

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