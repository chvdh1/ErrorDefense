using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class WaitingSeat : MonoBehaviour
{
    public GameObject[] pos = new GameObject[9];

    private void Awake()
    {
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = transform.GetChild(i).gameObject;
        }

    }

}
