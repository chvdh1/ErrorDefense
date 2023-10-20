using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.PackageManager;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static float HP;
    public static int Lv;

    int[] TierPercentage = new int[4]; //�� ������ �´� Ȯ�� ����

    public static int Synergy;
    public static int Augmentation;
    public static int Gamestat; // 0 = �κ� , 1 ���� ���� , 


    public void Reroll() //ī�� ��ȯ
    {
        int cells = 5; //������ ī�� ��
        
        // ���� ���� �� Ȯ�� ����
        switch(Lv)
        {
            case 1:
            case 2:
                TierPercentage[0] = 100;
                break;
            case 3:
                TierPercentage[0] = 75;
                TierPercentage[1] = 25;
                break;
            case 4:
                TierPercentage[0] = 55;
                TierPercentage[1] = 30;
                TierPercentage[2] = 15;
                break;
            case 5:
                TierPercentage[0] = 45;
                TierPercentage[1] = 33;
                TierPercentage[2] = 20;
                TierPercentage[3] = 2;
                break;
            case 6:
                TierPercentage[0] = 25;
                TierPercentage[1] = 40;
                TierPercentage[2] = 30;
                TierPercentage[3] = 5;
                break;
            case 7:
                TierPercentage[0] = 19;
                TierPercentage[1] = 30;
                TierPercentage[2] = 35;
                TierPercentage[3] = 15;
                TierPercentage[4] = 4;
                break;
            case 8:
                TierPercentage[0] = 16;
                TierPercentage[1] = 20;
                TierPercentage[2] = 35;
                TierPercentage[3] = 25;
                TierPercentage[4] = 4;
                break;
            case 9:
                TierPercentage[0] = 9;
                TierPercentage[1] = 15;
                TierPercentage[2] = 30;
                TierPercentage[3] = 30;
                TierPercentage[4] = 16;
                break;
            case 10:
                TierPercentage[0] = 5;
                TierPercentage[1] = 10;
                TierPercentage[2] = 20;
                TierPercentage[3] = 40;
                TierPercentage[4] = 25;
                break;
            case 11:
                TierPercentage[0] = 1;
                TierPercentage[1] = 2;
                TierPercentage[2] = 12;
                TierPercentage[3] = 50;
                TierPercentage[4] = 35;
                break;
        }

        for (int i = 0; i < cells; i++)
        {
            int ran = Random.Range(1, 101);

            if (ran <= TierPercentage[0]) //1Ƽ�� ��ȯ
            { }
            else if (ran <= TierPercentage[0]+TierPercentage[1])//2Ƽ�� ��ȯ
            { }
            else if (ran <= TierPercentage[0] + TierPercentage[1]+TierPercentage[2])//3Ƽ�� ��ȯ
            { }
            else if (ran <= TierPercentage[0] + TierPercentage[1] + TierPercentage[2] + TierPercentage[3])//4Ƽ�� ��ȯ
            { }
            else//5Ƽ�� ��ȯ
            { }
        }

    }

}
