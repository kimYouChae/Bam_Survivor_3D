using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    [Header("===Forder Name===")]
    [SerializeField]
    private string _skillCardFolderName = "SkillCardName";
    
    [Header("=== Skillcard Sprite===")]
    [SerializeField]
    private Dictionary<string, Sprite> DICT_skillCardSprite;        // 스킬 이름별 스프라이트 (skillCard Csv의 classSpriteName과 같음)

    [Header("=== Card Tier / Ability ===")]
    [SerializeField]
    private List<Sprite> _cardBackground;           // 카드 background
    [SerializeField]
    private List<Sprite> _cardTierSprite;           // 카드 티어 sprite
    [SerializeField]
    private List<Sprite> _cardIconSprite;           // 카드 아이콘 sprite
    [SerializeField]
    private Sprite _cardStartSprite;                // 카드 별 아이콘

    [Header("=== Props Building ===")]
    [SerializeField]
    private List<Sprite> _propsSprite;              // props 작물별 sprite

    [Header("=== default Sprite ===")]
    [SerializeField]
    private Sprite _defaultSprite;

    // Tier에 해당하는 Background Return
    public Sprite cardBackGround(CardTier _tier) => _cardBackground[(int)_tier];

    // Tier에 해당하는 스프라이트 return 
    public Sprite cardTierSprite(CardTier _tier) => _cardTierSprite[(int)_tier];
    // Ability에 해당하는 스프라이트 return
    public Sprite cardIconSprite(CardAbility _abili) => _cardIconSprite[(int)_abili];
    // 카드 별 icon return
    public Sprite cardStarSprite => _cardStartSprite;
    // props 에 해당하는 스프라이트 return
    public Sprite propsSprite(CropsType _pro) => _propsSprite[(int)_pro]; 

    protected override void Singleton_Awake()
    {

    }

    private void Start()
    {
        // skill card 딕셔너리 초기화
        F_InitSkillCardSprite();

    }

    // skill card 딕셔너리 초기화
    private void F_InitSkillCardSprite() 
    {
        DICT_skillCardSprite = new Dictionary<string, Sprite>();

        Sprite[] sprite = Resources.LoadAll<Sprite>(_skillCardFolderName);

        for (int i = 0; i < sprite.Length; i++) 
        {
            //Debug.Log(sprite[i].name);
            if (!DICT_skillCardSprite.ContainsKey(sprite[i].name)) 
            {
                DICT_skillCardSprite.Add(sprite[i].name , sprite[i]) ;
            }
        
        }
    }

    // string에 따른 sprite return
    public Sprite F_NameToCardSprite(string v_name) 
    {
        if( DICT_skillCardSprite.ContainsKey(v_name))
            return DICT_skillCardSprite[v_name];

        return _defaultSprite;
    }


}
