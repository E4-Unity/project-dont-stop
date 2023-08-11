using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoss : MonoBehaviour
{
    public enum PlusStats
    {
        AttackUp,
        DeffenseUp,
        AttackSpeedUp,
        HealthUp,
        SpeedUp
    }

    public PlusStats statName;

    void Update()
    {
        if (GameManagerBoss.Get().portalTouch == true)
            this.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);

            switch (statName)
            {
                case PlusStats.AttackUp:
                    break;
                case PlusStats.DeffenseUp:
                    break;
                case PlusStats.AttackSpeedUp:
                    break;
                case PlusStats.HealthUp:
                    break;
                case PlusStats.SpeedUp:
                    break;
            }
        }
    }
}
