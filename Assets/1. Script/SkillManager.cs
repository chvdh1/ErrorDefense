using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class SkillManager : MonoBehaviour
{
    Synergy sg;

    public int champNum;
    public float maxMp;
    public float mp;

    public float defSkillDmgPer;
    public float skillDmgPer;

    public PoolManager poolManager;

    private void Awake()
    {
        sg = GetComponent<Synergy>();

        champNum = sg.champNum;
    }

    public void Skill()
    {
        mp = 0;
        switch(champNum)
        {
            case 11:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 12:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 13:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 14:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 15:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 16:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 17:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 18:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 21:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 22:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 23:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 24:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 25:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 26:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 27:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 28:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 31:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 32:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 33:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 34:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 35:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 36:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 37:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 38:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 41:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 42:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 43:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 44:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 45:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 46:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 47:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 48:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 51:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 52:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 53:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 54:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 55:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 56:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 57:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
            case 58:
                Debug.Log(champNum + "유닛의 스킬 발동");
                break;
        }
    }
}
