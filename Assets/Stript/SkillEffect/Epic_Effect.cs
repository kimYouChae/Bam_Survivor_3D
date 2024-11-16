using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Epic_Bounce : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // 총알 bounce 갯수 1개 증가 (PlayerManager의 markerBulletController에 접근)
        PlayerManager.Instance.markerBulletController.F_UpdateBulletState(BulletCnt : 1);
    }
}
public class Epic_BulletStrom : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // 총알갯수 2개 증가 (PlayerManager의 markerBulletController에 접근)
        PlayerManager.Instance.markerBulletController.F_UpdateBulletState(BulletBounceCount: 2);
    }
}

public class Epic_BloodSiphon : SkillCard
{
    public override void F_SkillcardEffect(Marker _marker)
    {
        Debug.Log(this.classSpriteName);

        // blood 쉴드 pool에서 가져오기 
        ShieldManager.Instance.F_GetBloodShieldToPool( Shield_Effect.Epic_BloodSiphon, _marker);

        // particle 가져오기 , 위치는 marker 위치로 
        // ##TODO : Blood 이펙트 추가한 후 변경해야함
        ParticleManager.Instance.F_PlayerParticle(ParticleState.ShieldEndVFX, _marker.transform.position);

    }
}

public class Epic_ExperienceBoost : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // 경험치 획득량 5% 증가
        PlayerManager.Instance.F_UpdateMarkerSubState(ExperiencePercent : 0.05f);
    }
}

public class Epic_TouchOfLuck : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // 행운 5% 증가
        PlayerManager.Instance.F_UpdateMarkerSubState(LuckPercent : 0.05f);
    }
}