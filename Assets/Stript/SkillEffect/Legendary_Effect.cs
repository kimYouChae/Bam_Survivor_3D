using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legend_BombShield : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.cardName);

        // ���� ��� �� �ܰ��� ��ź ���� 
    }
}

public class Legend_Supernova : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.cardName);

        // ���� ��� �� unit�� �߽����� ������ ���� ������ ��
    }
}

public class Legend_Mayhem : SkillCard
{

    public override void F_SkillcardEffect()
    {
        Debug.Log(this.cardName);

        // �Ѿ� ƨ��� ���� +3 , �Ѿ� ũ�� 10% ����, �Ѿ� ������ 30% ����
        // (PlayerManager�� markerBulletController�� ����)

        PlayerManager.instance.markerBulletController.bulletSate.bulletBounceCount += 3;
        PlayerManager.instance.markerBulletController.bulletSate.bulletSize +=
            PlayerManager.instance.markerBulletController.bulletSate.bulletSize % 0.1f;
        PlayerManager.instance.markerBulletController.bulletSate.bulletDamage +=
            PlayerManager.instance.markerBulletController.bulletSate.bulletDamage % 0.3f;
    }
}