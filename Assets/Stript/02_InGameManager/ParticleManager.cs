using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ParticleManager : Singleton<ParticleManager>
{
    [Header("=== Container ===")]
    [SerializeField]
    private List<GameObject> _effectList;   // 이펙트 리스트 
    [SerializeField]
    private GameObject      _PoolParent;    // pool 동적 생성할 parent
    [SerializeField]
    private List<GameObject> _effectPool;   // effect 담을 pool  
    
    [SerializeField]
    private Dictionary<ParticleType, Stack<GameObject>> DICT_stateToParticle;

    // 프로퍼티
    public List<GameObject> effectList => _effectList;

    protected override void Singleton_Awake()
    {

    }

    private void Start()
    {
        // POOl 오브젝트 초기화 
        _effectPool = new List<GameObject>();

        // pool Parent 하위에 pool 만들기 
        for (int i = 0; i < System.Enum.GetValues(typeof(ParticleType)).Length; i++) 
        {
            GameObject _obj = Instantiate( GameManager.Instance.emptyObject, Vector3.zero , Quaternion.identity);

            _obj.name = System.Enum.GetName(typeof(ParticleType), i );

            _obj.gameObject.transform.parent = _PoolParent.transform;

            _effectPool.Add( _obj );    

        }

        // Particle State에 따른 Dictionary 초기화
        F_InitEffectDictionary();
    }

    private void F_InitEffectDictionary() 
    {
        // 초기화
        DICT_stateToParticle = new Dictionary<ParticleType, Stack<GameObject>>();

        ParticleType[] _state = (ParticleType[])System.Enum.GetValues(typeof(ParticleType));
        for (int i = 0; i < _state.Length; i++)
        {
            // stack 초기화
            Stack<GameObject> _stack = new Stack<GameObject>();

            for (int j = 0; j < GameManager.Instance.POOLCOUNT; j++) 
            {
                // 스택에 넣기 
                _stack.Push( F_CreateParticle( _state[i]) );
            }

            // 딕셔너리에 넣기 
            DICT_stateToParticle.Add(_state[i] , _stack);
        }
    }

    // particle 플레이
    public void F_PlayerParticle( ParticleType _state , Vector3 _playTrs) 
    {
        // particle state에 맞는 particle 실행     
        GameObject _partiObj = F_ParticleGet( _state );
        
        // 위치 수정 
        _partiObj.transform.position = _playTrs;

        // Play 시키기
        ParticleSystem _particle = _partiObj.GetComponent<ParticleSystem>();    
        _particle.Play();

        // 파티클 끝나면 pool로 return
        StartCoroutine(IE_CheckParticleAlive( _partiObj.GetComponent<ParticleSystem>() , _state));
        
    }

    private IEnumerator IE_CheckParticleAlive(ParticleSystem _particle , ParticleType _state) 
    {
        while (true) 
        {
            // 플레이중이 아니라면 ( = 끝나면 )
            if (!_particle.isPlaying)
            {
                //Debug.Log("파티클이 끝났습니다.");

                // pool로 돌려보내기
                F_ParticleReturn(_particle , _state);

                // 코루틴 종료 
                yield break;
            }

            yield return new WaitForSeconds(0.02f);
        }
    }

    private void F_ParticleReturn( ParticleSystem _particle , ParticleType _state ) 
    {
        // 위치 zero
        _particle.gameObject.SetActive( false );
        _particle.gameObject.transform.localPosition = Vector3.zero;

        // stack에 다시 넣기 
        DICT_stateToParticle[_state].Push(_particle.gameObject);
    }

    // particle Get
    private GameObject F_ParticleGet( ParticleType _state ) 
    {
        if (!DICT_stateToParticle.ContainsKey(_state))
        {
            Debug.LogError(this + " : Particle DICTIONARY ISNT CONTAIN KEY");
            return null;
        }

        // stack이 비어있으면 
        if (DICT_stateToParticle[_state].Count == 0) 
        {
            // 새로 particle 만들어서 스택에 넣기
            GameObject _obj = F_CreateParticle(_state);
            DICT_stateToParticle[_state].Push( _obj );
        }

        // 스택에서 pop 후 , return
        GameObject _returnObj = DICT_stateToParticle[_state].Pop();
        _returnObj.SetActive(true);

        return _returnObj;

    }

    private GameObject F_CreateParticle(ParticleType _state) 
    {
        // particle 인스턴스화
        GameObject _parti = Instantiate(_effectList[ (int)_state ]);

        // 기본 세팅
        _parti.transform.parent = _effectPool[(int)_state].transform;
        _parti.SetActive(false);
        _parti.transform.position = Vector3.zero;

        return _parti;
    }


}
