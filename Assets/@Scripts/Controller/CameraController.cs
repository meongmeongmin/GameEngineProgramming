using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Creature Target;

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        Camera.main.orthographicSize = 5.0f;
    }

    void LateUpdate()
    {
        if (Target == null)
            return;

        Vector3 targetPosition = new Vector3(Target.CenterPosition.x, Target.CenterPosition.y, -10f);
        transform.position = targetPosition;
    }
}
