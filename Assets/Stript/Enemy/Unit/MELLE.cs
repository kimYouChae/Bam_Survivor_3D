using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MELLE : Unit
{
    private void Awake()
    {
        // Awake는 초기1회에만 생성된다
        // 핸들러 초기화
        F_InitHandlerSetting();

        // lifeCycle을 exist로 
        _lifeCycle = LifeCycle.InitInstance;

        // Attack Interface 저장 
        F_AddToAttackStrtegy(new MELLE_Attack());
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

    internal class MELLE_Attack : IAttackStrategy
    {
        public void IS_Attack(Unit _unit)
        {
            Debug.Log("Melle가 Attack을 합니다");

            // Attack ( Trigger )애니메이션 실행
            _unit.F_TriggerAnimation(UnitAnimationType.BasicAttack);

            // 근접 공격 
            Collider[] _coll = Physics.OverlapSphere( _unit.hitPosition.position, _unit.unitSearchRadious , LayerManager.Instance.markerLayer);

            if (_coll.Length <= 0)
                return;

            // 감지되면
            foreach(Collider marker in _coll) 
            {
                //Debug.Log("MELLE_ATTACK이 실행되고 있습니다 . 타겟 : " + marker.gameObject.name);

                if (marker.GetComponent<Marker>() == null)
                    return;

                // 데미지 넣기 
                marker.GetComponent<Marker>().F_UpdateHP(_unit.unitDamage * (-1f));
            }
        }
    }

}
