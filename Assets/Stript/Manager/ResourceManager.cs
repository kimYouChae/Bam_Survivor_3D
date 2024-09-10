using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;

    [SerializeField]
    private List<Sprite> _skillCardSprite;  // 스킬 카드 스프라이트

    // 프로퍼티 
    public List<Sprite> skillCardSprites => _skillCardSprite;

    private void Awake()
    {
        instance = this;
    }
}
