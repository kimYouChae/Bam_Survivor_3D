using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Tanker : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.cardName);

        // marker�� max hp�� 30�� ���� ( PlayerManager�� marker ����Ʈ�� ���� )

        for(int i = 0; i < PlayerManager.instance.F_MarkerListCount(); i++) 
        {
            PlayerManager.instance.markers[i].markerState.markerMaxHp 
                += PlayerManager.instance.markers[i].markerState.markerMaxHp * 0.3f;
        }

    }
}
public class Basic_Speeder : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.cardName);

        // marker�� ������ �ӵ� 20% ���� ( PlayerManager�� marker ����Ʈ�� ���� )
        for (int i = 0; i < PlayerManager.instance.F_MarkerListCount(); i++)
        {
            PlayerManager.instance.markers[i].markerState.markerMaxHp 
                += PlayerManager.instance.markers[i].markerState.markerMoveSpeed * 0.2f; 
        }
    }
}
public class Basic_Homing : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.cardName);

        // �Ѿ��� unit�� ����
    }
}