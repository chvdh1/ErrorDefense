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

    public bool slow;//슬로우
    public bool knockback; //넉백
    public bool stun; //기절

    //투사체가 아닐 경우
    public bool spAtt;

    bool sActive;

    BtnManager bt;

    private void OnEnable()
    {
        slow = false;
        knockback = false;
        stun = false;
        sActive = true;
        bt = BtnManager.Btn;
        SkillEf();
    }

    public void SkillEf()
    {
        StartCoroutine(Move(2));

        switch (champNum)
        {
            case 1:
                break;

        }
    }


    IEnumerator Move(float time)
    {
        while (time > 0 && sActive)
        {
            if (targetEnemy != null)
                transform.position =
                   Vector2.MoveTowards(transform.position,
                   targetEnemy.position, shootSpeed * Time.fixedDeltaTime);
            else
                transform.position =
                   Vector2.MoveTowards(transform.position,
                   Vector2.left, shootSpeed * Time.fixedDeltaTime);

            time -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        sActive = false;
        gameObject.SetActive(false);

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
