using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampCard : MonoBehaviour
{
    public int ChampNum;
    public int ChampLv;

    Color TierColor;
    Color StarColor;

    //기본정보
    public int star;
    public int coin;

    //-일반 공격
    public int AtDMG;
    public int AtRange;
    public int AtSpeed;

    //-스킬 공격
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
