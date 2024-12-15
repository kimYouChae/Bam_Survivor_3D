using System.Collections;
using System.Collections.Generic;
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
        F_AddToAttackStrtegy(new RANGED_Attack());
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
    internal class RANGED_Attack : IAttackStrategy
    {
        public void IS_Attack(Unit _unit)
        {
            Debug.Log("RANGED�� Attack�� �մϴ�");

            // Attack ( Trigger )�ִϸ��̼� ����
            _unit.F_TriggerAnimation(UnitAnimationType.BasicAttack);

            // ���Ÿ� ����
            // ##TODO : pool���� get �ؾ��� 
            GameObject _obj = Instantiate(UnitManager.Instance.UnitBullet , _unit.hitPosition.position , Quaternion.identity);

            // ���� : �÷��̾�-unit���⺤��
            Collider[] _coll = Physics.OverlapSphere(_unit.hitPosition.position, 0.6f, LayerManager.Instance.markerLayer);
            
            //_obj.GetComponent<Rigidbody>().AddForce();


        }
    }

}
