using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Linq;

public class MarkerShieldController : MonoBehaviour
{

    [Header("===basic Shield Object===")]
    [SerializeField]
    private GameObject _basicShieldObject;                              // �⺻ ���� ������Ʈ

    [Header("===Shield State===")]
    [SerializeField] private ShieldState _shieldState;

    [Header("===�ߺ� �˻� Dictionary===")]
    private Dictionary< Shield_Effect, int> DICT_ShieldTOCount;
    // Shield Enum (key)�� �´� ���� int (value) 

    [Header("===Skill Effect Ratio===")]
    [SerializeField] const  float   BLOOD_SHIPON_RATIO      = 0.7f;     // ���� ���� 
    [SerializeField] const  float   BLOOD_EXCUTION_RATIO    = 5f;       // ##TODO �ӽ� : ���� 5f���ϸ� ó��
    [SerializeField] const  int     BOMB_GENERATE_RATIO     = 2;        // ��ź ���� ���� 

    public delegate void del_MarkerShield(Marker _unitTrs, float _size);

    // deligate ����
    public del_MarkerShield del_markerShieldUse;

    private void Start()
    {
        // ##TODO : �ӽ� ����state ���� , ���߿� ���� ������ ĳ���� ���� �޸����� ?  
        _shieldState = new ShieldState(3f);

        // ��ųʸ� �ʱ�ȭ
        F_InitDictionary();

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

    // marker���� ����
    public void F_StartShieldAction(Marker _marker) 
    {
        // �ڷ�ƾ ���� �� �����ϸ� ������ marker�� �ڷ�ƾ ������  
        // StopAllCoroutines();
        StartCoroutine(IE_Test(_marker));
    }


    // ## Test : ���� ���ư��� ���� 
    private IEnumerator IE_Test( Marker _marker ) 
    {
        // ##TODO : ���� pool���� �������� 
        GameObject _shieldIns               = Instantiate(_basicShieldObject, _marker.gameObject.transform);
        _shieldIns.transform.localPosition  = Vector3.zero;
        _shieldIns.transform.localScale     = new Vector3(0.1f, 0.1f, 0.1f);

        while (true) 
        {
            // ũ�� Ű��� 
            _shieldIns.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
            _shieldIns.transform.position   = _marker.gameObject.transform.position;

            // ũ�Ⱑ max�� �Ǹ� ���� 
            if (_shieldIns.transform.localScale.x >= _shieldState.shieldSize
                && _shieldIns.transform.localScale.y >= _shieldState.shieldSize)
                break;


            // �����ð� ��ٸ��� 
            yield return new WaitForSeconds(0.1f);
        }

        // ���� ��������Ʈ (ȿ�� ����) ����
        try 
        {
            del_markerShieldUse(_marker , _shieldState.shieldSize );
        }
        catch (Exception ex) 
        {
            Debug.LogError(ex);
        }

        // �����ð��Ŀ� ���� 
        yield return new WaitForSeconds(0.2f);

        // ##TODO : ���� pool�� ������ 
        Destroy(_shieldIns);

        // �ڷ�ƾ ����
        yield break;
    }

    // ���� ī�� ȹ�� �� ����
    public void F_ApplyShieldEffect( SkillCard v_card ) 
    {
        // ��ųʸ��� skillcard �˻� 
        F_DictionaryInt(v_card);

        // �߰��� SkillCard�� �°� ��������Ʈ�� �޼��� �߰� 
        // Blood Shipon : �ʱ� 1ȸ ȹ��
        if (DICT_ShieldTOCount[Shield_Effect.Epic_BloodSiphon] == 1) 
        {
            // Blood : �ʱ� 1ȸ�� ��������Ʈ�� �߰�
            // 1~3ȸ������ F_BloodShiponEffect()������ count�� ������ �߰�
            // 4ȸ���� ó���߰�
            del_markerShieldUse += v_card.F_SkillcardEffect;
        }
        // Blood Shipon : 4ȸ ȹ��
        if (DICT_ShieldTOCount[Shield_Effect.Epic_BloodSiphon] == 4) 
        {
            // TODO : blood ���� �ڵ� �����ؾ��� 
            // ���� ����
            del_markerShieldUse -= F_BloodShiponEffect;
            // ó��ȿ�� �߰�
            del_markerShieldUse += F_Test;
        }

    }

    // skill effect �ߺ� üũ 
    public void F_DictionaryInt(SkillCard v_card)
    {
        // �ʱ�ȭ �ȵǾ������� �ʱ�ȭ
        if (DICT_ShieldTOCount == null)
            DICT_ShieldTOCount = new Dictionary<Shield_Effect, int>();

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
        { 
            Debug.LogError("Shield effect cannot be default");
            return;
        }

        // card�� ������ �ȵǾ������� ?
        if (!DICT_ShieldTOCount.ContainsKey(_myEffect))
        {
            DICT_ShieldTOCount.Add(_myEffect, 0);
        }
        // ������ �Ǿ������� ?
        else
        {
            DICT_ShieldTOCount[_myEffect] += 1;
        }

    }

    // Shield Sate ������Ʈ 
    public void F_UpdateShieldState( float ShieldSizePercent = 0 ) 
    { 
        _shieldState.shieldSize += _shieldState.shieldSize * ShieldSizePercent;
    }

    // Epic_BloodShiphon Effect : ������ unit �� ����
    public void F_BloodShiponEffect(Marker _marker , float _size) 
    {
        // Unit �˻� 
        Collider[] _unitColliderList = Physics.OverlapSphere( _marker.gameObject.transform.position , _size , UnitManager.Instance.unitLayerInt);

        // �����ȿ� ������ x 
        if (_unitColliderList.Length <= 0)
            return;

        // �����ȿ� ������ �����Ǹ�
        foreach(Collider _coll in _unitColliderList) 
        {
            // ��� * ȹ�� count ��ŭ 
            float _bloodAmount = BLOOD_SHIPON_RATIO * DICT_ShieldTOCount[Shield_Effect.Epic_BloodSiphon];
                
            // marker State hp ���� 
            PlayerManager.instance.F_UpdateIndiMarkerHP(_marker , _bloodAmount);

            // unit�� hp ���� 
            try 
            {
                _coll.GetComponent<Unit>().F_GetDamage(_bloodAmount);
            }
            catch (Exception e) 
            {
                Debug.LogError(e);
            }

        }
        
    }

    // ##TODO : �ӽ� Blood�� 4�� �̻� �Ծ��� �� �� ó�� ȿ�� �߰� 
    private void F_Test( Marker _marker , float _size ) 
    {
        // Unit �˻� 
        Collider[] _unitColliderList = Physics.OverlapSphere(_marker.gameObject.transform.position, _size, UnitManager.Instance.unitLayerInt);

        // �����ȿ� ������ x 
        if (_unitColliderList.Length <= 0)
            return;

        // ó���� unit ���� ( Linq :  ���� hp�� ratio ���̸� temp�� ���� )
        var temp = from coll in _unitColliderList
                   where coll.GetComponent<Unit>() != null && coll.GetComponent<Unit>().unitHp <= BLOOD_EXCUTION_RATIO
                   select coll.GetComponent<Unit>();

        // �� �� ������ ?
        var temp2 = from coll in _unitColliderList
                    where coll.GetComponent<Unit>() != null && coll.GetComponent<Unit>().unitHp > BLOOD_EXCUTION_RATIO
                    select coll.GetComponent<Unit>();

        foreach (Unit unit in temp) 
        {
            // ##TODO : unit �� �� ó��
        }
        foreach (Unit unit in temp2) 
        {
            // ##TODO : �ӽ� �ڵ� �ٲٸ� �����ؾ��� (�������� �ֱ�)
            unit.F_GetDamage(2f);
        }

    }

    public void F_SupernovaEffect( Marker _marker, float _size ) 
    {
    
    }
}
