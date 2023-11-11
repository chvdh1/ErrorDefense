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


            case 11: //�ִ븶�� 20����
                iMp = 40;
                break;
            case 12:// ��Ÿ�� 10����ȸ��
                iMp = 20;
                iDmg = 10;
                break;
            case 13://��ų�� =��ų ��� Ƚ�� *10 % ����
                iMp = 20;
                iSkillDmg = 10;
                break;
            case 14://3��° ��Ÿ�� ���� ���� ���� 30 % ����
                iMp = 20;
                iDistance = 0.2f;
                break;
            case 15://15�� �߰� �����
                iMp = 20;
                iAttSpeed = 20;
                break;
            case 16://��Ÿ, ��ų �� 25%����
                iMp = 20;
                iDmg = 25;
                iSkillDmg = 25;
                iCriPer = 10;                
                break;
            case 17://ġ��Ÿ�ø��� 30ȸ��
                iMp = 20;
                iCriDmg = 10;
                break;


            case 22: // ��Ÿ�� �߰� 30%+

                iDmg = 50;
                break;
            case 23://��Ÿ, ��ų ��25 % +
                iDmg = 35;
                iSkillDmg = 35;
                break;
            case 24://��Ÿ� 0.3+ ��Ÿ�� 15 % +
                iDmg = 25;
                iDistance = 0.5f;
                break;
            case 25://��Ÿ ����  ���� 3 % +
                iDmg = 10;
                iAttSpeed = 20;
                break;
            case 26://��ų�� ġ��Ÿ �ߵ� ġ�� 35 % + ��Ÿ�� 10 %
                iDmg = 20;
                iCriPer = 45;
                break;
            case 27://ġ��Ÿ �� 20%+��Ÿ�� 10 % +
                iDmg = 20;
                iCriDmg = 30;
                break;


            case 33://��ų�� 50%+
                iSkillDmg = 70;
                break;
            case 34://��Ÿ� 0.3+��ų�� 15 % +
                iSkillDmg = 25;
                iDistance = 0.5f;
                break;
            case 35://��ų ���� ���� 10 % +
                iSkillDmg = 20;
                iAttSpeed = 30;
                break;
            case 36://��ų�� ġ��Ÿ �ߵ� ġ�� 35 % +��ų�� 10 %
                iSkillDmg = 20;
                iCriPer = 45;
                break;
            case 37://ġ��Ÿ �� 20%+ ��ų�� 10 % +
                iSkillDmg = 20;
                iCriDmg = 30;
                break;


            case 44://��Ÿ� 2 +��Ÿ�� 10 % +
                iDistance = 2f;
                iDmg = 10;
                break;
            case 45://��Ÿ� 0.5 +�� 20 % +
                iDistance = 0.5f;
                iAttSpeed = 40;
                break;
            case 46:// ��Ÿ� 0.5 + ġ�� 15 % +
                iDistance = 0.5f;
                iCriPer = 25;
                break;
            case 47:// ��Ÿ� 1 +  ġ��Ÿ �� 20 % +
                iDistance = 1f;
                iCriDmg = 30;
                break;




            case 55://���� 40 % +20 % Ȯ���� 2Ÿ
                iAttSpeed = 80;
                break;
            case 56://  ġ�� 15 % + ���� 20 % +
                iAttSpeed = 30;
                iCriPer = 30;
                break;
            case 57:// ġ��Ÿ��  ���� 5 % +  ġ��Ÿ �� 20 % +
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


            case 77://10%Ȯ����     +ġ�� 100 %
                iCriDmg = 100;
                break;
        }

    }
}

