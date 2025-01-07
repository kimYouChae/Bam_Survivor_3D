using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legend_HealingField : SkillCard
{
    public override void F_SkillcardEffect(Marker _marker)
    {
        //Debug.Log(this.classSpriteName);

        // Healing ���� pool���� �������� 
        ShieldManager.Instance.F_GetBloodShieldToPool(Shield_Effect.Legend_HealingField, _marker);

        // particle �������� , ��ġ�� marker ��ġ�� 
        ParticleManager.Instance.F_PlayerParticle(ParticleType.HealingEndVFX, _marker.transform.position);


    }
}

public class Legend_Supernova : SkillCard
{
    public override void F_SkillcardEffect(Marker _marker)
    {
        //Debug.Log(this.classSpriteName);

        // Supernova ���� pool���� �������� 
        ShieldManager.Instance.F_GetBloodShieldToPool(Shield_Effect.Legend_Supernova, _marker);

        // particle �������� , ��ġ�� marker ��ġ�� 
        ParticleManager.Instance.F_PlayerParticle(ParticleType.SupernovaVFX, _marker.transform.position);

    }
}

public class Legend_Mayhem : SkillCard
{

    public override void F_SkillcardEffect()
    {
        //Debug.Log(this.classSpriteName);

        // �Ѿ� ƨ��� ���� +3 , �Ѿ� ũ�� 10% ����, �Ѿ� ������ 30% ����
        // (PlayerManager�� markerBulletController�� ����)
        PlayerManager.Instance.markerBulletController.F_UpdateBulletState(  BulletDamagePercent : 0.3f , BulletSizePercent : 0.1f, BulletBounceCount : 3 );
    }
}

public class Legend_ExtraLife : SkillCard
{
    public override void F_SkillcardEffect()
    {
        // ��ȰȽ�� 1ȸ ���� 
        PlayerManager.Instance.F_UpdateMarkerSubState(RevivalCount : 1);
    }
}