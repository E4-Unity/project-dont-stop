using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Reposition : MonoBehaviour
{
    Collider2D m_Collider;

    void Awake()
    {
        m_Collider = GetComponent<Collider2D>();
    }

    void OnTriggerExit2D(Collider2D _other)
    {
        if (!_other.CompareTag("Area")) return;

        Vector3 playerPos = GameManager.Get().GetPlayer().transform.position;
        Vector3 myPos = transform.position;

        switch (transform.tag)
        {
            case "Ground":
                float diffX = playerPos.x - myPos.x;
                float diffY = playerPos.y - myPos.y;
                float dirX = diffX < 0 ? -1 : 1;
                float dirY = diffY < 0 ? -1 : 1;
                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);
                
                if(diffX > diffY)
                    transform.Translate(dirX * 40 * Vector3.right);
                else if(diffX < diffY)
                    transform.Translate(dirY * 40 * Vector3.up);
                break;
            case "Enemy":
                if (m_Collider.enabled)
                {
                    Vector3 dist = playerPos - myPos;
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    transform.Translate(ran + dist * 2);
                }
                break;
        }
    }
}
