using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legend_HealingField : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // ���� ��� �� �ܰ��� ��ź ���� 
    }
}

public class Legend_Supernova : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // ���� ��� �� unit�� �߽����� ������ ���� ������ ��
    }
}

public class Legend_Mayhem : SkillCard
{

    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // �Ѿ� ƨ��� ���� +3 , �Ѿ� ũ�� 10% ����, �Ѿ� ������ 30% ����
        // (PlayerManager�� markerBulletController�� ����)
        PlayerManager.instance.markerBulletController.F_UpdateBulletState(  BulletDamagePercent : 0.3f , BulletSizePercent : 0.1f, BulletBounceCount : 3 );
    }
}

public class Legend_ExtraLife : SkillCard
{
    public override void F_SkillcardEffect()
    {
        // ��ȰȽ�� 1ȸ ���� 
        PlayerManager.instance.F_UpdateMarkerSubState(RevivalCount : 1);
    }
}