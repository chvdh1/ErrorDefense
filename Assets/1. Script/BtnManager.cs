using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnManager : MonoBehaviour
{
    GameManager gm;

    private void Awake()
    {
        gm = gameObject.GetComponent<GameManager>();
    }

    public void StageOneBtn()
    {
        gm.mapIndex = 1;
        gm.GameStart();
    }

}
