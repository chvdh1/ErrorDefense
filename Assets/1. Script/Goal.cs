using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public GameManager gm;
    UIManager ui;

    private void Awake()
    {
        gm = GameManager.Instance;
        ui = UIManager.uIManager;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 7)
        {
            EnemyStat es = collision.gameObject.GetComponent<EnemyStat>();
            gm.hp -= es.dmg;

            collision.gameObject.SetActive(false);

            ui.HpUpdate();
            if (gm.hp < 0)
                GameManager.gameover();
        }
    }
}
