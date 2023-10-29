using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public GameObject goal;

    public float defmoveSpeed;
    public float moveSpeed;

    public Transform[] movePoint;
    public int movePointIndex;

    public bool fly;
    public bool slow;


    private void OnEnable()
    {
        movePointIndex = 0;
        slow = false;
        moveSpeed = defmoveSpeed;
    }
    private void Update()
    {
        MovePath();
    }

    public void SlowE(float sl , float slX)
    {
        slow = true;
        float ck = defmoveSpeed - sl! > 0.2f ? 0.2f : defmoveSpeed - sl;
        moveSpeed = (ck - sl) * (1 - (slX / 100));
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
