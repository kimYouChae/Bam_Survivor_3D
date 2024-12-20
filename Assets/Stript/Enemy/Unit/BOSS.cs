
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BOSS : Unit
{
    [SerializeField]
    private int _attackIndexer = 0;

    private void Awake()
    {
        // Awake�� �ʱ�1ȸ���� �����ȴ�
        // �ڵ鷯 �ʱ�ȭ
        F_InitHandlerSetting();

        // lifeCycle�� exist�� 
        _lifeCycle = LifeCycle.InitInstance;

        // Attack Interface ���� 
        F_AddToAttackStrtegy(UnitAnimationType.BasicAttack, new Boss_Basic_Attack(UnitAnimationType.BasicAttack));
        F_AddToAttackStrtegy(UnitAnimationType.RushAttack, new Boss_Rush_Attack(UnitAnimationType.RushAttack));
        F_AddToAttackStrtegy(UnitAnimationType.ProjectileAttack, new Boss_Projectile_Attack(UnitAnimationType.ProjectileAttack));
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
        _attackIndexer++;
        if (_attackIndexer >= 3)
            _attackIndexer = 0;

        switch (_attackIndexer) 
        {
            case 0:
                // rush
                return UnitAnimationType.RushAttack;
            case 1:
                // ����ü
                return UnitAnimationType.ProjectileAttack;
            case 2:
                return UnitAnimationType.BasicAttack;
        }

        // ����
        return UnitAnimationType.BasicAttack;
    }

    #region Boss_Rush_Attack Ŭ���� 

    public override void F_BossChangeSpeed() 
    {
        float _oriSpeed = _unitState.UnitSpeed;
        float _fastSpeed = _oriSpeed * 5;

        StartCoroutine(IE_ChangeSpeed(_oriSpeed , _fastSpeed));
    }

    private IEnumerator IE_ChangeSpeed(float _ori , float _fast) 
    {
        _unitState.UnitSpeed = _fast;

        yield return new WaitForSeconds(1f);

        _unitState.UnitSpeed = _ori;

        // ���� 
        yield break;
    }

    #endregion

}
