using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
using System;

public class Ag
{
    public GameObject aBtn;
    
    public int aNum;

    public Ag(GameObject brn, int num)
    {
        aBtn = brn;
        aNum = num;
    }
}
public class AgManager : MonoBehaviour
{
    GameManager gm;
    public int[] agClass = new int[3];
    public int agCount = 0;
    public GameObject[] agBtn = new GameObject[30];

    List<Ag> ags0 = new List<Ag>(); //실버
    List<Ag> ags1 = new List<Ag>();//골드
    List<Ag> ags2 = new List<Ag>();//플레티넘

    List<Ag> selags = new List<Ag>();//보여진 리스트 임시보관

    public GameObject agGroup;
    AgButten[] agButtens = new AgButten[30];

    //증강체 버튼의 이미지, 설명
    public Sprite[] sprites = new Sprite[30];
    Image[] agim = new Image[30];
    Text[] agtext = new Text[30];


    public GameObject aUibtnG;
    GameObject[] aUibtns = new GameObject[30];
    GameObject[] posnum = new GameObject[3];



    private void Awake()
    {
        gm = transform.parent.transform.GetChild(1).gameObject.GetComponent<GameManager>();
        for (int i = 0; i < agButtens.Length; i++)
        {
            agBtn[i] = agGroup.transform.GetChild(i).gameObject;

            agButtens[i] = agGroup.transform.GetChild(i).gameObject.GetComponent<AgButten>();
            agButtens[i].agNum = i;

            agim[i] = agGroup.transform.GetChild(i).GetChild(0).gameObject.GetComponent<Image>();
            agim[i].sprite = sprites[i];

            agtext[i] = agGroup.transform.GetChild(i).GetChild(1).gameObject.GetComponent<Text>();

            aUibtns[i] = aUibtnG.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < posnum.Length; i++)
        {
            posnum[i] = aUibtnG.transform.parent.transform.GetChild(i).gameObject;
        }

    }
    private void Start()
    {
        agtext[0].text = "<size=50>은수저</size>\n경험치 10 획득";
        agtext[1].text = "<size=50>짭짤한 연승</size>\n연승코인 2배";
        agtext[2].text = "<size=50>선물1</size>\n랜던한 아이템 \n2개 획득";
        agtext[3].text = "<size=50>실버티켓</size>\n10%확률로 \n새로고침 무료";
        agtext[4].text = "<size=50>빠른공격1</size>\n아군의 공격속도 \n10% 증가";
        agtext[5].text = "<size=50>정확한 공격1</size>\n아군의 딜 \n10%증가";
        agtext[6].text = "<size=50>자가진단1</size>\n스테이지 종료시 \n체력 2 회복";
        agtext[7].text = "<size=50>건강</size>\n체력 30 증가";
        agtext[8].text = "<size=50>기도메타1</size>\n5초마다 \n마나5+";
        agtext[9].text = "<size=50>포멧1</size>\n체력을 잃을 때마다\n적들에게 10대미지";
        agtext[10].text = "<size=50>충전</size>\n경험치\n연승중 = +2,\n연패중 = +3";
        agtext[11].text = "<size=50>펀드1</size>\n최대70원의 \n이자 획득";
        agtext[12].text = "<size=50>선물2</size>\n랜덤한 아이템 \n5개 중 2개 획득";
        agtext[13].text = "<size=50>골드티켓</size>\n30%획률로 \n새로고침 무료";
        agtext[14].text = "<size=50>빠른공격2</size>\n아군의 공격속도 \n25%증가";
        agtext[15].text = "<size=50>정확한공격</size>\n아군의 딜 \n25%증가";
        agtext[16].text = "<size=50>자가진단2</size>\n스테이지 종료시 \n체력 5회복";
        agtext[17].text = "<size=50>수혈1</size>\n잃은 체력 5당 \n공격력 3%증가";
        agtext[18].text = "<size=50>기도메타2</size>\n3초마다 \n마나5+";
        agtext[19].text = "<size=50>포멧2</size>\n체력을 잃을 때마다\n적들의 최대체력\n10%대미지";
        agtext[20].text = "<size=50>과충전</size>\n경험치 구매시\n추가3 획득";
        agtext[21].text = "<size=50>펀드2</size>\n최대 100원의\n이자 획득";
        agtext[22].text = "<size=50>선물3</size>\n랜던한 아이템\n5개 획득";
        agtext[23].text = "<size=50>프리미어티켓</size>\n50%획률로\n새로고침 무료";
        agtext[24].text = "<size=50>빠른공격3</size>\n아군의 공격속도\n50%증가";
        agtext[25].text = "<size=50>정확한 공격3</size>\n아군의 딜\n50%증가";
        agtext[26].text = "<size=50>자가진단3</size>\n체력소모가 있다먼\n이번 스테이지\n아군 공격력\n70%증가";
        agtext[27].text = "<size=50>수혈2</size>\n잃은 체력 1당\n공격력 1%증가";
        agtext[28].text = "<size=50>기도메타3</size>\n3초마다\n마나 10+";
        agtext[29].text = "<size=50>포멧3</size>\n체력을 잃으면\n적들의 방어력\n0으로 고정";
    }
    void AgReset()
    {
        ags0.Clear();
        ags1.Clear();
        ags2.Clear();
        selags.Clear();

        Ag node = new Ag(agBtn[0], 0);
        ags0.Add(node);

        for (int i = 1; i < 30; i++)
        {

            node = new Ag(agBtn[i], i);

            if (i < 10)
                ags0.Add(node);
            else if (i < 20)
                ags1.Add(node);
            else
                ags2.Add(node);
        }
    }

    public void AgClassSet()
    {
        int ran = UnityEngine.Random.Range(0, 100);

        switch (ran) // 0 = 실버 , 1 = 골드, 2 =프리즘
        {
            case 0: agClass[0] = 2; agClass[1] = 2; agClass[2] = 2; break;
            case 1: agClass[0] = 2; agClass[1] = 2; agClass[2] = 1; break;
            case 2: agClass[0] = 2; agClass[1] = 1; agClass[2] = 2; break;
            case 3: case 4: agClass[0] = 2; agClass[1] = 1; agClass[2] = 1; break;
            case 5: agClass[0] = 2; agClass[1] = 0; agClass[2] = 2; break;
            case < 10: agClass[0] = 2; agClass[1] = 0; agClass[2] = 1; break;
            case 10: agClass[0] = 1; agClass[1] = 2; agClass[2] = 2; break;
            case < 21: agClass[0] = 1; agClass[1] = 2; agClass[2] = 1; break;
            case < 27: agClass[0] = 1; agClass[1] = 2; agClass[2] = 0; break;
            case < 30: agClass[0] = 1; agClass[1] = 1; agClass[2] = 2; break;
            case < 52: agClass[0] = 1; agClass[1] = 1; agClass[2] = 1; break;
            case < 54: agClass[0] = 1; agClass[1] = 0; agClass[2] = 2; break;
            case < 72: agClass[0] = 1; agClass[1] = 0; agClass[2] = 1; break;
            case 72: agClass[0] = 0; agClass[1] = 2; agClass[2] = 2; break;
            case < 78: agClass[0] = 0; agClass[1] = 1; agClass[2] = 2; break;
            case < 90: agClass[0] = 0; agClass[1] = 1; agClass[2] = 1; break;
            case < 95: agClass[0] = 0; agClass[1] = 2; agClass[2] = 2; break;
            case < 100: agClass[0] = 0; agClass[1] = 2; agClass[2] = 1; break;
        }

        AgReset();
    }



    public void SpawnAg()
    {
        agGroup.transform.parent.gameObject.SetActive(true);

        switch (agClass[agCount])
        {
            case 0:
                for (int i = 0; i < 3; i++)
                {
                    int ran = UnityEngine.Random.Range(0, ags0.Count);
                    ags0[ran].aBtn.SetActive(true);
                    selags.Add(ags0[ran]);
                    ags0.Remove(ags0[ran]);
                }
                break;
            case 1:
                for (int i = 0; i < 3; i++)
                {
                    int ran = UnityEngine.Random.Range(0, ags1.Count);
                    ags1[ran].aBtn.SetActive(true);
                    selags.Add(ags1[ran]);
                    ags1.Remove(ags1[ran]);
                }
                break;

            case 2:
                for (int i = 0; i < 3; i++)
                {
                    int ran = UnityEngine.Random.Range(0, ags2.Count);
                    ags2[ran].aBtn.SetActive(true);
                    selags.Add(ags2[ran]);
                    ags2.Remove(ags2[ran]);
                }
                break;
        }


    }

    public void SelectAg()
    {
        //선택된 증강체 임시보관 리스트에서 제거
        AgButten ab = EventSystem.current.currentSelectedGameObject.GetComponent<AgButten>();
        for (int i = 0; i < selags.Count; i++)
        {
            selags[i].aBtn.SetActive(false);

            if (ab.agNum == selags[i].aNum)
                selags.Remove(selags[i]);
        }

        //정보주기
        AgEft(ab.agNum);
        aUibtns[ab.agNum].SetActive(true);
        aUibtns[ab.agNum].transform.position = posnum[agCount].transform.position;

        //선택안된 증강되돌리기
        for (int i = 0; i < selags.Count; i++)
        {
            if (selags[i].aNum < 10)
                ags0.Add(selags[i]);
            else if (selags[i].aNum < 20)
                ags1.Add(selags[i]);
            else
                ags2.Add(selags[i]);
        }
        agCount++;

        agGroup.transform.parent.gameObject.SetActive(false);
    }
    public static int agGetCoin = 1; //연승패 코인
    public static int agReRollFree1 = 0;//새로고침무료1
    public static int agAtSpeed1 = 0;//공속1
    public static int agAtDmg1 = 0;//딜1
    public static int agHeal1 = 0;//회복1
    public static int agGetMp1 = 0;//마나 회복1
    public static int agFormat1 = 0;//체력 잃으면 딜
    public static int agGetExp = 0;//연승패 경험치
    public static int agInterest = 50;//최대이자
    public static int agReRollFree2 = 0;//새로고침무료2
    public static int agAtSpeed2 = 0;//공속2
    public static int agAtDmg2 = 0;//딜2
    public static int agHeal2 = 0;//회복2
    public static int agBoold1 = 0;//잃은 체력5당 딜 3증가
    public static int agGetMp2 = 0;//마나 회복2
    public static int agFormat2 = 0;//체력 잃으면 퍼딜
    public static int agMoreGetExp = 0; //구매시 추가 경험치
    public static int agReRollFree3 = 0;//새로고침무료3
    public static int agAtSpeed3 = 0;//공속3
    public static int agAtDmg3 = 0;//딜3
    public static int agHeal3 = 0;//회복3
    public static int agBoold2 = 0;//잃은 체력당 딜증가
    public static int agGetMp3 = 0;//마나 회복3


    public void AgEft(int num)
    {
        GameManager.augmentation[num] = true;
        //1회성 증강
        switch (num)
        {
            case 0://경험치 10추가
                gm.exp += 10;
                gm.ui.ExpUpdate();
                break;

            case 2://랜덤한 아이템 2개 획득
                break;

            case 7://체력 30 증가
                gm.maxHp += 30;
                gm.hp += 30;
                gm.ui.HpUpdate();
                break;

            case 12://랜덤한 아이템 5개중 2개획득

                break;

            case 22://랜덤한 아이템 5개 획득

                break;

        }

        agGetCoin = GameManager.augmentation[1] ? 2 : 1;//연승코인 2배
        agReRollFree1 = GameManager.augmentation[3] ? 10 : 0;//10확률로 새로고침 무료
        agAtSpeed1 = GameManager.augmentation[4] ? 10 : 0;//아군의 공격속도 10% 증가
        agAtDmg1 = GameManager.augmentation[5] ? 10 : 0;//아군의 딜 10%증가
        agHeal1 = GameManager.augmentation[6] ? 2 : 0;//스테이지 종료시 체력 2회복
        agGetMp1 = GameManager.augmentation[8] ? 5 : 0;//5초마다 마나 5+
        agFormat1 = GameManager.augmentation[9] ? 10 : 0;//체력을 잃을 때 마다 적들에게 10데미지
        agGetExp = GameManager.augmentation[10] ? 2 : 0;//경험치 연승중에 +2,연패중 +3
        agInterest = GameManager.augmentation[21] ? 100 : GameManager.augmentation[11] ? 70 : 50;//최대이자 70원의 이자 혹은 100원
        agReRollFree2 = GameManager.augmentation[13] ? 30 : 0;//경험치 연승중에 +2,연패중 +3
        agAtSpeed2 = GameManager.augmentation[14] ? 25 : 0;//아군 공속 25%증가
        agAtDmg2 = GameManager.augmentation[15] ? 25 : 0;//아군의 딜 25% 증가
        agHeal2 = GameManager.augmentation[16] ? 5 : 0;//스테이지 종료시 체력 5회복
        agBoold1 = GameManager.augmentation[17] ? 3 : 0;//잃은 체력 5당 딜 3증가
        agGetMp2 = GameManager.augmentation[18] ? 5 : 0;//3초마다 마나 +5
        agFormat2 = GameManager.augmentation[19] ? 10 : 0;//체력 잃을 때 마다 적들의 체력 10%대미지
        agMoreGetExp = GameManager.augmentation[20] ? 3 : 0;//경험치 구매시 3추가
        agReRollFree3 = GameManager.augmentation[22] ? 50 : 0;//50% 새로고침 무료
        agAtSpeed3 = GameManager.augmentation[24] ? 50 : 0;//아군 공속 50%증가
        agAtDmg3 = GameManager.augmentation[25] ? 50 : 0;//아군의 딜 50% 증가
        agHeal3 = GameManager.augmentation[26] ? 70 : 0;//체력 모소가 있다면 아군 공격력 70% 증가
        //agBoold2 = 잃은 체력당 공격력 증가************************** 
        agGetMp3 = GameManager.augmentation[28] ? 10 : 0;//3초마다 마나 10+
        //포멧3 = 체력 잃으면 적 방어력 0************************** 
    }
}
