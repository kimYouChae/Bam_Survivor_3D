using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditorInternal;


public class MarkerShieldController : MonoBehaviour
{

    [Header("===basic Shield Object===")]
    [SerializeField]
    private GameObject _basicShieldObject;          // �⺻ ���� ������Ʈ

    [Header("===Shield State===")]
    [SerializeField] private ShieldState _shieldState;

    [Header("===�ߺ� �˻� Dictionary===")]
    private Dictionary< Shield_Effect, int> _markerShieldDuplication;
    // Shield Enum (key)�� �´� ���� int (value) 

    public delegate void del_MarkerShield(Marker _unitTrs, float _size);

    // deligate ����
    public del_MarkerShield del_markerShieldUse;

    private void Start()
    {
        // ##TODO : �ӽ� ����state ���� , ���߿� ���� ������ ĳ���� ���� �޸����� ?  
        _shieldState = new ShieldState(3f);
    }

    // ## Test : ���� ���ư��� ���� 
    private IEnumerator IE_Test( GameObject v_marker ) 
    {
        GameObject _shieldIns = Instantiate(_basicShieldObject, v_marker.transform);
        _shieldIns.transform.localPosition = Vector3.zero;

        while (true) 
        {
            // ũ�� Ű��� 
            _shieldIns.transform.localScale += new Vector3(0.2f, 0.2f, 0);

            // ũ�Ⱑ max�� �Ǹ� ���� 
            if (_shieldIns.transform.localScale.x >= _shieldState.shieldSize
                && _shieldIns.transform.localScale.y >= _shieldState.shieldSize)
                break;

            // ���� ��������Ʈ (ȿ�� ����) ����


            // �����ð� ��ٸ��� 
            yield return new WaitForSeconds(0.5f);
        }


        // �����ð��Ŀ� ���� 
        yield return new WaitForSeconds(0.2f);
        Destroy(_shieldIns);
    }

    private void F_AddToEffectDeligate(del_MarkerShield _effect) 
    {
        del_markerShieldUse += _effect;
    }

    private void F_RemoveFromEffectDeligate(del_MarkerShield _effect) 
    {
        del_markerShieldUse -= _effect;
    }

    private void F_UseEffectDeligate(Marker _marker, float _shieldSize) 
    {
        del_markerShieldUse.Invoke(_marker, _shieldSize);
    }

    public void F_ApplyShieldEffect( SkillCard v_card ) 
    {
        // ��ųʸ��� skillcard �˻� 
        F_DictionaryInt(v_card);

        // ##TODO : �߰��� ȿ���� �°� �߰� ȿ�� �־����
        // ex) dictionary[ ���� enum.����] == 1 && dictionary[���� enum.ex] == 1
        if (_markerShieldDuplication[Shield_Effect.Epic_BloodSiphon] == 1) 
        {
            // Blood : �ʱ� 1ȸ�� ��������Ʈ�� �߰�
            // 1~3ȸ������ F_BloodShiponEffect()������ count�� ������ �߰�
            // 4ȸ���� ó���߰�
            del_markerShieldUse += v_card.F_SkillcardEffect;
        }


    }

    // skill effect �ߺ� üũ 
    public void F_DictionaryInt(SkillCard v_card)
    {
        // �ʱ�ȭ �ȵǾ������� �ʱ�ȭ
        if (_markerShieldDuplication == null)
            _markerShieldDuplication = new Dictionary<Shield_Effect, int>();

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
        if (!_markerShieldDuplication.ContainsKey(_myEffect))
        {
            _markerShieldDuplication.Add(_myEffect, 0);
        }
        // ������ �Ǿ������� ?
        else
        {
            _markerShieldDuplication[_myEffect] += 1;
        }

    }

    // Shield Sate ������Ʈ 
    public void F_UpdateShieldState( float ShieldSizePercent = 0 ) 
    { 
        _shieldState.shieldSize += _shieldState.shieldSize * ShieldSizePercent;
    }

    // Epic_BloodShiphon Effect : ������ unit �� ����
    public void F_BloodShiponEffect(Marker v_marker , float v_size) 
    {
        // ##TODO : ������ ������ �� unit ���� �� ���� ó�� + �� ���� (�̷��� �� �˻��� �� linq ���� ������?)
        // ##TODO : ���尡 �����Ǵ� �ð��� ���� ?

        Collider[] _unitColliderList = Physics.OverlapSphere( v_marker.gameObject.transform.position , v_size , UnitManager.Instance.unitLayerInt);

        // �����ȿ� ������ x 
        if (_unitColliderList.Length < 0)
            return;

        // �����ȿ� ������ �����Ǹ�
        else 
        { 
            foreach(Collider _coll in _unitColliderList) 
            {
                // marker State hp ���� 
                

                // Shield_Effect.Epic_BloodSiphon�� count��ŭ ���� / ���ط� ���� 


            }
        }
    }

}
