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

    // ������Ƽ
    public List<ParticleSystem> effectList => _effectList;

    private void Start()
    {
        // Particle State�� ���� Dictionary �ʱ�ȭ
        F_InitEffectDictionary();
    }

    private void F_InitEffectDictionary() 
    {
        // �ʱ�ȭ
        DICT_stateToParticle = new Dictionary<ParticleState, Stack<ParticleSystem>>();

        ParticleState[] _state = (ParticleState[])System.Enum.GetValues(typeof(ParticleState));
        for (int i = 0; i < _state.Length; i++)
        {
            // stack �ʱ�ȭ
            Stack<ParticleSystem> _stack = new Stack<ParticleSystem>();

            for (int j = 0; j < POOLCOUNT; j++) 
            {
                // ���ÿ� �ֱ� 
                _stack.Push( F_CreateParticle(_state[i]).GetComponent<ParticleSystem>() );
            }

            // ��ųʸ��� �ֱ� 
            DICT_stateToParticle.Add(_state[i] , _stack);
        }
    }

    // particle �÷���
    public void F_PlayerParticle( ParticleState _state , Transform _playTrs) 
    {
        // particle state�� �´� particle ����     
        
    }

    // particle Get
    private void F_ParticleGet( ParticleState _state ) 
    {
        // stack�� ��������� 
        if (DICT_stateToParticle[_state].Count == 0) 
        { 
            // ���� particle ���� �ֱ�

        }
        
        // Particle �� return

    }

    private GameObject F_CreateParticle(ParticleState _state) 
    {
        // particle �ν��Ͻ�ȭ
        GameObject _parti = Instantiate(_effectList[ (int)_state ].gameObject);

        // �⺻ ����
        _parti.transform.parent = _effectPool[(int)_state].transform;
        _parti.SetActive(false);
        _parti.transform.position = Vector3.zero;

        return _parti;
    }


}
