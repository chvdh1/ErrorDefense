using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    public float scanRange;
    public float defScanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    private void OnEnable()
    {
        scanRange = defScanRange + SynergyManager.sDistance;
    }

    private void FixedUpdate()
    {
        // Physics2D.CircleCastAll(ĳ���� ���� ��ġ, ���� ������, ĳ���� ����,ĳ���� ����, ��� ���̾�);
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach(RaycastHit2D target in targets)
        {
            Vector3 mypos = transform.position;
            Vector3 targetpos = target.transform.position;
            float curDiff = Vector3.Distance(mypos, targetpos);

            if(curDiff<diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }
        return result;
    }
}
