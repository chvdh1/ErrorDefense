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
    public void PrintInfo()
    {
        string str = string.Format("이름({0}) 수량({1}) 단계({2}) 넘버({3})",
                                    sName, sCount, sStep, sSNum);
        Debug.Log(str);
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
        Debug.Log(a);
        exp.fillAmount = a;
        expText.text = string.Format("{0} / {1}", gm.exp, gm.maxExp[gm.lv]);
    }

    public void CoinUpdate()
    {
        coinText.text = string.Format("{0}", gm.coin);
    }

    public IEnumerator NoCoin()
    {
        if (noText.color.a != 0)
            yield break;

        noText.text = "코인이 부족합니다.";
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
        if (noText.color.a != 0)
            yield break;

        noText.text = "대기석에 자리가 없습니다.";
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
        if (noText.color.a != 0)
            yield break;

        noText.text = "빈자리가 없습니다.";
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
        if (noText.color.a != 0)
            yield break;

        noText.text = "설치할 수 없는 지역입니다.";
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

        int count = Mathf.Abs(gm.continuity); // 절대값 표시
        continuityText.text = string.Format("{0}", count);
    }

    List<SynergyInfo> synergyInfos = new List<SynergyInfo>();

    public void SynergyReset()
    {
        SynergyInfo node = new SynergyInfo("백신", 0, 0,0);
        synergyInfos.Add(node);
        node = new SynergyInfo("침착", 0, 0,1);
        synergyInfos.Add(node);
        node = new SynergyInfo("신중", 0, 0,2);
        synergyInfos.Add(node);
        node = new SynergyInfo("속기", 0, 0,3);
        synergyInfos.Add(node);
        node = new SynergyInfo("침착", 0, 0,4);
        synergyInfos.Add(node);
        node = new SynergyInfo("생각", 0, 0,5);
        synergyInfos.Add(node);
        node = new SynergyInfo("디자인", 0, 0,6);
        synergyInfos.Add(node);
        node = new SynergyInfo("프로토타이핑", 0, 0,7);
        synergyInfos.Add(node);
        for (int i = 0; i < synergyInfos.Count; i++)
        {
            synergyInfos[i].PrintInfo();
        }
    }
   
    int stepDESC(SynergyInfo a, SynergyInfo b) //DESC : 내림차순정렬(높은 순에서 낮은 순으로 정렬)
    {
        return b.sStep.CompareTo(a.sStep);
    }

    public void SynergyUpdate()
    {
        //활성화 된 시너지 중 높은 단계 순으로 정렬
        synergyInfos.Sort(stepDESC);//Sort : 리스트 정렬 함수

        //정령된 리스트의 순서에 따라 하이라이키 변경
        for (int i = synergyInfos.Count-1; i >= 0; i--)
        {
            sBtn[i].transform.SetSiblingIndex(i);
        }

        //하나라도 가지고 있는 시너지 시각화
        for (int i = 0; i < synergyInfos.Count; i++)
        {
            if (synergyInfos[i].sCount > 0)
                sBtn[i].SetActive(true);
            
            else
                sBtn[i].SetActive(false);
        }
    }
}
