using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

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
    public GameObject mapTileParent;
    public GameObject[] mapTiles = new GameObject[100];
    public GameObject[] mapPoints;//���� �̵� ����Ʈ��
    public int[] map1NoSetPosNum = new int[] { 0, 1, 2, 3, 6, 8, 10, 13, 16, 18, 20, 23, 26, 28, 30, 31, 32, 33, 34, 35, 36, 37, 38, 43, 46, 53, 56, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 73, 76, 79, 80, 83, 86, 89, 90, 91, 92, 93, 96, 97, 98, 99};
    public int[] map2NoSetPosNum = new int[] { 8, 18, 28, 38, 37, 36, 35, 34, 33, 32, 31, 30, 20, 10, 0, 1, 2, 3, 13, 23, 43, 53, 63, 73, 83, 93, 92, 91, 90, 80, 70, 60, 61, 62, 64, 65, 66, 67, 68, 78, 88, 98, 97, 96, 86, 76, 56, 46, 26, 16, 6 };
    public int[] map3NoSetPosNum = new int[51];
    LineR lineR;
    public int[] map1Lines = new int[] { 30, 0, 3, 93, 90, 60, 69, 99, 96};


    public int stageIndex;
    public static bool passEnemy;

    public static int totalEnemy;
    public static int curEnemy;

    //�� ���� ����
    public PoolManager enemyPool;
    public PoolManager bulletPool;
    public PoolManager skillPool;
    public GameObject goal;
    public Transform spawnPos;


    private void Awake()
    {
        Instance = this;
        gameover = () => { GameOver(); };
        bt = GetComponent<BtnManager>();
        lineR = mapTileParent.GetComponent<LineR>();
        //Ÿ�ϵ� ����
        for (int i = 0; i < mapTiles.Length; i++)
        {
            mapTiles[i] = mapTileParent.transform.GetChild(i).gameObject;
        }

        ul = GetComponent<UnitLvManager>();
        itempool = poolG.GetChild(13).gameObject.GetComponent<PoolManager>();
        Application.targetFrameRate = 60; //���� ������ �ӵ� 60���������� ���� ��Ű��.. �ڵ�
        QualitySettings.vSyncCount = 0;
        //����� �ֻ���(�÷�����)�� �ٸ� ��ǻ���� ��� ĳ���� ���۽� ������ ������ �� �ִ�.
    }
    Vector3 setVec = new Vector3(0, 0, 10);
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
        //��� ǥ��
        int noposzon = 0;
        if(mapIndex == 1)
            for (int i = 0; i < mapTiles.Length; i++)
            {
                SpriteRenderer sp = mapTiles[i].GetComponent<SpriteRenderer>();
                if(i == map1NoSetPosNum[noposzon])
                {
                    mapTiles[i].layer = 10;
                    sp.color = new Color(0, 0, 0, 0.5f);
                    noposzon++;
                }
                else
                {
                    mapTiles[i].layer = 14;
                    sp.color = new Color(0, 0, 0, 0);
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
        cm.cFire.skillPool = skillPool;
        cm.cFire.gm = Instance;
        cm.cFire.bulletPool = bulletPool;
        cm.cFire.StatUpdate();
        ws.obj[0] = ch.gameObject;
        ch.position = Camera.main.ScreenToWorldPoint(ws.pos[0].transform.position)+setVec;

        ul.lv1Units[z][0] = ch.gameObject;
       
        //�� ��������Ʈ ����
        if (mapIndex == 1)
        {
            mapPoints = new GameObject[] { mapTiles[38], mapTiles[30], mapTiles[0], mapTiles[3], mapTiles[93], mapTiles[90], mapTiles[60], mapTiles[69], mapTiles[99], mapTiles[96], goal };
            spawnPos.position = new Vector2 ( mapTiles[8].transform.position.x, mapTiles[8].transform.position.y + 1);
            goal.transform.position = new Vector2(mapTiles[6].transform.position.x, mapTiles[6].transform.position.y + 1);
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
            enemy.transform.position = spawnPos.position;
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
