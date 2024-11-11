using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ParticleManager : MonoBehaviour
{
    public static ParticleManager instance;

    [Header("=== Container ===")]
    [SerializeField]
    private List<GameObject> _effectList;   // ����Ʈ ����Ʈ 
    [SerializeField]
    private GameObject      _PoolParent;    // pool ���� ������ parent
    [SerializeField]
    private List<GameObject> _effectPool;   // effect ���� pool  
    
    [SerializeField]
    private Dictionary<ParticleState, Stack<GameObject>> DICT_stateToParticle;

    // ������Ƽ
    public List<GameObject> effectList => _effectList;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // POOl ������Ʈ �ʱ�ȭ 
        _effectPool = new List<GameObject>();

        // pool Parent ������ pool ����� 
        for (int i = 0; i < System.Enum.GetValues(typeof(ParticleState)).Length; i++) 
        {
            GameObject _obj = Instantiate( GameManager.instance.emptyObject, Vector3.zero , Quaternion.identity);

            _obj.name = System.Enum.GetName(typeof(ParticleState), i );

            _obj.gameObject.transform.parent = _PoolParent.transform;

            _effectPool.Add( _obj );    

        }

        // Particle State�� ���� Dictionary �ʱ�ȭ
        F_InitEffectDictionary();
    }

    private void F_InitEffectDictionary() 
    {
        // �ʱ�ȭ
        DICT_stateToParticle = new Dictionary<ParticleState, Stack<GameObject>>();

        ParticleState[] _state = (ParticleState[])System.Enum.GetValues(typeof(ParticleState));
        for (int i = 0; i < _state.Length; i++)
        {
            // stack �ʱ�ȭ
            Stack<GameObject> _stack = new Stack<GameObject>();

            for (int j = 0; j < GameManager.instance.POOLCOUNT; j++) 
            {
                // ���ÿ� �ֱ� 
                _stack.Push( F_CreateParticle( _state[i]) );
            }

            // ��ųʸ��� �ֱ� 
            DICT_stateToParticle.Add(_state[i] , _stack);
        }
    }

    // particle �÷���
    public void F_PlayerParticle( ParticleState _state , Vector3 _playTrs) 
    {
        // particle state�� �´� particle ����     
        GameObject _partiObj = F_ParticleGet( _state );
        
        // ��ġ ���� 
        _partiObj.transform.position = _playTrs;

        // Play ��Ű��
        ParticleSystem _particle = _partiObj.GetComponent<ParticleSystem>();    
        _particle.Play();

        // ��ƼŬ ������ pool�� return
        StartCoroutine(IE_CheckParticleAlive( _partiObj.GetComponent<ParticleSystem>() , _state));
        
    }

    private IEnumerator IE_CheckParticleAlive(ParticleSystem _particle , ParticleState _state) 
    {
        while (true) 
        {
            // �÷������� �ƴ϶�� ( = ������ )
            if (!_particle.isPlaying)
            {
                Debug.Log("��ƼŬ�� �������ϴ�.");

                // pool�� ����������
                F_ParticleReturn(_particle , _state);

                // �ڷ�ƾ ���� 
                yield break;
            }

            yield return new WaitForSeconds(0.02f);
        }
    }

    private void F_ParticleReturn( ParticleSystem _particle , ParticleState _state ) 
    {
        // ��ġ zero
        _particle.gameObject.SetActive( false );
        _particle.gameObject.transform.localPosition = Vector3.zero;

        // stack�� �ٽ� �ֱ� 
        DICT_stateToParticle[_state].Push(_particle.gameObject);
    }

    // particle Get
    private GameObject F_ParticleGet( ParticleState _state ) 
    {
        if (!DICT_stateToParticle.ContainsKey(_state))
        {
            Debug.LogError(this + " : Particle DICTIONARY ISNT CONTAIN KEY");
            return null;
        }

        // stack�� ��������� 
        if (DICT_stateToParticle[_state].Count == 0) 
        {
            // ���� particle ���� ���ÿ� �ֱ�
            GameObject _obj = F_CreateParticle(_state);
            DICT_stateToParticle[_state].Push( _obj );
        }

        // ���ÿ��� pop �� , return
        GameObject _returnObj = DICT_stateToParticle[_state].Pop();
        _returnObj.SetActive(true);

        return _returnObj;

    }

    private GameObject F_CreateParticle(ParticleState _state) 
    {
        // particle �ν��Ͻ�ȭ
        GameObject _parti = Instantiate(_effectList[ (int)_state ]);

        // �⺻ ����
        _parti.transform.parent = _effectPool[(int)_state].transform;
        _parti.SetActive(false);
        _parti.transform.position = Vector3.zero;

        return _parti;
    }


}
