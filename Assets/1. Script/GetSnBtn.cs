using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSnBtn : MonoBehaviour
{
    public int num;
    public int step;
    public int stepCount;

    string[] allinfo = new string[10];
    public string info;

    string[] allStep = new string[10];
    public string stepinfo;

    public Sprite[] sprites = new Sprite[10];

    private void Awake()
    {
        num = transform.GetSiblingIndex();//본인이 몇번째 인지

        allinfo[0] = "<size=50>백신</size>\n30%확률로 적을 \n둔화시킨다.\n스테이지 종료시 \n체력회복";
        allinfo[1] = "<size=50>침착</size>\n아군 유닛의 \n사거리 증가";
        allinfo[2] = "<size=50>신중</size>\n아군 유닛의 \n데미지 증가";
        allinfo[3] = "<size=50>속기</size>\n공격 속도와 \n탄속 증가";
        allinfo[4] = "<size=50>행운</size>\n치명타 확률과 \n치명타 데미지 증가";
        allinfo[5] = "<size=50>생각</size>\n둔화와 체력 \n회복을 증폭";
        allinfo[6] = "<size=50>디자인</size>\n적들에게 \n초당 데미지";
        allinfo[7] = "<size=50>프로토타이핑</size>\n아군 유닛의 \n데미지 증폭";
        allinfo[8] = "<size=50>마케팅</size>\n코인 및 아이템 \n획득 확률 증가";
        allinfo[9] = "<size=50>효율</size>\n2초마다 \n아군 유닛 \n마나 회복";

        info = allinfo[num];
    }

    public void StepText()
    {
        switch(num)
        {
            case 0:
                allStep[num] = step == 4 ? "2 : 감속 1,회복 1\n4 : 감속 1,회복 2\n6 : 감속 2,회복 2\n​<color=#333333><size=50>8</size> : 감속 3,회복 3​</color>" :
                    step == 3 ? "2 : 감속 1,회복 1\n4 : 감속 1,회복 2\n​<color=#333333><size=50>6</size> : 감속 2,회복 2​</color>\n8 : 감속 3,회복 3" :
                    step == 2 ? "2 : 감속 1,회복 1\n<color=#333333><size=50>4</size>​ : 감속 1,회복 2</color>\n6 : 감속 2,회복 2\n8 : 감속 3,회복 3" :
                    step == 1 ? "<color=#333333><size=50>2</size>​ : 감속 1,회복 1</color>\n4 : 감속 1,회복 2\n6 : 감속 2,회복 2\n8 : 감속 3,회복 3" :
                     "2 : 감속 1,회복 1\n4 : 감속 1,회복 2\n6 : 감속 2,회복 2\n8 : 감속 3,회복 3"; 
                break;
            case 1:
                allStep[num] = step == 4 ? "2 : 사거리 1증가\n4 : 사거리 2증가\n6 : 사거리 3 증가\n<color=#333333><size=50>8</size> : 사거리 5 증가</color>" :
                    step == 3 ? "2 : 사거리 1증가\n4 : 사거리 2증가\n<color=#333333><size=50>6</size>​ : 사거리 3 증가</color>\n8 : 사거리 5 증가" :
                    step == 2 ? "2 : 사거리 1증가\n<color=#333333><size=50>4</size> : 사거리 2증가​</color>\n6 : 사거리 3 증가\n8 : 사거리 5 증가" :
                    step == 1 ? "<color=#333333><size=50>2</size> : 사거리 1증가​</color>\n4 : 사거리 2증가\n6 : 사거리 3 증가\n8 : 사거리 5 증가" :
                     "2 : 사거리 1증가\n4 : 사거리 2증가\n6 : 사거리 3 증가\n8 : 사거리 5 증가";
                break;
            case 2:
                allStep[num] = step == 4 ? "2 : 데미지 5증가\n4 :  데미지 10증가\n6 : 데미지 20증가\n<color=#333333><size=50>8</size> : 데미지 45증가​</color>" :
                    step == 3 ? "2 : 데미지 5증가\n4 :  데미지 10증가\n<color=#333333><size=50>6</size> : 데미지 20증가​</color>\n8 : 데미지 45증가" :
                    step == 2 ? "2 : 데미지 5증가\n<color=#333333><size=50>4</size> :  데미지 10증가​</color>\n6 : 데미지 20증가\n8 : 데미지 45증가" :
                    step == 1 ? "<color=#333333><size=50>2</size> : 데미지 5증가​</color>\n4 :  데미지 10증가\n6 : 데미지 20증가\n8 : 데미지 45증가" : 
                     "2 : 데미지 5증가\n4 :  데미지 10증가\n6 : 데미지 20증가\n8 : 데미지 45증가";
                break;
            case 3:
                allStep[num] = step == 4 ? "2 : 공속 20+, 탄속 1+\n4 : 공속 35+, 탄속 2+\n6 : 공속 55+, 탄속 3+\n<color=#333333><size=50>8</size> : 공속 95+, 탄속 5+​</color>" :
                    step == 3 ? "2 : 공속 20+, 탄속 1+\n4 : 공속 35+, 탄속 2+\n<color=#333333><size=50>6</size> : 공속 55+, 탄속 3+​</color>\n8 : 공속 95+, 탄속 5+" :
                    step == 2 ? "2 : 공속 20+, 탄속 1+\n<color=#333333><size=50>4</size> : 공속 35+, 탄속 2+​</color>\n6 : 공속 55+, 탄속 3+\n8 : 공속 95+, 탄속 5+" :
                    step == 1 ? "<color=#333333><size=50>2</size> : 공속 20+, 탄속 1+​</color>\n4 : 공속 35+, 탄속 2+\n6 : 공속 55+, 탄속 3+\n8 : 공속 95+, 탄속 5+" :
                     "2 : 공속 20+, 탄속 1+\n4 : 공속 35+, 탄속 2+\n6 : 공속 55+, 탄속 3+\n8 : 공속 95+, 탄속 5+";
                break;
            case 4:
                allStep[num] = step == 4 ? "2 : 치명타 확률 10+,대미지 10+\n4 : 치명타 확률 20+,대미지 20+\n6 : 치명타 확률 30+,대미지 35+\n<color=#333333><size=50>8</size> : 치명타 확률 45+,대미지 65+​</color>" :
                    step == 3 ? "2 : 치명타 확률 10+,대미지 10+\n4 : 치명타 확률 20+,대미지 20+\n<color=#333333><size=50>6</size> : 치명타 확률 30+,대미지 35+​</color>\n8 : 치명타 확률 45+,대미지 65+" :
                    step == 2 ? "2 : 치명타 확률 10+,대미지 10+\n<color=#333333><size=50>4</size> : 치명타 확률 20+,대미지 20+​</color>\n6 : 치명타 확률 30+,대미지 35+\n8 : 치명타 확률 45+,대미지 65+" :
                    step == 1 ? "<color=#333333><size=50>2</size> : 치명타 확률 10+,대미지 10+​</color>\n4 : 치명타 확률 20+,대미지 20+\n6 : 치명타 확률 30+,대미지 35+\n8 : 치명타 확률 45+,대미지 65+" :
                     "2 : 치명타 확률 10+,대미지 10+\n4 : 치명타 확률 20+,대미지 20+\n6 : 치명타 확률 30+,대미지 35+\n8 : 치명타 확률 45+,대미지 65+";
                break;
            case 5:
                allStep[num] = step == 4 ? "2 : 둔화 10%, 회복 10% 증폭\n4 :둔화 20%, 회복 30% 증폭\n6 : 둔화 30%, 회복 50% 증폭\n<size=50>8</size> : 둔화 50%, 회복 100% 증폭​</color>" :
                    step == 3 ? "2 : 둔화 10%, 회복 10% 증폭\n4 :둔화 20%, 회복 30% 증폭\n<color=#333333><size=50>6</size> : 둔화 30%, 회복 50% 증폭​</color>\n8 : 둔화 50%, 회복 100% 증폭" :
                    step == 2 ? "2 : 둔화 10%, 회복 10% 증폭\n<color=#333333><size=50>4</size> :둔화 20%, 회복 30% 증폭​</color>\n6 : 둔화 30%, 회복 50% 증폭\n8 : 둔화 50%, 회복 100% 증폭" :
                    step == 1 ? "<color=#333333><size=50>2</size> : 둔화 10%, 회복 10% 증폭​</color>\n4 :둔화 20%, 회복 30% 증폭\n6 : 둔화 30%, 회복 50% 증폭\n8 : 둔화 50%, 회복 100% 증폭" :
                     "2 : 둔화 10%, 회복 10% 증폭\n4 :둔화 20%, 회복 30% 증폭\n6 : 둔화 30%, 회복 50% 증폭\n8 : 둔화 50%, 회복 100% 증폭";
                break;
            case 6:
                allStep[num] = step == 4 ? "2 : 데미지 2\n4 :데미지 4\n6 : 데미지 7\n<color=#333333><size=50>8</size> : 데미지 15​</color>" :
                    step == 3 ? "2 : 데미지 2\n4 :데미지 4\n<color=#333333><size=50>6</size> : 데미지 7​</color>\n8 : 데미지 15" :
                    step == 2 ? "2 : 데미지 2\n<color=#333333><size=50>4</size> :데미지 4​</color>\n6 : 데미지 7\n8 : 데미지 15" :
                    step == 1 ? "<color=#333333><size=50>2</size> : 데미지 2​</color>\n4 :데미지 4\n6 : 데미지 7\n8 : 데미지 15" :
                     "2 : 데미지 2\n4 :데미지 4\n6 : 데미지 7\n8 : 데미지 15";
                break;
            case 7:
                allStep[num] = step == 4 ? "2 : 데미지 10% 증폭\n4 : 데미지 30% 증폭\n6 : 데미지 50% 증폭\n<color=#333333><size=50>8</size> : 데미지 150% 증폭​</color>" :
                    step == 3 ? "2 : 데미지 10% 증폭\n4 : 데미지 30% 증폭\n<color=#333333><size=50>6</size> : 데미지 50% 증폭​</color>\n8 : 데미지 150% 증폭" :
                    step == 2 ? "2 : 데미지 10% 증폭\n<color=#333333><size=50>4</size> : 데미지 30% 증폭​</color>\n6 : 데미지 50% 증폭\n8 : 데미지 150% 증폭" :
                    step == 1 ? "<color=#333333><size=50>2</size> : 데미지 10% 증폭​</color>\n4 : 데미지 30% 증폭\n6 : 데미지 50% 증폭\n8 : 데미지 150% 증폭" :
                     "2 : 데미지 10% 증폭\n4 : 데미지 30% 증폭\n6 : 데미지 50% 증폭\n8 : 데미지 150% 증폭";
                break;
            case 8:
                allStep[num] = step == 4 ? "2 : 10%+ \n4 :20%+\n6 : 35%+\n<color=#333333><size=50>8</size> : 55%+​</color>" :
                    step == 3 ? "2 : 10%+ \n4 :20%+\n<color=#333333><size=50>6</size> : 35%+​</color>\n8 : 55%+" :
                    step == 2 ? "2 : 10%+ \n<color=#333333><size=50>4</size> :20%+​</color>\n6 : 35%+\n8 : 55%+" :
                    step == 1 ? "<color=#333333><size=50>2</size> : 10%+ ​</color>\n4 :20%+\n6 : 35%+\n8 : 55%+" :
                     "2 : 10%+ \n4 :20%+\n6 : 35%+\n8 : 55%+";
                break;
            case 9:
                allStep[num] = step == 4 ? "2 : 5+ \n4 :10+\n6 : 15%+\n<color=#333333><size=50>8</size> : 20+​</color>" :
                    step == 3 ? "2 : 5+ \n4 :10+\n<color=#333333><size=50>6</size> : 15%+​</color>\n8 : 20+" :
                    step == 2 ? "2 : 5+ \n<color=#333333><size=50>4</size> :10+​</color>\n6 : 15%+\n8 : 20+" :
                    step == 1 ? "<color=#333333><size=50>2</size> : 5+ ​</color>\n4 :10+\n6 : 15%+\n8 : 20+" :
                     "2 : 5+ \n4 :10+\n6 : 15%+\n8 : 20+";
                break;
        }

        stepinfo = allStep[num];
    }

}
