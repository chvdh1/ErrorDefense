using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetAgBtn : MonoBehaviour
{
    public int num;

    string[] allinfo = new string[30];
    public string info;

    public Sprite[] sprites = new Sprite[30];
    Image agim;

    private void Awake()
    {
        num = transform.GetSiblingIndex();//본인이 몇번째 인지
        agim = GetComponent<Image>();

        allinfo[0] = "<size=50>은수저</size>\n경험치 10 획득";
        allinfo[1] = "<size=50>짭짤한 연승</size>\n연승코인 2배";
        allinfo[2] = "<size=50>선물1</size>\n랜던한 아이템 \n2개 획득";
        allinfo[3] = "<size=50>실버티켓</size>\n10%확률로 \n새로고침 무료";
        allinfo[4] = "<size=50>빠른공격1</size>\n아군의 공격속도 \n10% 증가";
        allinfo[5] = "<size=50>정확한 공격1</size>\n아군의 딜 \n10%증가";
        allinfo[6] = "<size=50>자가진단1</size>\n스테이지 종료시 \n체력 2 회복";
        allinfo[7] = "<size=50>건강</size>\n체력 30 증가";
        allinfo[8] = "<size=50>기도메타1</size>\n5초마다 \n마나5+";
        allinfo[9] = "<size=50>포멧1</size>\n체력을 잃을 때마다\n적들에게 10대미지";
        allinfo[10] = "<size=50>충전</size>\n경험치\n연승중 = +2,\n연패중 = +3";
        allinfo[11] = "<size=50>펀드1</size>\n최대70원의 \n이자 획득";
        allinfo[12] = "<size=50>선물2</size>\n랜덤한 아이템 \n5개 중 2개 획득";
        allinfo[13] = "<size=50>골드티켓</size>\n30%획률로 \n새로고침 무료";
        allinfo[14] = "<size=50>빠른공격2</size>\n아군의 공격속도 \n25%증가";
        allinfo[15] = "<size=50>정확한공격</size>\n아군의 딜 \n25%증가";
        allinfo[16] = "<size=50>자가진단2</size>\n스테이지 종료시 \n체력 5회복";
        allinfo[17] = "<size=50>수혈1</size>\n잃은 체력 5당 \n공격력 3%증가";
        allinfo[18] = "<size=50>기도메타2</size>\n3초마다 \n마나5+";
        allinfo[19] = "<size=50>포멧2</size>\n체력을 잃을 때마다\n적들의 최대체력\n10%대미지";
        allinfo[20] = "<size=50>과충전</size>\n경험치 구매시\n추가3 획득";
        allinfo[21] = "<size=50>펀드2</size>\n최대 100원의\n이자 획득";
        allinfo[22] = "<size=50>선물3</size>\n랜던한 아이템\n5개 획득";
        allinfo[23] = "<size=50>프리미어티켓</size>\n50%획률로\n새로고침 무료";
        allinfo[24] = "<size=50>빠른공격3</size>\n아군의 공격속도\n50%증가";
        allinfo[25] = "<size=50>정확한 공격3</size>\n아군의 딜\n50%증가";
        allinfo[26] = "<size=50>자가진단3</size>\n체력소모가 있다먼\n이번 스테이지\n아군 공격력\n70%증가";
        allinfo[27] = "<size=50>수혈2</size>\n잃은 체력 1당\n공격력 1%증가";
        allinfo[28] = "<size=50>기도메타3</size>\n3초마다\n마나 10+";
        allinfo[29] = "<size=50>포멧3</size>\n체력을 잃으면\n적들의 방어력\n0으로 고정";

        info = allinfo[num];
        agim.sprite = sprites[num];
    }


}
