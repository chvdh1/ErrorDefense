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
        num = transform.GetSiblingIndex();//������ ���° ����
        agim = GetComponent<Image>();

        allinfo[0] = "<size=50>������</size>\n����ġ 10 ȹ��";
        allinfo[1] = "<size=50>¬©�� ����</size>\n�������� 2��";
        allinfo[2] = "<size=50>����1</size>\n������ ������ \n2�� ȹ��";
        allinfo[3] = "<size=50>�ǹ�Ƽ��</size>\n10%Ȯ���� \n���ΰ�ħ ����";
        allinfo[4] = "<size=50>��������1</size>\n�Ʊ��� ���ݼӵ� \n10% ����";
        allinfo[5] = "<size=50>��Ȯ�� ����1</size>\n�Ʊ��� �� \n10%����";
        allinfo[6] = "<size=50>�ڰ�����1</size>\n�������� ����� \nü�� 2 ȸ��";
        allinfo[7] = "<size=50>�ǰ�</size>\nü�� 30 ����";
        allinfo[8] = "<size=50>�⵵��Ÿ1</size>\n5�ʸ��� \n����5+";
        allinfo[9] = "<size=50>����1</size>\nü���� ���� ������\n���鿡�� 10�����";
        allinfo[10] = "<size=50>����</size>\n����ġ\n������ = +2,\n������ = +3";
        allinfo[11] = "<size=50>�ݵ�1</size>\n�ִ�70���� \n���� ȹ��";
        allinfo[12] = "<size=50>����2</size>\n������ ������ \n5�� �� 2�� ȹ��";
        allinfo[13] = "<size=50>���Ƽ��</size>\n30%ȹ���� \n���ΰ�ħ ����";
        allinfo[14] = "<size=50>��������2</size>\n�Ʊ��� ���ݼӵ� \n25%����";
        allinfo[15] = "<size=50>��Ȯ�Ѱ���</size>\n�Ʊ��� �� \n25%����";
        allinfo[16] = "<size=50>�ڰ�����2</size>\n�������� ����� \nü�� 5ȸ��";
        allinfo[17] = "<size=50>����1</size>\n���� ü�� 5�� \n���ݷ� 3%����";
        allinfo[18] = "<size=50>�⵵��Ÿ2</size>\n3�ʸ��� \n����5+";
        allinfo[19] = "<size=50>����2</size>\nü���� ���� ������\n������ �ִ�ü��\n10%�����";
        allinfo[20] = "<size=50>������</size>\n����ġ ���Ž�\n�߰�3 ȹ��";
        allinfo[21] = "<size=50>�ݵ�2</size>\n�ִ� 100����\n���� ȹ��";
        allinfo[22] = "<size=50>����3</size>\n������ ������\n5�� ȹ��";
        allinfo[23] = "<size=50>�����̾�Ƽ��</size>\n50%ȹ����\n���ΰ�ħ ����";
        allinfo[24] = "<size=50>��������3</size>\n�Ʊ��� ���ݼӵ�\n50%����";
        allinfo[25] = "<size=50>��Ȯ�� ����3</size>\n�Ʊ��� ��\n50%����";
        allinfo[26] = "<size=50>�ڰ�����3</size>\nü�¼Ҹ� �ִٸ�\n�̹� ��������\n�Ʊ� ���ݷ�\n70%����";
        allinfo[27] = "<size=50>����2</size>\n���� ü�� 1��\n���ݷ� 1%����";
        allinfo[28] = "<size=50>�⵵��Ÿ3</size>\n3�ʸ���\n���� 10+";
        allinfo[29] = "<size=50>����3</size>\nü���� ������\n������ ����\n0���� ����";

        info = allinfo[num];
        agim.sprite = sprites[num];
    }


}
