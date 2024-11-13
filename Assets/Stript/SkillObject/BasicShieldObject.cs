using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShieldObject : ShieldObject
{
    void Update()
    {
        F_ShieldUpdate();

        F_FllowMarker();
    }

    protected override void F_EndShiled()
    {
        // 쉴드 끝 particle 실행
        ParticleManager.instance.F_PlayerParticle(ParticleState.ShieldEndVFX, gameObject.transform.position);

        // 쉴드 pool로 되돌리기
        ShieldPooling.instance.F_ShieldSet(gameObject, Shield_Effect.Default);

    }

    protected override void F_ExpandingShield()
    {

    }


}
