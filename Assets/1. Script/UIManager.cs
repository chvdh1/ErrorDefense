using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SynergyInfo
{
    public string sName = "";
    public int sCount = 0;
    public int sStep1 = 0;
    public int sStep2 = 0;
    public int sStep3 = 0;
    public int sStep4 = 0;
    public int sStep = 0;
    public int sSNum = 0;

    public SynergyInfo(string Name, int count, int step1, int step2, int step3, int step4, int step, int num)
    {
        sName = Name;
        sCount = count;
        sStep1 = step1;
        sStep2 = step2;
        sStep3 = step3;
        sStep4 = step4;
        sStep = step;
        sSNum = num;
    }
}


public class UIManager : MonoBehaviour
{
    public static UIManager uIManager;

    GameManager gm;

    public Image timeBar;
    public GameObject lvHp;
    Image hp;
    Image exp;
    Text lvText;
    Text hpText;
    Text expText;

    public Text coinText;
    public Image continuity;
    public Sprite[] continuitySprites;
    Text continuityText;
    public Text noText;
    public GameObject sGroup;
    GameObject[] sBtn = new GameObject[8];
    Text[] sBtnText = new Text[8];

    private void Awake()
    {
        uIManager = this;
        hp = lvHp.transform.GetChild(0).gameObject.GetComponent<Image>();
        exp = lvHp.transform.GetChild(2).gameObject.GetComponent<Image>();
        lvText = lvHp.transform.GetChild(3).GetChild(0).gameObject.GetComponent<Text>();
        expText = lvHp.transform.GetChild(3).GetChild(1).gameObject.GetComponent<Text>();
        hpText = lvHp.transform.GetChild(4).gameObject.GetComponent<Text>();
        continuityText = continuity.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        for (int i = 0; i < sBtn.Length; i++)
        {
            sBtn[i] = sGroup.transform.GetChild(i).gameObject;
            sBtnText[i] = sBtn[i].transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
        }

        gm = GameManager.Instance;
    }

    public void HpUpdate()
    {
        float a = gm.hp / gm.maxHp;
        hp.fillAmount = a;
        hpText.text = string.Format("{0}", gm.hp);
    }

    public void ExpUpdate()
    {
        
        if (gm.exp >= gm.maxExp[gm.lv])
        {
            float reExp = gm.exp - gm.maxExp[gm.lv];
            gm.exp = 0;

            gm.lv++;
            gm.exp += reExp;
            lvText.text = string.Format("{0}", gm.lv);
        }
        float a = gm.exp / gm.maxExp[gm.lv];
        exp.fillAmount = a;
        expText.text = string.Format("{0} / {1}", gm.exp, gm.maxExp[gm.lv]);
    }

    public void CoinUpdate()
    {
        coinText.text = string.Format("{0}", gm.coin);
    }

    public IEnumerator NoCoin()
    {
        if (noText.color.a > 0.1f)
            yield break;

        noText.text = "������ �����մϴ�.";
        noText.color = new Color(1, 1, 1, 1);
        float c = 1;
        while (noText.color.a > 0)
        {
            noText.color = new Color(1, 1, 1, c);
            c -= 0.5f*Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
    public IEnumerator NoWaitingSeat()
    {
        if (noText.color.a > 0.1f)
            yield break;
        noText.text = "��⼮�� �ڸ��� �����ϴ�.";
        noText.color = new Color(1, 1, 1, 1);
        float c = 1;
        while (noText.color.a > 0)
        {
            noText.color = new Color(1, 1, 1, c);
            c -= 0.5f * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
    public IEnumerator NoEmptySeats()
    {
        if (noText.color.a > 0.1f)
            yield break;

        noText.text = "���ڸ��� �����ϴ�.";
        noText.color = new Color(1, 1, 1, 1);
        float c = 1;
        while (noText.color.a > 0)
        {
            noText.color = new Color(1, 1, 1, c);
            c -= 0.5f * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator NoSetPos()
    {
        if (noText.color.a > 0.1f)
            yield break;

        noText.text = "��ġ�� �� ���� �����Դϴ�.";
        noText.color = new Color(1, 1, 1, 1);
        float c = 1;
        while (noText.color.a > 0)
        {
            noText.color = new Color(1, 1, 1, c);
            c -= 0.5f * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }


    public void ContinuityUpdate()
    {
        continuity.sprite = gm.continuity < 0 ? continuitySprites[0] : continuitySprites[1];

        int count = Mathf.Abs(gm.continuity); // ���밪 ǥ��
        continuityText.text = string.Format("{0}", count);
    }

    List<SynergyInfo> synergyInfos = new List<SynergyInfo>();

    public void SynergyReset()
    {
        SynergyInfo node = new SynergyInfo("���", 0, 2, 4, 6, 8,0, 1);
        synergyInfos.Add(node);
        node = new SynergyInfo("ħ��", 0, 2, 4, 6, 8, 0, 2);
        synergyInfos.Add(node);
        node = new SynergyInfo("����", 0, 2, 4, 6, 8, 0, 3);
        synergyInfos.Add(node);
        node = new SynergyInfo("�ӱ�", 0, 2, 4, 6, 8, 0, 4);
        synergyInfos.Add(node);
        node = new SynergyInfo("���", 0, 2, 4, 6, 8, 0, 5);
        synergyInfos.Add(node);
        node = new SynergyInfo("����", 0, 2, 4, 6, 8, 0, 6);
        synergyInfos.Add(node);
        node = new SynergyInfo("������", 0, 2, 4, 6, 8, 0, 7);
        synergyInfos.Add(node);
        node = new SynergyInfo("������Ÿ����", 0, 2, 4, 6, 8, 0, 8);
        synergyInfos.Add(node);
    }


    int countDESC(SynergyInfo a, SynergyInfo b) //DESC : ������������(���� ������ ���� ������ ����)
    {
        return b.sCount.CompareTo(a.sCount);
    }

    int stepDESC(SynergyInfo a, SynergyInfo b) //DESC : ������������(���� ������ ���� ������ ����)
    {
        return b.sStep.CompareTo(a.sStep);
    }

    Color c0 = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    Color c1 = new Color(0.8f,0.8f,0.8f,1);
    Color c2 = new Color(0.8f, 0.5f, 0.3f, 1);
    Color c3 = new Color(0.95f, 0.95f, 0.95f, 1);
    Color c4 = new Color(1, 1, 0.5f, 1);

    public void SynergyUpdate()
    {
        //���ӸŴ����� ������ �ִ� �ó��� ������ ����Ʈ�� �߰�
        //�ó��� �ܰ� �ֽ�ȭ
        for (int i = 0; i < synergyInfos.Count; i++)
        {
            synergyInfos[i].sCount = gm.synergy[i + 1];
            synergyInfos[i].sStep = synergyInfos[i].sCount >= synergyInfos[i].sStep4 ? 4 :
                synergyInfos[i].sCount >= synergyInfos[i].sStep3 ? 3 :
                synergyInfos[i].sCount >= synergyInfos[i].sStep2 ? 2 :
                synergyInfos[i].sCount >= synergyInfos[i].sStep1 ? 1 : 0;
        }

        //�ó��� ī��Ʈ�� ���� ������ ���� ����
        synergyInfos.Sort(countDESC);

        //Ȱ��ȭ �� �ó��� �� ���� �ܰ� ������ ����
        synergyInfos.Sort(stepDESC);//Sort : ����Ʈ ���� �Լ�



        //�ϳ��� ������ �ִ� �ó��� �ð�ȭ �� �ܰ迡 �°� ���� ��ȭ
        for (int i = synergyInfos.Count - 1; i >= 0; i--)
        {
            if (synergyInfos[i].sCount > 0)
            {
                sBtn[i].SetActive(true);
                sBtn[i].transform.SetSiblingIndex(i); //���ɵ� ����Ʈ�� ������ ���� ���̶���Ű ����
                Image im = sBtn[i].GetComponent<Image>();
                im.color = synergyInfos[i].sStep == 4 ? c4 :
                    synergyInfos[i].sStep == 3 ? c3 :
                    synergyInfos[i].sStep == 2 ? c2 :
                    synergyInfos[i].sStep == 1 ? c1 : c0;

                //�˸´� �ý�Ʈ ��ȭ
                sBtnText[i].text = synergyInfos[i].sStep == 4 ? string.Format("{0} {1}\n{2} / {3} / {4} / <size=200>{5}</size>", synergyInfos[i].sName, synergyInfos[i].sCount, synergyInfos[i].sStep1, synergyInfos[i].sStep2, synergyInfos[i].sStep3, synergyInfos[i].sStep4) :
                     synergyInfos[i].sStep == 3 ? string.Format("{0} {1}\n{2} / {3} / <size=200>{4}</size> / {5}", synergyInfos[i].sName, synergyInfos[i].sCount, synergyInfos[i].sStep1, synergyInfos[i].sStep2, synergyInfos[i].sStep3, synergyInfos[i].sStep4) :
                      synergyInfos[i].sStep == 2 ? string.Format("{0} {1}\n{2} / <size=200>{3}</size> / {4} / {5}", synergyInfos[i].sName, synergyInfos[i].sCount, synergyInfos[i].sStep1, synergyInfos[i].sStep2, synergyInfos[i].sStep3, synergyInfos[i].sStep4) :
                       synergyInfos[i].sStep == 1 ? string.Format("{0} {1}\n<size=200>{2}</size> / {3} / {4} / {5}", synergyInfos[i].sName, synergyInfos[i].sCount, synergyInfos[i].sStep1, synergyInfos[i].sStep2, synergyInfos[i].sStep3, synergyInfos[i].sStep4) :
                       string.Format("{0} {1}\n{2} / {3} / {4} / {5}", synergyInfos[i].sName, synergyInfos[i].sCount, synergyInfos[i].sStep1, synergyInfos[i].sStep2, synergyInfos[i].sStep3, synergyInfos[i].sStep4);

                Debug.Log(synergyInfos[i].sName + "/" + synergyInfos[i].sCount+ "/" + synergyInfos[i].sStep);
            }
            else if (sBtn[i].activeSelf)
                sBtn[i].SetActive(false);
        }


        //�� �ó����� �´� ȿ�� �ߵ�
        SynergyE();
       
    }
    public void SynergyE()
    {
        switch (synergyInfos[0].sStep) //0 ���(��ȭ + ü�� ȸ��)
        {
            //fl sl =  (speed-slow) <= 0 ? 0.1f : speed-slow;
            //fl S = (speed-slow) * t * (1 - ( slowX / 100))
            case 0:
                SynergyManager.slow = 0;
                SynergyManager.heal = 0;
                break;
            case 1:
                SynergyManager.slow = 1;
                SynergyManager.heal = 1;
                break;
            case 2:
                SynergyManager.slow = 1;
                SynergyManager.heal = 2;
                break;
            case 3:
                SynergyManager.slow = 2;
                SynergyManager.heal = 2;
                break;
            case 4:
                SynergyManager.slow = 3;
                SynergyManager.heal = 3;
                break;
        }
        switch (synergyInfos[1].sStep)   //ħ��(��Ÿ� +)
        {
            case 0:
                SynergyManager.sDistance = 0;
                break;
            case 1:
                SynergyManager.sDistance = 1;
                break;
            case 2:
                SynergyManager.sDistance = 2;
                break;
            case 3:
                SynergyManager.sDistance = 3;
                break;
            case 4:
                SynergyManager.sDistance = 5;
                break;
        }
        switch (synergyInfos[2].sStep)   //����(������)
        {
            //fl dmgs = (dmg + pDmg) *  pDmgX/100;
            case 0:
                SynergyManager.pDmg = 0;
                break;
            case 1:
                SynergyManager.pDmg = 1;
                break;
            case 2:
                SynergyManager.pDmg = 3;
                break;
            case 3:
                SynergyManager.pDmg = 5;
                break;
            case 4:
                SynergyManager.pDmg = 10;
                break;
        }
        switch (synergyInfos[3].sStep)   //�ӱ�(���� + ź�� +)
        {
            case 0:
                SynergyManager.sSpeed = 0;
                SynergyManager.attspeed = 0;
                break;
            case 1:
                SynergyManager.sSpeed = 1;
                SynergyManager.attspeed = 20;
                break;
            case 2:
                SynergyManager.sSpeed = 2;
                SynergyManager.attspeed = 35;
                break;
            case 3:
                SynergyManager.sSpeed = 3;
                SynergyManager.attspeed = 55;
                break;
            case 4:
                SynergyManager.sSpeed = 5;
                SynergyManager.attspeed = 95;
                break;
        }


        switch (synergyInfos[4].sStep)   //���(ġ��Ÿ Ȯ��, ����+)
        {
            case 0:
                SynergyManager.criPer = 0;
                SynergyManager.criDmg = 0;
                break;
            case 1:
                SynergyManager.criPer = 10;
                SynergyManager.criDmg = 10;
                break;
            case 2:
                SynergyManager.criPer = 20;
                SynergyManager.criDmg = 20;
                break;
            case 3:
                SynergyManager.criPer = 30;
                SynergyManager.criDmg = 35;
                break;
            case 4:
                SynergyManager.criPer = 45;
                SynergyManager.criDmg = 65;
                break;
        }
        switch (synergyInfos[5].sStep)   //����(��ȭ + ü�� ȸ�� *)
        {
            //fl sl =  (speed-slow) <= 0 ? 0.1f : speed-slow;
            //fl S = (speed-slow) * t * (1 - ( slowX / 100))
            //fl H = heal + (heal * healX)
            case 0:
                SynergyManager.slowX = 0;
                SynergyManager.healX = 0;
                break;
            case 1:
                SynergyManager.slowX = 10;
                SynergyManager.healX = 20;
                break;
            case 2:
                SynergyManager.slowX = 20;
                SynergyManager.healX = 30;
                break;
            case 3:
                SynergyManager.slowX = 30;
                SynergyManager.healX = 50;
                break;
            case 4:
                SynergyManager.slowX = 50;
                SynergyManager.healX = 100;
                break;
        }
        switch (synergyInfos[6].sStep)   //������(�ʴ� ��ü ������)
        {
            case 0:
                SynergyManager.allAttdmg = 0;
                break;
            case 1:
                SynergyManager.allAttdmg = 2;
                break;
            case 2:
                SynergyManager.allAttdmg = 4;
                break;
            case 3:
                SynergyManager.allAttdmg = 7;
                break;
            case 4:
                SynergyManager.allAttdmg = 15;
                break;
        }
        switch (synergyInfos[7].sStep)   //������Ÿ����(������ x )
        {
            //fl dmgs = (dmg + pDmg) *  pDmgX/100;
            case 0:
                SynergyManager.pDmgX = 100;
                break;
            case 1:
                SynergyManager.pDmgX = 110;
                break;
            case 2:
                SynergyManager.pDmgX = 130;
                break;
            case 3:
                SynergyManager.pDmgX = 150;
                break;
            case 4:
                SynergyManager.pDmgX = 250;
                break;
        }


        //�ʵ� ���ֵ鿡�� ����
        for (int i = 0; i < gm.fieldUnit.Length; i++)
        {
            if (gm.fieldUnit[i] != null)
            {
                Targeting tt = gm.fieldUnit[i].GetComponent<Targeting>();
                tt.scanRange = tt.defScanRange + SynergyManager.sDistance;
                Fire fi = gm.fieldUnit[i].GetComponent<Fire>();
                fi.dmg = (fi.defDmg[fi.lv] + SynergyManager.pDmg) * SynergyManager.pDmgX / 100;
                fi.shootTime = (fi.defShootTime + SynergyManager.attspeed);
                fi.shootSpeed = (fi.defShootSpeed + SynergyManager.sSpeed);
                fi.criPer = (fi.defCriPer + SynergyManager.criPer);
                fi.criDmg = (fi.defCriDmg + SynergyManager.criDmg);
            }
        }
    }
}
