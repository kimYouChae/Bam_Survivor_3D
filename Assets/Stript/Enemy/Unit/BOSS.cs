
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BOSS : Unit
{
    [SerializeField]
    private int _attackIndexer = 0;

    private void Awake()
    {
        // Awake는 초기1회에만 생성된다
        // 핸들러 초기화
        F_InitHandlerSetting();

        // lifeCycle을 exist로 
        _lifeCycle = LifeCycle.InitInstance;

        // Attack Interface 저장 
        F_AddToAttackStrtegy(UnitAnimationType.BasicAttack, new Boss_Basic_Attack(UnitAnimationType.BasicAttack));
        F_AddToAttackStrtegy(UnitAnimationType.RushAttack, new Boss_Rush_Attack(UnitAnimationType.RushAttack));
        F_AddToAttackStrtegy(UnitAnimationType.ProjectileAttack, new Boss_Projectile_Attack(UnitAnimationType.ProjectileAttack));
    }

    // 켜졌을 때 enter (pool에서 on 될 때 )
    private void OnEnable()
    {
        switch (_lifeCycle)
        {
            // Init일때만
            case LifeCycle.InitInstance:
                _lifeCycle = LifeCycle.ExistingInstance;
                break;

            // 초기생성x pool에서 꺼낸 후 on 될때만
            case LifeCycle.ExistingInstance:
                // 현재상태 지정 
                F_SettingCurrState(UNIT_STATE.Tracking);
                // FSM enter 
                F_StateEnter();
                break;
        }
    }

    // 꺼졌을 때 (pool에 들어가서 off 될 때)
    private void OnDisable()
    {
        if (_lifeCycle != LifeCycle.ExistingInstance)
            return;

        // 이것저것 초기화  
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
                // 투사체
                return UnitAnimationType.ProjectileAttack;
            case 2:
                return UnitAnimationType.BasicAttack;
        }

        // 예외
        return UnitAnimationType.BasicAttack;
    }

    #region Boss_Rush_Attack 클래스 

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

        // 종료 
        yield break;
    }

    #endregion

}
