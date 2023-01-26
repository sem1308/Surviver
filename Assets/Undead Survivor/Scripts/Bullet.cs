using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public float duration; // 지속시간
    public float time;
    public float speed;

    public bool isPenetrate; // 발사체 관통 여부

    public Rigidbody2D rigid;
    public SpriteRenderer spriter;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
    }

    protected virtual void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        if (time > duration)
        {
            gameObject.SetActive(false);
        }
    }

    protected virtual void OnEnable()
    {
        time = 0f;
    }

    public virtual void Init(BulletData bullet)
    {
        this.damage = bullet.damage;
        this.duration = bullet.duration;
        this.speed = bullet.speed;
        this.isPenetrate = bullet.isPenetrate;
        transform.position = GameManager.instance.player.transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public Vector3 GetAngleVec(float angle)
    {
        return new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
    }
}