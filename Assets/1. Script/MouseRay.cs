using System;
using System.Collections;
using System.Collections.Generic;
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
            Fire fi = hit.transform.gameObject.GetComponent<Fire>();
            if (fi.inField && gm.gamestat == 2)
                return;

            dragChamp = hit.transform.gameObject;
            beforeVec = dragChamp.transform.position;
            Debug.Log(dragChamp);
        }
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
        if (fi.inField) //�ʵ� ������ �̵�(��Ʋ �� X)
        {
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject);
                if (hit.transform.gameObject.layer == 10) //��ġ �Ұ�����
                {
                    Debug.Log("�ʵ� ���� ��ġ �Ұ�����");
                    dragChamp.transform.position = beforeVec;
                    ui.StartCoroutine(ui.NoSetPos());
                }
                else if (hit.transform.gameObject.layer == 11) //��⼮���� �̵�
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
                    if(s == -1)//���ڸ� ������ ���� ��ġ��
                    {
                        Debug.Log("�ʵ� ���� �� �ڸ��� ����!");
                        dragChamp.transform.position = beforeVec;
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
                    }
                }
                else
                {
                    Debug.Log("�ʵ� ���� �̵��Ϸ�");
                    dragChamp.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + setVec;
                }
            }
        }
        else //��⼮ ������ �̵�(��Ʋ �� X)
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
                dragChamp = null;
                return;
            }

            if (hit.collider != null)
            {
                if (hit.transform.gameObject.layer == 10) //��ġ �Ұ�����
                {
                    dragChamp.transform.position = beforeVec;
                    ui.StartCoroutine(ui.NoSetPos());
                }
                else
                {
                    dragChamp.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + setVec;
                    ws.obj[fi.seaNum] = null;
                    gm.fieldUnit[ck] = dragChamp;

                    //�ó��� �߰�����
                }
            }
        }
      
        dragChamp = null;
    }
}
