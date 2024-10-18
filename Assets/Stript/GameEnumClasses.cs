using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardTier // ī�� Ƽ��
{
    Legendary,      // ������
    Epic,           // �����
    Rare,           // �����
    Common,         // �ʷϻ�
    Basic           // ȸ��
}
public enum CardAbility // ī�� �ɷ�ġ 
{
    Shield,         // ������
    PlayerState,    // �÷��̾� ���� ��
    BulletShoot,    // �Ѿ� �߻�
    BulletExplosion // �Ѿ� ���� (unit���� �꿴�� ��)
}

public enum UNIT_STATE
{
    Idle,
    Tracking,
    Attack,
    Die
}

public enum Unt_Type
{
    MELLE,
    RANGED,
    MIDDLE,
    BOSS
}

#region Unit

public class UnitState 
{
    [SerializeField] private float _unitHp;           // hp
    [SerializeField] private float _unitSpeed;        // speed 
    [SerializeField] private float _unitAttackTime;   // ���� ���ӽð�
    [SerializeField] private float _unitTimeStamp;    // ���� �� 0���� �ʱ�ȭ                                                 
    [SerializeField] private float _searchRadious;    // �÷��̾� ���� ���� 

    // ������Ƽ 
    public float UnitHp { get { return _unitHp; } set { _unitHp = value; } }
    public float UnitSpeed { get { return _unitSpeed; } set { _unitSpeed = value; } }
    public float UnitAttackTime
    { get { return _unitAttackTime; } set { _unitAttackTime = value; } }
    public float UnitTimeStamp
    { get { return _unitTimeStamp; } set { _unitTimeStamp = value; } }

    public float searchRadious
    { get { return _searchRadious; } set { _searchRadious = value; } }

    // ������
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
    [SerializeField] private float _markerBulletShootCoolTime;        // �Ѿ� �߻� ��Ÿ�� 
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