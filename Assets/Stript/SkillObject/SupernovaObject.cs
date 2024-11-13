using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SupernovaObject : ShieldObject
{
    private float[] dx = new float[4] { -3f, -7f, 5f, 8f};
    private float[] dy = new float[4] { 5f, 1f, -1f, 3f };  

    private bool _isCloned = false;

    public bool IsCloned { set { _isCloned = value; } }

    void Update()
    {
        F_ShieldUpdate();

        // Ŭ�� ���� ���� supernova�� follow
        if( ! _isCloned)
            F_FllowMarker();
    }

    protected override void F_EndShiled()
    {
        // ������ supernova�� �ƴϸ�-> clone �ϱ� 
        if ( _isCloned == false )
        {
            // supernova ���� 
            F_CloneSupernova();
        }

        // ���� pool�� �ǵ�����
        ShieldPooling.instance.F_ShieldSet(gameObject, Shield_Effect.Legend_Supernova);
    }

    protected override void F_ExpandingShield()
    {
        // ���� �ֺ� �ݶ��̴� ����, ������ �ְ�
        Collider[] _coll = F_ReturnUnitCollider(gameObject, gameObject.transform.position.x , LayerManager.instance.unitLayer);

        foreach (Collider unit in _coll)
        {
            try
            {
                // Unit ��ũ��Ʈ�� ������ ������� ! 
                // supernova Ƚ�� + supernova ����������
                float _damage = PlayerManager.instance.markerShieldController.F_ReturnCountToDic(Shield_Effect.Legend_Supernova) * 
                    PlayerManager.instance.markerShieldController.supernovaDamage;
                unit.gameObject.GetComponent<Unit>().F_GetDamage(_damage);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

    }

    private void F_CloneSupernova() 
    {
        // dx, dy ��ġ�� supernova ������Ʈ ���� 
        for (int i = 0; i < 4; i++)
        {
            float nx = gameObject.transform.position.x + dx[i] + Random.Range(-2f, 2f);
            float ny = gameObject.transform.position.z + dy[i] + Random.Range(-2f, 2f);

            // supernova pool���� ��������
            GameObject _supernova = ShieldPooling.instance.F_ShieldGet(Shield_Effect.Legend_Supernova);
            // ��ƼŬ pool���� ��������
            ParticleManager.instance.F_PlayerParticle(ParticleState.SupernovaVFX, new Vector3(nx, 0, ny));

            _supernova.transform.position = new Vector3(nx, 0, ny);

            // supernova ������Ʈ�� _isCloned�� true�� �ٲ��� �������� ���� x
            try
            {
                _supernova.GetComponent<SupernovaObject>().IsCloned = true;
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }

        }
    }
    
}
