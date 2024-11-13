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

        // 클론 되지 않은 supernova만 follow
        if( ! _isCloned)
            F_FllowMarker();
    }

    protected override void F_EndShiled()
    {
        // 복제된 supernova가 아니면-> clone 하기 
        if ( _isCloned == false )
        {
            // supernova 복제 
            F_CloneSupernova();
        }

        // 쉴드 pool로 되돌리기
        ShieldPooling.instance.F_ShieldSet(gameObject, Shield_Effect.Legend_Supernova);
    }

    protected override void F_ExpandingShield()
    {
        // 본인 주변 콜라이더 검출, 데미지 주가
        Collider[] _coll = F_ReturnUnitCollider(gameObject, gameObject.transform.position.x , LayerManager.instance.unitLayer);

        foreach (Collider unit in _coll)
        {
            try
            {
                // Unit 스크립트는 무조건 들어있음 ! 
                // supernova 횟수 + supernova 고정데미지
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
        // dx, dy 위치에 supernova 오브젝트 생성 
        for (int i = 0; i < 4; i++)
        {
            float nx = gameObject.transform.position.x + dx[i] + Random.Range(-2f, 2f);
            float ny = gameObject.transform.position.z + dy[i] + Random.Range(-2f, 2f);

            // supernova pool에서 가져오기
            GameObject _supernova = ShieldPooling.instance.F_ShieldGet(Shield_Effect.Legend_Supernova);
            // 파티클 pool에서 가져오기
            ParticleManager.instance.F_PlayerParticle(ParticleState.SupernovaVFX, new Vector3(nx, 0, ny));

            _supernova.transform.position = new Vector3(nx, 0, ny);

            // supernova 오브젝트에 _isCloned을 true로 바껴야 무한으로 생성 x
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
