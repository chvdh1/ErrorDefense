using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInfoText : MonoBehaviour
{
    public string Unitname;
    public int cost;
    public int type;
    public int atttype;

    [TextArea]
    public string attinfo;
    [TextArea]
    public string skillinfo;
}
