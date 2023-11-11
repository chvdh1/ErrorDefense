using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class ItemSocket : MonoBehaviour
{
    public int itemNum;

    public float iDmg = 0;
    public float iAttSpeed = 0;
    public float iCriPer = 0;
    public float iCriDmg = 0;
    public float iSkillDmg = 0;
    public float iMp = 0;
    public float iDistance = 0;

    public void ItemStat()
    {
        switch (itemNum)
        {
            case 1:
                iMp = 20;
                break;
            case 2:
                iDmg = 10;
                break;
            case 3:
                iSkillDmg = 10;
                break;
            case 4:
                iDistance = 0.2f;
                break;
            case 5:
                iAttSpeed = 20;
                break;
            case 6:
                iCriDmg = 10;
                break;
            case 7:
                iCriPer = 10;
                break;


            case 11: //최대마나 20감소
                iMp = 40;
                break;
            case 12:// 평타당 10마나회복
                iMp = 20;
                iDmg = 10;
                break;
            case 13://스킬딜 =스킬 사용 횟수 *10 % 증가
                iMp = 20;
                iSkillDmg = 10;
                break;
            case 14://3번째 평타시 맞은 유닛 방어력 30 % 감소
                iMp = 20;
                iDistance = 0.2f;
                break;
            case 15://15의 추가 대미지
                iMp = 20;
                iAttSpeed = 20;
                break;
            case 16://평타, 스킬 딜 25%증가
                iMp = 20;
                iDmg = 25;
                iSkillDmg = 25;
                iCriPer = 10;                
                break;
            case 17://치명타시마나 30회복
                iMp = 20;
                iCriDmg = 10;
                break;


            case 22: // 평타딜 추가 30%+

                iDmg = 50;
                break;
            case 23://평타, 스킬 딜25 % +
                iDmg = 35;
                iSkillDmg = 35;
                break;
            case 24://사거리 0.3+ 평타딜 15 % +
                iDmg = 25;
                iDistance = 0.5f;
                break;
            case 25://평타 마다  공속 3 % +
                iDmg = 10;
                iAttSpeed = 20;
                break;
            case 26://스킬에 치명타 발동 치명 35 % + 평타딜 10 %
                iDmg = 20;
                iCriPer = 45;
                break;
            case 27://치명타 딜 20%+평타딜 10 % +
                iDmg = 20;
                iCriDmg = 30;
                break;


            case 33://스킬딜 50%+
                iSkillDmg = 70;
                break;
            case 34://사거리 0.3+스킬딜 15 % +
                iSkillDmg = 25;
                iDistance = 0.5f;
                break;
            case 35://스킬 사용시 공속 10 % +
                iSkillDmg = 20;
                iAttSpeed = 30;
                break;
            case 36://스킬에 치명타 발동 치명 35 % +스킬딜 10 %
                iSkillDmg = 20;
                iCriPer = 45;
                break;
            case 37://치명타 딜 20%+ 스킬딜 10 % +
                iSkillDmg = 20;
                iCriDmg = 30;
                break;


            case 44://사거리 2 +평타딜 10 % +
                iDistance = 2f;
                iDmg = 10;
                break;
            case 45://사거리 0.5 +속 20 % +
                iDistance = 0.5f;
                iAttSpeed = 40;
                break;
            case 46:// 사거리 0.5 + 치명 15 % +
                iDistance = 0.5f;
                iCriPer = 25;
                break;
            case 47:// 사거리 1 +  치명타 딜 20 % +
                iDistance = 1f;
                iCriDmg = 30;
                break;




            case 55://공속 40 % +20 % 확률로 2타
                iAttSpeed = 80;
                break;
            case 56://  치명 15 % + 공속 20 % +
                iAttSpeed = 30;
                iCriPer = 30;
                break;
            case 57:// 치명타시  공속 5 % +  치명타 딜 20 % +
                iAttSpeed = 10;
                iCriDmg = 30;
                break;


            case 66:
                iCriPer = 70;
                iDmg = 10;
                break;
            case 67:
                iCriPer = 45;
                iCriDmg = 45;
                break;


            case 77://10%확률로     +치딜 100 %
                iCriDmg = 100;
                break;
        }

    }
}

