using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    public int enemyNum;

    public float MaxHp;
    public float hp;

    public int dmg;

    private void OnEnable()
    {
        hp = MaxHp;
    }

}
