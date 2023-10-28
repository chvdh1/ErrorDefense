using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineR : MonoBehaviour
{
    public GameManager gm;
    LineRenderer lr;
    public GameObject[] points;
    public int num;
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();

        points = new GameObject[transform.childCount+2];
        lr.positionCount = points.Length;

        for (int i = 0; i < points.Length; i++)
        {
            if(i==0)
                points[i] = gm.spawnPos[num].gameObject;
            else if (i < points.Length-1)
                points[i] = transform.GetChild(i-1).gameObject;
            else
                points[i] = gm.goal;
        }
        for (int i = 0; i < points.Length; i++)
        {
            lr.SetPosition(i, points[i].transform.position);
        }
          
        
    }
}
