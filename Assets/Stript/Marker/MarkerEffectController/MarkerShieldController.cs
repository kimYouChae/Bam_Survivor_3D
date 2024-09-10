using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class MarkerShieldController : MonoBehaviour
{

    [Header("===basic Shield Object===")]
    [SerializeField]
    private GameObject _basicShieldObject;          // �⺻ ���� ������Ʈ
    [SerializeField]
    private float _minShieldSize;       // �ּ� ���� ũ��
    [SerializeField]    
    private float _maxShieldSize;       // �ִ� ���� ũ�� 

    [Header("===�ߺ� �˻� Dictionary===")]
    private Dictionary<SkillCard, int> _markerShieldDuplication;
    // skillcard(key)�� �´� ���� int (value) 

    public delegate void del_MarkerShield(Transform v_parent);

    // deligate ����
    public del_MarkerShield del_markerShieldUse;

    private void Start()
    {
        // ��������Ʈ�� �⺻ ���� ��� �߰� 
        del_markerShieldUse += F_BasicShieldUse;

        // size
        _minShieldSize = 1f;
        _maxShieldSize = 2f;
    }

    private void F_BasicShieldUse(Transform v_parent) 
    {
        GameObject _ins = Instantiate(_basicShieldObject, v_parent);
        _ins.transform.localPosition = Vector3.zero;

        // �⺻ ���� ũ�� Ű��� 
        StartCoroutine(IE_basicShield(_ins));
    }

    IEnumerator IE_basicShield(GameObject v_obj) 
    {
        Transform _trs = v_obj.transform;

        while (true) 
        {
            _trs.localScale += new Vector3(0.2f, 0.2f, 0);

            if (_trs.localScale.x >= _maxShieldSize 
                && _trs.localScale.y >= _maxShieldSize)
                break;

            yield return new WaitForSeconds(0.2f);
        }

        // �����ð��Ŀ� ���� 
        yield return new WaitForSeconds(0.2f);
        Destroy(_trs.gameObject);
    }

    public void F_ApplyShieldEffect(SkillCard v_card ) 
    {
        // ##TODO : ȿ������ �ڵ� ¥�� 
    }

    public void F_DictionaryInt(SkillCard v_card)
    {
        // �ʱ�ȭ �ȵǾ������� �ʱ�ȭ
        if (_markerShieldDuplication == null)
            _markerShieldDuplication = new Dictionary<SkillCard, int>();

        // card�� ������ �ȵǾ������� ?
        if (!_markerShieldDuplication.ContainsKey(v_card))
        {
            _markerShieldDuplication.Add(v_card, 0);
        }
        // ������ �Ǿ������� ?
        else
        {
            _markerShieldDuplication[v_card] += 1;

            // ##TODO : �߰��� ȿ���� �°� �߰� ȿ�� �־����
        }


    }
}
