using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitBullet : Bullet
{
    Transform target;
    public Vector3 offset;
    public float range;

    protected override void Awake()
    {
        base.Awake();
        target = GameManager.instance.player.transform;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.position = target.position + offset;
        transform.RotateAround(target.position, Vector3.back, speed * range * Time.deltaTime);
        offset = transform.position - target.position;
    }
}