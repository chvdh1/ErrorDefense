using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public GameObject goal;

    public float moveSpeed;

    public Transform[] movePoint;
    public int movePointIndex;

    public bool fly;

    private void Update()
    {
        MovePath();
    }

    void MovePath()
    {
        if (movePoint.Length == 0)
            return;


        if (!fly)
        {
            transform.position =
           Vector2.MoveTowards(transform.position,
           movePoint[movePointIndex].position, moveSpeed * Time.deltaTime);

            if (transform.position == movePoint[movePointIndex].position)
                movePointIndex++;

            if (movePointIndex == movePoint.Length)
                movePointIndex = 0;
        }
        else
        {
            transform.position =
          Vector2.MoveTowards(transform.position,
          goal.transform.position, moveSpeed * Time.deltaTime);
        }
    }
}
