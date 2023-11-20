using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class MouseRay : MonoBehaviour
{
    public GameObject dragChamp;
    public GameManager gm;
    public WaitingSeat ws;
    public UIManager ui;

    public SpriteRenderer champRange;

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
            if (gm.bt.equipItem && gm.bt.itemNum != 0)//������ ����
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
                        StartCoroutine(champRangepos(ci.gameObject.GetComponent<Targeting>()));
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
                        StartCoroutine(champRangepos(ci.gameObject.GetComponent<Targeting>()));
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

                //��Ÿ� ǥ��
                Targeting tt = dragChamp.GetComponent<Targeting>();
                champRange.size = new Vector2(tt.scanRange * 2, tt.scanRange * 2);
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
        if(champRange.color.a == 0)
            champRange.color = new Color(0, 0.6f, 1, 0.3f);

        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragChamp.transform.position =new Vector2(pos.x, pos.y + defultVec.y);
        champRange.transform.position = pos;
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
            if (hit.transform.gameObject.layer == 10) //��ġ �Ұ����� (�ʵ�, ��⼮ ���� ����)
            {
                Debug.Log("�ʵ� ���� ��ġ �Ұ�����");
                dragChamp.transform.position = beforeVec;
                champRange.color = new Color(0, 0.6f, 1, 0);
                ui.StartCoroutine(ui.NoSetPos());
            }
            //--------------------��ġ �Ұ����� (�ʵ�, ��⼮ ���� ����)
            else if (hit.transform.gameObject.layer == 11) //��⼮
            {
                if (fi.inField)
                {
                    int s = -1;
                    for (int i = 0; i < ws.obj.Length; i++)//��⼮ ���ڸ� Ȯ��
                    {
                        if (ws.obj[i] == null)
                        {
                            s = i;
                            break;
                        }
                    }
                    if (s == -1)//���ڸ� ������ ���� ��ġ��
                    {
                        Debug.Log("��⼮ �� �ڸ��� ����!");
                        dragChamp.transform.position = beforeVec;
                        StartCoroutine(champRangepos(dragChamp.GetComponent<Targeting>()));
                    }

                    else//��⼮���� �̵�
                    {
                        Debug.Log("�ʵ� ���� ��⼮����");
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
                        //�ó��� ����
                        gm.SynergyUpdate();
                        champRange.color = new Color(0, 0.6f, 1, 0);
                    }
                }
                else//���� ��⼮�̸�? ���ڸ��� ��
                {
                    dragChamp.transform.position = beforeVec;
                    champRange.color = new Color(0, 0.6f, 1, 0);
                }
            }
            //------------------------------------------------------��⼮
            else if (hit.transform.gameObject.layer == 12) //�Ǹ�
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
                            Debug.Log("�ʵ� ���� �Ǹ�");
                            champRange.color = new Color(0, 0.6f, 1, 0);
                            break;
                        }
                    }
                    //�ó��� ����
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
                    Debug.Log("��� ���� �Ǹ�");
                    champRange.color = new Color(0, 0.6f, 1, 0);
                }
            }
            //------------------------------------------------------�Ǹ�
        }

        else//rayhit�� ���ٸ�?
        {
            if (fi.inField)
            {
                Debug.Log("�ʵ� ���� �̵��Ϸ�");
                dragChamp.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + setVec;
            }
            else
            {
                //�ʵ����� ���� �� Ȯ��
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
                    Debug.Log("��⼮ -> �ʵ� �Ϸ�!");
                    //�ó��� �߰�����
                    gm.SynergyUpdate();

                    //����ü ���� �߰�
                    fi.StatUpdate();
                }

            }

            StartCoroutine(champRangepos(dragChamp.GetComponent<Targeting>()));
        }


        dragChamp = null;
    }

    IEnumerator champRangepos(Targeting tt)
    {
        champRange.transform.position = tt.transform.position;
        champRange.color = new Color(0,0.6f,1,0.3f);
        champRange.size = new Vector2(tt.scanRange * 2, tt.scanRange * 2);
        float a = 0.3f;
        while(a > 0)
        {
            a -= 0.3f / 60f;
            champRange.color = new Color(0, 0.6f, 1, a);
            yield return new WaitForSeconds(1/60);
        }
        champRange.color = new Color(0, 0.6f, 1, 0);
    }
}
