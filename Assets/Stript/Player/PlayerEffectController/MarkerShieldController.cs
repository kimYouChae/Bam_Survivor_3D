using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MarkerShieldController : MonoBehaviour
{

    [Header("===basic Shield Object===")]
    [SerializeField]
    private GameObject _basicShieldObject;                              // �⺻ ���� ������Ʈ

    [Header("===Shield State===")]
    [SerializeField] private ShieldState _shieldState;

    [Header("===�ߺ� �˻� Dictionary===")]
    private Dictionary< Shield_Effect, int > DICT_ShieldTOCount;
    // Shield Enum (key)�� �´� ���� int (value) 

    [Header("===Skill Effect Ratio===")]
    [SerializeField] const  float   BLOOD_SHIPON_RATIO      = 0.7f;     // ���� ���� 
    [SerializeField] const  float   BLOOD_EXCUTION_RATIO    = 5f;       // ##TODO �ӽ� : ���� N���ϸ� ó��
    [SerializeField] const  int     BOMB_GENERATE_RATIO     = 2;        // ��ź ���� ���� 

    public delegate void del_ShieldExpanding        (Marker _unitTrs, float _size);             // ���� ���� ��
    public delegate void del_ShiledExpandingComplete(Marker _unitTrs, float _size );            // ���� �����Ϸ� �� 1ȸ 

    // deligate ����
    public del_ShieldExpanding          del_shieldExpading;
    public del_ShiledExpandingComplete  del_shieldExpandingComplete;

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
            _shieldIns.transform.localScale += new Vector3(0.2f, 0, 0.2f);
            _shieldIns.transform.position   = _marker.gameObject.transform.position;

            // ���� ���� �� 
            try
            {
                del_shieldExpading(_marker, _shieldState.shieldSize);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }

            // ũ�Ⱑ max�� �Ǹ� ���� 
            if (_shieldIns.transform.localScale.x >= _shieldState.shieldSize
                && _shieldIns.transform.localScale.y >= _shieldState.shieldSize)
                break;

            // �����ð� ��ٸ��� 
            yield return new WaitForSeconds(0.02f);
        }

        // ���� ���� �� 
        try 
        {
            del_shieldExpandingComplete(_marker , _shieldState.shieldSize );
        }
        catch (Exception ex) 
        {
            Debug.LogError(ex);
        }

        // �����ð��Ŀ� ���� 
        yield return new WaitForSeconds(0.2f);

        // ��ƼŬ ����
        ParticleManager.instance.F_PlayerParticle( ParticleState.ShieldEndVFX , _shieldIns.transform );

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
            // ���� ���� �� : Blood Shipon ȿ�� ���� 
            del_shieldExpading += v_card.F_SkillcardEffect;
        }
        // Blood Shipon : 4ȸ ȹ�� 
        if (DICT_ShieldTOCount[Shield_Effect.Epic_BloodSiphon] == 4) 
        {
            // ���� ���� �� : ó�� �߰� 
            del_shieldExpandingComplete += F_Rein_BloodShipon_Excution;
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

    // collider ���� �� return


    // Epic_BloodShiphon Effect : ������ unit �� ����
    public void F_BloodShiponEffect(Marker _marker , float _size) 
    {

        // Unit �˻� 
        Collider[] _unitColliderList = F_ReturnColliser( _marker.gameObject.transform , _size , UnitManager.Instance.unitLayerInt);

        // �����ȿ� ������ x 
        if (_unitColliderList.Length <= 0)
            return;

        // ���� 
        // ��� * ȹ�� count ��ŭ 
        float _bloodAmount = BLOOD_SHIPON_RATIO * DICT_ShieldTOCount[Shield_Effect.Epic_BloodSiphon];

        // �����ȿ� ������ �����Ǹ�
        foreach (Collider _coll in _unitColliderList) 
        {
            if (_coll.GetComponent<Unit>() == null)
                continue;

            // marker State hp ���� 
            PlayerManager.instance.F_UpdateIndiMarkerHP(_marker, _bloodAmount);

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

    // Epic_BloodShiphon Effect : ���� hp ���� �� ó�� 
    private void F_Rein_BloodShipon_Excution( Marker _marker , float _size ) 
    {
        // Unit �˻� 
        Collider[] _unitColliderList = F_ReturnColliser(_marker.gameObject.transform, _size, UnitManager.Instance.unitLayerInt);

        // �����ȿ� ������ x 
        if (_unitColliderList.Length <= 0)
            return;

        // ó�� unit
        var _excutionUnit = from coll in _unitColliderList
                   where coll.GetComponent<Unit>() != null && coll.GetComponent<Unit>().unitHp <= BLOOD_EXCUTION_RATIO
                   select coll.GetComponent<Unit>();

        // ó�� 
        foreach (Unit unit in _excutionUnit) 
        {
            // ##TODO : Pool�� �ǵ����� 
            Destroy( unit , 0.1f );
        }


    }

    // �ݶ��̴� ���� 
    public Collider[] F_ReturnColliser(Transform v_trs, float v_radious, LayerMask v_layer)
    {
        Collider[] _coll = Physics.OverlapSphere(v_trs.position, v_radious, v_layer);

        return _coll;
    }

    public void F_SupernovaEffect( Marker _marker, float _size ) 
    {
    
    }


}
