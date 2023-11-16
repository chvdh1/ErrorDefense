using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemList : MonoBehaviour
{
    public static RectTransform itemListTrans;

    private void Awake()
    {
        itemListTrans = GetComponent<RectTransform>();
    }
}
