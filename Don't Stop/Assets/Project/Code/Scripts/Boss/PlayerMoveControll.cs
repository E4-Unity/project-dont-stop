using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveControll : MonoBehaviour
{
    public GameObject m_player;

    void Update()
    {
        float clampX = Mathf.Clamp(transform.position.x, -12f, 12f);
        float clampY = Mathf.Clamp(transform.position.y, -8f, 8f);

        transform.position = new Vector3(clampX, clampY, transform.position.z);
    }
}
