using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
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

    public Text aText;
    public Text sText;
    public Text sSText;

    public Text reRollText;
    int costToPay = 2;

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
        Debug.Log("구매 완료!");
        
        Fire fi = ch.gameObject.GetComponent<Fire>();
        SkillManager skill = ch.gameObject.GetComponent<SkillManager>();
        skill.poolManager = gm.skillPool;
        fi.gm = gm;
        fi.bulletPool = gm.bulletPool;
        fi.seaNum = seatPosNum;
        fi.StatUpdate();
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

        //진화 확인
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

        gm.exp += GameManager.augmentation[20] ? 7 : 4;

        ui.ExpUpdate();
        ui.CoinUpdate();
    }

    public void ReRollBtn()
    {
        int ran = Random.Range(0, 100);

        if (gm.coin < costToPay)
        {
            ui.StartCoroutine(ui.NoCoin());
            return;
        }

        gm.coin -= costToPay;
        ui.CoinUpdate();

        gm.Reroll();

        costToPay = ran < (AgManager.agReRollFree1 + AgManager.agReRollFree2 + AgManager.agReRollFree3) ? 0 : 2;
        reRollText.text = string.Format("새로고침 {0}", costToPay);
    }

    //시너지, 증강체 정보 확인 버튼
    public void SinfoBtn()//시너지 정보 확인
    {
        GetSnBtn gs = EventSystem.current.currentSelectedGameObject.GetComponent<GetSnBtn>();
        if (!sText.transform.parent.gameObject.activeSelf)
        {
            sText.transform.parent.gameObject.SetActive(true);
            sText.text = gs.info;
            gs.StepText();
            sSText.text = gs.stepinfo;
        }
        else
        {
            sText.transform.parent.gameObject.SetActive(false);
        }
    }
    public void AinfoBtn()//증강체 정보 확인
    {
        GetAgBtn ga = EventSystem.current.currentSelectedGameObject.GetComponent<GetAgBtn>();
        if (!aText.transform.parent.gameObject.activeSelf)
        {
            aText.transform.parent.gameObject.SetActive(true);
            aText.text = ga.info;
        }
        else
        {
            aText.transform.parent.gameObject.SetActive(false);
        }
    }

}
