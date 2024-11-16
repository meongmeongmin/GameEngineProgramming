using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BaseObject : MonoBehaviour
{
    public EObjectType ObjectType { get; protected set; }
    public Rigidbody2D RigidBody { get; private set; }
    public CircleCollider2D Collider { get; private set; }
    public float ColliderRadius { get { return Collider != null ? Collider.radius : 0.0f; } }
    
    public Vector3 CenterPosition { get { return transform.position + Vector3.up * ColliderRadius; } }

    public int DataID { get; set; }

    bool _init = false;

    void Awake()
    {
        Init();
    }

    public virtual bool Init()
    {
        if (_init)
            return false;
        
        RigidBody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<CircleCollider2D>();

        _init = true;
        return true;
    }
}
