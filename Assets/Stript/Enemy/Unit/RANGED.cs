using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using static MELLE;

public class RANGES : Unit
{
    private void Awake()
    {
        // Awake�� �ʱ�1ȸ���� �����ȴ�
        // �ڵ鷯 �ʱ�ȭ
        F_InitHandlerSetting();

        // lifeCycle�� exist�� 
        _lifeCycle = LifeCycle.InitInstance;

        // Attack Interface ���� 
        F_AddToAttackStrtegy(UnitAnimationType.BasicAttack, new RANGED_Basic_Attack(UnitAnimationType.BasicAttack));
    }

    // ������ �� enter (pool���� on �� �� )
    private void OnEnable()
    {
        switch (_lifeCycle)
        {
            // Init�϶���
            case LifeCycle.InitInstance:
                _lifeCycle = LifeCycle.ExistingInstance;
                break;

            // �ʱ����x pool���� ���� �� on �ɶ���
            case LifeCycle.ExistingInstance:
                // ������� ���� 
                F_SettingCurrState(UNIT_STATE.Tracking);
                // FSM enter 
                F_StateEnter();
                break;
        }
    }

    // ������ �� (pool�� ���� off �� ��)
    private void OnDisable()
    {
        if (_lifeCycle != LifeCycle.ExistingInstance)
            return;

        // �̰����� �ʱ�ȭ  
        F_OnDisable();
    }

    private void Update()
    {
        // FSM excute 
        F_StateExcute();
    }

    public override UnitAnimationType F_returnAttackType()
    {
        return UnitAnimationType.BasicAttack;
    }
}
