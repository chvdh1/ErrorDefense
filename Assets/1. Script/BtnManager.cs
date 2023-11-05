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

    public void BuyChamp()//è�� ���Ž�
    {
        //��⼮ ��ġ ����
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
        Debug.Log("���� �Ϸ�!");
        
        Fire fi = ch.gameObject.GetComponent<Fire>();
        fi.bulletPool = gm.bulletPool;
        fi.seaNum = seatPosNum;
        ch.position = ws.pos[seatPosNum].transform.position;
        ws.obj[seatPosNum] = ch.gameObject;
        cc.gameObject.SetActive(false);

        //ī�� ���� ����
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

        //��ȭ Ȯ��
        int c = ((ccost - 1) * 8) + cc.Num - 1;
        for (int star1i = 0; star1i < gm.ul.lv1Units[c].Length; star1i++)
        {
            if (gm.ul.lv1Units[c][star1i] == null && star1i != 2)
            {
                gm.ul.lv1Units[c][star1i] = ch.gameObject;
                break;
            }
            else if (gm.ul.lv1Units[c][star1i] == null && star1i == 2)
            {
                Fire setunit1 = gm.ul.lv1Units[c][0].GetComponent<Fire>();
                Fire setunit2 = gm.ul.lv1Units[c][1].GetComponent<Fire>();
                setunit1.seaNum = 0;
                setunit1.inField = false;
                setunit2.seaNum = 0;
                setunit2.inField = false;
                for (int m = 0; m < gm.fieldUnit.Length; m++)
                {
                    if (gm.ul.lv1Units[c][0] == gm.fieldUnit[m] || gm.ul.lv1Units[c][1] == gm.fieldUnit[m])
                    {
                        gm.fieldUnit[m].SetActive(false);
                        gm.fieldUnit[m] = null;
                    }
                       
                }
                for (int w = 0; w < ws.obj.Length; w++)
                {
                    if (gm.ul.lv1Units[c][0] == ws.obj[w] || gm.ul.lv1Units[c][1] == ws.obj[w])
                    {
                        ws.obj[w].SetActive(false);
                        ws.obj[w] = null;
                    }
                }
                gm.ul.lv1Units[c][0] = null;
                gm.ul.lv1Units[c][1] = null;
                fi.lv++;
                fi.LvUp();
                for (int star2i = 0; star2i < gm.ul.lv2Units[c].Length; star2i++)
                {
                    if (gm.ul.lv2Units[c][star2i] == null && star2i != 2)
                    {
                        gm.ul.lv2Units[c][star2i] = ch.gameObject;
                        break;
                    }
                    else if (gm.ul.lv1Units[c][star2i] == null && star2i == 2)
                    {
                        Fire setunit3 = gm.ul.lv2Units[c][0].GetComponent<Fire>();
                        Fire setunit4 = gm.ul.lv2Units[c][1].GetComponent<Fire>();
                        setunit3.seaNum = 0;
                        setunit3.inField = false;
                        setunit4.seaNum = 0;
                        setunit4.inField = false;
                        for (int m = 0; m < gm.fieldUnit.Length; m++)
                        {
                            if (gm.ul.lv2Units[c][0] == gm.fieldUnit[m] || gm.ul.lv2Units[c][1] == gm.fieldUnit[m])
                            {
                                gm.fieldUnit[m].SetActive(false);
                                gm.fieldUnit[m] = null;
                            }
                               
                        }
                        for (int w = 0; w < ws.obj.Length; w++)
                        {
                            if (gm.ul.lv2Units[c][0] == ws.obj[w] || gm.ul.lv2Units[c][1] == ws.obj[w])
                            {
                                ws.obj[w].SetActive(false);
                                ws.obj[w] = null;
                            }
                        }
                        gm.ul.lv2Units[c][0] = null;
                        gm.ul.lv2Units[c][1] = null;
                        fi.lv++;
                        fi.LvUp();
                        gm.SynergyUpdate();
                        break;
                    }
                }
            }
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
