using System.Collections;
using UnityEngine;

public class PowerUpVida : MonoBehaviour
{
    [SerializeField] private float cantidadCuracion = 50f;
    private static bool isQuitting = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("PlayerHitbox"))
        {
            Recoger(collision.gameObject);
        }
    }

    public void Recoger(GameObject player)
    {
        // restaurar vida
        SistemaVidas sistemaVidas = player.GetComponent<SistemaVidas>();
        if (sistemaVidas != null)
        {
            sistemaVidas.RestaurarVida(cantidadCuracion);
        }

        // destruir PowerUp
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
