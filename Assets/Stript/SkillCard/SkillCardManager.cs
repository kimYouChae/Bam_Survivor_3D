using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum CardTier // ī�� Ƽ��
{
    Legendary,      // ������
    Epic,           // �����
    Rare,           // �����
    Common,         // �ʷϻ�
    Basic           // ȸ��
}
public enum CardAbility // ī�� �ɷ�ġ 
{
    Shield,         // ������
    PlayerState,    // �÷��̾� ���� ��
    BulletShoot,    // �Ѿ� �߻�
    BulletExplosion // �Ѿ� ���� (unit���� �꿴�� ��)
}

public class SkillCardManager : MonoBehaviour
{
    public static SkillCardManager instance;

    [Header("===Script====")]
    [SerializeField]
    private SkillCardDatabase _skillDatabase;

    [Header("===RandomCard===")]
    [SerializeField]
    private List<Tuple<CardTier, SkillCard>> _randomSelectCard; // �������� ���� �� ī�� 

    // ������Ƽ
    public SkillCardDatabase SkillCardDatabase => _skillDatabase;

    private void Awake()
    {
        instance = this;
        _randomSelectCard = new List<Tuple<CardTier, SkillCard>>();
    }

    // �������� N �� ���õ� ī�� List ��ȯ
    public List<Tuple<CardTier, SkillCard>> F_FinalSelectCard()
    {
        // �ʱ�ȭ 
        _randomSelectCard.Clear();

        for (int i = 0; i < 5; i++)
        {
            // ratio�� ���� ���� ī�� ����
            F_SelectCardTierAccorRatio();
        }

        // ���������� ������ ���� ������ ī�帮��Ʈ return
        return _randomSelectCard;
    }

    // ������ ���� ī�� select 
    private void F_SelectCardTierAccorRatio()
    {
        // ## TODO : gameManager�� ratio ���� tier�� list ����
        float _randomRatio = Random.Range(0, 1f);

        //Debug.Log(_randomRatio);

        // legend 
        if (_randomRatio >= 1f - GameManager.instance.LegaryRatio)
        {
            // legend tier�� ����Ʈ �ȿ��� ������ 
            F_SelectCardInList(CardTier.Legendary, _skillDatabase.tierBySkillCard[CardTier.Legendary]);
        }
        // epic
        else if (_randomRatio >= 1f - GameManager.instance.EpicRatio)
        {
            // epic tier�� ����Ʈ �ȿ��� ������ 
            F_SelectCardInList(CardTier.Epic, _skillDatabase.tierBySkillCard[CardTier.Epic]);
        }
        // rare
        else if (_randomRatio >= 1f - GameManager.instance.RareRatio)
        {
            // rare tier�� ����Ʈ �ȿ��� ������ 
            F_SelectCardInList(CardTier.Rare, _skillDatabase.tierBySkillCard[CardTier.Rare]);
        }
        // common
        else if (_randomRatio >= 1f - GameManager.instance.CommonRatio)
        {
            // common tier�� ����Ʈ �ȿ��� ������ 
            F_SelectCardInList(CardTier.Common, _skillDatabase.tierBySkillCard[CardTier.Common]);
        }
        // basic
        else
        {
            // basic tier�� ����Ʈ �ȿ��� ������ 
            F_SelectCardInList(CardTier.Basic, _skillDatabase.tierBySkillCard[CardTier.Basic]);
        }
    }

    private void F_SelectCardInList(CardTier v_tier, List<SkillCard> v_cardList)
    {
        // list ������ ������ ���ϱ�
        int _rand = Random.Range(0, v_cardList.Count);

        // ##TODO : list�������� ���� �� ������ �ȵ� 


        // �������� ���õ� ī�带 ����Ʈ�� �߰� 
        _randomSelectCard.Add(new Tuple<CardTier, SkillCard>(v_tier, v_cardList[_rand]));
    }

    // ��ųī�忡 ���� ȿ������
    public void F_applyEffectBySkillcard(Tuple<CardTier , SkillCard> v_selectCard )  
    {
        CardTier _cardTier = v_selectCard.Item1;
        SkillCard skillCard = v_selectCard.Item2;

        // skillcard�� cardAbility�� ���� ��ũ��Ʈ �Ѱ��ִ°� �ٸ� 
        switch (skillCard.cardAbility) 
        {
            // PlayerManager�� ����
            case CardAbility.PlayerState:
                PlayerManager.instance.F_ApplyCardEffect(skillCard);
                break;

            // MarkerShieldController�� ���� 
            case CardAbility.Shield:
                PlayerManager.instance.markerShieldController.F_ApplyShieldEffect(skillCard);
                break;
            
            // MarkerBulletController�� ����
            case CardAbility.BulletShoot:
                PlayerManager.instance.markerBulletController.F_ApplyBulletEffect(skillCard);
                break;
            
            // MarkerExplosionConteroller�� ����
            case CardAbility.BulletExplosion:
                PlayerManager.instance.markerExplosionConteroller.F_ApplyExplosionEffect(skillCard);
                break;
        }
    }
}
