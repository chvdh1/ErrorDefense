using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Champitems : MonoBehaviour
{
    public ItemSocket[] sockets = new ItemSocket[3];

    public float cDmg = 0;
    public float cAttSpeed = 0;
    public float cCriPer = 0;
    public float cCriDmg = 0;
    public float cSkillDmg = 0;
    public float cMp = 0;
    public float cDistance = 0;

    public bool[] mixture = new bool[80];

    public void ItemEffect()
    {
        for (int i = 0; i < mixture.Length; i++)
        {
            mixture[i] = false;
        }

        cDmg = 0;
        cAttSpeed = 0;
        cCriPer = 0;
        cCriDmg = 0;
        cSkillDmg = 0;
        cMp = 0;
        cDistance = 0;


        for (int i = 0; i < sockets.Length; i++)
        { 
            if(sockets[i] != null)
            {
                cDmg += sockets[i].iDmg;
                cAttSpeed += sockets[i].iAttSpeed;
                cCriPer += sockets[i].iCriPer;
                cCriDmg += sockets[i].iCriDmg;
                cSkillDmg += sockets[i].iSkillDmg;
                cMp += sockets[i].iMp;
                cDistance += sockets[i].iDistance;

                mixture[sockets[i].itemNum] = true;
            }
        }

    }

}
