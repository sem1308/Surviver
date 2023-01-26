using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Property
{
    DAMAGE,
    RANGE,
    PROJECTILE,
    COOLTIME
}

public class Skill : MonoBehaviour
{
    public Weapon weapon;
    public Property property;
    public float amount;
    public static (float min, float max)[] amountRange = { (0.3f, 0.7f), (0.1f, 0.2f), (1, 3), (0.05f, 0.2f) };
    public static string[] propToString = { "대미지가", "범위가", "발사체 수가", "쿨타임이" };
    public static string[] amountToString = { "증가합니다.", "증가합니다.", "증가합니다.", "감소합니다." };

    public void SkillUp()
    {
        switch (property)
        {
            case Property.DAMAGE:
                weapon.damage *= (1 + amount);
                break;
            case Property.RANGE:
                weapon.range *= (1 + amount);
                break;
            case Property.PROJECTILE:
                weapon.projectileCnt += (int)amount;
                break;
            case Property.COOLTIME:
                weapon.coolTime *= (1 - amount);
                if (weapon.bulletData.type == bulletType.ORBIT)
                {
                    weapon.coolTime = Mathf.Max(weapon.bulletData.duration, weapon.coolTime);
                }
                break;
            default:
                break;
        }
    }

    public string explainToString()
    {
        string p = propToString[(int)property];
        string a = amountToString[(int)property];
        string am = (property == Property.PROJECTILE) ? $"{(int)amount}" : $"{amount:P0}";
        return $"{p} {am} {a}";
    }
}
