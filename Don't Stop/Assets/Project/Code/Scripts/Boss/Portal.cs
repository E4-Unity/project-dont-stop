using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject portal_activated;
    public bool activate = false;
    Collider2D coll;

    void Start()
    {
        coll = GetComponent<Collider2D>();
    }

    void Update()
    {
        if(activate == true)
        {
            portal_activated.SetActive(true);
            coll.enabled = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(activate == true)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                GameManagerBoss.Get().portalTouch = true;
                this.gameObject.SetActive(false);
            }
        }
    }
}
