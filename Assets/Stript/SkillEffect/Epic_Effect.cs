using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Epic_Bounce : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // �Ѿ� bounce ���� 1�� ���� (PlayerManager�� markerBulletController�� ����)
        PlayerManager.instance.markerBulletController.F_UpdateBulletState(BulletCnt : 1);
    }
}
public class Epic_BulletStrom : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // �Ѿ˰��� 2�� ���� (PlayerManager�� markerBulletController�� ����)
        PlayerManager.instance.markerBulletController.F_UpdateBulletState(BulletBounceCount: 2);
    }
}

public class Epic_BloodSiphon : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // ���� ���� ���� �� unit ���� 
    }
}

public class Epic_ExperienceBoost : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // ����ġ ȹ�淮 5% ����
        PlayerManager.instance.F_UpdateMarkerSubState(ExperiencePercent : 0.05f);
    }
}

public class Epic_TouchOfLuck : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // ��� 5% ����
        PlayerManager.instance.F_UpdateMarkerSubState(LuckPercent : 0.05f);
    }
}