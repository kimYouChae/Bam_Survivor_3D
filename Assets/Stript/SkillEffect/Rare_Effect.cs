using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rare_PoisionBullet : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.cardName);

        // �Ѿ� ���� �� ��Ÿ� �ȿ� �ִ� unit���� �� ȿ�� 
        // markerExplosion �� �Լ��� ȣ���ϸ� ���ҵ�? ���⼭ ���� �ۼ����ص� ?
    }
}
public class Rare_RapidBarier : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.cardName);

        // ���� ���ð� 10% ���� (PlayerManager�� markerState ����)
        for (int i = 0; i < PlayerManager.instance.F_MarkerListCount(); i++)
        {
            PlayerManager.instance.markers[i].markerState.markerShieldCoolTime
                += PlayerManager.instance.markers[i].markerState.markerShieldCoolTime * 0.1f;
        }
    }
}
public class Rare_IceBullet : SkillCard
{

    public override void F_SkillcardEffect()
    {
        Debug.Log(this.cardName);

        // �Ѿ� ���� �� ��Ÿ� �ȿ� �ִ� unit���� ����ȿ�� 
    }
}