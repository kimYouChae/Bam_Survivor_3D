using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Common_BigBullet : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.cardName);

        // �Ѿ�ũ�� 30% ���� (PlayerManager�� markerbulletController�� ����)
        PlayerManager.instance.markerBulletController.bulletSate.bulletSize
            += PlayerManager.instance.markerBulletController.bulletSate.bulletSize * 0.3f;

    }
}
public class Common_DamageUp : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.cardName);

        // �Ѿ� ������ 20% ���� (PlayerManager�� markerbulletController�� ����)
        PlayerManager.instance.markerBulletController.bulletSate.bulletDamage
            += PlayerManager.instance.markerBulletController.bulletSate.bulletDamage * 0.2f;
    }
}
public class Common_RapidReload : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.cardName);

        // �Ѿ� ������ �ӵ��� 10% ���� (PlayerManager�� markerState ����)

        for (int i = 0; i < PlayerManager.instance.F_MarkerListCount(); i++)
        {
            PlayerManager.instance.markers[i].markerState.markerShootCoolTime
                += PlayerManager.instance.markers[i].markerState.markerShootCoolTime * 0.1f;
        }
    }
}