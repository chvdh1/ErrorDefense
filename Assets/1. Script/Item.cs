using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public int itemNum;

    public Sprite[] im = new Sprite[35];
    Image image;

    Text text;

    Button btn;
    private void Awake()
    {
        text = transform.GetChild(0).GetComponent<Text>();
        btn = GetComponent<Button>();
        image = GetComponent<Image>();
        image.sprite = itemNum < 10 ? im[itemNum-1] : itemNum < 20 ? im[itemNum - 4] : itemNum < 30 ? im[itemNum - 8] : itemNum < 40 ? im[itemNum - 13] :
            itemNum < 50 ? im[itemNum - 19] : itemNum < 60 ? im[itemNum - 26] : itemNum < 70 ? im[itemNum - 34] : im[34] ;

        text.text = string.Format("{0}", itemNum);
        text.fontSize = 50;
        btn.onClick.AddListener(BtnManager.Btn.ItemsInfo);
    }
}
