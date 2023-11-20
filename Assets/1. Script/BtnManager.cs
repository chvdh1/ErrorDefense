using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.Burst.CompilerServices;

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
        gm.mapIndex = 1;
        gm.GameStart();       
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
        string coin = string.Format("��� : {0}", cost);
        string type = ty == 1 ? "Ÿ�� : ��Ÿ����" :
            ty == 2 ? "Ÿ�� : ��ų����" :
             ty == 3 ? "Ÿ�� : �����" : "Ÿ�� : ����";
        string at = att == 1 ? "�ٰŸ�(1.5)" : "���Ÿ�(3)";
        string atinfo = attinfo;
        string skinfo = skill;

        all = string.Format("{0}\n{1}\n{2}\n{4}\n{5}", na, coin, type, at, atinfo, skinfo);

        return all;
    }
    public void BuyChamp()//è�� ���Ž�
    {
        if(buycard != null)
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
            Debug.Log("���� �Ϸ�!");


            ChampMng cm = ch.gameObject.GetComponent<ChampMng>();
            cm.cFire.skillPool = gm.skillPool;
            cm.cFire.gm = gm;
            cm.cFire.bulletPool = gm.bulletPool;
            cm.cFire.seaNum = seatPosNum;
            cm.cFire.StatUpdate();
            ch.position = ws.pos[seatPosNum].transform.position;
            ws.obj[seatPosNum] = ch.gameObject;
            cc.gameObject.SetActive(false);

            //ī�� ���� ����
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
        reRollText.text = string.Format("���ΰ�ħ {0}", costToPay);
    }

    //�ó���, ����ü ���� Ȯ�� ��ư
    public void SinfoBtn()//�ó��� ���� Ȯ��
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
    public void AinfoBtn()//����ü ���� Ȯ��
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

    
    public void InfoOrItems()//����â �� ������ Ȯ�� ��ư
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
    public void ItemsInfo()//������ ����â
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
                    itemInfoText.text = "���� ����\r\n20����";
                    break;
                case 2:
                    itemInfoText.text = "��Ÿ ������\r\n10%����";
                    break;
                case 3:
                     itemInfoText.text = "��ų ������\r\n10%����";
                    break;
                case 4:
                     itemInfoText.text = "��Ÿ�\r\n0.2����";
                    break;
                case 5:
                     itemInfoText.text = "���� �ӵ�\r\n20%����";
                    break;
                case 6:
                     itemInfoText.text = "ġ��Ÿ Ȯ��\r\n10%����";
                    break;
                case 7:
                     itemInfoText.text = "ġ��Ÿ ������\r\n10% ����";
                    break;

                case 11:
                     itemInfoText.text = "�ִ븶�� 20����\r\n���� ����\r\n40 ����";
                    break;
                case 12:
                     itemInfoText.text = "��Ÿ�� 10 ����ȸ��\r\n��Ÿ ������\r\n10%����\r\n���� ����\r\n20 ����";
                    break;
                case 13:
                     itemInfoText.text = "��ų ������ =\r\n��ų ��� Ƚ�� * 10%����\r\n��ų ������\r\n10%����\r\n���� ����\r\n20 ����";
                    break;
                case 14:
                     itemInfoText.text = "3��° ��Ÿ��\r\n���� ���� ���� 30%����\r\n��Ÿ� \r\n0.2����\r\n���� ����\r\n20 ����";
                    break;
                case 15:
                     itemInfoText.text = "15�� ����������\r\n���� �ӵ� \r\n20%����\r\n���� ����\r\n20 ����";
                    break;
                case 16:
                     itemInfoText.text = "��Ÿ, ��ų ������\r\n25%����\r\nġ��Ÿ Ȯ�� 10%����\r\n���� ����\r\n20 ����";
                    break;
                case 17:
                     itemInfoText.text = "ġ��Ÿ��\r\n���� 30ȸ��\r\nġ��Ÿ ������ 10%\r\n����\r\n���� ����\r\n20 ����\r\n";
                    break;

                case 22:
                     itemInfoText.text = "��Ÿ ������\r\n50%����";
                    break;
                case 23:
                     itemInfoText.text = "��Ÿ, ��ų ������\r\n35%��ų";
                    break;
                case 24:
                     itemInfoText.text = "��Ÿ� 0.5+\r\n��Ÿ ������\r\n25% ����";
                    break;
                case 25:
                     itemInfoText.text = "��Ÿ ����\r\n���� �ӵ� 3% ����\r\n���� �ӵ� \r\n20%����";
                    break;
                case 26:
                     itemInfoText.text = "��ų�� ġ��Ÿ �ߵ�\r\nġ��Ÿ Ȯ�� 45%+\r\n��Ÿ ������ 20%";
                    break;
                case 27:
                     itemInfoText.text = "ġ��Ÿ ������\r\n30% ����\r\n��Ÿ ������\r\n20% ����";
                    break;


                case 33:
                     itemInfoText.text = "��ų ������\r\n50%����\r\n";
                    break;
                case 34:
                     itemInfoText.text = "��Ÿ� \r\n0.5����\r\n��ų ������\r\n25%����";
                    break;
                case 35:
                     itemInfoText.text = "���� �ӵ� \r\n20% ����\r\n��ų ����\r\n���� �ӵ�\r\n10% ��ø ����\r\n��ų ������\r\n10%����";
                    break;
                case 36:
                     itemInfoText.text = "��ų�� ġ��Ÿ �ߵ�\r\nġ��Ÿ Ȯ��\r\n45% ����\r\n��ų ������\r\n20% ����";
                    break;
                case 37:
                     itemInfoText.text = "ġ��Ÿ ������\r\n30% ����\r\n��ų ������\r\n20% ����";
                    break;


                case 44:
                     itemInfoText.text = "��Ÿ�\r\n2.4 ����\r\n��Ÿ ������\r\n10% ����";
                    break;
                case 45:
                     itemInfoText.text = "��Ÿ�\r\n0.7 ����\r\n���� �ӵ�\r\n40% ����";
                    break;
                case 46:
                     itemInfoText.text = "��Ÿ�\r\n0.7 ����\r\nġ��Ÿ Ȯ��\r\n25% ����";
                    break;
                case 47:
                     itemInfoText.text = "��Ÿ�\r\n1.2 ����\r\nġ��Ÿ ������\r\n30% ����";
                    break;


                case 55:
                     itemInfoText.text = "���� �ӵ�\r\n80% ����\r\n20% Ȯ���� 2Ÿ";
                    break;
                case 56:
                     itemInfoText.text = "���� �ӵ�\r\n40% ����\r\nġ��Ÿ Ȯ��\r\n25% ����";
                    break;
                case 57:
                     itemInfoText.text = "���� �ӵ�\r\n20% ����\r\nġ��Ÿ ������\r\n30% ����\r\nġ��Ÿ��\r\n���� �ӵ�\r\n5% ��ø ����";
                    break;


                case 66:
                     itemInfoText.text = "ġ��Ÿ Ȯ��\r\n70% ����\r\n��Ÿ ������\r\n10% ����";
                    break;
                case 67:
                     itemInfoText.text = "ġ��Ÿ Ȯ��\r\n45% ����\r\nġ��Ÿ ������\r\n45% ����";
                    break;

                case 77:
                     itemInfoText.text = "ġ��Ÿ ������\r\n100% ����\r\n10% Ȯ����\r\nġ��Ÿ ������ 100%\r\n�߰��� ���";
                    break;
            }

            if(itemNum < 10)
                mixItmeListBtn.SetActive(true);
            else
                mixItmeListBtn.SetActive(false);
        }
    }
    public GameObject mixItmeList;
    public void MixItmeList()//��� ������ ����
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
    public void MixItmeListInfo()//���� ������ ����â
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
                    mixItemText.text = "�ִ븶�� 20����\r\n���� ����\r\n40 ����";
                    break;
                case 12:
                    mixItemText.text = "��Ÿ�� 10 ����ȸ��\r\n��Ÿ ������\r\n10%����\r\n���� ����\r\n20 ����";
                    break;
                case 13:
                    mixItemText.text = "��ų ������ =\r\n��ų ��� Ƚ�� * 10%����\r\n��ų ������\r\n10%����\r\n���� ����\r\n20 ����";
                    break;
                case 14:
                    mixItemText.text = "3��° ��Ÿ��\r\n���� ���� ���� 30%����\r\n��Ÿ� \r\n0.2����\r\n���� ����\r\n20 ����";
                    break;
                case 15:
                    mixItemText.text = "15�� ����������\r\n���� �ӵ� \r\n20%����\r\n���� ����\r\n20 ����";
                    break;
                case 16:
                    mixItemText.text = "��Ÿ, ��ų ������\r\n25%����\r\nġ��Ÿ Ȯ�� 10%����\r\n���� ����\r\n20 ����";
                    break;
                case 17:
                    mixItemText.text = "ġ��Ÿ��\r\n���� 30ȸ��\r\nġ��Ÿ ������ 10%\r\n����\r\n���� ����\r\n20 ����\r\n";
                    break;

                case 22:
                    mixItemText.text = "��Ÿ ������\r\n50%����";
                    break;
                case 23:
                    mixItemText.text = "��Ÿ, ��ų ������\r\n35%��ų";
                    break;
                case 24:
                    mixItemText.text = "��Ÿ� 0.5+\r\n��Ÿ ������\r\n25% ����";
                    break;
                case 25:
                    mixItemText.text = "��Ÿ ����\r\n���� �ӵ� 3% ����\r\n���� �ӵ� \r\n20%����";
                    break;
                case 26:
                    mixItemText.text = "��ų�� ġ��Ÿ �ߵ�\r\nġ��Ÿ Ȯ�� 45%+\r\n��Ÿ ������ 20%";
                    break;
                case 27:
                    mixItemText.text = "ġ��Ÿ ������\r\n30% ����\r\n��Ÿ ������\r\n20% ����";
                    break;


                case 33:
                    mixItemText.text = "��ų ������\r\n50%����\r\n";
                    break;
                case 34:
                    mixItemText.text = "��Ÿ� \r\n0.5����\r\n��ų ������\r\n25%����";
                    break;
                case 35:
                    mixItemText.text = "���� �ӵ� \r\n20% ����\r\n��ų ����\r\n���� �ӵ�\r\n10% ��ø ����\r\n��ų ������\r\n10%����";
                    break;
                case 36:
                    mixItemText.text = "��ų�� ġ��Ÿ �ߵ�\r\nġ��Ÿ Ȯ��\r\n45% ����\r\n��ų ������\r\n20% ����";
                    break;
                case 37:
                    mixItemText.text = "ġ��Ÿ ������\r\n30% ����\r\n��ų ������\r\n20% ����";
                    break;


                case 44:
                    mixItemText.text = "��Ÿ�\r\n2.4 ����\r\n��Ÿ ������\r\n10% ����";
                    break;
                case 45:
                    mixItemText.text = "��Ÿ�\r\n0.7 ����\r\n���� �ӵ�\r\n40% ����";
                    break;
                case 46:
                    mixItemText.text = "��Ÿ�\r\n0.7 ����\r\nġ��Ÿ Ȯ��\r\n25% ����";
                    break;
                case 47:
                    mixItemText.text = "��Ÿ�\r\n1.2 ����\r\nġ��Ÿ ������\r\n30% ����";
                    break;


                case 55:
                    mixItemText.text = "���� �ӵ�\r\n80% ����\r\n20% Ȯ���� 2Ÿ";
                    break;
                case 56:
                    mixItemText.text = "���� �ӵ�\r\n40% ����\r\nġ��Ÿ Ȯ��\r\n25% ����";
                    break;
                case 57:
                    mixItemText.text = "���� �ӵ�\r\n20% ����\r\nġ��Ÿ ������\r\n30% ����\r\nġ��Ÿ��\r\n���� �ӵ�\r\n5% ��ø ����";
                    break;


                case 66:
                    mixItemText.text = "ġ��Ÿ Ȯ��\r\n70% ����\r\n��Ÿ ������\r\n10% ����";
                    break;
                case 67:
                    mixItemText.text = "ġ��Ÿ Ȯ��\r\n45% ����\r\nġ��Ÿ ������\r\n45% ����";
                    break;

                case 77:
                    mixItemText.text = "ġ��Ÿ ������\r\n100% ����\r\n10% Ȯ����\r\nġ��Ÿ ������ 100%\r\n�߰��� ���";
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
}
