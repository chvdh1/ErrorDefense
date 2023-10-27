using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class ChampShop
{

    public string cName = "";
    public int cCost = 0;
    public int cCount = 0;
    public int cMax = 0;
    public int cNum = 0;

   

    public ChampShop(string Name, int count, int max , int num, int cost)
    {
        cName = Name;
        cCount = count;
        cMax = max;
        cNum = num;
        cCost = cost;
    }
    public void PrintInfo()
    {
        string str = string.Format("이름({0}) 수량({1}) 최대수량({2}) 넘버({3}) 코스트({4})",
                                    cName, cCount, cMax, cNum, cCost);
        Debug.Log(str);
    }
}


public class ShopManager : MonoBehaviour
{
    public static ShopManager shopManager;

    public List<ChampShop> cost1 = new List<ChampShop>();
    List<ChampShop> cost2 = new List<ChampShop>();
    List<ChampShop> cost3 = new List<ChampShop>();
    List<ChampShop> cost4 = new List<ChampShop>();
    List<ChampShop> cost5 = new List<ChampShop>();


    // ------------ 카드 소환

    public GameObject pools;
    public PoolManager[] costPool = new PoolManager[5];

    public GameObject[] cells = new GameObject[5];
    private void Awake()
    {
        shopManager = this;
        for (int i = 0; i < costPool.Length; i++)
        {
            costPool[i] = pools.transform.GetChild(i + 7).gameObject.GetComponent<PoolManager>();
        }
    }

    public void ChampReset() //코스트 초기화
    {
        cost1.Clear();
        cost2.Clear();
        cost3.Clear();
        cost4.Clear();
        cost5.Clear();


        // 1코스트 초기화
        ChampShop node = new ChampShop("11", 0, 20,0,1);
        cost1.Add(node);
        node = new ChampShop("12", 0,  20,1, 1);
        cost1.Add(node);
        node = new ChampShop("13", 0, 20, 2, 1);
        cost1.Add(node);
        node = new ChampShop("14", 0,  20, 3, 1);
        cost1.Add(node);
        node = new ChampShop("15", 0,  20,4, 1);
        cost1.Add(node);
        node = new ChampShop("16", 0,  20,5, 1);
        cost1.Add(node);
        node = new ChampShop("17", 0,  20,6, 1);
        cost1.Add(node);
        node = new ChampShop("18", 0,  20,7, 1);
        cost1.Add(node);

        // 2코스트 초기화
        ChampShop node2 = new ChampShop("21", 0,  20, 0, 2);
        cost2.Add(node2);
        node2 = new ChampShop("22", 0,  20,1, 2);
        cost2.Add(node2);
        node2 = new ChampShop("23", 0, 20,2, 2);
        cost2.Add(node2);
        node2 = new ChampShop("24", 0,  20,3, 2);
        cost2.Add(node2);
        node2 = new ChampShop("25", 0,  20,4, 2);
        cost2.Add(node2);
        node2 = new ChampShop("26", 0,  20,5, 2);
        cost2.Add(node2);
        node2 = new ChampShop("27", 0,  20,6, 2);
        cost2.Add(node2);
        node2 = new ChampShop("28", 0,  20,7, 2);
        cost2.Add(node2);

        // 3코스트 초기화
        ChampShop node3 = new ChampShop("31", 0, 18, 0, 3);
        cost3.Add(node3);
        node3 = new ChampShop("32", 0, 18,1, 3);
        cost3.Add(node3);
        node3 = new ChampShop("33", 0, 18,2, 3);
        cost3.Add(node3);
        node3 = new ChampShop("34", 0, 18,3, 3);
        cost3.Add(node3);
        node3 = new ChampShop("35", 0, 18,4, 3);
        cost3.Add(node3);
        node3 = new ChampShop("36", 0, 18,5, 3);
        cost3.Add(node3);
        node3 = new ChampShop("37", 0, 18,6, 3);
        cost3.Add(node3);
        node3 = new ChampShop("38", 0, 18,7, 3);
        cost3.Add(node3);

        // 4코스트 초기화
        ChampShop node4 = new ChampShop("41", 0, 12,0, 4);
        cost4.Add(node4);
        node4 = new ChampShop("42", 0, 12,1, 4);
        cost4.Add(node4);
        node4 = new ChampShop("44", 0, 12,2, 4);
        cost4.Add(node4);
        node4 = new ChampShop("44", 0, 12,3, 4);
        cost4.Add(node4);
        node4 = new ChampShop("45", 0, 12,4, 4);
        cost4.Add(node4);
        node4 = new ChampShop("46", 0, 12,5, 4);
        cost4.Add(node4);
        node4 = new ChampShop("47", 0, 12,6, 4);
        cost4.Add(node4);
        node4 = new ChampShop("48", 0, 12,7, 4);
        cost4.Add(node4);

        // 5코스트 초기화
        ChampShop node5 = new ChampShop("51", 0, 9,0, 5);
        cost5.Add(node5);
        node5 = new ChampShop("52", 0, 9,1, 5);
        cost5.Add(node5);
        node5 = new ChampShop("55", 0, 9,2, 5);
        cost5.Add(node5);
        node5 = new ChampShop("55", 0, 9,3, 5);
        cost5.Add(node5);
        node5 = new ChampShop("55", 0, 9,4, 5);
        cost5.Add(node5);
        node5 = new ChampShop("56", 0, 9,5, 5);
        cost5.Add(node5);
        node5 = new ChampShop("57", 0, 9,6, 5);
        cost5.Add(node5);
        node5 = new ChampShop("58", 0, 9,7, 5);
        cost5.Add(node5);
    }
    public void DelCards()
    {
        for (int c = 0; c < cells.Length; c++)
        {
            if (cells[c] != null)
            {
                cells[c].SetActive(false);
                cells[c] = null;
            }
        }
    }


    public void Champ1Produce()
    {
        int i = Random.Range(0, cost1.Count);
        int num = cost1[i].cNum;
       
        Transform ch1 = costPool[0].Get(num).transform;
        ch1.SetParent(transform);

        for (int c = 0; c < cells.Length; c++)
        {
            if (cells[c] == null)
            {
                cells[c] = ch1.gameObject;
                break;
            }
        }
        

        cost1[i].cCount++;
        if (cost1[i].cCount >= cost1[i].cMax)
            cost1.RemoveAt(i);
    }

    public void Champ2Produce()
    {
        int i = Random.Range(0, cost2.Count);
        int num = cost2[i].cNum;
        Transform ch2 = costPool[1].Get(num).transform;
        ch2.SetParent(transform);
        for (int c = 0; c < cells.Length; c++)
        {
            if (cells[c] == null)
            {
                cells[c] = ch2.gameObject;
                break;
            }
        }
        cost2[i].cCount++;
        if (cost2[i].cCount >= cost2[i].cMax)
            cost2.RemoveAt(i);
    }
    public void Champ3Produce()
    {
        int i = Random.Range(0, cost3.Count);
        int num = cost3[i].cNum;
        Transform ch3 = costPool[2].Get(num).transform;
        ch3.SetParent(transform);
        for (int c = 0; c < cells.Length; c++)
        {
            if (cells[c] == null)
            {
                cells[c] = ch3.gameObject;
                break;
            }
        }
        cost3[i].cCount++;
        if (cost3[i].cCount >= cost3[i].cMax)
            cost3.RemoveAt(i);
    }
    public void Champ4Produce()
    {
        int i = Random.Range(0, cost4.Count);
        int num = cost4[i].cNum;
        Transform ch4 = costPool[3].Get(num).transform;
        ch4.SetParent(transform);
        for (int c = 0; c < cells.Length; c++)
        {
            if (cells[c] == null)
            {
                cells[c] = ch4.gameObject;
                break;
            }
        }
        cost4[i].cCount++;
        if (cost4[i].cCount >= cost4[i].cMax)
            cost4.RemoveAt(i);
    }
    public void Champ5Produce()
    {
        int i = Random.Range(0, cost5.Count);
        int num = cost5[i].cNum;
        Transform ch5 = costPool[4].Get(num).transform;
        ch5.SetParent(transform);
        for (int c = 0; c < cells.Length; c++)
        {
            if (cells[c] == null)
            {
                cells[c] = ch5.gameObject;
                break;
            }
        }
        cost5[i].cCount++;
        if (cost5[i].cCount >= cost5[i].cMax)
            cost5.RemoveAt(i);
    }

}
