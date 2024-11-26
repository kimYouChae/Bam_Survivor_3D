using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsManager : Singleton<GoodsManager>
{
    [Header("===Goods===")]
    [SerializeField]
    private int _energy;         // ������
    [SerializeField]
    private int _gold;           // ���
    [SerializeField]
    private int _crystal;        // ũ����Ż

    // ������Ƽ

    public int Gold { get => _gold; set => _gold = value; }

    public int Energy { get => _energy; set => _energy = value; }
    public int Crystal { get => _crystal; set => _crystal = value; }

    protected override void Singleton_Awake()
    {
        Energy = 0;
        Gold    = 0;
        Crystal = 0;
    }

    
}
