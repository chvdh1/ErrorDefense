using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    public int enemyNum;

    public float MaxHp;
    public float hp;

    public int dmg;

    float allattdmg;
    float defhittime = 1;
    float hittime = 0;
    private void OnEnable()
    {
        hp = MaxHp;
        hittime = 0;
        allattdmg = SynergyManager.allAttdmg;
    }

    private void Update()
    {
        hittime += Time.deltaTime;

        if (hittime >= defhittime)
        {
            hp -= allattdmg;
            hittime = 0;
        }
    }

}
