using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsManager : Singleton<GoodsManager>
{
    [Header("===Goods===")]
    [SerializeField]
    private int _energy;         // ¿¡³ÊÁö
    [SerializeField]
    private int _gold;           // °ñµå
    [SerializeField]
    private int _crystal;        // Å©¸®½ºÅ»

    // ÇÁ·ÎÆÛÆ¼

    public int Gold { get => _gold; set => _gold = value; }

    public int Energy { get => _energy; set => _energy = value; }
    public int Crystal { get => _crystal; set => _crystal = value; }

    protected override void Singleton_Awake()
    {
        Energy = 0;
        Gold    = 0;
        Crystal = 0;
    }

    // grystal È¹µæ
    public void F_GetGoods(GoodsType _Type) 
    {
        // ##TODO : È¹µæ
    }
    
}
