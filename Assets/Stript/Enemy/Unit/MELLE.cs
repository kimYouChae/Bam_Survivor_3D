using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MELLE : Unit
{
    private void Awake()
    {
        // Awake�� �ʱ�1ȸ���� �����ȴ�
        // �ڵ鷯 �ʱ�ȭ
        F_InitHandlerSetting();

        // lifeCycle�� exist�� 
        _lifeCycle = LifeCycle.InitInstance;

        // Attack Interface ���� 
        F_AddToAttackStrtegy(new MELLE_Attack());
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

    internal class MELLE_Attack : IAttackStrategy
    {
        public void IS_Attack(Unit _unit)
        {
            Debug.Log("Melle�� Attack�� �մϴ�");

            // Attack ( Trigger )�ִϸ��̼� ����
            _unit.F_TriggerAnimation(UnitAnimationType.BasicAttack);

            // ���� ���� 
            Collider[] _coll = Physics.OverlapSphere( _unit.hitPosition.position, _unit.unitSearchRadious , LayerManager.Instance.markerLayer);

            if (_coll.Length <= 0)
                return;

            // �����Ǹ�
            foreach(Collider marker in _coll) 
            {
                //Debug.Log("MELLE_ATTACK�� ����ǰ� �ֽ��ϴ� . Ÿ�� : " + marker.gameObject.name);

                if (marker.GetComponent<Marker>() == null)
                    return;

                // ������ �ֱ� 
                marker.GetComponent<Marker>().F_UpdateHP(_unit.unitDamage * (-1f));
            }
        }
    }

}
