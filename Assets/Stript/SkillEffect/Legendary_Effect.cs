using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legend_HealingField : SkillCard
{
    public override void F_SkillcardEffect(Marker _marker)
    {
        //Debug.Log(this.classSpriteName);

        // Healing 쉴드 pool에서 가져오기 
        ShieldManager.Instance.F_GetBloodShieldToPool(Shield_Effect.Legend_HealingField, _marker);

        // particle 가져오기 , 위치는 marker 위치로 
        ParticleManager.Instance.F_PlayerParticle(ParticleType.HealingEndVFX, _marker.transform.position);


    }
}

public class Legend_Supernova : SkillCard
{
    public override void F_SkillcardEffect(Marker _marker)
    {
        //Debug.Log(this.classSpriteName);

        // Supernova 쉴드 pool에서 가져오기 
        ShieldManager.Instance.F_GetBloodShieldToPool(Shield_Effect.Legend_Supernova, _marker);

        // particle 가져오기 , 위치는 marker 위치로 
        ParticleManager.Instance.F_PlayerParticle(ParticleType.SupernovaVFX, _marker.transform.position);

    }
}

public class Legend_Mayhem : SkillCard
{

    public override void F_SkillcardEffect()
    {
        //Debug.Log(this.classSpriteName);

        // 총알 튕기는 갯수 +3 , 총알 크기 10% 증가, 총알 데미지 30% 증가
        // (PlayerManager의 markerBulletController에 접근)
        PlayerManager.Instance.markerBulletController.F_UpdateBulletState(  BulletDamagePercent : 0.3f , BulletSizePercent : 0.1f, BulletBounceCount : 3 );
    }
}

public class Legend_ExtraLife : SkillCard
{
    public override void F_SkillcardEffect()
    {
        // 부활횟수 1회 증가 
        PlayerManager.Instance.F_UpdateMarkerSubState(RevivalCount : 1);
    }
}