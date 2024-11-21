using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkillCardManager : Singleton<SkillCardManager>
{
    [Header("===Script====")]
    [SerializeField]
    private SkillCardCsvImporter _skillCsvImporter;

    [Header("===RandomCard===")]
    [SerializeField]
    private List<SkillCard> _randomSelectCard; // 랜덤으로 선택 된 카드 

    [Header("===SkillCard 중복===")]
    [SerializeField]
    private Dictionary<string, int> DICT_skillcardToCount;      // 스킬카드 이름, 획득 count

    // 프로퍼티
    public SkillCardCsvImporter SkillCardDatabase => _skillCsvImporter;

    protected override void Singleton_Awake()
    {
        
    }

    private void Start()
    {
        _randomSelectCard       = new List<SkillCard>();
        DICT_skillcardToCount   = new Dictionary<string, int>();
    }

    // 랜덤으로 N 장 선택된 카드 List 반환
    public List<SkillCard> F_FinalSelectCard()
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
        if (_randomRatio >= 1f - GameManager.Instance.LegaryRatio)
        {
            // legend tier의 리스트 안에서 랜덤값 
            F_SelectCardInList(CardTier.Legendary, _skillCsvImporter.tierBySkillCard[CardTier.Legendary]);
        }
        // epic
        else if (_randomRatio >= 1f - GameManager.Instance.EpicRatio)
        {
            // epic tier의 리스트 안에서 랜덤값 
            F_SelectCardInList(CardTier.Epic, _skillCsvImporter.tierBySkillCard[CardTier.Epic]);
        }
        // rare
        else if (_randomRatio >= 1f - GameManager.Instance.RareRatio)
        {
            // rare tier의 리스트 안에서 랜덤값 
            F_SelectCardInList(CardTier.Rare, _skillCsvImporter.tierBySkillCard[CardTier.Rare]);
        }
        // common
        else if (_randomRatio >= 1f - GameManager.Instance.CommonRatio)
        {
            // common tier의 리스트 안에서 랜덤값 
            F_SelectCardInList(CardTier.Common, _skillCsvImporter.tierBySkillCard[CardTier.Common]);
        }
        // basic
        else
        {
            // basic tier의 리스트 안에서 랜덤값 
            F_SelectCardInList(CardTier.Basic, _skillCsvImporter.tierBySkillCard[CardTier.Basic]);
        }
    }

    private void F_SelectCardInList(CardTier v_tier, List<SkillCard> v_cardList)
    {
        // list 내에서 랜덤값 구하기
        int _rand = Random.Range(0, v_cardList.Count);

        // ##TODO : list내에서도 같은 수 나오면 안됨 


        // 랜덤으로 선택된 카드를 리스트에 추가 
        _randomSelectCard.Add(v_cardList[_rand]);
    }

    // 스킬카드에 따라 효과적용
    public void F_applyEffectBySkillcard(SkillCard _skillCard)  
    {
        // skillname으로 중복검사
        F_CheckTuplication(_skillCard.skillCardName);

        // skillcard의 cardAbility에 따라 스크립트 넘겨주는게 다름 
        switch (_skillCard.cardAbility) 
        {
            // PlayerManager에 접근
            case CardAbility.PlayerState:
                PlayerManager.Instance.F_ApplyCardEffect(_skillCard);
                break;

            // MarkerShieldController에 접근 
            case CardAbility.Shield:
                ShieldManager.Instance.F_ApplyShieldEffect(_skillCard);
                break;
            
            // MarkerBulletController에 접근
            case CardAbility.BulletShoot:
                PlayerManager.Instance.markerBulletController.F_ApplyBulletEffect(_skillCard);
                break;
            
            // MarkerExplosionConteroller에 접근
            case CardAbility.BulletExplosion:
                PlayerManager.Instance.markerExplosionConteroller.F_ApplyExplosionEffect(_skillCard);
                break;
        }
    }

    // name으로 중복 체크
    private void F_CheckTuplication(string _name) 
    {
        // 포함 x 
        if (!DICT_skillcardToCount.ContainsKey(_name))
        {
            DICT_skillcardToCount.Add(_name, 1);
            Debug.Log(_name + "처음획득! ");
            return;
        }
        else 
        {
            DICT_skillcardToCount[_name]++;
            Debug.Log(_name + " : " + DICT_skillcardToCount[_name] + "번째 획득 ");
        }
    }

    // name 으로 획득 count return
    public int F_SkillAcquiCount(string name) 
    {
        // 포함안되어있으면 -> 0  
        if (!DICT_skillcardToCount.ContainsKey(name))
            return 0;

        return DICT_skillcardToCount[name];
    }

}
