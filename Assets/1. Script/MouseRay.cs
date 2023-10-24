using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRay : MonoBehaviour
{
    GameObject dragChamp;
    GameManager gm;
    UIManager ui;

    Vector2 defultVec = new Vector2(0, 2);
    Vector2 beforeVec;


    private void Awake()
    {
        gm = GameManager.Instance;
        ui = UIManager.uIManager;
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

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit))
        {
            if (hit.transform.gameObject.layer != 9)
                return;

            dragChamp = hit.transform.gameObject;
            beforeVec = dragChamp.transform.position;
        }
    }

    void SetPos()
    {
        if (dragChamp == null)
            return;

        dragChamp.transform.position =new Vector2(Input.mousePosition.x, Input.mousePosition.y + defultVec.y);

      
    }

    void ResetObj()
    {
        if (dragChamp == null)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

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



        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.layer == 10 || ck !< gm.lv) //서치 불가지역
                dragChamp.transform.position = beforeVec;
            else
            {
                dragChamp.transform.position = Input.mousePosition;

                //시너지 추내가
            }
              
        }

        

        dragChamp = null;
    }
}
