using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public abstract class ProjectileMotionBase : MonoBehaviour
{
    Coroutine _coLaunchProjectile;

    public Vector2 StartPos { get; private set; }
    public Vector3 Dir { get; private set; }
    public Data.ProjectileData Data { get; private set; }
    protected Action EndCallback { get; private set; }

    protected float _speed;

    protected void SetInfo(int dataID, Vector2 spawnPos, Vector3 dir, Action endCallback = null)
    {
        _speed = 5.0f;

        if (dataID != 0)
        {
            Data = Managers.Data.ProjectileDataDic[dataID];
            _speed = Data.ProjSpeed;
        }

        StartPos = spawnPos;
        Dir = dir;
        EndCallback = endCallback;

        if (_coLaunchProjectile != null)
            StopCoroutine(_coLaunchProjectile);

        _coLaunchProjectile = StartCoroutine(CoLaunchProjectile());
    }

    protected void LookAt2D(Vector2 forward)
    {
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }

    protected abstract IEnumerator CoLaunchProjectile();
}
