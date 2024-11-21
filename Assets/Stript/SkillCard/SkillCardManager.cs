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
    private List<SkillCard> _randomSelectCard; // �������� ���� �� ī�� 

    [Header("===SkillCard �ߺ�===")]
    [SerializeField]
    private Dictionary<string, int> DICT_skillcardToCount;      // ��ųī�� �̸�, ȹ�� count

    // ������Ƽ
    public SkillCardCsvImporter SkillCardDatabase => _skillCsvImporter;

    protected override void Singleton_Awake()
    {
        
    }

    private void Start()
    {
        _randomSelectCard       = new List<SkillCard>();
        DICT_skillcardToCount   = new Dictionary<string, int>();
    }

    // �������� N �� ���õ� ī�� List ��ȯ
    public List<SkillCard> F_FinalSelectCard()
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
        if (_randomRatio >= 1f - GameManager.Instance.LegaryRatio)
        {
            // legend tier�� ����Ʈ �ȿ��� ������ 
            F_SelectCardInList(CardTier.Legendary, _skillCsvImporter.tierBySkillCard[CardTier.Legendary]);
        }
        // epic
        else if (_randomRatio >= 1f - GameManager.Instance.EpicRatio)
        {
            // epic tier�� ����Ʈ �ȿ��� ������ 
            F_SelectCardInList(CardTier.Epic, _skillCsvImporter.tierBySkillCard[CardTier.Epic]);
        }
        // rare
        else if (_randomRatio >= 1f - GameManager.Instance.RareRatio)
        {
            // rare tier�� ����Ʈ �ȿ��� ������ 
            F_SelectCardInList(CardTier.Rare, _skillCsvImporter.tierBySkillCard[CardTier.Rare]);
        }
        // common
        else if (_randomRatio >= 1f - GameManager.Instance.CommonRatio)
        {
            // common tier�� ����Ʈ �ȿ��� ������ 
            F_SelectCardInList(CardTier.Common, _skillCsvImporter.tierBySkillCard[CardTier.Common]);
        }
        // basic
        else
        {
            // basic tier�� ����Ʈ �ȿ��� ������ 
            F_SelectCardInList(CardTier.Basic, _skillCsvImporter.tierBySkillCard[CardTier.Basic]);
        }
    }

    private void F_SelectCardInList(CardTier v_tier, List<SkillCard> v_cardList)
    {
        // list ������ ������ ���ϱ�
        int _rand = Random.Range(0, v_cardList.Count);

        // ##TODO : list�������� ���� �� ������ �ȵ� 


        // �������� ���õ� ī�带 ����Ʈ�� �߰� 
        _randomSelectCard.Add(v_cardList[_rand]);
    }

    // ��ųī�忡 ���� ȿ������
    public void F_applyEffectBySkillcard(SkillCard _skillCard)  
    {
        // skillname���� �ߺ��˻�
        F_CheckTuplication(_skillCard.skillCardName);

        // skillcard�� cardAbility�� ���� ��ũ��Ʈ �Ѱ��ִ°� �ٸ� 
        switch (_skillCard.cardAbility) 
        {
            // PlayerManager�� ����
            case CardAbility.PlayerState:
                PlayerManager.Instance.F_ApplyCardEffect(_skillCard);
                break;

            // MarkerShieldController�� ���� 
            case CardAbility.Shield:
                ShieldManager.Instance.F_ApplyShieldEffect(_skillCard);
                break;
            
            // MarkerBulletController�� ����
            case CardAbility.BulletShoot:
                PlayerManager.Instance.markerBulletController.F_ApplyBulletEffect(_skillCard);
                break;
            
            // MarkerExplosionConteroller�� ����
            case CardAbility.BulletExplosion:
                PlayerManager.Instance.markerExplosionConteroller.F_ApplyExplosionEffect(_skillCard);
                break;
        }
    }

    // name���� �ߺ� üũ
    private void F_CheckTuplication(string _name) 
    {
        // ���� x 
        if (!DICT_skillcardToCount.ContainsKey(_name))
        {
            DICT_skillcardToCount.Add(_name, 1);
            Debug.Log(_name + "ó��ȹ��! ");
            return;
        }
        else 
        {
            DICT_skillcardToCount[_name]++;
            Debug.Log(_name + " : " + DICT_skillcardToCount[_name] + "��° ȹ�� ");
        }
    }

    // name ���� ȹ�� count return
    public int F_SkillAcquiCount(string name) 
    {
        // ���ԾȵǾ������� -> 0  
        if (!DICT_skillcardToCount.ContainsKey(name))
            return 0;

        return DICT_skillcardToCount[name];
    }

}
