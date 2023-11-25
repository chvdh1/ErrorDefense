using System.Collections;
using System.Collections.Generic;
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

    public Text itemInfoText;

    public Text reRollText;
    int costToPay = 2;

    public PoolManager[] costObjs = new PoolManager[5];

    public GameObject[] infoOrItemUI;
    public GameObject mixItmeListG;
    GameObject[] mixItmeListbtn;

    public GameObject cardInfo;
    GameObject buyBt;
    Text infoText;
    CardInfoText card;
    ChampCard buycard;
    GameObject beforCardName;

    private void Awake()
    {
        Btn = this;
        gm = gameObject.GetComponent<GameManager>();
        ws = gm.ws;
        ui = manager.GetChild(0).gameObject.GetComponent<UIManager>();
        infoText = cardInfo.transform.GetChild(0).GetComponent<Text>();
        buyBt = cardInfo.transform.GetChild(1).gameObject;
        for (int i = 0; i < costObjs.Length; i++)
        {
            costObjs[i] = poolG.transform.GetChild(i+2).gameObject.GetComponent<PoolManager>();
        }
        int mixItmeListbtnL = mixItmeListG.transform.childCount;
        mixItmeListbtn = new GameObject[mixItmeListbtnL];
        for (int i = 0; i < mixItmeListbtn.Length; i++)
        {
            mixItmeListbtn[i] = mixItmeListG.transform.GetChild(i).gameObject;
            MixItemListUI ml = mixItmeListbtn[i].GetComponent<MixItemListUI>();
            ml.num = i < 7 ? i+11 : i < 13 ? i + 15 : i < 18 ? i + 20 : i < 22 ? i + 26 : i < 25 ? i + 33 : i < 27 ? i + 41 : 77;
            Text text = mixItmeListbtn[i].transform.GetChild(0).GetComponent<Text>();
            text.text = string.Format("{0}", ml.num);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        { gm.stageIndex = 4; }

        if (Input.GetKeyDown(KeyCode.Q))
        { Time.timeScale = 2; }
        if (Input.GetKeyDown(KeyCode.W))
        { Time.timeScale = 1; }
    }

    public void StageOneBtn()
    {
        GameObject startbt = EventSystem.current.currentSelectedGameObject;
        gm.mapIndex = 1;
        gm.GameStart();
        startbt.SetActive(false) ;
    }

  

    public  void ObjInfo()
    {
        if (beforCardName != null || beforCardName != EventSystem.current.currentSelectedGameObject)
        {
            cardInfo.SetActive(true);
            
            beforCardName = EventSystem.current.currentSelectedGameObject;
            card = beforCardName.GetComponent<CardInfoText>();
            buycard = beforCardName.GetComponent<ChampCard>();

            infoText.text = info(card.Unitname, card.cost, card.type, card.atttype, card.attinfo, card.skillinfo);
            buyBt.transform.position = card.transform.position;
        }
        else
        {
            cardInfo.SetActive(false);
            beforCardName = null;
            buycard = null;
        }
    }
    public void ObjInfofalse()
    {
        cardInfo.SetActive(false);
        beforCardName = null;
        buycard = null;
    }

    string info(string namem, int cost, int ty, int att, string attinfo, string skill)
    {
        string all = "";
        string na = namem;
        string coin = string.Format("비용 : {0}", cost);
        string type = ty == 1 ? "타입 : 평타딜러" :
            ty == 2 ? "타입 : 스킬딜러" :
             ty == 3 ? "타입 : 디버프" : "타입 : 버프";
        string at = att == 1 ? "근거리(1.5)" : "원거리(3)";
        string atinfo = attinfo;
        string skinfo = skill;

        all = string.Format("{0}\n{1}\n{2}\n{4}\n{5}", na, coin, type, at, atinfo, skinfo);

        return all;
    }
    Vector3 setVec = new Vector3(0, 0, 10);
    public void BuyChamp()//챔프 구매시
    {
        if(buycard != null)
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
                cardInfo.SetActive(false);
                beforCardName = null;
                buycard = null;
                return;
            }
            ChampCard cc = buycard;
            int ccost = cc.champCost;

            if (gm.coin < ccost)
            {
                StartCoroutine(ui.NoCoin());
                cardInfo.SetActive(false);
                beforCardName = null;
                buycard = null;
                return;
            }
            else
            {
                gm.coin -= ccost;
                ui.CoinUpdate();
            }

            Transform ch = costObjs[ccost - 1].Get(cc.Num - 1).transform;
            Debug.Log("구매 완료!");


            ChampMng cm = ch.gameObject.GetComponent<ChampMng>();
            cm.cFire.skillPool = gm.skillPool;
            cm.cFire.gm = gm;
            cm.cFire.bulletPool = gm.bulletPool;
            cm.cFire.seaNum = seatPosNum;
            cm.cFire.StatUpdate();
            ch.position = Camera.main.ScreenToWorldPoint(ws.pos[seatPosNum].transform.position)+ setVec;
            ws.obj[seatPosNum] = ch.gameObject;
            cc.gameObject.SetActive(false);

            //카드 수량 제한
            switch (ccost)
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
                    cm.cFire.lv++;
                    cm.cFire.LvUp();
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
                            cm.cFire.lv++;
                            cm.cFire.LvUp();
                            gm.SynergyUpdate();
                            break;
                        }
                    }
                }
            }
        }

        cardInfo.SetActive(false);
        beforCardName = null;
        buycard = null;

    }
    public RectTransform shop;
    public void ShopOpen()
    {
        float x = shop.anchoredPosition.x;
        if (x > 1000)
            shop.anchoredPosition = new Vector2(0, 0);
        else
            shop.anchoredPosition = new Vector2(1200, 0);
    }
    public RectTransform item;
    public void ItemOpen()
    {
        float x = item.anchoredPosition.x;
        if (x < -1000)
            item.anchoredPosition = new Vector2(0, 0);
        else
            item.anchoredPosition = new Vector2(-1200, 0);
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

    
    public void InfoOrItems()//정보창 및 아이템 확인 버튼
    {
        if (infoOrItemUI[0].activeSelf)
        {
            infoOrItemUI[1].SetActive(true);
            infoOrItemUI[0].SetActive(false);
        }
        else
        {
            infoOrItemUI[1].SetActive(false);
            infoOrItemUI[0].SetActive(true);
        }
    }


    public int itemNum;
    int beforeNum;
    public GameObject mixItmeListBtn;
    public GameObject thisItem;
    public void ItemsInfo()//아이템 정보창
    {
        
        GameObject text  = itemInfoText.transform.parent.gameObject;
        Item item = EventSystem.current.currentSelectedGameObject.GetComponent<Item>();
        thisItem = item.gameObject;
        itemNum = item.itemNum;
        if (itemNum == beforeNum)
        {
            text.SetActive(false);
            mixItmeListBtn.SetActive(false);
            mixItmeList.SetActive(false);
            if (box != null)
                box.SetActive(false);
            itemNum = 0;
            thisItem = null;
            beforeNum = -1;
        }
        else
        {
            beforeNum = itemNum;
            text.SetActive(true);
            switch (itemNum)
            {
                case 1:
                    itemInfoText.text = "시작 마나\r\n20증가";
                    break;
                case 2:
                    itemInfoText.text = "평타 데미지\r\n10%증가";
                    break;
                case 3:
                     itemInfoText.text = "스킬 데미지\r\n10%증가";
                    break;
                case 4:
                     itemInfoText.text = "사거리\r\n0.2증가";
                    break;
                case 5:
                     itemInfoText.text = "공격 속도\r\n20%증가";
                    break;
                case 6:
                     itemInfoText.text = "치명타 확률\r\n10%증가";
                    break;
                case 7:
                     itemInfoText.text = "치명타 데미지\r\n10% 증가";
                    break;

                case 11:
                     itemInfoText.text = "최대마나 20감소\r\n시작 마나\r\n40 증가";
                    break;
                case 12:
                     itemInfoText.text = "평타당 10 마나회복\r\n평타 데미지\r\n10%증가\r\n시작 마나\r\n20 증가";
                    break;
                case 13:
                     itemInfoText.text = "스킬 데미지 =\r\n스킬 사용 횟수 * 10%증가\r\n스킬 데미지\r\n10%증가\r\n시작 마나\r\n20 증가";
                    break;
                case 14:
                     itemInfoText.text = "3번째 평타시\r\n맞은 유닛 방어력 30%감소\r\n사거리 \r\n0.2증가\r\n시작 마나\r\n20 증가";
                    break;
                case 15:
                     itemInfoText.text = "15의 고정데미지\r\n공격 속도 \r\n20%증가\r\n시작 마나\r\n20 증가";
                    break;
                case 16:
                     itemInfoText.text = "평타, 스킬 데미지\r\n25%증가\r\n치명타 확률 10%증가\r\n시작 마나\r\n20 증가";
                    break;
                case 17:
                     itemInfoText.text = "치명타시\r\n마나 30회복\r\n치명타 데미지 10%\r\n증가\r\n시작 마나\r\n20 증가\r\n";
                    break;

                case 22:
                     itemInfoText.text = "평타 데미지\r\n50%증가";
                    break;
                case 23:
                     itemInfoText.text = "평타, 스킬 데미지\r\n35%스킬";
                    break;
                case 24:
                     itemInfoText.text = "사거리 0.5+\r\n평타 데미지\r\n25% 증가";
                    break;
                case 25:
                     itemInfoText.text = "평타 마다\r\n공격 속도 3% 증가\r\n공격 속도 \r\n20%증가";
                    break;
                case 26:
                     itemInfoText.text = "스킬에 치명타 발동\r\n치명타 확률 45%+\r\n평타 데미지 20%";
                    break;
                case 27:
                     itemInfoText.text = "치명타 데미지\r\n30% 증가\r\n평타 데미지\r\n20% 증가";
                    break;


                case 33:
                     itemInfoText.text = "스킬 데미지\r\n50%증가\r\n";
                    break;
                case 34:
                     itemInfoText.text = "사거리 \r\n0.5증가\r\n스킬 데미지\r\n25%증가";
                    break;
                case 35:
                     itemInfoText.text = "공격 속도 \r\n20% 증가\r\n스킬 사용시\r\n공격 속도\r\n10% 중첩 증가\r\n스킬 데미지\r\n10%증가";
                    break;
                case 36:
                     itemInfoText.text = "스킬에 치명타 발동\r\n치명타 확률\r\n45% 증가\r\n스킬 데미지\r\n20% 증가";
                    break;
                case 37:
                     itemInfoText.text = "치명타 데미지\r\n30% 증가\r\n스킬 데미지\r\n20% 증가";
                    break;


                case 44:
                     itemInfoText.text = "사거리\r\n2.4 증가\r\n평타 데미지\r\n10% 증가";
                    break;
                case 45:
                     itemInfoText.text = "사거리\r\n0.7 증가\r\n공격 속도\r\n40% 증가";
                    break;
                case 46:
                     itemInfoText.text = "사거리\r\n0.7 증가\r\n치명타 확률\r\n25% 증가";
                    break;
                case 47:
                     itemInfoText.text = "사거리\r\n1.2 증가\r\n치명타 데미지\r\n30% 증가";
                    break;


                case 55:
                     itemInfoText.text = "공격 속도\r\n80% 증가\r\n20% 확률로 2타";
                    break;
                case 56:
                     itemInfoText.text = "공격 속도\r\n40% 증가\r\n치명타 확률\r\n25% 증가";
                    break;
                case 57:
                     itemInfoText.text = "공격 속도\r\n20% 증가\r\n치명타 데미지\r\n30% 증가\r\n치명타시\r\n공격 속도\r\n5% 중첩 증가";
                    break;


                case 66:
                     itemInfoText.text = "치명타 확률\r\n70% 증가\r\n평타 데미지\r\n10% 증가";
                    break;
                case 67:
                     itemInfoText.text = "치명타 확률\r\n45% 증가\r\n치명타 데미지\r\n45% 증가";
                    break;

                case 77:
                     itemInfoText.text = "치명타 데미지\r\n100% 증가\r\n10% 확률로\r\n치명타 데미지 100%\r\n추가로 계산";
                    break;
            }

            if(itemNum < 10)
                mixItmeListBtn.SetActive(true);
            else
                mixItmeListBtn.SetActive(false);
        }
    }
    public GameObject mixItmeList;
    public void MixItmeList()//목록 아이템 보기
    {
        if(!mixItmeList.activeSelf)
            mixItmeList.SetActive(true);
        else
        {
            mixItmeList.SetActive(false);
            if(box != null)
                box.SetActive(false);
            return;
        }
          
        switch (itemNum)
        {
            case 1:
                for(int i = 0; i < mixItmeListbtn.Length;i++)
                {
                    if (i == 0 || i == 1 || i == 2 || i == 3 || i == 4 || i == 5 || i ==6)
                        mixItmeListbtn[i].SetActive(true);
                    else
                        mixItmeListbtn[i].SetActive(false);
                }
                break;
            case 2:
                for (int i = 0; i < mixItmeListbtn.Length; i++)
                {
                    if (i == 1 || i == 7 || i == 8 || i ==9 || i == 10 || i == 11 || i == 12)
                        mixItmeListbtn[i].SetActive(true);
                    else
                        mixItmeListbtn[i].SetActive(false);
                }
                break;
            case 3:
                for (int i = 0; i < mixItmeListbtn.Length; i++)
                {
                    if (i == 2 || i == 8 || i == 13 || i ==14 || i ==15 || i == 16 || i == 17)
                        mixItmeListbtn[i].SetActive(true);
                    else
                        mixItmeListbtn[i].SetActive(false);
                }
                break;
            case 4:
                for (int i = 0; i < mixItmeListbtn.Length; i++)
                {
                    if (i == 3 || i == 9 || i == 14 || i == 18 || i == 19 || i == 20 || i == 21)
                        mixItmeListbtn[i].SetActive(true);
                    else
                        mixItmeListbtn[i].SetActive(false);
                }
                break;
            case 5:
                for (int i = 0; i < mixItmeListbtn.Length; i++)
                {
                    if (i == 4 || i == 10|| i == 15 || i == 19 || i == 22 || i == 23 || i == 24)
                        mixItmeListbtn[i].SetActive(true);
                    else
                        mixItmeListbtn[i].SetActive(false);
                }
                break;
            case 6:
                for (int i = 0; i < mixItmeListbtn.Length; i++)
                {
                    if (i == 5 || i == 11|| i == 16 || i == 20 || i == 23 || i == 25 || i == 26)
                        mixItmeListbtn[i].SetActive(true);
                    else
                        mixItmeListbtn[i].SetActive(false);
                }
                break;
            case 7:
                for (int i = 0; i < mixItmeListbtn.Length; i++)
                {
                    if (i == 6 || i == 12|| i == 17 || i == 21 || i == 24 || i == 26 || i == 27)
                        mixItmeListbtn[i].SetActive(true);
                    else
                        mixItmeListbtn[i].SetActive(false);
                }
                break;
        }
    }
    public Text mixItemText;
    int mixNum = 0;
    GameObject box;
    public void MixItmeListInfo()//조합 아이템 정보창
    {
        box = mixItemText.gameObject.transform.parent.gameObject;
        MixItemListUI item = EventSystem.current.currentSelectedGameObject.GetComponent<MixItemListUI>();
        if (mixNum == item.num)
        {
            mixNum = 0;
            box.SetActive(false);
        }
        else
        {
            mixNum = item.num;
            box.SetActive(true);
            switch (item.num)
            {
                case 11:
                    mixItemText.text = "최대마나 20감소\r\n시작 마나\r\n40 증가";
                    break;
                case 12:
                    mixItemText.text = "평타당 10 마나회복\r\n평타 데미지\r\n10%증가\r\n시작 마나\r\n20 증가";
                    break;
                case 13:
                    mixItemText.text = "스킬 데미지 =\r\n스킬 사용 횟수 * 10%증가\r\n스킬 데미지\r\n10%증가\r\n시작 마나\r\n20 증가";
                    break;
                case 14:
                    mixItemText.text = "3번째 평타시\r\n맞은 유닛 방어력 30%감소\r\n사거리 \r\n0.2증가\r\n시작 마나\r\n20 증가";
                    break;
                case 15:
                    mixItemText.text = "15의 고정데미지\r\n공격 속도 \r\n20%증가\r\n시작 마나\r\n20 증가";
                    break;
                case 16:
                    mixItemText.text = "평타, 스킬 데미지\r\n25%증가\r\n치명타 확률 10%증가\r\n시작 마나\r\n20 증가";
                    break;
                case 17:
                    mixItemText.text = "치명타시\r\n마나 30회복\r\n치명타 데미지 10%\r\n증가\r\n시작 마나\r\n20 증가\r\n";
                    break;

                case 22:
                    mixItemText.text = "평타 데미지\r\n50%증가";
                    break;
                case 23:
                    mixItemText.text = "평타, 스킬 데미지\r\n35%스킬";
                    break;
                case 24:
                    mixItemText.text = "사거리 0.5+\r\n평타 데미지\r\n25% 증가";
                    break;
                case 25:
                    mixItemText.text = "평타 마다\r\n공격 속도 3% 증가\r\n공격 속도 \r\n20%증가";
                    break;
                case 26:
                    mixItemText.text = "스킬에 치명타 발동\r\n치명타 확률 45%+\r\n평타 데미지 20%";
                    break;
                case 27:
                    mixItemText.text = "치명타 데미지\r\n30% 증가\r\n평타 데미지\r\n20% 증가";
                    break;


                case 33:
                    mixItemText.text = "스킬 데미지\r\n50%증가\r\n";
                    break;
                case 34:
                    mixItemText.text = "사거리 \r\n0.5증가\r\n스킬 데미지\r\n25%증가";
                    break;
                case 35:
                    mixItemText.text = "공격 속도 \r\n20% 증가\r\n스킬 사용시\r\n공격 속도\r\n10% 중첩 증가\r\n스킬 데미지\r\n10%증가";
                    break;
                case 36:
                    mixItemText.text = "스킬에 치명타 발동\r\n치명타 확률\r\n45% 증가\r\n스킬 데미지\r\n20% 증가";
                    break;
                case 37:
                    mixItemText.text = "치명타 데미지\r\n30% 증가\r\n스킬 데미지\r\n20% 증가";
                    break;


                case 44:
                    mixItemText.text = "사거리\r\n2.4 증가\r\n평타 데미지\r\n10% 증가";
                    break;
                case 45:
                    mixItemText.text = "사거리\r\n0.7 증가\r\n공격 속도\r\n40% 증가";
                    break;
                case 46:
                    mixItemText.text = "사거리\r\n0.7 증가\r\n치명타 확률\r\n25% 증가";
                    break;
                case 47:
                    mixItemText.text = "사거리\r\n1.2 증가\r\n치명타 데미지\r\n30% 증가";
                    break;


                case 55:
                    mixItemText.text = "공격 속도\r\n80% 증가\r\n20% 확률로 2타";
                    break;
                case 56:
                    mixItemText.text = "공격 속도\r\n40% 증가\r\n치명타 확률\r\n25% 증가";
                    break;
                case 57:
                    mixItemText.text = "공격 속도\r\n20% 증가\r\n치명타 데미지\r\n30% 증가\r\n치명타시\r\n공격 속도\r\n5% 중첩 증가";
                    break;


                case 66:
                    mixItemText.text = "치명타 확률\r\n70% 증가\r\n평타 데미지\r\n10% 증가";
                    break;
                case 67:
                    mixItemText.text = "치명타 확률\r\n45% 증가\r\n치명타 데미지\r\n45% 증가";
                    break;

                case 77:
                    mixItemText.text = "치명타 데미지\r\n100% 증가\r\n10% 확률로\r\n치명타 데미지 100%\r\n추가로 계산";
                    break;
            }
        }
    }

    public bool equipItem;
    public GameObject[] itemList = new GameObject[10];
    public void EquipItem()
    {
        GameObject text = itemInfoText.transform.parent.gameObject;
        if (!equipItem)
        {
            equipItem = true;
            gm.ui.StartCoroutine(gm.ui.EquipItem());
            text.SetActive(false);
            mixItmeListBtn.SetActive(false);
            mixItmeList.SetActive(false);
            if (box != null)
                box.SetActive(false);
        }
           
        else
        {
            equipItem = false;
            text.SetActive(false);
            mixItmeListBtn.SetActive(false);
            mixItmeList.SetActive(false);
            thisItem = null;
            if (box != null)
                box.SetActive(false);
            itemNum = 0;
        }
    }

    public GameObject pause;
    public GameObject settins;
    public GameObject gameUIG;

    public void PauseBtnOnOff()
    {
        if(pause.activeSelf)
        {
            settins.SetActive(false);
            gameUIG.SetActive(true);
            Time.timeScale = 1;
        }
        else
        {
            pause.SetActive(true);
            gameUIG.SetActive(false);
            Time.timeScale = 0;
        }    
    }

    public void Settins() 
    {
        if (settins.activeSelf)
        {
            settins.SetActive(false);
        }
        else
        {
            settins.SetActive(true);
        }

    }

    public void Replay()
    {

    }
    public void Home()
    {

    }
}
