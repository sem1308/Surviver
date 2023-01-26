using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchBullet : Bullet
{
    public float transAngle;
    public float waitTime; // 발사체 대기 시간

    public bool doForceFollowTrans;
    public bool isRotate;

    bool isLaunched;
    Vector3 transVec;

    AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!isLaunched)
        {
            if (time > waitTime)
            {
                isLaunched = true;
                spriter.color = new Color(1, 1, 1, 1);
                audioSource.Play();
                Launch();
            }
            else
            {
                transform.position = GameManager.instance.player.transform.position;
            }
        }
        if (isRotate)
        {
            transform.Rotate(Vector3.forward * Time.fixedDeltaTime * 360f);
        }
    }

    public override void Init(BulletData bullet)
    {
        base.Init(bullet);
        spriter.color = new Color(1, 1, 1, 0);
        this.waitTime = bullet.waitTime;
        this.transAngle = bullet.transAngle;
        this.doForceFollowTrans = bullet.doForceFollowTrans;
        this.isRotate = bullet.isRotate;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        isLaunched = false;
    }

    void Launch()
    {
        Vector3 playerDir = GameManager.instance.player.prevVec;
        Vector3 forceVec;

        float playerAngle = Mathf.Atan2(playerDir.y, playerDir.x); // angle 조정하여 발사체 공격 방향 결정
        float tangle = playerAngle + transAngle;
        float rangle = doForceFollowTrans ? tangle : playerAngle;

        transVec = GetAngleVec(playerAngle + transAngle);
        forceVec = GetAngleVec(rangle) * speed;

        rangle *= Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(rangle - 90, Vector3.forward);
        transform.Translate(transVec, Space.World);


        rigid.AddForce(forceVec, ForceMode2D.Impulse);
    }
}
