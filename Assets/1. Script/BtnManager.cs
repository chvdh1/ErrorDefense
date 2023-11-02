using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnManager : MonoBehaviour
{
    public static BtnManager Btn;

    public GameObject poolG;
    public Transform manager;
    GameManager gm;
    public ShopManager sm;
    WaitingSeat ws;
    UIManager ui;

    public PoolManager[] costObjs = new PoolManager[5];

    private void Awake()
    {
        Btn = this;
        gm = gameObject.GetComponent<GameManager>();
        ws = gm.ws;
        ui = manager.GetChild(0).gameObject.GetComponent<UIManager>();
        for (int i = 0; i < costObjs.Length; i++)
        {
            costObjs[i] = poolG.transform.GetChild(i+2).gameObject.GetComponent<PoolManager>();
        }
    }

    public void StageOneBtn()
    {
        gm.mapIndex = 1;
        gm.GameStart();       
    }

    public void BuyChamp()//챔프 구매시
    {
        //대기석 위치 선정
        int seatPosNum = 0;
        bool full = false;
        for (int i = 0; i < 10; i++)
        {
            if (i < 9 && ws.obj[i] == null)
            {
                seatPosNum = i;
                full = false;
                break;
            }
            else
                full = true;
        }

        if (full)
        {
            StartCoroutine(ui.NoWaitingSeat());
            return;
        }
        ChampCard cc = EventSystem.current.currentSelectedGameObject.GetComponent<ChampCard>();
        int ccost = cc.champCost;

        if (gm.coin < ccost)
        {
            StartCoroutine(ui.NoCoin());
            return;
        }
        else
        {
            gm.coin -= ccost;
            ui.CoinUpdate();
        }
          
        Transform ch = costObjs[ccost - 1].Get(cc.Num-1).transform;
        
        Fire fi = ch.gameObject.GetComponent<Fire>();
        fi.bulletPool = gm.bulletPool;
        fi.seaNum = seatPosNum;
        ch.position = ws.pos[seatPosNum].transform.position;
        ws.obj[seatPosNum] = ch.gameObject;
        cc.gameObject.SetActive(false);

        //카드 수량 제한
        switch(ccost)
        {
            case 1:
                sm.cost1[cc.Num - 1].cCount++;
                Debug.Log(sm.cost1[cc.Num - 1].cName + "/" + sm.cost1[cc.Num - 1].cCount + "/" + sm.cost1[cc.Num - 1].cMax);
                break;
            case 2:
                sm.cost2[cc.Num - 1].cCount++;
                Debug.Log(sm.cost2[cc.Num - 1].cName + "/" + sm.cost2[cc.Num - 1].cCount + "/" + sm.cost2[cc.Num - 1].cMax);
                break;
            case 3:
                sm.cost3[cc.Num - 1].cCount++;
                Debug.Log(sm.cost3[cc.Num - 1].cName + "/" + sm.cost3[cc.Num - 1].cCount + "/" + sm.cost3[cc.Num - 1].cMax);
                break;
            case 4:
                sm.cost4[cc.Num - 1].cCount++;
                Debug.Log(sm.cost4[cc.Num - 1].cName + "/" + sm.cost4[cc.Num - 1].cCount + "/" + sm.cost4[cc.Num - 1].cMax);
                break;
            case 5:
                sm.cost5[cc.Num - 1].cCount++;
                Debug.Log(sm.cost5[cc.Num - 1].cName + "/" + sm.cost5[cc.Num - 1].cCount + "/" + sm.cost5[cc.Num - 1].cMax);
                break;
        }
    }

    public void BuyExp()
    {
        if (gm.coin < 4)
        {
            ui.StartCoroutine(ui.NoCoin());
            return;
        }

        gm.coin -= 4;
        gm.exp += 4;
        ui.ExpUpdate();
        ui.CoinUpdate();
    }

    public void ReRollBtn()
    {
        if (gm.coin < 2)
        {
            ui.StartCoroutine(ui.NoCoin());
            return;
        }

        gm.coin -= 2;
        ui.CoinUpdate();

        gm.Reroll();
    }

}
