using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsManager : Singleton<GoodsManager>
{
    [Header("===Goods===")]
    [SerializeField]
    private GoodsType[] _goodsType;
    [SerializeField]
    private Dictionary<GoodsType, int> DICT_GoodsToAmount;  // 굿즈 별 획득량

    [Header("===script===")]
    [SerializeField]
    private GoodsUi _goodsUi;

    // 프로퍼티
    public GoodsUi goodsUi => _goodsUi;
    // type별 amount 
    public int F_TypeToAmount(GoodsType _type) => DICT_GoodsToAmount[_type];
    // index별 goods Type
    public GoodsType F_IndexToType(int _idx) => _goodsType[_idx];

    protected override void Singleton_Awake()
    {
        _goodsType = (GoodsType[])Enum.GetValues(typeof(GoodsType));
        DICT_GoodsToAmount = new Dictionary<GoodsType, int>();

        for (int i = 0; i < _goodsType.Length; i++) 
        {
            DICT_GoodsToAmount.Add(_goodsType[i], 0 );
        }

        // ##TODO : 테스트 용으로 add 해놓은것
        DICT_GoodsToAmount[GoodsType.Gold] = 2000;

    }

    // Goods 획득 
    public void F_UpdateGoods(GoodsType _Type , int _amount) 
    {
        // goods 획득 of 사용 
        try 
        {
            DICT_GoodsToAmount[_Type] += _amount;
            
            // 만약 0 이하면 0으로 초기화
            if (DICT_GoodsToAmount[_Type] <= 0)
                DICT_GoodsToAmount[_Type] = 0;
        }
        catch(Exception e) 
        {
            Debug.Log(e);
        }
    }

    // 충분한 재화가 있는지 
    public bool F_HaveEnoughMoney( GoodsType _Type , int _amount) 
    {
        // 현재재화 - 사용재화가 0 이상이면 true, 아니면 false 
        return DICT_GoodsToAmount[_Type] - _amount >= 0;
    }
}
