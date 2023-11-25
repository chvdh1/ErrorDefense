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
    }

    public void Setline(int stagei)
    {
        int pojnt = stagei == 1 ? gm.map1Lines.Length+2 : 0;
        points = new GameObject[pojnt];
        lr.positionCount = points.Length;

        int posCount = 0;
        for (int i = 0; i < points.Length; i++)
        {
            if (i == 0)
                points[i] = gm.spawnPos.gameObject;
            else if (i < points.Length - 1)
            {
                points[i] = gm.mapTiles[gm.map1Lines[posCount]];
                Debug.Log(points[i]);
                posCount++;
            }
            else
                points[i] = gm.goal;
        }
        for (int i = 0; i < points.Length; i++)
        {
            lr.SetPosition(i, points[i].transform.position);
        }
    }
}
