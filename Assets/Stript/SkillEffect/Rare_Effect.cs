using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rare_PoisionBullet : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // 총알 폭발 시 사거리 안에 있는 unit에게 독 효과 
        // markerExplosion 의 함수를 호출하면 편할듯? 여기서 굳이 작성안해도 ?
    }
}
public class Rare_RapidBarier : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // 쉴드 쿨타임 10% 감소
        PlayerManager.instance.F_UpdateMarkerState(ShieldCoolTimePercent : 0.1f);
    }
}
public class Rare_IceBullet : SkillCard
{

    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // 총알 폭발 시 사거리 안에 있는 unit에게 얼음효과 
    }
}

public class Rare_ShieldExpention : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // 쉴드 범위 10% 증가   
        PlayerManager.instance.markerShieldController.F_UpdateShieldState(ShieldSizePercent : 0.1f);
    }
}

public class Rare_MagneticUP : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // 자석 범위 10% 증가
        PlayerManager.instance.F_UpdateMarkerState(MagnetPercent: 0.1f);
    }
}