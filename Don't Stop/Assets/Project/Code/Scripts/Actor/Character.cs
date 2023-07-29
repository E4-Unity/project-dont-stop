using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static float Speed => GameManager.Get().PlayerID == 0 ? 1.1f : 1;
    public static float WeaponSpeed => GameManager.Get().PlayerID == 1 ? 1.1f : 1; // 근거리
    public static float WeaponRate => GameManager.Get().PlayerID == 1 ? 0.9f : 1; // 원거리
    public static float Damage => GameManager.Get().PlayerID == 2 ? 1.2f : 1;
    public static int Count => GameManager.Get().PlayerID == 3 ? 1 : 0;
}
