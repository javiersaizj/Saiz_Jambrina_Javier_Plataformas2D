using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHitbox"))
        {
            SistemaVidas sistemaVidas = collision.GetComponentInParent<SistemaVidas>();
            if (sistemaVidas != null)
            {
                // Reduce vida a 0
                sistemaVidas.RecibirDanho(sistemaVidas.VidasActuales);
            }
        }
    }
}
