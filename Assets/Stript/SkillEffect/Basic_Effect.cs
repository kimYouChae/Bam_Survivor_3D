using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Tanker : SkillCard
{
    public override void F_SkillcardEffect()
    {
        Debug.Log(this.cardName);

        // marker의 max hp가 30퍼 증가 ( PlayerManager의 marker 리스트에 접근 )

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

        // marker의 움직임 속도 20% 증가 ( PlayerManager의 marker 리스트에 접근 )
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

        // 총알이 unit을 따라감
    }
}