using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SynergyInfo
{
    public string sName = "";
    public int sCount = 0;
    public int sStep = 0;

    public SynergyInfo(string Name, int count, int step)
    {
        sName = Name;
        sCount = count;
        sStep = step;
    }
    public void PrintInfo()
    {
        string str = string.Format("이름({0}) 수량({1}) 단계({2})",
                                    sName, sCount, sStep);
        Debug.Log(str);
    }
}


public class UIManager : MonoBehaviour
{
    public static UIManager uIManager;

    GameManager gm;

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

    private void Awake()
    {
        uIManager = this;
        hp = transform.GetChild(0).gameObject.GetComponent<Image>();
        exp = transform.GetChild(2).gameObject.GetComponent<Image>();
        lvText = transform.GetChild(3).GetChild(0).gameObject.GetComponent<Text>();
        expText = transform.GetChild(3).GetChild(1).gameObject.GetComponent<Text>();
        hpText = transform.GetChild(4).gameObject.GetComponent<Text>();
        continuityText = continuity.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();


        gm = GameManager.Instance;
    }

    public void HpUpdate()
    {
        hp.fillAmount = gm.hp / gm.maxHp;
        hpText.text = string.Format("{0} / {1}", gm.hp, gm.maxHp);
    }

    public void ExpUpdate()
    {
        
        if (gm.exp > gm.maxExp[gm.lv])
        {
            int reExp = gm.exp - gm.maxExp[gm.lv];
            gm.exp = 0;

            gm.lv++;
            gm.exp += reExp;
            lvText.text = string.Format("{0}", gm.lv);
        }
        exp.fillAmount = gm.exp / gm.maxExp[gm.lv];
        expText.text = string.Format("{0} / {1}", gm.exp, gm.maxExp[gm.lv]);
    }

    public void CoinUpdate()
    {
        coinText.text = string.Format("{0}", gm.coin);
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
        SynergyInfo node = new SynergyInfo("백신", 0, 0);
        synergyInfos.Add(node);
        node = new SynergyInfo("침착", 0, 0);
        synergyInfos.Add(node);
        node = new SynergyInfo("신중", 0, 0);
        synergyInfos.Add(node);
        node = new SynergyInfo("속기", 0, 0);
        synergyInfos.Add(node);
        node = new SynergyInfo("침착", 0, 0);
        synergyInfos.Add(node);
        node = new SynergyInfo("생각", 0, 0);
        synergyInfos.Add(node);
        node = new SynergyInfo("디자인", 0, 0);
        synergyInfos.Add(node);
        node = new SynergyInfo("프로토타이핑", 0, 0);
        synergyInfos.Add(node);
        for (int i = 0; i < synergyInfos.Count; i++)
        {
            synergyInfos[i].PrintInfo();
        }
    }
   
    public void SynergyUpdate()
    {

    }
}
