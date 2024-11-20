using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StraightMotion : ProjectileMotionBase
{
    public new void SetInfo(int dataID, Vector2 startPos, Vector3 dir, Action endCallback)
    {
        base.SetInfo(dataID, startPos, dir, endCallback);
    }

    protected override IEnumerator CoLaunchProjectile()
    {
        float duration = Data.Duration;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // 방향으로 이동
            LookAt2D(Dir);
            transform.position += Dir * Data.ProjSpeed * Time.deltaTime;

            yield return null;
        }

        EndCallback?.Invoke();
    }
}
