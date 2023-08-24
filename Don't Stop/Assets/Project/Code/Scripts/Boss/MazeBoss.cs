using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBoss : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Bullet_01(Clone)")
            other.gameObject.SetActive(false);
    }
}