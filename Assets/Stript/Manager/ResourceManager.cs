using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;

    [SerializeField]
    private string _skillCardFolderName = "SkillCardName";
    [SerializeField]
    private Dictionary<string, Sprite> _skillCardSprite;
    [SerializeField]
    private Sprite _defaultSprite;

    // «¡∑Œ∆€∆º 
    public Dictionary<string, Sprite> skillCardSprite => _skillCardSprite;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // skill card µÒº≈≥ ∏Æ √ ±‚»≠
        F_InitSkillCardSprite();
    }

    // skill card µÒº≈≥ ∏Æ √ ±‚»≠
    private void F_InitSkillCardSprite() 
    {
        _skillCardSprite = new Dictionary<string, Sprite>();

        Sprite[] sprite = Resources.LoadAll<Sprite>(_skillCardFolderName);

        for (int i = 0; i < sprite.Length; i++) 
        {
            //Debug.Log(sprite[i].name);
            if (!_skillCardSprite.ContainsKey(sprite[i].name)) 
            {
                _skillCardSprite.Add(sprite[i].name , sprite[i]) ;
            }
        
        }
    }

    // stringø° µ˚∏• sprite return
    public Sprite F_ReturnSkillNameToSprite(string v_name) 
    {
        if( _skillCardSprite.ContainsKey(v_name))
            return _skillCardSprite[v_name];

        return _defaultSprite;
    }
    
}
