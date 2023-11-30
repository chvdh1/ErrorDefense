using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static Action startGame;
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
    public int continuity; // 연승패 코인
    public int champBlank;
    public int[] synergy = new int[21]; //0 백신 / 1침착 /2 신중 /3 속기/4 행운/5생각/6디자인/7프로토타이핑
    public bool[] synergyOverlapCk = new bool[60];//챔프가 겹쳐 시너지오르는 현상 막기위한 변수
    public static bool[] augmentation = new bool[30]; // 증강
    int[] tierPercentage = new int[5]; //각 레벨에 맞는 확률 기입


    public static Action gameover;

    public int gamestat; // 0 = 로비 , 1 게임 진행 , 2 전투중


    //스테이지 관련 변수
    public int mapIndex;
    public GameObject mapTileParent;
    public GameObject[] mapTiles = new GameObject[100];
    public GameObject[] mapPoints;//유닛 이동 포인트들
    public int[] map1NoSetPosNum = new int[] { 0, 1, 2, 3, 6, 8, 10, 13, 16, 18, 20, 23, 26, 28, 30, 31, 32, 33, 34, 35, 36, 37, 38, 43, 46, 53, 56, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 73, 76, 79, 80, 83, 86, 89, 90, 91, 92, 93, 96, 97, 98, 99};
    public int[] map2NoSetPosNum = new int[] { 8, 18, 28, 38, 37, 36, 35, 34, 33, 32, 31, 30, 20, 10, 0, 1, 2, 3, 13, 23, 43, 53, 63, 73, 83, 93, 92, 91, 90, 80, 70, 60, 61, 62, 64, 65, 66, 67, 68, 78, 88, 98, 97, 96, 86, 76, 56, 46, 26, 16, 6 };
    public int[] map3NoSetPosNum = new int[51];
    LineR lineR;
    public int[] map1Lines = new int[] { 30, 0, 3, 93, 90, 60, 69, 99, 96};


    public int stageIndex;
    public static bool passEnemy;

    public static int totalEnemy;
    public static int curEnemy;

    //적 관련 변수
    public PoolManager enemyPool;
    public PoolManager bulletPool;
    public PoolManager skillPool;
    public GameObject goal;
    public Transform spawnPos;


    private void Awake()
    {
        Instance = this;
        gameover = () => { GameOver(); };
        startGame = () => { GameStart(); };
        bt = GetComponent<BtnManager>();
        lineR = mapTileParent.GetComponent<LineR>();
        //타일들 인지
        for (int i = 0; i < mapTiles.Length; i++)
        {
            mapTiles[i] = mapTileParent.transform.GetChild(i).gameObject;
        }

        ul = GetComponent<UnitLvManager>();
        itempool = poolG.GetChild(13).gameObject.GetComponent<PoolManager>();
        Application.targetFrameRate = 60; //실행 프레임 속도 60프레임으로 고정 시키기.. 코드
        QualitySettings.vSyncCount = 0;
        //모니터 주사율(플레임율)이 다른 컴퓨터일 경우 캐릭터 조작시 빠르게 움직일 수 있다.
    }
    Vector3 setVec = new Vector3(0, 0, 10);

    private void Start()
    {
        mapIndex = 1;
        GameStart();
    }
    public void GameStart()
    {
        //최대체력으로 시작
        maxHp = 100;
        hp = maxHp;
        lv = 1;
        coin = 0;
        exp = 0;
        ui.CoinUpdate();
        ui.ExpUpdate();
        ui.ContinuityUpdate();

        //스테이지 초기화
        stageIndex = 1;
        passEnemy = false;

        //지정 맵 소환, 그외 비활성화
        //경로 표시
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

        //시너지,증강체 및 챔프 리스트 초기화
        ui.SynergyReset();
        sm.ChampReset();
        am.AgClassSet();

        //1코 유닛 활성화
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
       
        //적 무브포인트 인지
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

        //증강체 획득(5,12,19)
        if (stageIndex == 5 || stageIndex == 12 || stageIndex == 19)
            am.SpawnAg();

        //대기중
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

        //증창체 관련 함수(마나)
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
        //-----------------------증창체 관련 함수(마나)

        passEnemy = false;
        curEnemy = 0;

        //시작전 시너지 확인
        ui.SynergyE();

        //전투중 
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

        //적 아이템 보유 수량 확인-------------
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
        //-------------적 아이템 보유 수량 확인
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

            //적 아이템 보유 확인
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

        //스테이지 끝 확인
        totalEnemy = enemySpawnindex;
        while(totalEnemy > curEnemy)
        {
            Debug.Log(totalEnemy + " /" + curEnemy);
            yield return new WaitForSeconds(1);
        }

        //스테이지 끝!
        switch (coin) //이자 수익
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
                //증강체 관련 변수
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
            Debug.Log("이겼다! continuity = " + continuity);
        } // 승리 및 연승패 코인 보너스
        else
        {
            if (continuity < 0)
                continuity--;
            else
                continuity = -1;

            hp -= stageIndex;
            Debug.Log("놓쳤따! continuity = " + continuity);
        }
        int count = Mathf.Abs(continuity) < 5 ? Mathf.Abs(continuity) : 5; // 절대값 표시

        coin += count * AgManager.agGetCoin; //증강체 변수
       
        coin += stageIndex < 5 ? stageIndex+1 : 5;//기본 수익

        if (augmentation[10])
            exp += passEnemy ? 5 : 4;
        else
            exp += 2;


        //힐 관련 시너지
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

    public void Reroll() //카드 소환
    {
        sm.DelCards();
        // 레벨 인지 후 확률 기입
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

            if (ran <= tierPercentage[0]) //1티어 소환
            { sm.Champ1Produce(); }
            else if (ran <= tierPercentage[0] + tierPercentage[1])//2티어 소환
            { sm.Champ2Produce(); }
            else if (ran <= tierPercentage[0] + tierPercentage[1] + tierPercentage[2])//3티어 소환
            { sm.Champ3Produce(); }
            else if (ran <= tierPercentage[0] + tierPercentage[1] + tierPercentage[2] + tierPercentage[3])//4티어 소환
            { sm.Champ4Produce(); }
            else//5티어 소환
            { sm.Champ5Produce(); }
        }
    }

    public void SynergyUpdate() //1 백신 / 2침착 /3 신중 /4 속기/5 행운/6생각/7디자인/8프로토타이핑/9마케팅 / 10 효율
    {
        //스택 초기화
        for (int i = 0; i < synergy.Length; i++)
            synergy[i] = 0;
        for (int i = 0; i < synergyOverlapCk.Length; i++)
            synergyOverlapCk[i] = false;



        //유닛들의 시너지만큼 그 시너지에 스택 추가
        for (int z = 0; z < fieldUnit.Length; z++)
        {
            if (fieldUnit[z] != null)
            {
                Synergy sn = fieldUnit[z].GetComponent<Synergy>();
                if (!synergyOverlapCk[sn.champNum]) //중복 챔프가 없다면 시너지 추가
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

      
        //시너지 ui업데이트
        ui.SynergyUpdate();
    }
}
