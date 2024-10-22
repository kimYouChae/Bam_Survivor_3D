using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public abstract class SkillCard
{
    [SerializeField] protected CardTier _cardTier;
    [SerializeField] protected CardAbility _cardAbility;
    [SerializeField] protected string _classSpritName;          // Ŭ���� ����, ��������Ʈ �̸� 
    [SerializeField] protected string _skillCardName;           // ui�� ǥ�õ� �̸�
    [SerializeField] protected string _cardToolTip;

    //������Ƽ
    public CardTier cardTier => _cardTier;
    public CardAbility cardAbility => _cardAbility; 
    public string classSpriteName => _classSpritName;
    public string skillCardName => _skillCardName;
    public string cardToolTip => _cardToolTip;

    // ������
    public SkillCard() { }

    // �ʱ�ȭ �Լ�
    public void F_InitField( string[] v_str ) 
    {
        // 0. card tier ( string to enum )
        this._cardTier = (CardTier)Enum.Parse(typeof(CardTier), v_str[0]);
        
        // 1. card ability (string to enum)
        this._cardAbility = (CardAbility)Enum.Parse(typeof(CardAbility), v_str[1]);
        
        // 2. script Name
        this._classSpritName = v_str[2];

        // 3. skillName
        this._skillCardName = v_str[3];

        // 4. card tool tip
        this._cardToolTip = v_str[4];
    }

    // �� skillcard���� ����ȿ��
    public virtual void F_SkillcardEffect() { }
    public virtual void F_SkillcardEffect(Marker _marker, float _size) { }

}
