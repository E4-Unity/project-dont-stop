using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static float Speed => SurvivalGameManager.Get().PlayerID == 0 ? 1.1f : 1;
    public static float WeaponSpeed => SurvivalGameManager.Get().PlayerID == 1 ? 1.1f : 1; // 근거리
    public static float WeaponRate => SurvivalGameManager.Get().PlayerID == 1 ? 0.9f : 1; // 원거리
    public static float Damage => SurvivalGameManager.Get().PlayerID == 2 ? 1.2f : 1;
    public static int Count => SurvivalGameManager.Get().PlayerID == 3 ? 1 : 0;
}
