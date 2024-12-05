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

    // �ִϸ����� - �ĸ������� bool
    public void F_SetAnimatorBoolByState(UnitAnimationType _paramaterName, bool _flag)
    {
        try
        {
            //Debug.Log(_paramaterName + " �� ���º�ȭ : " + _flag);

            // setbool�� true
            _unitAnimator.SetBool(_paramaterName.ToString(), _flag);

            // �ϴ� ����� 
            _currAniState = _paramaterName;

        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString() + " / " + "Animation Bool �� �������� �ʽ��ϴ� ");
        }
    }

    // �ִϸ����� - �Ķ������ trigger
    public void F_SetAnimatorTriggerByState(UnitAnimationType _paramaterName)
    {
        try
        {
            //Debug.Log(_paramaterName + " �� ���º�ȭ : " + _flag);

            // setbool�� true
            _unitAnimator.SetTrigger(_paramaterName.ToString());

            // �ϴ� ����� 
            _currAniState = _paramaterName;

        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString() + " / " + "Animation Trigger�� �������� �ʽ��ϴ� ");
        }

    }

    // ���� animation �����ϴ��� check
    public void F_UnitAttackAnimationCheck()
    {
        // ex) Pig_BasicAttack �̷������� (State �̸�)
        string _nowAnimationString = _unit.unitName + "_" + _currAniState.ToString();

        // �ִϸ��̼� ������ flag�� true��
        // StartCoroutine(IE_AnimationPlaying(_nowAnimationString));
    }

    private IEnumerator IE_AnimationPlaying(string _aniState)
    {
        //Debug.Log("�ڷ�ƾ����!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

        // ���� animation������� ���
        yield return new WaitUntil(() => _unitAnimator.GetCurrentAnimatorStateInfo(0).IsName(_aniState));

        //Debug.Log("���� �ִϸ��̼� ����" + _unitAnimator.GetCurrentAnimatorStateInfo(0).length);

        // ���� �ִϸ��̼��� �ð����� 
        float _time = _unitAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(_time);

        // animation �����ϰ� ���� tracking���� ���º�ȭ
        // F_ChangeState(UNIT_STATE.Tracking);

        // ����
        yield break;

    }

}
