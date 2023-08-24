using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultBoss : MonoBehaviour
{
    public GameObject victory;
    public GameObject lose;

    public void Win()
    {
        this.gameObject.SetActive(true);
        victory.SetActive(true);
    }

    public void Lose()
    {
        this.gameObject.SetActive(true);
        lose.SetActive(true);
    }
}
