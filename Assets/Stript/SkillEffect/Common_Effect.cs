using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Common_BigBullet : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // �Ѿ�ũ�� 30% ���� (PlayerManager�� markerbulletController�� ����)
        PlayerManager.instance.markerBulletController.bulletSate.bulletSize
            += PlayerManager.instance.markerBulletController.bulletSate.bulletSize * 0.3f;

    }
}
public class Common_DamageUp : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // �Ѿ� ������ 20% ���� (PlayerManager�� markerbulletController�� ����)
        PlayerManager.instance.markerBulletController.bulletSate.bulletDamage
            += PlayerManager.instance.markerBulletController.bulletSate.bulletDamage * 0.2f;
    }
}
public class Common_RapidReload : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // �Ѿ� ������ �ӵ��� 10% ���� (PlayerManager�� markerState ����)

    }
}

public class Common_DefenceUP : SkillCard
{
    public override void F_SkillcardEffect()
    {
        // ���� 10% ���� 
        PlayerManager.instance.F_UpdateMarkerState(DefencePercent : 0.2f);
    }
}