using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum CardTier // 카드 티어
{
    Legendary,      // 빨강색
    Epic,           // 노랑색
    Rare,           // 보라색
    Common,         // 초록색
    Basic           // 회색
}
public enum CardAbility // 카드 능력치 
{
    Shield,         // 쉴드형
    PlayerState,    // 플레이어 스탯 형
    BulletShoot,    // 총알 발사
    BulletExplosion // 총알 폭발 (unit에게 닿였을 때)
}

public class SkillCardManager : MonoBehaviour
{
    public static SkillCardManager instance;

    [Header("===Script====")]
    [SerializeField]
    private SkillCardDatabase _skillDatabase;

    [Header("===RandomCard===")]
    [SerializeField]
    private List<Tuple<CardTier, SkillCard>> _randomSelectCard; // 랜덤으로 선택 된 카드 

    // 프로퍼티
    public SkillCardDatabase SkillCardDatabase => _skillDatabase;

    private void Awake()
    {
        instance = this;
        _randomSelectCard = new List<Tuple<CardTier, SkillCard>>();
    }

    // 랜덤으로 N 장 선택된 카드 List 반환
    public List<Tuple<CardTier, SkillCard>> F_FinalSelectCard()
    {
        // 초기화 
        _randomSelectCard.Clear();

        for (int i = 0; i < 5; i++)
        {
            // ratio에 따른 랜덤 카드 셀렉
            F_SelectCardTierAccorRatio();
        }

        // 최종적으로 비율에 따라 셀렉된 카드리스트 return
        return _randomSelectCard;
    }

    // 비율에 따른 카드 select 
    private void F_SelectCardTierAccorRatio()
    {
        // ## TODO : gameManager의 ratio 따라서 tier별 list 선택
        float _randomRatio = Random.Range(0, 1f);

        //Debug.Log(_randomRatio);

        // legend 
        if (_randomRatio >= 1f - GameManager.instance.LegaryRatio)
        {
            // legend tier의 리스트 안에서 랜덤값 
            F_SelectCardInList(CardTier.Legendary, _skillDatabase.tierBySkillCard[CardTier.Legendary]);
        }
        // epic
        else if (_randomRatio >= 1f - GameManager.instance.EpicRatio)
        {
            // epic tier의 리스트 안에서 랜덤값 
            F_SelectCardInList(CardTier.Epic, _skillDatabase.tierBySkillCard[CardTier.Epic]);
        }
        // rare
        else if (_randomRatio >= 1f - GameManager.instance.RareRatio)
        {
            // rare tier의 리스트 안에서 랜덤값 
            F_SelectCardInList(CardTier.Rare, _skillDatabase.tierBySkillCard[CardTier.Rare]);
        }
        // common
        else if (_randomRatio >= 1f - GameManager.instance.CommonRatio)
        {
            // common tier의 리스트 안에서 랜덤값 
            F_SelectCardInList(CardTier.Common, _skillDatabase.tierBySkillCard[CardTier.Common]);
        }
        // basic
        else
        {
            // basic tier의 리스트 안에서 랜덤값 
            F_SelectCardInList(CardTier.Basic, _skillDatabase.tierBySkillCard[CardTier.Basic]);
        }
    }

    private void F_SelectCardInList(CardTier v_tier, List<SkillCard> v_cardList)
    {
        // list 내에서 랜덤값 구하기
        int _rand = Random.Range(0, v_cardList.Count);

        // ##TODO : list내에서도 같은 수 나오면 안됨 


        // 랜덤으로 선택된 카드를 리스트에 추가 
        _randomSelectCard.Add(new Tuple<CardTier, SkillCard>(v_tier, v_cardList[_rand]));
    }

    // 스킬카드에 따라 효과적용
    public void F_applyEffectBySkillcard(Tuple<CardTier , SkillCard> v_selectCard )  
    {
        CardTier _cardTier = v_selectCard.Item1;
        SkillCard skillCard = v_selectCard.Item2;

        // skillcard의 cardAbility에 따라 스크립트 넘겨주는게 다름 
        switch (skillCard.cardAbility) 
        {
            // PlayerManager에 접근
            case CardAbility.PlayerState:
                PlayerManager.instance.F_ApplyCardEffect(skillCard);
                break;

            // MarkerShieldController에 접근 
            case CardAbility.Shield:
                PlayerManager.instance.markerShieldController.F_ApplyShieldEffect(skillCard);
                break;
            
            // MarkerBulletController에 접근
            case CardAbility.BulletShoot:
                PlayerManager.instance.markerBulletController.F_ApplyBulletEffect(skillCard);
                break;
            
            // MarkerExplosionConteroller에 접근
            case CardAbility.BulletExplosion:
                PlayerManager.instance.markerExplosionConteroller.F_ApplyExplosionEffect(skillCard);
                break;
        }
    }
}
