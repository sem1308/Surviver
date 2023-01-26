using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D coll;

    int tileSize = 40;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Area"))
        {
            Vector3 playerPos = GameManager.instance.player.transform.position;
            Vector3 myPos = transform.position;

            float diffX = playerPos.x - myPos.x;
            float diffY = playerPos.y - myPos.y;

            float dirX = diffX < 0 ? -1 : 1;
            float dirY = diffY < 0 ? -1 : 1;

            diffX = Mathf.Abs(diffX);
            diffY = Mathf.Abs(diffY);

            Vector3 playerDir = GameManager.instance.player.inputVec;

            switch (transform.tag)
            {
                case "Ground":
                    if (diffX > diffY)
                    {
                        transform.Translate(Vector3.right * dirX * tileSize);
                    }
                    else if (diffX < diffY)
                    {
                        transform.Translate(Vector3.up * dirY * tileSize);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
