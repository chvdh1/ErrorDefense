using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    List<Ag> ags0 = new List<Ag>(); //�ǹ�
    List<Ag> ags1 = new List<Ag>();//���
    List<Ag> ags2 = new List<Ag>();//�÷�Ƽ��

    List<Ag> selags = new List<Ag>();//������ ����Ʈ �ӽú���

    public GameObject agGroup;
    AgButten[] agButtens = new AgButten[30];

    //����ü ��ư�� �̹���, ����
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
        agtext[0].text = "<size=50>������</size>\n����ġ 10 ȹ��";
        agtext[1].text = "<size=50>¬©�� ����</size>\n�������� 2��";
        agtext[2].text = "<size=50>����1</size>\n������ ������ \n2�� ȹ��";
        agtext[3].text = "<size=50>�ǹ�Ƽ��</size>\n10%Ȯ���� \n���ΰ�ħ ����";
        agtext[4].text = "<size=50>��������1</size>\n�Ʊ��� ���ݼӵ� \n10% ����";
        agtext[5].text = "<size=50>��Ȯ�� ����1</size>\n�Ʊ��� �� \n10%����";
        agtext[6].text = "<size=50>�ڰ�����1</size>\n�������� ����� \nü�� 2 ȸ��";
        agtext[7].text = "<size=50>�ǰ�</size>\nü�� 30 ����";
        agtext[8].text = "<size=50>�⵵��Ÿ1</size>\n5�ʸ��� \n����5+";
        agtext[9].text = "<size=50>����1</size>\nü���� ���� ������\n���鿡�� 10�����";
        agtext[10].text = "<size=50>����</size>\n����ġ\n������ = +2,\n������ = +3";
        agtext[11].text = "<size=50>�ݵ�1</size>\n�ִ�70���� \n���� ȹ��";
        agtext[12].text = "<size=50>����2</size>\n������ ������ \n5�� �� 2�� ȹ��";
        agtext[13].text = "<size=50>���Ƽ��</size>\n30%ȹ���� \n���ΰ�ħ ����";
        agtext[14].text = "<size=50>��������2</size>\n�Ʊ��� ���ݼӵ� \n25%����";
        agtext[15].text = "<size=50>��Ȯ�Ѱ���</size>\n�Ʊ��� �� \n25%����";
        agtext[16].text = "<size=50>�ڰ�����2</size>\n�������� ����� \nü�� 5ȸ��";
        agtext[17].text = "<size=50>����1</size>\n���� ü�� 5�� \n���ݷ� 3%����";
        agtext[18].text = "<size=50>�⵵��Ÿ2</size>\n3�ʸ��� \n����5+";
        agtext[19].text = "<size=50>����2</size>\nü���� ���� ������\n������ �ִ�ü��\n10%�����";
        agtext[20].text = "<size=50>������</size>\n����ġ ���Ž�\n�߰�3 ȹ��";
        agtext[21].text = "<size=50>�ݵ�2</size>\n�ִ� 100����\n���� ȹ��";
        agtext[22].text = "<size=50>����3</size>\n������ ������\n5�� ȹ��";
        agtext[23].text = "<size=50>�����̾�Ƽ��</size>\n50%ȹ����\n���ΰ�ħ ����";
        agtext[24].text = "<size=50>��������3</size>\n�Ʊ��� ���ݼӵ�\n50%����";
        agtext[25].text = "<size=50>��Ȯ�� ����3</size>\n�Ʊ��� ��\n50%����";
        agtext[26].text = "<size=50>�ڰ�����3</size>\nü�¼Ҹ� �ִٸ�\n�̹� ��������\n�Ʊ� ���ݷ�\n70%����";
        agtext[27].text = "<size=50>����2</size>\n���� ü�� 1��\n���ݷ� 1%����";
        agtext[28].text = "<size=50>�⵵��Ÿ3</size>\n3�ʸ���\n���� 10+";
        agtext[29].text = "<size=50>����3</size>\nü���� ������\n������ ����\n0���� ����";
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

        switch (ran) // 0 = �ǹ� , 1 = ���, 2 =������
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
        if (agGroup.transform.parent.gameObject.activeSelf)
            return;
        agGroup.transform.parent.gameObject.SetActive(true);

        switch (agClass[agCount])
        {
            case 0:
                for (int i = 0; i < 3; i++)
                {
                    int ran = Random.Range(0, ags0.Count);
                    ags0[ran].aBtn.SetActive(true);
                    selags.Add(ags0[ran]);
                    ags0.Remove(ags0[ran]);
                }
                break;
            case 1:
                for (int i = 0; i < 3; i++)
                {
                    int ran = Random.Range(0, ags1.Count);
                    ags1[ran].aBtn.SetActive(true);
                    selags.Add(ags1[ran]);
                    ags1.Remove(ags1[ran]);
                }
                break;

            case 2:
                for (int i = 0; i < 3; i++)
                {
                    int ran = Random.Range(0, ags2.Count);
                    ags2[ran].aBtn.SetActive(true);
                    selags.Add(ags2[ran]);
                    ags2.Remove(ags2[ran]);
                }
                break;
        }


    }

    public void SelectAg()
    {
        //���õ� ����ü �ӽú��� ����Ʈ���� ����
        AgButten ab = EventSystem.current.currentSelectedGameObject.GetComponent<AgButten>();
        for (int i = 0; i < selags.Count; i++)
        {
            selags[i].aBtn.SetActive(false);

            if (ab.agNum == selags[i].aNum)
                selags.Remove(selags[i]);
        }

        //�����ֱ�
        AgEft(ab.agNum);
        aUibtns[ab.agNum].SetActive(true);
        aUibtns[ab.agNum].transform.position = posnum[agCount].transform.position;

        //���þȵ� �����ǵ�����
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
    public static int agGetCoin = 1; //������ ����
    public static int agReRollFree1 = 0;//���ΰ�ħ����1
    public static int agAtSpeed1 = 0;//����1
    public static int agAtDmg1 = 0;//��1
    public static int agHeal1 = 0;//ȸ��1
    public static int agGetMp1 = 0;//���� ȸ��1
    public static int agFormat1 = 0;//ü�� ������ ��
    public static int agGetExp = 0;//������ ����ġ
    public static int agInterest = 50;//�ִ�����
    public static int agReRollFree2 = 0;//���ΰ�ħ����2
    public static int agAtSpeed2 = 0;//����2
    public static int agAtDmg2 = 0;//��2
    public static int agHeal2 = 0;//ȸ��2
    public static int agBoold1 = 0;//���� ü��5�� �� 3����
    public static int agGetMp2 = 0;//���� ȸ��2
    public static int agFormat2 = 0;//ü�� ������ �۵�
    public static int agMoreGetExp = 0; //���Ž� �߰� ����ġ
    public static int agReRollFree3 = 0;//���ΰ�ħ����3
    public static int agAtSpeed3 = 0;//����3
    public static int agAtDmg3 = 0;//��3
    public static int agHeal3 = 0;//ȸ��3
    public static int agBoold2 = 0;//���� ü�´� ������
    public static int agGetMp3 = 0;//���� ȸ��3


    public void AgEft(int num)
    {
        GameManager.augmentation[num] = true;
        //1ȸ�� ����
        switch (num)
        {
            case 0://����ġ 10�߰�
                gm.exp += 10;
                gm.ui.ExpUpdate();
                break;

            case 2://������ ������ 2�� ȹ��
                break;

            case 7://ü�� 30 ����
                gm.maxHp += 30;
                gm.hp += 30;
                gm.ui.HpUpdate();
                break;

            case 12://������ ������ 5���� 2��ȹ��

                break;

            case 22://������ ������ 5�� ȹ��

                break;

        }

        agGetCoin = GameManager.augmentation[1] ? 2 : 1;//�������� 2��
        agReRollFree1 = GameManager.augmentation[3] ? 10 : 0;//10Ȯ���� ���ΰ�ħ ����
        agAtSpeed1 = GameManager.augmentation[4] ? 10 : 0;//�Ʊ��� ���ݼӵ� 10% ����
        agAtDmg1 = GameManager.augmentation[5] ? 10 : 0;//�Ʊ��� �� 10%����
        agHeal1 = GameManager.augmentation[6] ? 2 : 0;//�������� ����� ü�� 2ȸ��
        agGetMp1 = GameManager.augmentation[8] ? 5 : 0;//5�ʸ��� ���� 5+
        agFormat1 = GameManager.augmentation[9] ? 10 : 0;//ü���� ���� �� ���� ���鿡�� 10������
        agGetExp = GameManager.augmentation[10] ? 2 : 0;//����ġ �����߿� +2,������ +3
        agInterest = GameManager.augmentation[21] ? 100 : GameManager.augmentation[11] ? 70 : 50;//�ִ����� 70���� ���� Ȥ�� 100��
        agReRollFree2 = GameManager.augmentation[13] ? 30 : 0;//����ġ �����߿� +2,������ +3
        agAtSpeed2 = GameManager.augmentation[14] ? 25 : 0;//�Ʊ� ���� 25%����
        agAtDmg2 = GameManager.augmentation[15] ? 25 : 0;//�Ʊ��� �� 25% ����
        agHeal2 = GameManager.augmentation[16] ? 5 : 0;//�������� ����� ü�� 5ȸ��
        agBoold1 = GameManager.augmentation[17] ? 3 : 0;//���� ü�� 5�� �� 3����
        agGetMp2 = GameManager.augmentation[18] ? 5 : 0;//3�ʸ��� ���� +5
        agFormat2 = GameManager.augmentation[19] ? 10 : 0;//ü�� ���� �� ���� ������ ü�� 10%�����
        agMoreGetExp = GameManager.augmentation[20] ? 3 : 0;//����ġ ���Ž� 3�߰�
        agReRollFree3 = GameManager.augmentation[22] ? 50 : 0;//50% ���ΰ�ħ ����
        agAtSpeed3 = GameManager.augmentation[24] ? 50 : 0;//�Ʊ� ���� 50%����
        agAtDmg3 = GameManager.augmentation[25] ? 50 : 0;//�Ʊ��� �� 50% ����
        agHeal3 = GameManager.augmentation[26] ? 70 : 0;//ü�� ��Ұ� �ִٸ� �Ʊ� ���ݷ� 70% ����
        //agBoold2 = ���� ü�´� ���ݷ� ����************************** 
        agGetMp3 = GameManager.augmentation[28] ? 10 : 0;//3�ʸ��� ���� 10+
        //����3 = ü�� ������ �� ���� 0************************** 
    }
}
