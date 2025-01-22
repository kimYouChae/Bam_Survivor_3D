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
    private Dictionary<GoodsType, int> DICT_GoodsToAmount;  // ���� �� ȹ�淮

    [Header("===script===")]
    [SerializeField]
    private GoodsUi _goodsUi;

    // ������Ƽ
    public GoodsUi goodsUi => _goodsUi;
    // type�� amount 
    public int F_TypeToAmount(GoodsType _type) => DICT_GoodsToAmount[_type];
    // index�� goods Type
    public GoodsType F_IndexToType(int _idx) => _goodsType[_idx];

    protected override void Singleton_Awake()
    {
        _goodsType = (GoodsType[])Enum.GetValues(typeof(GoodsType));
        DICT_GoodsToAmount = new Dictionary<GoodsType, int>();

        for (int i = 0; i < _goodsType.Length; i++) 
        {
            DICT_GoodsToAmount.Add(_goodsType[i], 0 );
        }

        // ##TODO : �׽�Ʈ ������ add �س�����
        DICT_GoodsToAmount[GoodsType.Gold] = 2000;

    }

    // Goods ȹ�� 
    public void F_UpdateGoods(GoodsType _Type , int _amount) 
    {
        // goods ȹ�� of ��� 
        try 
        {
            DICT_GoodsToAmount[_Type] += _amount;
            
            // ���� 0 ���ϸ� 0���� �ʱ�ȭ
            if (DICT_GoodsToAmount[_Type] <= 0)
                DICT_GoodsToAmount[_Type] = 0;
        }
        catch(Exception e) 
        {
            Debug.Log(e);
        }
    }

    // ����� ��ȭ�� �ִ��� 
    public bool F_HaveEnoughMoney( GoodsType _Type , int _amount) 
    {
        // ������ȭ - �����ȭ�� 0 �̻��̸� true, �ƴϸ� false 
        return DICT_GoodsToAmount[_Type] - _amount >= 0;
    }
}
