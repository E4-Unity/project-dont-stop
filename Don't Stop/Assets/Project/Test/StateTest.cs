using System;
using System.Collections;
using UnityEngine;

public class StateTest : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Record());
    }

    IEnumerator Record()
    {
        yield return null;
        GameState.Get().Gold += 10;
        PlayerState.Get().PlayerData.Exp += 20;
        GameState.Get().UpdateResult();
    }
}
