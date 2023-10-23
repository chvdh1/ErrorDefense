using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using System;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public UIManager ui;
    ShopManager sm;
    public int lv;

    public int maxHp;
    public int hp;

    public int[] maxExp;
    public int exp;
    public int coin;
    public int continuity;
    public int champBlank;
    public int[] synergy = new int[21];
    public int[] augmentation = new int[3]; // ����
    int[] tierPercentage = new int[4]; //�� ������ �´� Ȯ�� ����

    public static Action gameover;

    public static int gamestat; // 0 = �κ� , 1 ���� ���� , 


    //�������� ���� ����
    public int mapIndex;
    public GameObject maps;
    public GameObject[] mapPointParent;
    public GameObject[] mapPoints;
    int stageIndex;

    public static int totalEnemy;
    public static int curEnemy;

    //�� ���� ����
    public PoolManager enemyPool;
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
        ui = UIManager.uIManager;
        sm = ShopManager.shopManager;
    }

    public void GameStart()
    {
        //�ִ�ü������ ����
        hp = maxHp;

        //�������� �ʱ�ȭ
        stageIndex = 1;

        //�ó��� �ʱ�ȭ
        ui.SynergyReset();

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
        yield return new WaitForFixedUpdate();
        while(t>0)
        {
            t -= Time.fixedDeltaTime;
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
        while(totalEnemy != curEnemy)
        {
            yield return new WaitForSeconds(1);
        }

        stageIndex ++;
        StartCoroutine(DelayTime());
    }
    public void Reroll() //ī�� ��ȯ
    {
        int cells = 5; //������ ī�� ��

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

        for (int i = 0; i < cells; i++)
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
