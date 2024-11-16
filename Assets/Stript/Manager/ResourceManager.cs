using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    [Header("===Sprite===")]
    [SerializeField]
    private string _skillCardFolderName = "SkillCardName";
    [SerializeField]
    private Dictionary<string, Sprite> DICT_skillCardSprite;
    [SerializeField]
    private Sprite _defaultSprite;

    protected override void Singleton_Awake()
    {

    }

    private void Start()
    {
        // skill card µÒº≈≥ ∏Æ √ ±‚»≠
        F_InitSkillCardSprite();

    }

    // skill card µÒº≈≥ ∏Æ √ ±‚»≠
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

    // stringø° µ˚∏• sprite return
    public Sprite F_ReturnSkillNameToSprite(string v_name) 
    {
        if( DICT_skillCardSprite.ContainsKey(v_name))
            return DICT_skillCardSprite[v_name];

        return _defaultSprite;
    }


}
