using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    
    public static ItemManager Instance;

    public Sprite[] itemImg = new Sprite[35];


    private void Awake()
    {
        Instance = transform.gameObject.GetComponent<ItemManager>();
    }
}
