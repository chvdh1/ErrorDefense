using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRay : MonoBehaviour
{
    public GameObject dragChamp;
    public GameManager gm;
    public WaitingSeat ws;
    public UIManager ui;

    Vector2 defultVec = new Vector2(0, 0.5f);
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
        Debug.Log("down");
        if (dragChamp != null)
            return;

       Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int layerM = 1 << LayerMask.NameToLayer("Champ");
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero,10, layerM);
       
       if(hit.collider != null)
        {
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
        Debug.Log("up");
        if (dragChamp == null)
            return;

        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);



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
            dragChamp = null;
            return;
        }



        if (hit.collider != null)
        {
            if (hit.transform.gameObject.layer == 10) //서치 불가지역
            {
                dragChamp.transform.position = beforeVec;
                ui.StartCoroutine(ui.NoSetPos());
            }
            else
            {
                dragChamp.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + setVec;
                Fire fi = dragChamp.GetComponent<Fire>();
                ws.pos[fi.seaNum] = null;
                gm.fieldUnit[ck] = dragChamp;

                //시너지 추가내용
            }
        }

        

        dragChamp = null;
    }
}
