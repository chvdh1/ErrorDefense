using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public PoolManager bulletPool;
    public Targeting layder;

    public bool inField;
    //대기석의 번호
    public int seaNum;

    public int cost;
    public int num;
    public int lv;

    SpriteRenderer star;

    public float[] defDmg = new float[3];
    public float defShootTime;
    public float defShootSpeed;
    public float defCriPer;
    public float defCriDmg;

    public float dmg;
    public float shootTime;
    public float shootSpeed;
    public float criPer;
    public float criDmg;

    private void Awake()
    {
        star = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        shootTime = defShootTime;
        shootSpeed = defShootSpeed;
        criPer = defCriPer;
        criDmg = defCriDmg;
        LvUp();
    }

    private void Update()
    {
        if (!inField)
            return;

        if (shootTime > 0)
            shootTime -= Time.deltaTime;
        else
            Shoot();
    }

    public void LvUp()
    {
        dmg = defDmg[lv];
        switch(lv)
        {
            case 0:
                star.color = new Color(1, 1, 1, 0);
                break;
            case 1:
                star.color = new Color(0.6f, 0.6f, 0.6f, 1);
                break;
            case 2:
                star.color = new Color(1,1, 0, 1);
                break;
        }
        
    }

    void Shoot()
    {
        if (layder.nearestTarget == null)
            return;

        shootTime = defShootTime;
        PlBullet pb = bulletPool.Get(0).gameObject.GetComponent<PlBullet>();
        pb.gameObject.transform.position = transform.position;
        pb.targetEnemy = layder.nearestTarget;
        pb.shootSpeed = shootSpeed;
        //크리 계산
        int cri = Random.Range(0, 100);
        pb.dmg = cri < criPer ? dmg + (dmg * (50+ criDmg) /100) : dmg;
        pb.cri = cri < criPer ? true : false;
    }
}
