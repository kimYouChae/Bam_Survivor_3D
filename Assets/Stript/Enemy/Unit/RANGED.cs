using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MELLE;

public class RANGES : Unit
{
    private void Awake()
    {

    }

    // 켜졌을 때 enter (pool에서 on 될 때 )
    private void OnEnable()
    {

    }

    private void Update()
    {

    }

    internal class RANGED_Attack : IAttackStrategy
    {
        public void Attack(Unit _unit)
        {
            Debug.Log("RANGED가 Attack을 합니다");
        }
    }

}
