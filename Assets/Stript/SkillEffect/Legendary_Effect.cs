using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legend_BombShield : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.cardName);

        // 쉴드 사용 시 외각에 폭탄 터짐 
    }
}

public class Legend_Supernova : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.cardName);

        // 쉴드 사용 시 unit을 중심으로 모으고 강한 데미지 줌
    }
}

public class Legend_Mayhem : SkillCard
{

    public override void F_SkillcardEffect()
    {
        Debug.Log(this.cardName);

        // 총알 튕기는 갯수 +3 , 총알 크기 10% 증가, 총알 데미지 30% 증가
        // (PlayerManager의 markerBulletController에 접근)

        PlayerManager.instance.markerBulletController.bulletSate.bulletBounceCount += 3;
        PlayerManager.instance.markerBulletController.bulletSate.bulletSize +=
            PlayerManager.instance.markerBulletController.bulletSate.bulletSize % 0.1f;
        PlayerManager.instance.markerBulletController.bulletSate.bulletDamage +=
            PlayerManager.instance.markerBulletController.bulletSate.bulletDamage % 0.3f;
    }
}