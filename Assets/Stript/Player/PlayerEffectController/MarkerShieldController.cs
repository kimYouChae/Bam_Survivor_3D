using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class MarkerShieldController : MonoBehaviour
{

    [Header("===basic Shield Object===")]
    [SerializeField]
    private GameObject _basicShieldObject;          // 기본 쉴드 오브젝트
    [SerializeField]
    private float _minShieldSize;       // 최소 쉴드 크기
    [SerializeField]    
    private float _maxShieldSize;       // 최대 쉴드 크기 

    [Header("===중복 검사 Dictionary===")]
    private Dictionary<SkillCard, int> _markerShieldDuplication;
    // skillcard(key)에 맞는 갯수 int (value) 

    public delegate void del_MarkerShield(Transform v_parent);

    // deligate 선언
    public del_MarkerShield del_markerShieldUse;

    private void Start()
    {
        // 델리게이트에 기본 쉴드 사용 추가 
        del_markerShieldUse += F_BasicShieldUse;

        // size
        _minShieldSize = 1f;
        _maxShieldSize = 2f;
    }

    private void F_BasicShieldUse(Transform v_parent) 
    {
        GameObject _ins = Instantiate(_basicShieldObject, v_parent);
        _ins.transform.localPosition = Vector3.zero;

        // 기본 쉴드 크기 키우기 
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

        // 일정시간후에 삭제 
        yield return new WaitForSeconds(0.2f);
        Destroy(_trs.gameObject);
    }

    public void F_ApplyShieldEffect(SkillCard v_card ) 
    {
        // ##TODO : 효과적용 코드 짜기 
    }

    public void F_DictionaryInt(SkillCard v_card)
    {
        // 초기화 안되어있으면 초기화
        if (_markerShieldDuplication == null)
            _markerShieldDuplication = new Dictionary<SkillCard, int>();

        // card가 포함이 안되어있으면 ?
        if (!_markerShieldDuplication.ContainsKey(v_card))
        {
            _markerShieldDuplication.Add(v_card, 0);
        }
        // 포함이 되어있으면 ?
        else
        {
            _markerShieldDuplication[v_card] += 1;

            // ##TODO : 추가된 효과에 맞게 추가 효과 넣어야함
        }


    }
}
