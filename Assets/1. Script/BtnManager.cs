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

    GameManager gm;
    ShopManager sm;
    WaitingSeat ws;
    UIManager ui;

    public PoolManager[] costObjs = new PoolManager[5];

    private void Awake()
    {
        Btn = this;
        gm = gameObject.GetComponent<GameManager>();
        sm = ShopManager.shopManager;
        ws = WaitingSeat.waitingSeat;
        ui = UIManager.uIManager;
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
            if (i < 9 && ws.boolint[i] == 0)
            {
                ws.boolint[i] = 1;
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
          
        Transform ch = costObjs[ccost - 1].Get(cc.Num).transform;
        
        Fire fi = ch.gameObject.GetComponent<Fire>();
        fi.bulletPool = gm.bulletPool;
        ch.position = ws.pos[seatPosNum].transform.position;
        cc.gameObject.SetActive(false);
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
