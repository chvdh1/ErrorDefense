using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public int maxHp;
    public int hp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 7)
        {
            EnemyStat es = collision.gameObject.GetComponent<EnemyStat>();
            hp -= es.dmg;

            if (hp < 0)
                GameManager.gameover();
        }
    }
}
