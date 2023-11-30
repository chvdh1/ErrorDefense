using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDis : MonoBehaviour
{
    private void Awake()
    {
        var obj = FindObjectsOfType<DontDis>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
