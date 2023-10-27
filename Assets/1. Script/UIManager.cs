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
    public int sStep = 0;
    public int sSNum = 0;

    public SynergyInfo(string Name, int count, int step, int num)
    {
        sName = Name;
        sCount = count;
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
            sBtn[i] = sGroup.transform.GetChild(i).gameObject.GetComponent<GameObject>();
        }

        gm = GameManager.Instance;
    }

    public void HpUpdate()
    {
        float a = gm.hp / gm.maxHp;
        Debug.Log(a);
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
        SynergyInfo node = new SynergyInfo("���", 0, 0,0);
        synergyInfos.Add(node);
        node = new SynergyInfo("ħ��", 0, 0,1);
        synergyInfos.Add(node);
        node = new SynergyInfo("����", 0, 0,2);
        synergyInfos.Add(node);
        node = new SynergyInfo("�ӱ�", 0, 0,3);
        synergyInfos.Add(node);
        node = new SynergyInfo("ħ��", 0, 0,4);
        synergyInfos.Add(node);
        node = new SynergyInfo("����", 0, 0,5);
        synergyInfos.Add(node);
        node = new SynergyInfo("������", 0, 0,6);
        synergyInfos.Add(node);
        node = new SynergyInfo("������Ÿ����", 0, 0,7);
        synergyInfos.Add(node);
    }
   
    int stepDESC(SynergyInfo a, SynergyInfo b) //DESC : ������������(���� ������ ���� ������ ����)
    {
        return b.sStep.CompareTo(a.sStep);
    }

    public void SynergyUpdate()
    {
        //Ȱ��ȭ �� �ó��� �� ���� �ܰ� ������ ����
        synergyInfos.Sort(stepDESC);//Sort : ����Ʈ ���� �Լ�

        //���ɵ� ����Ʈ�� ������ ���� ���̶���Ű ����
        for (int i = synergyInfos.Count-1; i >= 0; i--)
        {
            sBtn[i].transform.SetSiblingIndex(i);
        }

        //�ϳ��� ������ �ִ� �ó��� �ð�ȭ
        for (int i = 0; i < synergyInfos.Count; i++)
        {
            if (synergyInfos[i].sCount > 0)
                sBtn[i].SetActive(true);
            
            else
                sBtn[i].SetActive(false);
        }
    }
}
