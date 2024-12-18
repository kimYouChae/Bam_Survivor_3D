using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
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

    // �ִϸ����� - �ĸ������� bool
    public void F_SetAnimatorBoolByState(UnitAnimationType _paramaterName, bool _flag)
    {
        try
        {
            //Debug.Log(_paramaterName + " �� ���º�ȭ : " + _flag);

            // setbool�� true
            _unitAnimator.SetBool(_paramaterName.ToString(), _flag);

            // type ����  
            _currAniState = _paramaterName;

            /// �ִϸ��̼� flag (������ �� �̴ϱ�)
            _animationEndFlag = false;

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

            // type ����   
            _currAniState = _paramaterName;

            /// �ִϸ��̼� flag (������ �� �̴ϱ�)
            _animationEndFlag = false;

        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString() + " / " + "Animation Trigger�� �������� �ʽ��ϴ� ");
        }

    }

    // �ִϸ��̼� ������ flag�� true��
    public IEnumerator IE_AnimationPlaying()
    {
        // ex) Pig_BasicAttack �̷������� (State �̸�)
        string _aniState = _unit.unitName + "_" + _currAniState.ToString();

        //Debug.Log(_aniState);

        /*
        while (true) 
        {
            // �ִϸ��̼��� ����ǰ� ������ 
            if (_unitAnimator.GetCurrentAnimatorStateInfo(0).IsName(_aniState) == true) 
            {
                Debug.Log("����ǰ� �ֽ��ϴ� : " + _aniState );

                // �÷���Ÿ��
                float _playTime = _unitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                // ������ 
                if (_playTime >= 0.9f)
                    break;
            }

            // �������� 
            yield return null;
        }
        */
        
        // ���� animation������� ���
        yield return new WaitUntil(() => _unitAnimator.GetCurrentAnimatorStateInfo(0).IsName(_aniState));

        // ���� �ִϸ��̼��� �ð����� 
        float _time = _unitAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(_time);

        Debug.Log("====�ִϸ��̼� �� : " + _aniState + "=====");

        // �ִϸ��̼� ��
        _animationEndFlag = true;

        // ����
        yield break;
    }

    public bool AnimationEndFlag => _animationEndFlag;

}