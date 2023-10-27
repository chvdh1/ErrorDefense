using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineR : MonoBehaviour
{
    public GameManager gm;
    LineRenderer lr;
    public GameObject[] points;
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();

        points = new GameObject[transform.childCount+1];
        lr.positionCount = points.Length+1;

        for (int i = 0; i <= points.Length; i++)
        {
            if (i < points.Length)
                points[i] = transform.GetChild(i).gameObject;
            else
                points[i] = gm.goal;
        }
        for (int i = 0; i < points.Length; i++)
        {
            lr.SetPosition(i, points[i].transform.position);
        }
          
        
    }
}
