using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public abstract class FSM 
{
    // 상태 진입, 1회 실행
    public abstract void FSM_Enter();

    // 상태 실행 , 매프레임 실행
    public abstract void FSM_Excute();

    // 상태 종료, 1회 실행
    public abstract void FSM_Exit();
}
