using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Epic_Bounce : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.cardName);

        // ÃÑ¾Ë bounce °¹¼ö 1°³ Áõ°¡ (PlayerManagerÀÇ markerBulletController¿¡ Á¢±Ù)
        PlayerManager.instance.markerBulletController.bulletSate.bulletCount += 1;
    }
}
public class Epic_BulletStrom : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.cardName);

        // ÃÑ¾Ë°¹¼ö 2°³ Áõ°¡ (PlayerManagerÀÇ markerBulletController¿¡ Á¢±Ù)
        PlayerManager.instance.markerBulletController.bulletSate.bulletBounceCount += 2;
    }
}

public class Epic_BloodSiphon : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.cardName);

        // ½¯µå »ç¿ë½Ã ¹üÀ§ ³» unit ÈíÇ÷ 
    }
}