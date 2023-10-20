using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampCard : MonoBehaviour
{
    public int ChampNum;
    public int ChampLv;

    Color TierColor;
    Color StarColor;

    //-�Ϲ� ����
    public int AtDMG;
    public int AtRange;
    public int AtSpeed;

    //-��ų ����
    public float Mp;
    public int SkDMG;
    public int SkRange;
    public int SkSpeed;
    private void Awake()
    {
        TierColor = transform.GetChild(0).GetComponent<Color>();
        StarColor = transform.GetChild(1).GetComponent<Color>();
    }


}
