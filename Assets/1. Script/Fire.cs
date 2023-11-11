using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public PoolManager bulletPool;
    public Targeting layder;
    public GameManager gm;

    public bool inField;
    //대기석의 번호
    public int seaNum;

    public int cost;
    public int num;
    public int lv;

    SpriteRenderer star;
    SkillManager sm;
    Champitems ci;

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

    float timespeed;

    private void Awake()
    {
        star = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
        sm = GetComponent<SkillManager>();
        ci = GetComponent<Champitems>();
    }

    private void OnEnable()
    {
        LvUp();
    }

    private void Update()
    {
        if (!inField)
            return;

        if (shootTime > 0)
            shootTime -= timespeed;
        else
            Shoot();
    }
    public void StatUpdate()
    {
        ci.ItemEffect();
        //마나 초기화 및 스킬 딜 초기화
        sm.mp = ci.cMp;
        sm.skillDmgPer = sm.defSkillDmgPer + ci.cSkillDmg;

        //agHeal3 증강체 관련 변수
        float agHeal3 = inField && GameManager.augmentation[26] && GameManager.passEnemy ? 70 : 1;
        //agBoold1 증강체 관련 변수
        int quotient = (int)System.Math.Truncate(gm.maxHp - gm.hp);
        float agBoold1 = inField && GameManager.augmentation[17] ? quotient*3 : 0;
        //agBoold2 증강체 관련 변수
        float agBoold2 = inField && GameManager.augmentation[27] ? (gm.maxHp - gm.hp) : 0;

        dmg = (defDmg[lv]+ agBoold1+ agBoold2+ ci.cDmg) * ((AgManager.agAtDmg1 + AgManager.agAtDmg2 + AgManager.agAtDmg3 + agHeal3) / 100);
        timespeed = Time.deltaTime + Time.deltaTime * ((AgManager.agAtSpeed1 + AgManager.agAtSpeed2 + AgManager.agAtSpeed3 + ci.cAttSpeed) / 100);
        shootSpeed = defShootSpeed;
        criPer = defCriPer+ ci.cCriPer;
        criDmg = defCriDmg + ci.cCriDmg;
    }

    public void LvUp()
    {
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

        //마나 회복
        sm.mp += 5;
    }
}
