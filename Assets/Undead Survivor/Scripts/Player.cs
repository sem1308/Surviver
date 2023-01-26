using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public Vector2 prevVec;

    public float health;
    public int maxHealth;
    public float speed;
    public float curExp;
    public float maxExp;
    public int level = 1;
    public int curWeaponCnt;
    public List<Weapon> weapons;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    AudioSource audioSource;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        weapons = new List<Weapon>();
        prevVec = new Vector2(1, 0);
    }

    private void FixedUpdate()
    {
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + nextVec);
        GameManager.instance.HPBox.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, -0.8f, 0));
    }

    private void Update()
    {
        foreach (Weapon weapon in weapons)
        {
            weapon.time += Time.deltaTime;

            if (weapon.time > weapon.coolTime)
            {
                weapon.time = 0;
                weapon.Attack();
            }
        }
    }

    void OnMove(InputValue input)
    {
        inputVec = input.Get<Vector2>();
        if (!inputVec.Equals(Vector2.zero))
            prevVec = inputVec;
    }

    private void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0 ? true : false;
        }
    }

    public void AddHealth(float h)
    {
        health = Mathf.Min(health + h, maxHealth);
        GameManager.instance.imageHP.fillAmount = health / maxHealth;
    }

    public void AddExp(int exp)
    {
        curExp += exp;
        if (curExp >= maxExp)
        {
            LevelUp(maxExp - curExp);
        }
        GameManager.instance.imageExp.fillAmount = curExp / maxExp;
    }
    private void LevelUp(float remain)
    {
        // 게임 일시정지
        GameManager.instance.Pause();

        // 레벨업
        if (level++ == GameManager.instance.maxLevel)
        {
            curExp = maxExp;
            level--;
            GameManager.instance.Resume();
        }
        else
        {
            curExp = remain;
            maxExp = GameManager.instance.maxExp[level - 1];
            // 스킬 고르기
            GameManager.instance.ShowSkills();
        }
        GameManager.instance.textLV.text = level.ToString();
    }

    public void OnHit(float damage)
    {
        audioSource.Play();
        health -= damage;
        gameObject.layer = 3;
        spriter.color = new Color(1, 1, 1, 0.4f);
        GameManager.instance.imageHP.fillAmount = health / maxHealth;
        if (health <= 0)
        {
            // 게임 오버
        }
        Invoke("OffHit", 0.5f);
    }

    public void OffHit()
    {
        gameObject.layer = 7;
        spriter.color = new Color(1, 1, 1, 1);
    }
}
