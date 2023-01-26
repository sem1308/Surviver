using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    COMMON,
    RARE,
    BOSS
}

public class Enemy : MonoBehaviour
{
    // 몬스터 기본 정보
    public float speed;
    public float health;
    public float maxHealth;
    public float damage;
    public EnemyType type;

    // 몬스터가 떨어뜨리는 경험치 정보
    public int expSpriteType;
    public int expMagnitude;


    public RuntimeAnimatorController[] animControllers;

    Rigidbody2D target;
    Vector2 nextVec;

    bool isLive;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // is called once per frame
    void FixedUpdate()
    {
        if (!isLive) return;

        Vector2 dirVec = target.position - rigid.position;
        nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if (!isLive) return;

        spriter.flipX = nextVec.x < 0 ? true : false;
    }

    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        health = maxHealth;
    }

    private void OnDisable()
    {
        isLive = false;
    }

    public void Init(SpawnData spawnData)
    {
        anim.runtimeAnimatorController = animControllers[spawnData.spriteType];
        maxHealth = spawnData.health;
        health = maxHealth;
        speed = spawnData.speed;
        damage = spawnData.damage;

        expMagnitude = spawnData.exp;
        expSpriteType = spawnData.expSpriteType;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.transform.tag)
        {
            case "Player":
                GameManager.instance.player.OnHit(damage);
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.transform.tag)
        {
            case "Bullet":
                transform.Translate((transform.position - (other.transform.position + GameManager.instance.player.transform.position) / 2).normalized);
                Bullet bullet = other.gameObject.GetComponent<Bullet>();
                OnHit(bullet.damage);
                if (!bullet.isPenetrate) bullet.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void OnHit(float damage)
    {
        // Debug.Log("아픔");
        health -= damage;
        gameObject.layer = 3;
        spriter.color = new Color(1, 1, 1, 0.4f);
        if (health <= 0)
        {
            Vector3 pos = transform.position;
            gameObject.SetActive(false);
            health = maxHealth;
            DropItem(pos);
        }
        audioSource.Play();
        Invoke("OffHit", 0.05f);
    }

    public void OffHit()
    {
        gameObject.layer = 6;
        spriter.color = new Color(1, 1, 1, 1);
    }

    public void DropItem(Vector3 pos)
    {
        int prob = Random.Range(0, 1000);
        if (20 < prob)
        {
            GameObject exp = GameManager.instance.pool.Get((int)prefabIdx.EXP);
            exp.GetComponent<Exp>().Init(expMagnitude, expSpriteType);
            exp.transform.position = pos;
        }
        else if (5 < prob && prob < 20)
        {
            GameObject health = GameManager.instance.pool.Get((int)prefabIdx.HEALTH);
            health.transform.position = pos;
        }
        else
        {
            GameObject mag = GameManager.instance.pool.Get((int)prefabIdx.MAG);
            mag.transform.position = pos;
        }
    }
}
