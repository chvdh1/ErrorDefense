using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    public int enemyNum;

    public float MaxHp;
    public float hp;

    public int dmg;

    private void OnEnable()
    {
        hp = MaxHp;
    }


    //public void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    if (hit.gameObject.layer == 8)
    //    {
    //        PlBullet bulletdmg = hit.gameObject.GetComponent<PlBullet>();
    //        hp -= bulletdmg.dmg;
    //        hit.gameObject.SetActive(false);

    //        if (hp < 0)
    //            gameObject.SetActive(false);
    //    }

    //    if (hit.gameObject.layer == 6)
    //    {
    //        Goal goal = hit.gameObject.GetComponent<Goal>();
    //        gameObject.SetActive(false);
    //        goal.hp -= dmg;
    //    }
    //}

}
