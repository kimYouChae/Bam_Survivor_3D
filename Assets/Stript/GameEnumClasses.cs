using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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