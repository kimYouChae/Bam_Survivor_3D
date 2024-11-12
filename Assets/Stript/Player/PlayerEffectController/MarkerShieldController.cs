using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UIElements;

public class MarkerShieldController : MonoBehaviour
{
    /// <summary>
    /// ##TODO
    /// ���߿� Shieldmanager�� ��ũ��Ʈ �̸� �����ϰ� �̱������� ���� �ɵ� 
    /// </summary>

    [Header("===basic Shield Object===")]
    [SerializeField]
    private GameObject _basicShieldObject;                              // �⺻ ���� ������Ʈ

    [Header("===Shield State===")]
    [SerializeField] private ShieldState _shieldState;

    [Header("===�ߺ� �˻� Dictionary===")]
    private Dictionary< Shield_Effect, int > DICT_ShieldTOCount;
    // Shield Enum (key)�� �´� ���� int (value) 

    [Header("===Skill Effect Ratio===")]
    private const float BLOOD_SHIPON_RATIO  = 0.7f;
    private const float BLOOD_EXCUTION_LIMIT      = 15f;
    private const int   BLOOD_EXUTION_CNT   = 4;
    private const float SUPERNOVA_DAMAGE    = 10f;

    public delegate void del_ShieldCreate       (Marker _unitTrs);             

    // deligate ����
    public del_ShieldCreate del_shieldCreate;

    // effect�� �ش��ϴ� count�� return
    public int F_ReturnCountToDic(Shield_Effect _effect) 
    {
        if (!DICT_ShieldTOCount.ContainsKey(_effect))
            return 0;

        return DICT_ShieldTOCount[_effect];
    }

    public bool F_IsBloodExution() 
    {
        // count�� ������ true, �ƴϸ� false
        return DICT_ShieldTOCount[Shield_Effect.Epic_BloodSiphon] >= BLOOD_EXUTION_CNT;
    }

    public float supernovaDamage    => SUPERNOVA_DAMAGE;
    public float bloodShiponRatio   => BLOOD_SHIPON_RATIO;
    public int bloodExcutionCnt     => BLOOD_EXUTION_CNT;
    public float bloodExcutionLimit      => BLOOD_EXCUTION_LIMIT;


    private void Start()
    {
        // ##TODO : �ӽ� ����state ���� , ���߿� ���� ������ ĳ���� ���� �޸����� ?  
        _shieldState                = new ShieldState(3f);

        // ��ųʸ� �ʱ�ȭ
        F_InitDictionary();

        // ��������Ʈ�� �⺻ ���� use �߰�
        del_shieldCreate += F_BasicShieldUse;

    }

    private void F_InitDictionary() 
    {
        DICT_ShieldTOCount = new Dictionary<Shield_Effect, int>();

        Shield_Effect[] _effect = (Shield_Effect[])System.Enum.GetValues(typeof(Shield_Effect));

        for (int i = 0; i < _effect.Length; i++)
        {
            if (_effect[i] == Shield_Effect.Default)
                continue;

            // key�� ������ ���� ������ 
            if (!DICT_ShieldTOCount.ContainsKey(_effect[i]))
                DICT_ShieldTOCount.Add(_effect[i], 0);
        }
    }


    // ���� ī�� ȹ�� �� ����
    public void F_ApplyShieldEffect( SkillCard v_card ) 
    {
        // ��ųʸ��� skillcard �˻� 
        Shield_Effect _effect = F_cardEqualShieldEffect(v_card);

        // default�� return
        if (_effect == Shield_Effect.Default)
            return;

        // ��ųʸ� + 1
        DICT_ShieldTOCount[_effect] += 1;

        // ó�� ȹ��� delegate�� �߰�
        if (DICT_ShieldTOCount[_effect] == 1) 
        {
            del_shieldCreate += v_card.F_SkillcardEffect;
        }

    }

    // skill effect �ߺ� üũ 
    internal Shield_Effect F_cardEqualShieldEffect(SkillCard v_card)
    {
        // v_card�� _className������ ���� enum�� ã�� 
        Shield_Effect _myEffect = default;

        try 
        {
            // _myEffect�� _className�� ���� enum�� ���
            Enum.TryParse(v_card.classSpriteName , out _myEffect);
        }
        catch (Exception e) 
        {
            Debug.LogError(e.ToString());
        }

        // ���� ������ �ȵǰ� default�� ���������� ? -> return 
        if (_myEffect == default)
            return Shield_Effect.Default;

        return _myEffect;
        
    }

    // effect�� ���� pool���� ��������
    internal void F_GetBloodShieldToPool(Shield_Effect _effect, Marker _marker)
    {
        // ���� ������Ʈ Ǯ������ ��������
        GameObject _obj = ShieldPooling.instance.F_ShieldGet(_effect);

        _obj.transform.position = _marker.transform.position;   
    }

    // ���� ������ ���� 
    internal void F_UpdateShieldState(float ShieldSizePercent = 0f)
    {
        
    }

    internal void F_BasicShieldUse(Marker _marker) 
    {
        Debug.Log("BasocShieldUse �Լ� ����");
        F_GetBloodShieldToPool(Shield_Effect.Default , _marker);
    }

}
