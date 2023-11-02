using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLvManager : MonoBehaviour
{
    public GameObject[][] lv1Units = new GameObject[40][];
    public GameObject[][] lv2Units = new GameObject[40][];

    private void Awake()
    {
        for (int i = 0; i < lv1Units.Length; i++)
        {
            lv1Units[i] = new GameObject[3];
            lv2Units[i] = new GameObject[3];
        }
    }
}
