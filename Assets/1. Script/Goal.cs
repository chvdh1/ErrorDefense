using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public Transform manager; 
    
    GameManager gm;
    UIManager ui;

    private void Awake()
    {
        gm = manager.GetChild(1).gameObject.GetComponent<GameManager>();
        ui = manager.GetChild(0).gameObject.GetComponent<UIManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 7)
        {
            EnemyStat es = collision.gameObject.GetComponent<EnemyStat>();
            gm.hp -= es.dmg;
            GameManager.curEnemy++;
            collision.gameObject.SetActive(false);
            gm.passEnemy = true;
            ui.HpUpdate();
            if (gm.hp <= 0)
                GameManager.gameover();
        }
    }
}
