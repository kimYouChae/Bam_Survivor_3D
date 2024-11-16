using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rare_PoisionBullet : SkillCard
{
    public override void F_SkillcardEffect(Transform _trs, float _size)
    {
        Debug.Log(this.classSpriteName);

        // �Ѿ� ���� �� ��Ÿ� �ȿ� �ִ� unit���� �� ȿ�� 
        // PlayerManager�� MarkerExplotion Controller�� ���� 
        PlayerManager.Instance.markerExplosionConteroller.F_PositionBullet(_trs , _size);
    }
}
public class Rare_RapidBarier : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // ���� ��Ÿ�� 10% ����
        PlayerManager.Instance.F_UpdateMarkerState(ShieldCoolTimePercent : 0.1f);
    }
}
public class Rare_IceBullet : SkillCard
{

    public override void F_SkillcardEffect(Transform _trs, float _size)
    {
        Debug.Log(this.classSpriteName);

        // �Ѿ� ���� �� ��Ÿ� �ȿ� �ִ� unit���� ����ȿ�� 
        // PlayerManager�� MarkerExplotion Controller�� ���� 
        PlayerManager.Instance.markerExplosionConteroller.F_IceBullet(_trs, _size);
    }
}

public class Rare_ShieldExpention : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // (PlayerState)���� ���� 10% ����   
        ShieldManager.Instance.F_UpdateShieldState(ShieldSizePercent : 0.1f);
    }
}

public class Rare_MagneticUP : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // �ڼ� ���� 10% ����
        PlayerManager.Instance.F_UpdateMarkerState(MagnetPercent: 0.1f);
    }
}