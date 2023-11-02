using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChampCard : MonoBehaviour
{

    public int champCost;
    public int champLv;
    public int Num;

    Button btn;
    BtnManager bm;
    Color[] tierColor = new Color[5];
    Color[] starColor = new Color[3];


    //기본정보
    Image costLine;
    
    private void Awake()
    {
        btn = GetComponent<Button>();
        tierColor[0] = new Color(0.8f, 0.75f, 0.9f, 1);
        tierColor[1] = new Color(0.39f,0.82f,0.49f,1);
        tierColor[2] = new Color(0.3f, 0.55f, 1, 1);
        tierColor[3] = new Color(0.8f, 0.3f, 1, 1);
        tierColor[4] = new Color(1, 0.85f, 0, 1);

        starColor[0] = new Color(1, 1, 1, 0);
        starColor[1] = new Color(0.8f, 0.8f, 0.8f, 1);
        starColor[2] = new Color(0.95f, 1, 0.25f, 1);

        costLine = transform.GetChild(0).gameObject.GetComponent<Image>();
        costLine.color = tierColor[champCost - 1];
        bm = BtnManager.Btn;
        btn.onClick.AddListener(bm.BuyChamp);
    }

}
