using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legend_HealingField : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // 쉴드 사용 시 외각에 폭탄 터짐 
    }
}

public class Legend_Supernova : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // 쉴드 사용 시 unit을 중심으로 모으고 강한 데미지 줌
    }
}

public class Legend_Mayhem : SkillCard
{

    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // 총알 튕기는 갯수 +3 , 총알 크기 10% 증가, 총알 데미지 30% 증가
        // (PlayerManager의 markerBulletController에 접근)
        PlayerManager.instance.markerBulletController.F_UpdateBulletState(  BulletDamagePercent : 0.3f , BulletSizePercent : 0.1f, BulletBounceCount : 3 );
    }
}

public class Legend_ExtraLife : SkillCard
{
    public override void F_SkillcardEffect()
    {
        // 부활횟수 1회 증가 
        PlayerManager.instance.F_UpdateMarkerSubState(RevivalCount : 1);
    }
}