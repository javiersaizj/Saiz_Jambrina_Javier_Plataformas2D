using System.Collections;
using UnityEngine;

public class Mago : MonoBehaviour
{
    [SerializeField] private GameObject bolaFuego; // Prefab bola de fuego
    [SerializeField] private Transform puntoSpawn; // Punto desde donde se lanza la bola
    [SerializeField] private float tiempoAtaques; // cd ataques
    [SerializeField] private float danhoAtaque; // Daño 
    [SerializeField] private GameObject monedaPrefab; // Moneda 
    [SerializeField] private GameObject PUVida;
    private Animator anim;
    private Transform objetivo; // Ref jugador detectado
    private static bool isQuitting = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (objetivo != null) // jugador en el rango de detección
        {
            Vector3 direccion = (objetivo.position - transform.position).normalized;
            transform.localScale = new Vector3(direccion.x > 0 ? 1 : -1, 1, 1); // Mirar hacia el jugador
        }
    }

    private IEnumerator RutinaAtaque()
    {
        while (objetivo != null) // Solo atacar si hay un objetivo
        {
            anim.SetTrigger("atacar");
            yield return new WaitForSeconds(tiempoAtaques);
        }
    }

    private void LanzarBola()
    {
        if (objetivo != null)
        {
            GameObject bola = Instantiate(bolaFuego, puntoSpawn.position, Quaternion.identity);

            float direccion = transform.localScale.x; 
            bola.transform.localScale = new Vector3(direccion, 1, 1); // dir de la bola

            // vel de la bola
            Rigidbody2D rb = bola.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = new Vector2(direccion * 5f, 0); 
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("DeteccionPlayer"))
        {
            objetivo = elOtro.transform; 
            StartCoroutine(RutinaAtaque()); 
        }
    }

    private void OnTriggerExit2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("DeteccionPlayer"))
        {
            objetivo = null; 
            StopCoroutine(RutinaAtaque()); 
        }
    }

    private void OnDestroy()
    {
        if (!isQuitting && Application.isPlaying)
        {
            GenerarMonedas();
            GenerarPUVida();
        }
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void GenerarPUVida()
    {
        float probabilidad = Random.Range(0f, 100f);
        if (probabilidad <= 60f) // 60% de probabilidad
        {
            Instantiate(PUVida, transform.position, Quaternion.identity);
        }
    }

    private void GenerarMonedas()
    {
        Instantiate(monedaPrefab, transform.position + Vector3.left * 0.5f, Quaternion.identity);
        Instantiate(monedaPrefab, transform.position, Quaternion.identity);
        Instantiate(monedaPrefab, transform.position + Vector3.right * 0.5f, Quaternion.identity);
    }
}
