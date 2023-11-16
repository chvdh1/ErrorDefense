using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    public int enemyNum;

    public float maxHp;
    public float hp;

    public float defDef;
    public float def;

    public int dmg;

    float allattdmg;
    float defhittime = 1;
    float hittime = 0;

    public bool haveItem;

    private void OnEnable()
    {
        hp = maxHp;
        def = defDef;
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
