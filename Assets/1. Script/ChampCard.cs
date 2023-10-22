using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampCard : MonoBehaviour
{

    public int ChampNum;
    public int ChampLv;

    Color[] TierColor = new Color[5];
    Color[] StarColor = new Color[3];

    //기본정보
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
        TierColor[0] = new Color(0.8f, 0.75f, 0.9f, 1);
        TierColor[1] = new Color(0.39f,0.82f,0.49f,1);
        TierColor[2] = new Color(0.3f, 0.55f, 1, 1);
        TierColor[3] = new Color(0.8f, 0.3f, 1, 1);
        TierColor[4] = new Color(1, 0.85f, 0, 1);

        StarColor[0] = new Color(1, 1, 1, 0);
        StarColor[1] = new Color(0.8f, 0.8f, 0.8f, 0);
        StarColor[2] = new Color(0.95f, 1, 0.25f, 0);

    }


}
