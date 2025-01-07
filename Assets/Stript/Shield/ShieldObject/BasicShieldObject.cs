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
        // ���� �� particle ����
        ParticleManager.Instance.F_PlayerParticle(ParticleType.ShieldEndVFX, gameObject.transform.position);

        // ���� pool�� �ǵ�����
        ShieldManager.Instance.shieldPooling.F_ShieldSet(gameObject, Shield_Effect.Default);

    }

    protected override void F_ExpandingShield()
    {

    }


}
