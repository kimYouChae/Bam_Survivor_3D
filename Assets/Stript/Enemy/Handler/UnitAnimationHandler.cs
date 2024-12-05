using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitAnimationHandler 
{
    [Header("===Unit===")]
    private Unit _unit;

    [Header("===Animator===")]
    [SerializeField] protected UnitAnimationType _currAniState;
    [SerializeField] protected Animator _unitAnimator;
    [SerializeField] protected bool _animationEndFlag;

    public UnitAnimationHandler(Unit _unit) 
    {
        this._unit = _unit;
        _unitAnimator = _unit.GetComponent<Animator>();
    }

    // 애니메이터 - 파리미터의 bool
    public void F_SetAnimatorBoolByState(UnitAnimationType _paramaterName, bool _flag)
    {
        try
        {
            //Debug.Log(_paramaterName + " 의 상태변화 : " + _flag);

            // setbool을 true
            _unitAnimator.SetBool(_paramaterName.ToString(), _flag);

            // 일단 보기용 
            _currAniState = _paramaterName;

        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString() + " / " + "Animation Bool 이 존재하지 않습니다 ");
        }
    }

    // 애니메이터 - 파라미터의 trigger
    public void F_SetAnimatorTriggerByState(UnitAnimationType _paramaterName)
    {
        try
        {
            //Debug.Log(_paramaterName + " 의 상태변화 : " + _flag);

            // setbool을 true
            _unitAnimator.SetTrigger(_paramaterName.ToString());

            // 일단 보기용 
            _currAniState = _paramaterName;

        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString() + " / " + "Animation Trigger가 존재하지 않습니다 ");
        }

    }

    // 현재 animation 실행하는지 check
    public void F_UnitAttackAnimationCheck()
    {
        // ex) Pig_BasicAttack 이런식으로 (State 이름)
        string _nowAnimationString = _unit.unitName + "_" + _currAniState.ToString();

        // 애니메이션 끝나면 flag를 true로
        // StartCoroutine(IE_AnimationPlaying(_nowAnimationString));
    }

    private IEnumerator IE_AnimationPlaying(string _aniState)
    {
        //Debug.Log("코루틴실행!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

        // 현재 animation실행까지 대기
        yield return new WaitUntil(() => _unitAnimator.GetCurrentAnimatorStateInfo(0).IsName(_aniState));

        //Debug.Log("현재 애니메이션 길이" + _unitAnimator.GetCurrentAnimatorStateInfo(0).length);

        // 현재 애니메이션의 시간까지 
        float _time = _unitAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(_time);

        // animation 실행하고 나서 tracking으로 상태변화
        // F_ChangeState(UNIT_STATE.Tracking);

        // 종료
        yield break;

    }

}
