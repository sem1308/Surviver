using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.transform.tag)
        {
            case "Player":
                if (transform.tag == "Health")
                {
                    GameManager.instance.player.AddHealth(30f);
                    gameObject.SetActive(false);
                }
                else if (transform.tag == "Mag")
                {
                    List<GameObject> exps = GameManager.instance.pool.GetAll((int)prefabIdx.EXP);
                    foreach (GameObject obj in exps)
                    {
                        Exp exp = obj.GetComponent<Exp>();
                        exp.SetMag();
                    }
                    gameObject.SetActive(false);
                }
                break;
            default:
                break;
        }
    }
}
