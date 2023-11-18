using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Champitems : MonoBehaviour
{
    public ItemSocket[] sockets = new ItemSocket[3];

    public float cDmg = 0;
    public float cAttSpeed = 0;
    public float cCriPer = 0;
    public float cCriDmg = 0;
    public float cSkillDmg = 0;
    public float cMp = 0;
    public float cDistance = 0;

    //조합 능력치
    public float cMaxMp = 0;
    public float cAttMp = 0;
    public float cSkillMDmg = 0;
    public bool cDefDebuff = false;
    public float cTrueDmg = 0;
    public float cCriMp = 0;
    public float cMoreAttSpeed = 0;
    public bool cSkillCri = false;
    public float cMoreSkillAttSpeed = 0;
    public bool cMoreAtt = false;
    public float cMoreCriAttSpeed = 0;
    public bool cSuperCri = false;

    ItemManager itemManager;


    private void Awake()
    {
        
        for (int i = 0; i < sockets.Length; i++)
        {
            sockets[i] = transform.GetChild(3).GetChild(i).gameObject.GetComponent<ItemSocket>();
        }
    }

    private void OnEnable()
    {
        if(itemManager == null)
            itemManager = ItemManager.Instance;
    }

    public void ItemEffect()
    {
        //아이템 넘버에 따른 이미지 넘버 주기
        
        //소켓들의 정보 정리
        for (int i = 0; i < sockets.Length; i++)
        {
            if (sockets[i].itemNum != 0)
            {
                int imNum = sockets[i].itemNum < 10 ? sockets[i].itemNum - 1 : sockets[i].itemNum < 20 ? sockets[i].itemNum - 4 :
                     sockets[i].itemNum < 30 ? sockets[i].itemNum - 8 : sockets[i].itemNum < 40 ? sockets[i].itemNum - 13 :
                      sockets[i].itemNum < 50 ? sockets[i].itemNum - 19 : sockets[i].itemNum < 60 ? sockets[i].itemNum - 24 :
                       sockets[i].itemNum < 70 ? sockets[i].itemNum - 34 : 34;
                sockets[i].ItemStat();
                sockets[i].st.sprite = itemManager.itemImg[imNum];
            }
        }


        cDmg = 0;
        cAttSpeed = 0;
        cCriPer = 0;
        cCriDmg = 0;
        cSkillDmg = 0;
        cMp = 0;
        cDistance = 0;

        cMaxMp = 0;
        cAttMp = 0;
        cSkillMDmg = 0;
        cDefDebuff = false;
        cTrueDmg = 0;
        cCriMp = 0;
        cMoreAttSpeed = 0;
        cSkillCri = false;
        cMoreSkillAttSpeed = 0;
        cMoreAtt = false;
        cMoreCriAttSpeed = 0;
        cSuperCri = false;


        for (int i = 0; i < sockets.Length; i++)
        {
            if (sockets[i] != null)
            {
                cDmg += sockets[i].iDmg;
                cAttSpeed += sockets[i].iAttSpeed;
                cCriPer += sockets[i].iCriPer;
                cCriDmg += sockets[i].iCriDmg;
                cSkillDmg += sockets[i].iSkillDmg;
                cMp += sockets[i].iMp;
                cDistance += sockets[i].iDistance;

                cMaxMp = sockets[i].iMaxMp;
                cAttMp = sockets[i].iAttMp;
                cSkillMDmg = sockets[i].iSkillMDmg;
                
                cTrueDmg = sockets[i].iTrueDmg;
                cCriMp = sockets[i].iCriMp;
                cMoreAttSpeed = sockets[i].iMoreAttSpeed;
                cMoreSkillAttSpeed = sockets[i].iMoreSkillAttSpeed;
                cMoreCriAttSpeed = sockets[i].iMoreCriAttSpeed;
                
                if(!cDefDebuff)
                    cDefDebuff = sockets[i].iDefDebuff;
                if (!cSkillCri)
                    cSkillCri = sockets[i].iSkillCri;
                if (!cMoreAtt)
                    cMoreAtt = sockets[i].iMoreAtt;
                if (!cSuperCri)
                    cSuperCri = sockets[i].iSuperCri;
            }
        }

    }

}
