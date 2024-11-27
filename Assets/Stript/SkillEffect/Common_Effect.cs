using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Common_BigBullet : SkillCard
{
    public override void F_SkillcardEffect()
    {
        //Debug.Log(this.classSpriteName);

        // 총알크기 30% 증가 (PlayerManager의 markerbulletController에 접근)
        PlayerManager.Instance.markerBulletController.F_UpdateBulletState(BulletSizePercent : 0.3f);

    }
}
public class Common_DamageUp : SkillCard
{
    public override void F_SkillcardEffect()
    {
        //Debug.Log(this.classSpriteName);

        // 총알 데미지 20% 증가 (PlayerManager의 markerbulletController에 접근)
        PlayerManager.Instance.markerBulletController.F_UpdateBulletState(BulletDamagePercent: 0.2f);
    }
}
public class Common_RapidReload : SkillCard
{
    public override void F_SkillcardEffect()
    {
        //Debug.Log(this.classSpriteName);

        // 총알 재장전 속도가 10% 감소 (PlayerManager의 markerState 접근)
        PlayerManager.Instance.F_UpdateMarkerState(BulletCoolTimePercent: 0.1f);

    }
}

public class Common_DefenceUP : SkillCard
{
    public override void F_SkillcardEffect()
    {
        // 방어력 10% 증가 
        PlayerManager.Instance.F_UpdateMarkerState(DefencePercent : 0.2f);
    }
}