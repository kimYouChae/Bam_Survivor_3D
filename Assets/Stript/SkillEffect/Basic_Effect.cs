using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Tanker : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // marker의 max hp가 30퍼 증가 ( PlayerManager의 marker 리스트에 접근 )
        PlayerManager.instance.F_UpdateMarkerState(MaxHpPercent : 0.3f);
        

    }
}
public class Basic_Speeder : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // marker의 움직임 속도 20% 증가 ( PlayerManager의 marker 리스트에 접근 )
        PlayerManager.instance.F_UpdateMarkerState(SpeedPercent : 0.2f);
    }
}

public class Basic_NaturalRecovery : SkillCard
{
    public override void F_SkillcardEffect()
    {
        // 자연회복량이 0.5f 증가함
        PlayerManager.instance.F_UpdateMarkerState(RecoveryIncrease: 0.5f);
    }
}

public class Basic_QuickRecovery : SkillCard
{
    public override void F_SkillcardEffect()
    {
        // 자연회복량 쿨타임이 5% 감소 
        PlayerManager.instance.F_UpdateMarkerState(RecoveryCoolTimeDecrease: 0.05f);
    }
}

public class Basic_RapidBullet : SkillCard
{
    public override void F_SkillcardEffect()
    {
        // 종알속도가 10% 증가합니다
    }
}