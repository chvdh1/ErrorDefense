using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlBullet : MonoBehaviour
{
    public float dmg;
    public float shootSpeed;
    public Transform targetEnemy;
    public bool cri;

    private void Update()
    {
        if (targetEnemy != null)
            transform.position =
               Vector2.MoveTowards(transform.position,
               targetEnemy.position, shootSpeed * Time.deltaTime);
        else
            transform.position =
               Vector2.MoveTowards(transform.position,
               Vector2.left, shootSpeed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer ==7)
        {
            EnemyStat es = collision.gameObject.GetComponent<EnemyStat>();

            es.hp -= dmg;

            if (es.hp <= 0)
            {
                collision.gameObject.SetActive(false);
                GameManager.curEnemy++;
            }

            float slowran = Random.Range(0, 100);
            if(slowran < 45)
            {
                EnemyMove em = collision.gameObject.GetComponent<EnemyMove>();
                if (!em.slow)
                    em.SlowE(SynergyManager.slow, SynergyManager.slowX);
            }

            gameObject.SetActive(false);
        }
    }
}
