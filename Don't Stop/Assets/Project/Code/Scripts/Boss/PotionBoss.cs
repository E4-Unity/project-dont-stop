using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionBoss : MonoBehaviour
{
    public enum PlusStats
    {
        AttackUp,
        DefenseUp,
        AttackSpeedUp,
        HealthUp,
        SpeedUp
    }
    public PlusStats statName;

    PlayerBoss m_Player;

    void Update()
    {
        if (GameManagerBoss.Get().portalTouch == true)
            this.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        m_Player = GameObject.Find("Player").GetComponent<PlayerBoss>();

        if (other.gameObject.CompareTag("Player"))
        {
            switch (statName)
            {
                case PlusStats.AttackUp:
                    m_Player.attack += 200f;
                    break;
                case PlusStats.DefenseUp:
                    m_Player.defense += 50f;
                    break;
                case PlusStats.AttackSpeedUp:
                    m_Player.attackSpeed += 0.3f;
                    break;
                case PlusStats.HealthUp:
                    m_Player.health += 500f;
                    break;
                case PlusStats.SpeedUp:
                    m_Player.speed += 0.5f;
                    break;
            }

            this.gameObject.SetActive(false);
        }
    }
}
