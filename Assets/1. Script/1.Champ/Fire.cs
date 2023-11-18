using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.PlayerSettings;

public class Fire : MonoBehaviour
{
    public PoolManager bulletPool;
    public PoolManager skillPool;
    public Targeting layder;
    public GameManager gm;

    public bool inField;
    //��⼮�� ��ȣ
    public int seaNum;

    public int cost;
    public int num;
    public int lv;

    SpriteRenderer star;
    Champitems ci;
    Synergy sg;

    public float[] defDmg = new float[3];
    public float[] defSkillDmg = new float[3];
    public float defShootTime;
    public float defShootSpeed;
    public float defCriPer;
    public float defCriDmg;
    public float defMaxMp;

    public float maxMp;
    public float mp;
    public float dmg;
    public float skillDmg;
    public float shootTime;
    public float shootSpeed;
    public float criPer;
    public float criDmg;

    int skilluse = 1;
    int debuffCount = 0;
    int moreSpeedCount = 0;
    int moreSkillAttSpeed = 0;
    int moreCriAttSpeed = 0;

    float timespeed;

    private void Awake()
    {
        star = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
        ci = GetComponent<Champitems>();
        sg = GetComponent<Synergy>();
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
        //���� �ʱ�ȭ �� ��ų �� �ʱ�ȭ
        maxMp = defMaxMp - ci.cMaxMp;
        mp = ci.cMp;

        //agHeal3 ����ü ���� ����
        float agHeal3 = inField && GameManager.augmentation[26] && GameManager.passEnemy ? 70 : 1;
        //agBoold1 ����ü ���� ����
        int quotient = (int)System.Math.Truncate(gm.maxHp - gm.hp);
        float agBoold1 = inField && GameManager.augmentation[17] ? quotient*3 : 0;
        //agBoold2 ����ü ���� ����
        float agBoold2 = inField && GameManager.augmentation[27] ? (gm.maxHp - gm.hp) : 0;

        dmg = (defDmg[lv]+ agBoold1+ agBoold2+ ci.cDmg) * ((AgManager.agAtDmg1 + AgManager.agAtDmg2 + AgManager.agAtDmg3 + agHeal3) / 100);
        skillDmg = defSkillDmg[lv];
        timespeed = Time.deltaTime + Time.deltaTime * ((AgManager.agAtSpeed1 + AgManager.agAtSpeed2 + AgManager.agAtSpeed3 + ci.cAttSpeed) / 100);
        shootSpeed = defShootSpeed;
        criPer = defCriPer+ ci.cCriPer;
        criDmg = defCriDmg + ci.cCriDmg;
        layder.scanRange = layder.defScanRange + SynergyManager.sDistance+ ci.cDistance;
        moreSpeedCount = 0;
        debuffCount = 0;
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

        if(mp < maxMp)
        {
            debuffCount++;
          

            int moreAtt = Random.Range(0, 100);
            if (ci.cMoreAtt && moreAtt < 20) //Ȱ�� 2Ÿ ������ Ȯ�� ���ǹ�
                shootTime = 0.1f;
            else
                shootTime = defShootTime;
            PlBullet pb = bulletPool.Get(SkillNum()).gameObject.GetComponent<PlBullet>();
            pb.gameObject.transform.position = transform.position;
            pb.targetEnemy = layder.nearestTarget;
            pb.shootSpeed = shootSpeed;
            pb.debuff = ci.cDefDebuff && debuffCount > 2 ? true : false;

                //ũ�� ���
                int cri = Random.Range(0, 100);
            int superCri = Random.Range(0, 100);
            pb.dmg = cri < criPer ?  ci.cSuperCri && superCri < 10 ?
            (dmg + (dmg * (150 + criDmg) / 100)) //����ũ��
            : dmg + (dmg * (50 + criDmg) / 100) ://ũ��
               dmg;//�⺻ ��ų��

            pb.cri = cri < criPer ? true : false;
            pb.trueDmg = ci.cTrueDmg;

            moreCriAttSpeed = ci.cMoreCriAttSpeed > 0 && cri < criPer ? moreCriAttSpeed++ : moreCriAttSpeed;

            if (ci.cMoreAttSpeed > 0 || ci.cMoreCriAttSpeed > 0)
            {
                moreSpeedCount++;
                timespeed = Time.deltaTime +
                    Time.deltaTime * ((AgManager.agAtSpeed1 + AgManager.agAtSpeed2 + AgManager.agAtSpeed3 + ci.cAttSpeed +
                    (moreSpeedCount * ci.cMoreAttSpeed) + (moreSkillAttSpeed * ci.cMoreSkillAttSpeed) + (moreCriAttSpeed * ci.cMoreCriAttSpeed)) / 100);
            }

            //���� ȸ��
            mp += (5+ci.cAttMp);
            mp += cri < criPer ? ci.cCriMp : 0;
        }
        else
        {
            shootTime = defShootTime;
            ChampSkill cs = skillPool.Get(SkillNum()).gameObject.GetComponent<ChampSkill>();
            cs.gameObject.transform.position = transform.position;
            cs.targetEnemy = layder.nearestTarget;
            cs.shootSpeed = shootSpeed;


            //ũ�� ���
            int cri = Random.Range(0, 100);
            int superCri = Random.Range(0, 100);
            float skill = (dmg * skillDmg) * (100+ ci.cSkillMDmg)* skilluse + ci.cSkillDmg / 100;

            cs.dmg = ci.cSkillCri &&  cri < criPer ? 
                ci.cSuperCri && superCri < 10 ?
                (skill + (skill * (150 + criDmg) / 100)) //����ũ��
                : skill + (skill * (50 + criDmg) / 100) ://ũ��
                skill;//�⺻ ��ų��
            cs.trueDmg = ci.cTrueDmg;

            cs.cri = ci.cSkillCri && cri < criPer ? true : false;
            cs.superCri = cs.cri && ci.cSuperCri && superCri > 10 ? true : false;

            moreCriAttSpeed = ci.cMoreCriAttSpeed > 0 && cri < criPer ? moreCriAttSpeed++ : moreCriAttSpeed;

            if (ci.cMoreSkillAttSpeed > 0 || ci.cMoreCriAttSpeed > 0)
            {
                moreSkillAttSpeed++;
                timespeed = Time.deltaTime +
                    Time.deltaTime * ((AgManager.agAtSpeed1 + AgManager.agAtSpeed2 + AgManager.agAtSpeed3 + ci.cAttSpeed +
                    (moreSpeedCount * ci.cMoreAttSpeed) + (moreSkillAttSpeed * ci.cMoreSkillAttSpeed) + (moreCriAttSpeed * ci.cMoreCriAttSpeed)) / 100);
            }

            //���� �ʱ�ȭ
            mp = 0;

            skilluse++;
        }
        
    }
    int SkillNum()
    {
        int champNum = 0;

        switch (sg.champNum)
        {
            case < 20:
                
                champNum = sg.champNum - 11;
                break;
            
            case <30:
                champNum = sg.champNum - 13;
                break;
            case <40:
                champNum = sg.champNum - 15;
                break;
            case < 50:
                champNum = sg.champNum - 17;
                break;
            case < 60:
                champNum = sg.champNum - 19;
                break;
        }
        Debug.Log(champNum + "������ ��ų �ߵ�");
        return champNum;
    }
}
