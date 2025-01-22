using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoodsUi : MonoBehaviour
{
    [Header("===Amount Text===")]
    [SerializeField]
    private TextMeshProUGUI[] _goodsText;
    // enum 순서대로 [0]energy [1]gold [2]crystal

    private void Start()
    {
        // Ui 업데이트
        for (int i = 0; i < _goodsText.Length; i++)
        {
            GoodsType _currType = GoodsManager.Instance.F_IndexToType(i);

            _goodsText[i].text = GoodsManager.Instance.F_TypeToAmount(_currType).ToString();
        }
    }

    public void F_UpdateGoodsText(GoodsType _type) 
    {
        _goodsText[(int)_type].text = GoodsManager.Instance.F_TypeToAmount(_type).ToString();
    }

}
