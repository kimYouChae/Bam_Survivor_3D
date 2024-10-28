using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ParticleState
{
    BulletExplotion
}

public class ParticleManager : MonoBehaviour
{
    [Header("=== Container ===")]
    [SerializeField]
    private List<ParticleSystem> _effectList;
    [SerializeField]
    private List<GameObject> _effectPool;
    
    [SerializeField]
    private Dictionary<ParticleState, Stack<ParticleSystem>> DICT_stateToParticle;

    private const int POOLCOUNT = 15;

    // 프로퍼티
    public List<ParticleSystem> effectList => _effectList;

    private void Start()
    {
        // Particle State에 따른 Dictionary 초기화
        F_InitEffectDictionary();
    }

    private void F_InitEffectDictionary() 
    {
        // 초기화
        DICT_stateToParticle = new Dictionary<ParticleState, Stack<ParticleSystem>>();

        ParticleState[] _state = (ParticleState[])System.Enum.GetValues(typeof(ParticleState));
        for (int i = 0; i < _state.Length; i++)
        {
            // stack 초기화
            Stack<ParticleSystem> _stack = new Stack<ParticleSystem>();

            for (int j = 0; j < POOLCOUNT; j++) 
            {
                // 스택에 넣기 
                _stack.Push( F_CreateParticle(_state[i]).GetComponent<ParticleSystem>() );
            }

            // 딕셔너리에 넣기 
            DICT_stateToParticle.Add(_state[i] , _stack);
        }
    }

    // particle 플레이
    public void F_PlayerParticle( ParticleState _state , Transform _playTrs) 
    {
        // particle state에 맞는 particle 실행     
        
    }

    // particle Get
    private void F_ParticleGet( ParticleState _state ) 
    {
        // stack이 비어있으면 
        if (DICT_stateToParticle[_state].Count == 0) 
        { 
            // 새로 particle 만들어서 넣기

        }
        
        // Particle 을 return

    }

    private GameObject F_CreateParticle(ParticleState _state) 
    {
        // particle 인스턴스화
        GameObject _parti = Instantiate(_effectList[ (int)_state ].gameObject);

        // 기본 세팅
        _parti.transform.parent = _effectPool[(int)_state].transform;
        _parti.SetActive(false);
        _parti.transform.position = Vector3.zero;

        return _parti;
    }


}
