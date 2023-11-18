using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampMng : MonoBehaviour
{
    public Champitems cItems;
    public ChampSkill cSkill;
    public Fire cFire;
    public PlBullet cPl;
    public Synergy cSynergy;
    public Targeting cTargeting;

    GameManager gm;

    private void Awake()
    {
        cItems = GetComponent<Champitems>();
        cSkill = GetComponent<ChampSkill>();
        cFire = GetComponent<Fire>();
        cPl = GetComponent<PlBullet>();
        cSynergy = GetComponent<Synergy>();
        cTargeting = GetComponent<Targeting>();
    }
}
