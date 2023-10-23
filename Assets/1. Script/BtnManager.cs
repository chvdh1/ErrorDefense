using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnManager : MonoBehaviour
{
    GameManager gm;
    ShopManager sm;


    private void Awake()
    {
        gm = gameObject.GetComponent<GameManager>();
        sm = ShopManager.shopManager;
    }

    public void StageOneBtn()
    {
        gm.mapIndex = 1;
        gm.GameStart();
    }

    public void BuyChamp()
    {
        ChampCard cc = EventSystem.current.currentSelectedGameObject.GetComponent<ChampCard>();
        int ccost = cc.champCost;

        cc.gameObject.SetActive(false);
    }

}
