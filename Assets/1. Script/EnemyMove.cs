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

    GameObject sprite;

    private void Awake()
    {
        sprite = transform.GetChild(0).gameObject;
    }
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
        float ck = defmoveSpeed - sl > 0.2f ? 0.2f : defmoveSpeed - sl;
        moveSpeed = (ck) * (1 - (slX / 100));
    }

    void MovePath()
    {
        if (movePoint.Length == 0)
            return;
        Vector2 before = transform.position;

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
        Vector2 after = transform.position;
        float y = before.y - after.y > 0 ? 180 : 0;
        float x = before.x - after.x > 0 ? 90 : before.x - after.x < 0 ? -90 : 0;
        sprite.transform.rotation = Quaternion.Euler(0, 0, x+y);  

    }
}
