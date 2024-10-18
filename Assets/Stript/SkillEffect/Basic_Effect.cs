using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Tanker : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // marker�� max hp�� 30�� ���� ( PlayerManager�� marker ����Ʈ�� ���� )
        PlayerManager.instance.F_UpdateMarkerState(MaxHpPercent : 0.3f);
        

    }
}
public class Basic_Speeder : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.classSpriteName);

        // marker�� ������ �ӵ� 20% ���� ( PlayerManager�� marker ����Ʈ�� ���� )
        PlayerManager.instance.F_UpdateMarkerState(SpeedPercent : 0.2f);
    }
}

public class Basic_NaturalRecovery : SkillCard
{
    public override void F_SkillcardEffect()
    {
        // �ڿ�ȸ������ 0.5f ������
        PlayerManager.instance.F_UpdateMarkerState(RecoveryIncrease: 0.5f);
    }
}

public class Basic_QuickRecovery : SkillCard
{
    public override void F_SkillcardEffect()
    {
        // �ڿ�ȸ���� ��Ÿ���� 5% ���� 
        PlayerManager.instance.F_UpdateMarkerState(RecoveryCoolTimeDecrease: 0.05f);
    }
}

public class Basic_RapidBullet : SkillCard
{
    public override void F_SkillcardEffect()
    {
        // ���˼ӵ��� 10% �����մϴ�
    }
}