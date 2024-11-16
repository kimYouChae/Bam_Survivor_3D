using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rare_PoisionBullet : SkillCard
{
    public override void F_SkillcardEffect(Transform _trs, float _size)
    {
        Debug.Log(this.classSpriteName);

        // 총알 폭발 시 사거리 안에 있는 unit에게 독 효과 
        // PlayerManager의 MarkerExplotion Controller에 접근 
        PlayerManager.Instance.markerExplosionConteroller.F_PositionBullet(_trs , _size);
    }
}
public class Rare_RapidBarier : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // 쉴드 쿨타임 10% 감소
        PlayerManager.Instance.F_UpdateMarkerState(ShieldCoolTimePercent : 0.1f);
    }
}
public class Rare_IceBullet : SkillCard
{

    public override void F_SkillcardEffect(Transform _trs, float _size)
    {
        Debug.Log(this.classSpriteName);

        // 총알 폭발 시 사거리 안에 있는 unit에게 얼음효과 
        // PlayerManager의 MarkerExplotion Controller에 접근 
        PlayerManager.Instance.markerExplosionConteroller.F_IceBullet(_trs, _size);
    }
}

public class Rare_ShieldExpention : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // (PlayerState)쉴드 범위 10% 증가   
        ShieldManager.Instance.F_UpdateShieldState(ShieldSizePercent : 0.1f);
    }
}

public class Rare_MagneticUP : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // 자석 범위 10% 증가
        PlayerManager.Instance.F_UpdateMarkerState(MagnetPercent: 0.1f);
    }
}