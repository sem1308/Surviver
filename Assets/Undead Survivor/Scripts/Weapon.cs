using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum bulletType
{
    ORBIT,
    LAUNCH
}

[System.Serializable]
public class Weapon : MonoBehaviour
{
    public string weaponName; // 이름
    public int weaponIdx; // 무기 인덱스
    public int initCharIdx; // 초기 캐릭터 인덱스
    public string explainment; // 설명

    public float damage; // 대미지
    public float range; // 발사체 범위
    public int projectileCnt; // 발사체 개수
    public float coolTime; // 쿨타임

    public float time;

    public BulletData bulletData;

    public SpriteRenderer spriter;

    private void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
    }

    public void Init(WeaponData weaponData)
    {
        weaponName = weaponData.name;
        weaponIdx = weaponData.weaponIdx;
        initCharIdx = weaponData.initCharIdx;
        explainment = weaponData.explainment;
        bulletData = weaponData.bulletData;
        damage = weaponData.damage;
        projectileCnt = weaponData.projectileCnt;
        coolTime = weaponData.coolTime;
        time = coolTime;
        range = weaponData.range;
        bulletData.damage = damage;
    }

    public void Attack()
    {
        Vector3 playerDir = GameManager.instance.player.prevVec;
        float angle = Mathf.Atan2(playerDir.y, playerDir.x);

        float circleDeg = 360f / projectileCnt;
        float circleRad;

        // 무기 타입에 따른 공격
        switch (bulletData.type)
        {
            case bulletType.ORBIT:
                for (int i = 0; i < projectileCnt; i++)
                {
                    OrbitBullet bullet = GetBullet<OrbitBullet>();
                    bullet.Init(bulletData);
                    circleRad = circleDeg * i * Mathf.Deg2Rad;
                    bullet.transform.Translate(bullet.GetAngleVec(circleRad) * range, Space.World);
                    bullet.range = range;
                    bullet.offset = bullet.transform.position - GameManager.instance.player.transform.position;
                }
                break;
            case bulletType.LAUNCH:
                for (int i = 0; i < projectileCnt; i++)
                {
                    LaunchBullet bullet = GetBullet<LaunchBullet>();
                    bulletData.transAngle = (bulletData.isRandTrans ? Random.Range(bulletData.transAngleStart, bulletData.transAngleEnd) : circleDeg * i) * Mathf.Deg2Rad;
                    bulletData.waitTime = bulletData.baseWaitTime * i;
                    bullet.Init(bulletData);
                    bullet.transform.localScale = new Vector3(1, 1, 0) * range;
                }
                break;
            default:
                break;
        }
    }

    private T GetBullet<T>() where T : Bullet
    {
        T bullet = GameManager.instance.pool.Get((int)prefabIdx.BULLET + weaponIdx).GetComponent<T>();
        return bullet;
    }
}

[System.Serializable]
public class BulletData
{
    public float damage; // 대미지
    public bulletType type; // 타입
    public float duration; // 지속시간
    public float speed; // 발사체 속도
    public bool isPenetrate; // 발사체 관통 여부
    public float baseWaitTime; // 발사체 대기 시간
    public float waitTime; // 발사체 대기 시간
    public bool isRandTrans;
    public float transAngleStart; // 랜덤 발사체 위치 시작범위
    public float transAngleEnd; // 랜덤 발사체 위치 끝범위
    public float transAngle;
    public bool doForceFollowTrans; // 발사체에 가하는 힘이 위치를 따라가는지
    public bool isRotate; // 발사체가 회전하는지
}
