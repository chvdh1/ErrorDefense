using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using System;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;
using UnityEngine.PlayerLoop;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public UIManager ui;
    public WaitingSeat ws;
    public ShopManager sm;
    public BtnManager bt;
    public UnitLvManager ul;
    public AgManager am;
    public static PoolManager itempool;
    public Transform poolG;
    public int lv;

    public GameObject[] fieldUnit=new GameObject[15];
    public GameObject[] fieldEnemys = new GameObject[20];

    public float maxHp;
    public float hp;

    public float[] maxExp;
    public float exp;
    public int coin;
    public int continuity; // ������ ����
    public int champBlank;
    public int[] synergy = new int[21]; //0 ��� / 1ħ�� /2 ���� /3 �ӱ�/4 ���/5����/6������/7������Ÿ����
    public bool[] synergyOverlapCk = new bool[60];//è���� ���� �ó��������� ���� �������� ����
    public static bool[] augmentation = new bool[30]; // ����
    int[] tierPercentage = new int[5]; //�� ������ �´� Ȯ�� ����


    public static Action gameover;

    public int gamestat; // 0 = �κ� , 1 ���� ���� , 2 ������


    //�������� ���� ����
    public int mapIndex;
    public GameObject maps;
    public GameObject[] mapPointParent;
    public GameObject[] mapPoints;
    public GameObject noSetP;
    GameObject[] noSet;
    public int stageIndex;
    public static bool passEnemy;

    public static int totalEnemy;
    public static int curEnemy;

    //�� ���� ����
    public PoolManager enemyPool;
    public PoolManager bulletPool;
    public PoolManager skillPool;
    public GameObject goal;
    public Transform[] spawnPos;


    private void Awake()
    {
        Instance = this;
        gameover = () => { GameOver(); };
        bt = GetComponent<BtnManager>();

        int mapPointParents = maps.transform.childCount;
        mapPointParent = new GameObject[mapPointParents];
        for (int i = 0; i < mapPointParent.Length; i++)
        {
            mapPointParent[i] = maps.transform.GetChild(i).gameObject;
        }
        int n = noSetP.transform.childCount;
        noSet = new GameObject[n];
        for (int i = 0; i < noSet.Length; i++)
        {
            noSet[i] = noSetP.transform.GetChild(i).gameObject;
        }
        ul = GetComponent<UnitLvManager>();
        itempool = poolG.GetChild(13).gameObject.GetComponent<PoolManager>();
        Application.targetFrameRate = 60; //���� ������ �ӵ� 60���������� ���� ��Ű��.. �ڵ�
        QualitySettings.vSyncCount = 0;
        //����� �ֻ���(�÷�����)�� �ٸ� ��ǻ���� ��� ĳ���� ���۽� ������ ������ �� �ִ�.
    }

    public void GameStart()
    {
        //�ִ�ü������ ����
        maxHp = 100;
        hp = maxHp;
        lv = 1;
        coin = 0;
        exp = 0;
        ui.CoinUpdate();
        ui.ExpUpdate();
        ui.ContinuityUpdate();

        //�������� �ʱ�ȭ
        stageIndex = 1;
        passEnemy = false;

        //���� �� ��ȯ, �׿� ��Ȱ��ȭ
        for (int i = 0; i < noSet.Length; i++)
        {
            if(i == mapIndex-1)
            {
                noSet[i].SetActive(true);
                mapPointParent[i].SetActive(true);
            }
            else
            {
                noSet[i].SetActive(false);
                mapPointParent[i].SetActive(false);
            }
        }

        //�ó���,����ü �� è�� ����Ʈ �ʱ�ȭ
        ui.SynergyReset();
        sm.ChampReset();
        am.AgClassSet();

        //1�� ���� Ȱ��ȭ
        int z = UnityEngine.Random.Range(0, sm.cost1.Count);
        Transform ch = bt.costObjs[0].Get(z).transform;
        sm.cost1[z].cCount++;


        ChampMng cm = ch.gameObject.GetComponent<ChampMng>();
        cm.cSkill.poolManager = skillPool;
        cm.cFire.skillPool = skillPool;
        cm.cFire.gm = Instance;
        cm.cFire.bulletPool = bulletPool;
        cm.cFire.StatUpdate();
        ws.obj[0] = ch.gameObject;
        ch.position = ws.pos[0].transform.position;

        ul.lv1Units[z][0] = ch.gameObject;

        //�� ��������Ʈ ����
        int count = mapPointParent[mapIndex-1].transform.childCount;
        mapPoints = new GameObject[count+1];
        for (int i = 0; i < mapPoints.Length; i++)
        {
            if (i < mapPoints.Length - 1)
                mapPoints[i] = mapPointParent[mapIndex-1].transform.GetChild(i).gameObject;
            else
                mapPoints[i] = goal;
        }

        StartCoroutine(DelayTime());
    }

    public void GameOver()
    {
        Time.timeScale = 0;
    }

    IEnumerator DelayTime()
    {
        float t = stageIndex == 1 ? 5 : stageIndex == 5 || stageIndex  == 12 || stageIndex == 19 ? 15 : 10;
        float maxt = t;

        //����ü ȹ��(5,12,19)
        if (stageIndex == 5 || stageIndex == 12 || stageIndex == 19)
            am.SpawnAg();

        //�����
        gamestat = 1;

        yield return new WaitForFixedUpdate();
        while(t>0)
        {
            t -= Time.fixedDeltaTime;
            ui.timeBar.fillAmount = t / maxt;
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForFixedUpdate();
        StartCoroutine(SpawnEnemy());
    }


    IEnumerator SpawnEnemy()
    {
        int enemySpawnindex = stageIndex + 2;
        int enemytype = 1;
        int enemytypeMin = 0;

        //��âü ���� �Լ�(����)
        for (int i = 0; i < fieldUnit.Length; i++)
        {
            if (fieldUnit[i] != null)
            {
                ChampMng cm = fieldUnit[i].GetComponent<ChampMng>();
                cm.cSkill.poolManager = skillPool;
                cm.cFire.skillPool = skillPool;
                cm.cFire.gm = Instance;
                cm.cFire.StatUpdate();
            }
            yield return new WaitForFixedUpdate();
        }
        if (augmentation[8])
        { StartCoroutine(Augmentation8()); }
        if (augmentation[18])
        { StartCoroutine(Augmentation18()); }
        if (augmentation[28])
        { StartCoroutine(Augmentation28()); }
        //-----------------------��âü ���� �Լ�(����)

        passEnemy = false;
        curEnemy = 0;

        //������ �ó��� Ȯ��
        ui.SynergyE();

        //������ 
        gamestat = 2;


        switch (stageIndex)
        {
            case < 5:
                enemytype = 1;
                enemytypeMin = 0;
                break;
            case < 10:
                enemytype = 2;
                enemytypeMin = 0;
                break;
            case < 15:
                enemytype = 3;
                enemytypeMin = 0;
                break;
            case < 20:
                enemytype = 4;
                enemytypeMin = 1;
                break;
            case < 25:
                enemytype = 5;
                enemytypeMin = 1;
                break;
            case < 30:
                enemytype = 6;
                enemytypeMin = 2;
                break;
            case < 35:
                enemytype = 7;
                enemytypeMin = 3;
                break;
            case >= 35:
                enemytype = 8;
                enemytypeMin = 5;
                break;
        }

        for (int i = 0; i < fieldEnemys.Length; i++)
        {
            fieldEnemys[i] = null;
        }
        int rantype = UnityEngine.Random.Range(enemytypeMin, enemytype);

        //�� ������ ���� ���� Ȯ��-------------
        List<int> haveItemlist = new List<int>();
        int[] haveItemnum = new int[3];
        if (stageIndex % 5  == 0)
        {
            for (int i = 0; i < enemySpawnindex; i++)
            {
                haveItemlist.Add(i);
                yield return new WaitForFixedUpdate();
            }
            for (int i = 3; i > 0; i--)
            {
                int ra = UnityEngine.Random.Range(0, haveItemlist.Count);
                haveItemnum[i-1] = ra;
                haveItemlist.RemoveAt(ra);
                yield return new WaitForFixedUpdate();
            }
        }
        //-------------�� ������ ���� ���� Ȯ��
        yield return new WaitForFixedUpdate();
        for (int i = 0; i < enemySpawnindex; i++)
        {
            GameObject enemy = enemyPool.Get(rantype);
            EnemyStat enemyStat = enemy.GetComponent<EnemyStat>();
            enemy.transform.position = spawnPos[mapIndex-1].position;
            fieldEnemys[i] = enemy;
            EnemyMove em = enemy.GetComponent<EnemyMove>();
            em.goal = goal;
            em.movePoint = new Transform[mapPoints.Length];

            //�� ������ ���� Ȯ��
            if(stageIndex % 5 == 0 && (i == haveItemnum[0] || i == haveItemnum[1] || i == haveItemnum[2]))
                enemyStat.haveItem = true;
            else
                enemyStat.haveItem = false;


            for (int m = 0; m < em.movePoint.Length; m++)
            {
                em.movePoint[m] = mapPoints[m].transform;
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForSeconds(1);
        }

        //�������� �� Ȯ��
        totalEnemy = enemySpawnindex;
        while(totalEnemy > curEnemy)
        {
            Debug.Log(totalEnemy + " /" + curEnemy);
            yield return new WaitForSeconds(1);
        }

        //�������� ��!
        switch (coin) //���� ����
        {
            case < 10:
                coin += 0;
                break;
            case < 20:
                coin += 1;
                break;
            case < 30:
                coin += 2;
                break;
            case < 40:
                coin += 3;
                break;
            case < 50:
                coin += 4;
                break;
            case < 60:
                coin += 5;
                break;
                //����ü ���� ����
            case < 70:
                coin += augmentation[21]||augmentation[11] ? 6 : 5;
                break;
            case < 80:
                coin += augmentation[21] || augmentation[11] ? 7 : 5;
                break;
            case < 90:
                coin += augmentation[21] ? 8 : 5;
                break;
            case < 100:
                coin += augmentation[21] ? 9 : 5;
                break;
            case >= 100:
                coin += augmentation[21] ? 10 : 5;
                break;
        }

        if (!passEnemy)
        {
            coin++;
            if (continuity > 0)
                continuity++;
            else
                continuity = 1;
            Debug.Log("�̰��! continuity = " + continuity);
        } // �¸� �� ������ ���� ���ʽ�
        else
        {
            if (continuity < 0)
                continuity--;
            else
                continuity = -1;

            hp -= stageIndex;
            Debug.Log("���Ƶ�! continuity = " + continuity);
        }
        int count = Mathf.Abs(continuity) < 5 ? Mathf.Abs(continuity) : 5; // ���밪 ǥ��

        coin += count * AgManager.agGetCoin; //����ü ����
       
        coin += stageIndex < 5 ? stageIndex+1 : 5;//�⺻ ����

        if (augmentation[10])
            exp += passEnemy ? 5 : 4;
        else
            exp += 2;


        //�� ���� �ó���
        float healP = SynergyManager.heal + AgManager.agHeal1 + AgManager.agHeal2;
        float H = healP + (healP * SynergyManager.healX);
        if (hp + H > maxHp)
            hp = maxHp;
        else
            hp += H;

        ui.HpUpdate();
        ui.CoinUpdate();
        ui.ExpUpdate();
        ui.ContinuityUpdate();
        Reroll();
        stageIndex ++;
        StartCoroutine(DelayTime());
    }

    IEnumerator Augmentation8()
    {
        yield return new WaitForFixedUpdate();
        while(totalEnemy > curEnemy)
        {
            yield return new WaitForSeconds(5);
            for (int i = 0; i < fieldUnit.Length; i++)
            {
                if (fieldUnit[i] != null)
                {
                    Fire sm = fieldUnit[i].GetComponent<Fire>();
                    sm.mp += 5;
                }
                yield return new WaitForFixedUpdate();
            }
        }
    }
    IEnumerator Augmentation18()
    {
        yield return new WaitForFixedUpdate();
        while (totalEnemy > curEnemy)
        {
            yield return new WaitForSeconds(3);
            for (int i = 0; i < fieldUnit.Length; i++)
            {
                if (fieldUnit[i] != null)
                {
                    Fire sm = fieldUnit[i].GetComponent<Fire>();
                    sm.mp += 5;
                }
                yield return new WaitForFixedUpdate();
            }
        }
    }
    IEnumerator Augmentation28()
    {
        yield return new WaitForFixedUpdate();
        while (totalEnemy > curEnemy)
        {
            yield return new WaitForSeconds(3);
            for (int i = 0; i < fieldUnit.Length; i++)
            {
                if (fieldUnit[i] != null)
                {
                    Fire sm = fieldUnit[i].GetComponent<Fire>();
                    sm.mp += 10;
                }
                yield return new WaitForFixedUpdate();
            }
        }
    }

    public void Reroll() //ī�� ��ȯ
    {
        sm.DelCards();
        // ���� ���� �� Ȯ�� ����
        switch (lv)
        {
            case 1:
            case 2:
                tierPercentage[0] = 100;
                break;
            case 3:
                tierPercentage[0] = 75;
                tierPercentage[1] = 25;
                break;
            case 4:
                tierPercentage[0] = 55;
                tierPercentage[1] = 30;
                tierPercentage[2] = 15;
                break;
            case 5:
                tierPercentage[0] = 45;
                tierPercentage[1] = 33;
                tierPercentage[2] = 20;
                tierPercentage[3] = 2;
                break;
            case 6:
                tierPercentage[0] = 25;
                tierPercentage[1] = 40;
                tierPercentage[2] = 30;
                tierPercentage[3] = 5;
                break;
            case 7:
                tierPercentage[0] = 19;
                tierPercentage[1] = 30;
                tierPercentage[2] = 35;
                tierPercentage[3] = 15;
                tierPercentage[4] = 4;
                break;
            case 8:
                tierPercentage[0] = 16;
                tierPercentage[1] = 20;
                tierPercentage[2] = 35;
                tierPercentage[3] = 25;
                tierPercentage[4] = 4;
                break;
            case 9:
                tierPercentage[0] = 9;
                tierPercentage[1] = 15;
                tierPercentage[2] = 30;
                tierPercentage[3] = 30;
                tierPercentage[4] = 16;
                break;
            case 10:
                tierPercentage[0] = 5;
                tierPercentage[1] = 10;
                tierPercentage[2] = 20;
                tierPercentage[3] = 40;
                tierPercentage[4] = 25;
                break;
            case 11:
                tierPercentage[0] = 1;
                tierPercentage[1] = 2;
                tierPercentage[2] = 12;
                tierPercentage[3] = 50;
                tierPercentage[4] = 35;
                break;
        }

        for (int i = 0; i < 5; i++)
        {
            int ran = UnityEngine.Random.Range(1, 101);

            if (ran <= tierPercentage[0]) //1Ƽ�� ��ȯ
            { sm.Champ1Produce(); }
            else if (ran <= tierPercentage[0] + tierPercentage[1])//2Ƽ�� ��ȯ
            { sm.Champ2Produce(); }
            else if (ran <= tierPercentage[0] + tierPercentage[1] + tierPercentage[2])//3Ƽ�� ��ȯ
            { sm.Champ3Produce(); }
            else if (ran <= tierPercentage[0] + tierPercentage[1] + tierPercentage[2] + tierPercentage[3])//4Ƽ�� ��ȯ
            { sm.Champ4Produce(); }
            else//5Ƽ�� ��ȯ
            { sm.Champ5Produce(); }
        }
    }

    public void SynergyUpdate() //1 ��� / 2ħ�� /3 ���� /4 �ӱ�/5 ���/6����/7������/8������Ÿ����/9������ / 10 ȿ��
    {
        //���� �ʱ�ȭ
        for (int i = 0; i < synergy.Length; i++)
            synergy[i] = 0;
        for (int i = 0; i < synergyOverlapCk.Length; i++)
            synergyOverlapCk[i] = false;



        //���ֵ��� �ó�����ŭ �� �ó����� ���� �߰�
        for (int z = 0; z < fieldUnit.Length; z++)
        {
            if (fieldUnit[z] != null)
            {
                Synergy sn = fieldUnit[z].GetComponent<Synergy>();
                if (!synergyOverlapCk[sn.champNum]) //�ߺ� è���� ���ٸ� �ó��� �߰�
                {
                    for (int s = 0; s < sn.synergy.Length; s++)
                    {
                        int c = sn.synergy[s];
                        synergy[c]++;
                    }
                    synergyOverlapCk[sn.champNum] = true;
                }
            }
        }

      
        //�ó��� ui������Ʈ
        ui.SynergyUpdate();
    }
}
