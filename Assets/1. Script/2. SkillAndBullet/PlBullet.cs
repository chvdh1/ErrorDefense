
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlBullet : MonoBehaviour
{
    public int num;

    public float dmg;

    public float trueDmg;
    public float shootSpeed;
    public Transform targetEnemy;
    public bool cri;
    public bool superCri;
    public bool debuff;//방어력 감소

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
        BulletEf();
    }
    public void BulletEf()
    {

        if(!spAtt) //특수 공격은 나중에.
           StartCoroutine(Move(2));


        if (num == 13 || num == 43 || num == 53 )//넉백 확률
        {
            int ran = Random.Range(0, 100);
            knockback = num == 13 && ran < 25 ? true : num == 43 && ran < 35 ? true : num == 53 && ran < 55 ? true : false;
        }

        if (num == 16 || num == 26 || num == 36 || num == 46 || num == 56 || num == 33 || num == 53)////슬로우
        {
            int ran = Random.Range(0, 100);
            int tint = num == 16 || num == 26 || num == 36 || num == 46 || num == 56 ? 30 : 80;
            slow = ran < tint ? true : false;
        }

        if (num == 53 || num == 57)//기절
        {
            int ran = Random.Range(0, 100);
            int tint = num == 53 ? 25 : 10;
            slow = ran < tint ? true : false;
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
        if(collision.gameObject.layer ==7)
        {
            EnemyStat es = collision.gameObject.GetComponent<EnemyStat>();

            es.hp -= dmg - es.def > 1 ? dmg - es.def : 1;
            es.hp -= trueDmg;

            if (debuff)
                es.def = es.defDef * 0.7f;

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
                        else if(i == bt.itemList.Length-1)
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

            sActive = false;

            gameObject.SetActive(false);
        }
    }
}
