using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Charactor : MonoBehaviour
{
    public Weapon weapon;
    public Sprite charSprite;
    GameObject[] stats;

    public int charIdx;

    // Start is called before the first frame update
    void Start()
    {
        GameObject imgWeapon = GameObject.Find("Weapon Image" + charIdx);
        imgWeapon.GetComponent<Image>().sprite = weapon.spriter.sprite;
        GameObject imgChar = GameObject.Find("Img Char" + charIdx);
        imgChar.GetComponent<Image>().sprite = charSprite;
        stats = GameObject.FindGameObjectsWithTag("Stat");
        int[] statNums = { (int)weapon.damage, (int)weapon.range, weapon.projectileCnt, (int)weapon.coolTime };
        int start = (statNums.Length + 1) * charIdx;
        for (int i = start; i < start + statNums.Length; i++)
        {
            stats[i].GetComponent<TextMeshProUGUI>().text = statNums[i - start].ToString();
        }
        stats[start + statNums.Length].GetComponent<TextMeshProUGUI>().text = weapon.bulletData.isPenetrate ? "O" : "X";
    }
}
