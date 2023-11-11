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
            GameManager.passEnemy = true;
            ui.HpUpdate();
            if (gm.hp <= 0)
                GameManager.gameover();

            //증강체 관련 함수
            if (GameManager.augmentation[9] || GameManager.augmentation[19] || GameManager.augmentation[29])
                for (int i = 0; i < gm.fieldEnemys.Length; i++)
                {
                    EnemyStat aes = gm.fieldEnemys[i].gameObject.GetComponent<EnemyStat>();
                    if (gm.fieldEnemys[i].activeSelf)
                    {
                        if (GameManager.augmentation[29])
                            aes.def = 0;
                        if (GameManager.augmentation[9])
                            aes.hp -= 10;
                        if (GameManager.augmentation[19])
                            aes.hp -= aes.maxHp / 10;
                    }
                    
                }
        }
    }
}
