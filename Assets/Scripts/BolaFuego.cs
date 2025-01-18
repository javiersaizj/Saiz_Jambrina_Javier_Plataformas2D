using System.Collections;
using UnityEngine;

public class BolaFuego : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float impulsoDisparo;
    [SerializeField] private float danhoAtaque;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            
            float direccion = transform.localScale.x; // 1 para der, -1 para izq
            rb.AddForce(new Vector2(direccion * impulsoDisparo, 0), ForceMode2D.Impulse);
        }

        // Destruir en 3 seg
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("PlayerHitbox"))
        {
            SistemaVidas sistemaVidasPlayer = elOtro.gameObject.GetComponent<SistemaVidas>();
            if (sistemaVidasPlayer != null)
            {
                sistemaVidasPlayer.RecibirDanho(danhoAtaque);
            }
            Destroy(gameObject);
        }
    }
}
