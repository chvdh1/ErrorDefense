using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UIElements;

public class MouseRay : MonoBehaviour
{
    public GameObject dragChamp;
    public GameManager gm;
    public WaitingSeat ws;
    public UIManager ui;

    public Vector2 defultVec = new Vector2(0, 1);
    Vector3 setVec = new Vector3(0, 0,10);
    Vector2 beforeVec;

    public bool test;

    private void Awake()
    {
        if (test)
            return;
        gm = GetComponent<GameManager>();
        ui = gm.ui;
        ws = gm.ws;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            GetObj();

        if (Input.GetMouseButton(0))
            SetPos();

        if (Input.GetMouseButtonUp(0))
            ResetObj();
    }
    void GetObj()
    {
        if (dragChamp != null)
            return;

        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int layerM = 1 << LayerMask.NameToLayer("Champ");
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero,10, layerM);


        if (hit.collider != null)
        {
            if (gm.bt.equipItem && gm.bt.itemNum != 0)//아이템 장착
            {
                Champitems ci = hit.transform.gameObject.GetComponent<Champitems>();

                for (int i = 0; i < ci.sockets.Length; i++)
                {
                    if (ci.sockets[i].itemNum == 0 )
                    {
                        ci.sockets[i].itemNum = gm.bt.itemNum;
                        ci.ItemEffect();
                        gm.bt.equipItem = false;
                        gm.bt.thisItem.SetActive(false);
                        gm.bt.itemNum = 0;
                        gm.bt.thisItem = null;
                        break;
                    }
                    else if(ci.sockets[i].itemNum < 10)
                    {
                        ci.sockets[i].itemNum = MixItemNum(ci.sockets[i].itemNum, gm.bt.itemNum);
                        ci.ItemEffect();
                        gm.bt.equipItem = false;
                        gm.bt.thisItem.SetActive(false);
                        gm.bt.itemNum = 0;
                        gm.bt.thisItem = null;
                        break;
                    }
                    else if (i == ci.sockets.Length - 1 && ci.sockets[i].itemNum > 10)
                    {
                        ui.StartCoroutine(ui.NoItemS());
                        gm.bt.thisItem = null;
                        break;
                    }
                }
            }
            else
            {
                Fire fi = hit.transform.gameObject.GetComponent<Fire>();
                if (fi.inField && gm.gamestat == 2)
                    return;

                dragChamp = hit.transform.gameObject;
                beforeVec = dragChamp.transform.position;
                Debug.Log(dragChamp);
            }
        }
    }

    int MixItemNum(int chitmeNum, int btNum)
    {
        int num = 0;

        if(chitmeNum <= btNum)
            num = (chitmeNum*10) + btNum;
        else
            num = (btNum * 10) + chitmeNum;

        return num;
    }


    void SetPos()
    {
        if (dragChamp == null)
            return;
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragChamp.transform.position =new Vector2(pos.x, pos.y + defultVec.y);

      
    }

    void ResetObj()
    {
        if (dragChamp == null)
            return;
                
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

        
        Fire fi = dragChamp.GetComponent<Fire>();


        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject);
            if (hit.transform.gameObject.layer == 10) //설치 불가지역 (필드, 대기석 차이 없음)
            {
                Debug.Log("필드 유닛 설치 불가지역");
                dragChamp.transform.position = beforeVec;
                ui.StartCoroutine(ui.NoSetPos());
            }
            //--------------------설치 불가지역 (필드, 대기석 차이 없음)
            else if (hit.transform.gameObject.layer == 11) //대기석
            {
                if (fi.inField)
                {
                    int s = -1;
                    for (int i = 0; i < ws.obj.Length; i++)//대기석 빈자리 확인
                    {
                        if (ws.obj[i] == null)
                        {
                            s = i;
                            break;
                        }
                    }
                    if (s == -1)//빈자리 없으면 이전 위치로
                    {
                        Debug.Log("대기석 빈 자리가 없음!");
                        dragChamp.transform.position = beforeVec;

                    }

                    else//대기석으로 이동
                    {
                        Debug.Log("필드 유닛 대기석으로");
                        ws.obj[s] = dragChamp;
                        fi.seaNum = s;
                        dragChamp.transform.position = ws.pos[s].transform.position;
                        for (int i = 0; i < gm.fieldUnit.Length; i++)
                        {
                            if (dragChamp == gm.fieldUnit[i])
                            {
                                gm.fieldUnit[i] = null;
                                fi.inField = false;
                                break;
                            }
                        }
                        //시너지 삭제
                        gm.SynergyUpdate();
                    }
                }
                else//같은 대기석이면? 제자리로 ㅎ
                {
                    dragChamp.transform.position = beforeVec;
                }
            }
            //------------------------------------------------------대기석
            else if (hit.transform.gameObject.layer == 12) //판매
            {
                if(fi.inField)
                {
                    fi.seaNum = 0;
                    for (int i = 0; i < gm.fieldUnit.Length; i++)
                    {
                        if (dragChamp == gm.fieldUnit[i])
                        {
                            gm.fieldUnit[i] = null;

                            fi.inField = false;

                            if (fi.cost == 1)
                            {
                                gm.sm.cost1[fi.num - 1].cCount--;
                                Debug.Log(gm.sm.cost1[fi.num - 1].cName + "/" + gm.sm.cost1[fi.num - 1].cCount + "/" + gm.sm.cost1[fi.num - 1].cMax);
                            }
                            else if (fi.cost == 2)
                            {
                                gm.sm.cost2[fi.num - 1].cCount--;
                                Debug.Log(gm.sm.cost2[fi.num - 1].cName + "/" + gm.sm.cost2[fi.num - 1].cCount + "/" + gm.sm.cost2[fi.num - 1].cMax);
                            }
                            else if (fi.cost == 3)
                            {
                                gm.sm.cost3[fi.num - 1].cCount--;
                                Debug.Log(gm.sm.cost3[fi.num - 1].cName + "/" + gm.sm.cost3[fi.num - 1].cCount + "/" + gm.sm.cost3[fi.num - 1].cMax);
                            }
                            else if (fi.cost == 4)
                            {
                                gm.sm.cost4[fi.num - 1].cCount--;
                                Debug.Log(gm.sm.cost4[fi.num - 1].cName + "/" + gm.sm.cost4[fi.num - 1].cCount + "/" + gm.sm.cost4[fi.num - 1].cMax);
                            }
                            else if (fi.cost == 5)
                            {
                                gm.sm.cost5[fi.num - 1].cCount--;
                                Debug.Log(gm.sm.cost5[fi.num - 1].cName + "/" + gm.sm.cost5[fi.num - 1].cCount + "/" + gm.sm.cost5[fi.num - 1].cMax);
                            }

                            int coinget = fi.lv == 1 ? fi.cost : fi.lv == 2 ? fi.cost * 3 - 1 : fi.cost * 9 - 2;
                            gm.coin += coinget;
                            ui.CoinUpdate();

                            dragChamp.SetActive(false);
                            Debug.Log("필드 유닛 판매");
                            break;
                        }
                    }
                    //시너지 삭제
                    gm.SynergyUpdate();
                }
                else
                {
                    if (fi.cost == 1)
                    {
                        gm.sm.cost1[fi.num - 1].cCount--;
                        Debug.Log(gm.sm.cost1[fi.num - 1].cName + "/" + gm.sm.cost1[fi.num - 1].cCount + "/" + gm.sm.cost1[fi.num - 1].cMax);
                    }
                    else if (fi.cost == 2)
                    {
                        gm.sm.cost2[fi.num - 1].cCount--;
                        Debug.Log(gm.sm.cost2[fi.num - 1].cName + "/" + gm.sm.cost2[fi.num - 1].cCount + "/" + gm.sm.cost2[fi.num - 1].cMax);
                    }
                    else if (fi.cost == 3)
                    {
                        gm.sm.cost3[fi.num - 1].cCount--;
                        Debug.Log(gm.sm.cost3[fi.num - 1].cName + "/" + gm.sm.cost3[fi.num - 1].cCount + "/" + gm.sm.cost3[fi.num - 1].cMax);
                    }
                    else if (fi.cost == 4)
                    {
                        gm.sm.cost4[fi.num - 1].cCount--;
                        Debug.Log(gm.sm.cost4[fi.num - 1].cName + "/" + gm.sm.cost4[fi.num - 1].cCount + "/" + gm.sm.cost4[fi.num - 1].cMax);
                    }
                    else if (fi.cost == 5)
                    {
                        gm.sm.cost5[fi.num - 1].cCount--;
                        Debug.Log(gm.sm.cost5[fi.num - 1].cName + "/" + gm.sm.cost5[fi.num - 1].cCount + "/" + gm.sm.cost5[fi.num - 1].cMax);
                    }

                    int coinget = fi.lv == 0 ? fi.cost : fi.lv == 1 ? fi.cost * 3 - 1 : fi.cost * 9 - 2;
                    gm.coin += coinget;
                    ui.CoinUpdate();
                    ws.obj[fi.seaNum] = null;
                    fi.seaNum = 0;
                    dragChamp.SetActive(false);
                    Debug.Log("대기 유닛 판매");
                }
            }
            //------------------------------------------------------판매
        }

        else//rayhit이 없다면?
        {
            if (fi.inField)
            {
                Debug.Log("필드 유닛 이동완료");
                dragChamp.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + setVec;
            }
            else
            {
                //필드위에 유닛 수 확인
                int ck = -1;
                for (int i = 0; i < gm.lv; i++)
                {
                    if (gm.fieldUnit[i] == null)
                    {
                        ck = i;
                        break;
                    }
                }

                if (ck == -1)
                {
                    ui.StartCoroutine(ui.NoEmptySeats());
                    dragChamp.transform.position = beforeVec;
                }
                else
                {
                    dragChamp.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + setVec;
                    ws.obj[fi.seaNum] = null;
                    gm.fieldUnit[ck] = dragChamp;
                    fi.inField = true;
                    Debug.Log("대기석 -> 필드 완료!");
                    //시너지 추가내용
                    gm.SynergyUpdate();

                    //증강체 내용 추가
                    fi.StatUpdate();
                }

            }
        }


        dragChamp = null;
    }
}
