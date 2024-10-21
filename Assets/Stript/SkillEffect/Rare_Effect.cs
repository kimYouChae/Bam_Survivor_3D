using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rare_PoisionBullet : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // �Ѿ� ���� �� ��Ÿ� �ȿ� �ִ� unit���� �� ȿ�� 
        // markerExplosion �� �Լ��� ȣ���ϸ� ���ҵ�? ���⼭ ���� �ۼ����ص� ?
    }
}
public class Rare_RapidBarier : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // ���� ��Ÿ�� 10% ����
        PlayerManager.instance.F_UpdateMarkerState(ShieldCoolTimePercent : 0.1f);
    }
}
public class Rare_IceBullet : SkillCard
{

    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // �Ѿ� ���� �� ��Ÿ� �ȿ� �ִ� unit���� ����ȿ�� 
    }
}

public class Rare_ShieldExpention : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // ���� ���� 10% ����   
        PlayerManager.instance.markerShieldController.F_UpdateShieldState(ShieldSizePercent : 0.1f);
    }
}

public class Rare_MagneticUP : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // �ڼ� ���� 10% ����
        PlayerManager.instance.F_UpdateMarkerState(MagnetPercent: 0.1f);
    }
}