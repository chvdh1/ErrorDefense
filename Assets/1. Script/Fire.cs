using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public PoolManager bulletPool;
    public Targeting layder;

    public int dmg;
    public float coolTime;
    public float shootTime;

    private void Update()
    {
        if (shootTime > 0)
            shootTime -= Time.deltaTime;
        else
            Shoot();
    }

    void Shoot()
    {
        if (layder.nearestTarget == null)
            return;

        shootTime = coolTime;
        PlBullet pb = bulletPool.Get(0).gameObject.GetComponent<PlBullet>();
        pb.gameObject.transform.position = transform.position;
        pb.targetEnemy = layder.nearestTarget;
        pb.dmg = dmg;


    }
}
