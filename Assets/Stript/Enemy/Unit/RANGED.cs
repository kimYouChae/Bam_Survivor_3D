using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MELLE;

public class RANGES : Unit
{
    private void Awake()
    {

    }

    // ������ �� enter (pool���� on �� �� )
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
            Debug.Log("RANGED�� Attack�� �մϴ�");
        }
    }

}
