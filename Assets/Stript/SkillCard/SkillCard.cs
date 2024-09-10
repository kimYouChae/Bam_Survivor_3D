using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public abstract class SkillCard
{
    [SerializeField] protected int _cardIndex;
    [SerializeField] protected CardTier _cardTier;
    [SerializeField] protected CardAbility _cardAbility;
    [SerializeField] protected string _cardName;
    [SerializeField] protected string _cardToolTip;

    // ������Ƽ
    public int cardIndex => _cardIndex;
    public CardTier cardTier => _cardTier;
    public CardAbility cardAbility => _cardAbility; 
    public string cardName => _cardName;
    public string cardToolTip => _cardToolTip;

    // ������
    public SkillCard() { }

    // �ʱ�ȭ �Լ�
    public void F_InitField( string[] v_str ) 
    {
        // 0. card idx
        this._cardIndex = int.Parse(v_str[0]);
        // 1. card tier ( string to enum )
        this._cardTier = (CardTier)Enum.Parse(typeof(CardTier), v_str[1]);
        // 2. card ability (string to enum)
        this._cardAbility = (CardAbility)Enum.Parse(typeof(CardAbility), v_str[2]);
        // 3. card name
        this._cardName = v_str[3];
        // 4. card tool tip
        this._cardToolTip = v_str[4];
    }

    // �� skillcard���� ����ȿ��
    public abstract void F_SkillcardEffect();
}
