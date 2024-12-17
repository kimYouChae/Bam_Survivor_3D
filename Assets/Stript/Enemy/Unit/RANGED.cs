using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using static MELLE;

public class RANGES : Unit
{
    [SerializeField]
    private const float BulletForce = 2f;

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

            // marker ���� 
            Collider[] _coll = Physics.OverlapSphere(_unit.hitPosition.position, _unit.unitSearchRadious, LayerManager.Instance.markerLayer);

            // ���� : �÷��̾�- unit���⺤��
            Vector3 _dir;

            if (_coll.Length > 0 )
                _dir = _coll[0].transform.position - _unit.transform.position;
                        
            else
                _dir = PlayerManager.Instance.markerHeadTrasform.position - _unit.transform.position;

            _obj.GetComponent<Rigidbody>().AddForce(_dir * BulletForce, ForceMode.Impulse );

        }
    }

}
