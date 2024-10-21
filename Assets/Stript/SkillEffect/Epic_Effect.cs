using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Epic_Bounce : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // 총알 bounce 갯수 1개 증가 (PlayerManager의 markerBulletController에 접근)
        PlayerManager.instance.markerBulletController.F_UpdateBulletState(BulletCnt : 1);
    }
}
public class Epic_BulletStrom : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // 총알갯수 2개 증가 (PlayerManager의 markerBulletController에 접근)
        PlayerManager.instance.markerBulletController.F_UpdateBulletState(BulletBounceCount: 2);
    }
}

public class Epic_BloodSiphon : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // 쉴드 사용시 범위 내 unit 흡혈 
    }
}

public class Epic_ExperienceBoost : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // 경험치 획득량 5% 증가
        PlayerManager.instance.F_UpdateMarkerSubState(ExperiencePercent : 0.05f);
    }
}

public class Epic_TouchOfLuck : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // 행운 5% 증가
        PlayerManager.instance.F_UpdateMarkerSubState(LuckPercent : 0.05f);
    }
}