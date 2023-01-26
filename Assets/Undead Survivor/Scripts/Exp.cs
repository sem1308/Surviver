using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp : MonoBehaviour
{
    public int exp;

    public Sprite[] spriters;
    public float speed = 2;
    public bool isMag;

    SpriteRenderer spriter;

    private void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (isMag)
        {
            Vector2 transVec = GameManager.instance.player.transform.position - transform.position;
            transform.Translate(transVec * speed * Time.fixedDeltaTime);
            speed *= (1.1f);
        }
    }

    public void Init(int magnitude, int spriteType)
    {
        spriter.sprite = spriters[spriteType];
        exp = magnitude;
    }

    private void OnEnable()
    {
        isMag = false;
        speed = 2;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.transform.tag)
        {
            case "Player":
                other.gameObject.GetComponentInParent<Player>().AddExp(exp);
                gameObject.SetActive(false);
                break;
            case "ExpArea":
                isMag = true;
                break;
            default:
                break;
        }
    }
}
