using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Apple.ReplayKit;

public class Basic_Unit :  Unit 
{
    private void Awake()
    {
        // ���� �ʱ�ȭ 
        F_InitUnitUnitState();
        
        // FSM ���� 
        F_InitUnitState(this);

        // FSM enter 
        F_CurrStateEnter();

    }

    private void Update()
    {
        // FSM excute 
        F_CurrStateExcute();

        // Ray �׸��� 
        F_DrawLine();
    }

    protected override void F_InitUnitUnitState()
    {
        this._unitHp = 10;
        this._unitSpeed = 3f;
        this._unitAttackTime = 2f;
        this._searchRadious = 1.5f;
        this._unitTimeStamp = 0;
    }

    public override void F_UnitAttatk()
    {
        _unitTimeStamp += Time.deltaTime;

        if(_unitTimeStamp < _unitAttackTime)
        {
            // ##TODO : �� ���ݵ��� �߰� 
        }
        else if(_unitTimeStamp >= _unitAttackTime )
        {
            // tracking���� ���º�ȭ
            F_ChangeState(UNIT_STATE.Tracking);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(gameObject.transform.position, this._searchRadious);
    }



}
