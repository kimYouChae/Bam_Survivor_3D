using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public abstract class FSM 
{
    // ���� ����, 1ȸ ����
    public abstract void FSM_Enter();

    // ���� ���� , �������� ����
    public abstract void FSM_Excute();

    // ���� ����, 1ȸ ����
    public abstract void FSM_Exit();
}
