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
    private Dictionary<string, Sprite> DICT_skillCardSprite;        // ��ų �̸��� ��������Ʈ (skillCard Csv�� classSpriteName�� ����)

    [Header("=== Card Tier / Ability ===")]
    [SerializeField]
    private List<Sprite> _cardBackground;           // ī�� background
    [SerializeField]
    private List<Sprite> _cardTierSprite;           // ī�� Ƽ�� sprite
    [SerializeField]
    private List<Sprite> _cardIconSprite;           // ī�� ������ sprite
    [SerializeField]
    private Sprite _cardStartSprite;                // ī�� �� ������

    [Header("=== Props Building ===")]
    [SerializeField]
    private List<Sprite> _propsSprite;              // props �۹��� sprite

    [Header("=== default Sprite ===")]
    [SerializeField]
    private Sprite _defaultSprite;

    // Tier�� �ش��ϴ� Background Return
    public Sprite cardBackGround(CardTier _tier) => _cardBackground[(int)_tier];

    // Tier�� �ش��ϴ� ��������Ʈ return 
    public Sprite cardTierSprite(CardTier _tier) => _cardTierSprite[(int)_tier];
    // Ability�� �ش��ϴ� ��������Ʈ return
    public Sprite cardIconSprite(CardAbility _abili) => _cardIconSprite[(int)_abili];
    // ī�� �� icon return
    public Sprite cardStarSprite => _cardStartSprite;
    // props �� �ش��ϴ� ��������Ʈ return
    public Sprite propsSprite(CropsType _pro) => _propsSprite[(int)_pro]; 

    protected override void Singleton_Awake()
    {

    }

    private void Start()
    {
        // skill card ��ųʸ� �ʱ�ȭ
        F_InitSkillCardSprite();

    }

    // skill card ��ųʸ� �ʱ�ȭ
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

    // string�� ���� sprite return
    public Sprite F_NameToCardSprite(string v_name) 
    {
        if( DICT_skillCardSprite.ContainsKey(v_name))
            return DICT_skillCardSprite[v_name];

        return _defaultSprite;
    }


}
