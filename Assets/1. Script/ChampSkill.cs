using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ChampSkill : MonoBehaviour
{


    public int champNum;

    public PoolManager poolManager;
    public float dmg;
    public float trueDmg;
    public float shootSpeed;
    public Transform targetEnemy;
    public bool cri;
    public bool superCri;

    BtnManager bt;

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

    private void OnEnable()
    {
        bt = BtnManager.Btn;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            EnemyStat es = collision.gameObject.GetComponent<EnemyStat>();

            es.hp -= dmg - es.def > 1 ? dmg - es.def : 1;
            es.hp -= trueDmg;

            if (es.hp <= 0)
            {
                collision.gameObject.SetActive(false);
                GameManager.curEnemy++;
                if (es.haveItem)
                {
                    int itemran = Random.Range(0, 7);
                    RectTransform item = GameManager.itempool.Get(itemran).GetComponent<RectTransform>();
                    for (int i = 0; i < bt.itemList.Length; i++)
                    {
                        if (bt.itemList[i] == null)
                        {
                            bt.itemList[i] = item.gameObject;
                            item.SetParent(ItemList.itemListTrans);
                            break;
                        }
                        else if (i == bt.itemList.Length - 1)   
                        {
                            item.gameObject.SetActive(false);
                            break;
                        }
                    }
                }
            }

            float slowran = Random.Range(0, 100);
            if (SynergyManager.slow > 0 && slowran < 30)
            {
                EnemyMove em = collision.gameObject.GetComponent<EnemyMove>();
                if (!em.slow)
                    em.SlowE(SynergyManager.slow, SynergyManager.slowX);
            }

            gameObject.SetActive(false);
        }
    }

   
}
