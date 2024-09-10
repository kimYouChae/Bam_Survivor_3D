using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Epic_Bounce : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.cardName);

        // �Ѿ� bounce ���� 1�� ���� (PlayerManager�� markerBulletController�� ����)
        PlayerManager.instance.markerBulletController.bulletSate.bulletCount += 1;
    }
}
public class Epic_BulletStrom : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.cardName);

        // �Ѿ˰��� 2�� ���� (PlayerManager�� markerBulletController�� ����)
        PlayerManager.instance.markerBulletController.bulletSate.bulletBounceCount += 2;
    }
}

public class Epic_BloodSiphon : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.cardName);

        // ���� ���� ���� �� unit ���� 
    }
}