using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampMng : MonoBehaviour
{
    public Champitems cItems;
    public Fire cFire;
    public Synergy cSynergy;
    public Targeting cTargeting;

    GameManager gm;

    private void Awake()
    {
        cItems = GetComponent<Champitems>();
        cFire = GetComponent<Fire>();
        cSynergy = GetComponent<Synergy>();
        cTargeting = GetComponent<Targeting>();
    }
}
