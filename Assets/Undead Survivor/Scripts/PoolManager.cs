using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum prefabIdx
{
    ENEMY,
    EXP,
    BULLET,
    HEALTH = 8,
    MAG
}

public class PoolManager : MonoBehaviour
{
    // Prefab 보관 변수
    public GameObject[] prefabs;

    // 풀 담당 리스트
    List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }

        // Debug.Log(pools.Length);
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        foreach (GameObject obj in pools[index])
        {
            if (!obj.activeSelf)
            {
                select = obj;
                select.SetActive(true);
                break;
            }
        }

        if (select == null)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }

    public List<GameObject> GetAll(int index)
    {
        List<GameObject> selects = new List<GameObject>();
        foreach (GameObject obj in pools[index])
        {
            if (obj.activeSelf)
            {
                selects.Add(obj);
            }
        }

        return selects;
    }
}
