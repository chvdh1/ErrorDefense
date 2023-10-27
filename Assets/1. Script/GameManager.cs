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
    public int lv;

    public GameObject[] fieldUnit=new GameObject[15];

    public float maxHp;
    public float hp;

    public float[] maxExp;
    public float exp;
    public int coin;
    public int continuity; // ������ ����
    public int champBlank;
    public int[] synergy = new int[21];
    public int[] augmentation = new int[3]; // ����
    int[] tierPercentage = new int[4]; //�� ������ �´� Ȯ�� ����

    public static Action gameover;

    public int gamestat; // 0 = �κ� , 1 ���� ���� , 2 ������


    //�������� ���� ����
    public int mapIndex;
    public GameObject maps;
    public GameObject[] mapPointParent;
    public GameObject[] mapPoints;
    public GameObject noSetP;
    GameObject[] noSet;
    int stageIndex;
    public bool passEnemy;

    public static int totalEnemy;
    public static int curEnemy;

    //�� ���� ����
    public PoolManager enemyPool;
    public PoolManager bulletPool;
    public GameObject goal;
    public Transform[] spawnPos;


    private void Awake()
    {
        Instance = this;
        gameover = () => { GameOver(); };


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

        //�ó��� �ʱ�ȭ �� è�� ����Ʈ �ʱ�ȭ
        ui.SynergyReset();
        sm.ChampReset();

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
        float t = stageIndex == 1 ? 5 : 10;
        float maxt = t;

        //������ �ʵ� ���� ���º�ȭ(�̵������� ����)
        gamestat = 1;
        for (int i = 0; i < fieldUnit.Length; i++)
        {
            if (fieldUnit[i] != null)
            {
                Fire fi = fieldUnit[i].GetComponent<Fire>();
                fi.inField = false;
                yield return new WaitForFixedUpdate();
            }
        }
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
    
        passEnemy = false;
        curEnemy = 0;

        //������ �ʵ� ���� ���º�ȭ(�̵������� ����)
        gamestat = 2;
        for (int i = 0; i < fieldUnit.Length; i++)
        {
            if (fieldUnit[i] != null)
            {
                Fire fi = fieldUnit[i].GetComponent<Fire>();
                fi.inField = true;
                yield return new WaitForFixedUpdate();
            }
        }


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

        int rantype = UnityEngine.Random.Range(enemytypeMin, enemytype);
        yield return new WaitForFixedUpdate();
        for (int i = 0; i < enemySpawnindex; i++)
        {
            GameObject enemy = enemyPool.Get(rantype);
            enemy.transform.position = spawnPos[mapIndex-1].position;
            EnemyMove em = enemy.GetComponent<EnemyMove>();
            em.goal = goal;
            em.movePoint = new Transform[mapPoints.Length];
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
       
        if (!passEnemy)
        {
            coin++;
            continuity = continuity > 0 ? continuity++ : 1;
        } // �¸� �� ������ ���� ���ʽ�
        else
        {
            continuity = continuity < 0 ? continuity-- : -1;
        }
           
        switch(coin)
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
            case >= 50:
                coin += 5;
                break;
        } //���� ����
        coin += stageIndex < 5 ? stageIndex+1 : 5;//�⺻ ����

        exp += 2;

        ui.CoinUpdate();
        ui.ExpUpdate();
        ui.ContinuityUpdate();
        Reroll();
        stageIndex ++;
        StartCoroutine(DelayTime());
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
}
