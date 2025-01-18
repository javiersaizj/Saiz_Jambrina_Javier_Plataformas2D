using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private static bool isQuitting = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("PlayerHitbox"))
        {
            Recoger();
        }
    }

    public void Recoger()
    {
        Destroy(this.gameObject);
    }
    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void OnDestroy()
    {
        if (!isQuitting)
        {
            Destroy(this.gameObject);
        }
    }
}