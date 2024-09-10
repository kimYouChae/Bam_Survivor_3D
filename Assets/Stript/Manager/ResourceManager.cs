using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;

    [SerializeField]
    private List<Sprite> _skillCardSprite;  // ��ų ī�� ��������Ʈ

    // ������Ƽ 
    public List<Sprite> skillCardSprites => _skillCardSprite;

    private void Awake()
    {
        instance = this;
    }
}
