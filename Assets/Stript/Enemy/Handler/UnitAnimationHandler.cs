using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class UnitAnimationHandler 
{
    [Header("===Unit===")]
    private Unit _unit;

    [Header("===Animator===")]
    [SerializeField] private UnitAnimationType _currAniState;
    [SerializeField] private Animator _unitAnimator;
    [SerializeField] private bool _animationEndFlag;

    public UnitAnimationHandler(Unit _unit) 
    {
        this._unit = _unit;
        _unitAnimator = _unit.GetComponent<Animator>();
    }

    public void F_UpdateAnimationType(UnitAnimationType _type) 
    {
        this._currAniState = _type;
    }

    // 애니메이터 - 파리미터의 bool
    public void F_SetAnimatorBoolByState(UnitAnimationType _paramaterName, bool _flag)
    {
        try
        {
            //Debug.Log(_paramaterName + " 의 상태변화 : " + _flag);

            // setbool을 true
            _unitAnimator.SetBool(_paramaterName.ToString(), _flag);

            // type 저장  
            _currAniState = _paramaterName;

            /// 애니메이션 flag (끝나기 전 이니까)
            _animationEndFlag = false;

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

            // type 저장   
            _currAniState = _paramaterName;

            /// 애니메이션 flag (끝나기 전 이니까)
            _animationEndFlag = false;

        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString() + " / " + "Animation Trigger가 존재하지 않습니다 ");
        }

    }

    // 애니메이션 끝나면 flag를 true로
    public IEnumerator IE_AnimationPlaying()
    {
        // ex) Pig_BasicAttack 이런식으로 (State 이름)
        string _aniState = _unit.unitName + "_" + _currAniState.ToString();

        //Debug.Log(_aniState);

        /*
        while (true) 
        {
            // 애니메이션이 실행되고 있으면 
            if (_unitAnimator.GetCurrentAnimatorStateInfo(0).IsName(_aniState) == true) 
            {
                Debug.Log("실행되고 있습니다 : " + _aniState );

                // 플레이타임
                float _playTime = _unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                // 끝나면 
                if (_playTime >= 0.9f)
                    break;
            }

            // 매프레임 
            yield return null;
        }
        */
        
        // 현재 animation실행까지 대기
        yield return new WaitUntil(() => _unitAnimator.GetCurrentAnimatorStateInfo(0).IsName(_aniState));

        // 현재 애니메이션의 시간까지 
        float _time = _unitAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(_time);

        //Debug.Log("====애니메이션 끝 : " + _aniState + "=====");

        // 애니메이션 끝
        _animationEndFlag = true;

        // 종료
        yield break;
    }

    public bool AnimationEndFlag => _animationEndFlag;

}
