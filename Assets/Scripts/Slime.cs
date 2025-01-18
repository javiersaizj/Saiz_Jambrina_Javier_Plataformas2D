using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float velocidadPatrulla;
    [SerializeField] private float danhoAtaque;
    [SerializeField] private GameObject monedaPrefab;
    [SerializeField] private GameObject PUVida;

    private Animator animator;
    private Vector3 destinoActual;
    private int indiceActual = 0;
    private static bool isQuitting = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (waypoints.Length > 0)
        {
            destinoActual = waypoints[indiceActual].position;
            EnfocarDestino();
            StartCoroutine(Patrulla());
        }
    }

    IEnumerator Patrulla()
    {
        while (true)
        {
            while (transform.position != destinoActual)
            {
                transform.position = Vector3.MoveTowards(transform.position, destinoActual, velocidadPatrulla * Time.deltaTime);
                yield return null;
            }
            DefinirNuevoDestino();
        }
    }

    private void DefinirNuevoDestino()
    {
        indiceActual++;
        if (indiceActual >= waypoints.Length)
        {
            indiceActual = 0;
        }
        destinoActual = waypoints[indiceActual].position;
        EnfocarDestino();
    }

    private void EnfocarDestino()
    {
        if (destinoActual.x > transform.position.x)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("DeteccionPlayer"))
        {
            print("Detectado!!");
        }
        else if (elOtro.gameObject.CompareTag("PlayerHitbox"))
        {
            SistemaVidas sistemaVidasPlayer = elOtro.gameObject.GetComponent<SistemaVidas>();
            sistemaVidasPlayer.RecibirDanho(danhoAtaque);
            animator.SetTrigger("atacar");
        }
    }

    private void GenerarPUVida()
    {
        float probabilidad = Random.Range(0f, 100f);
        if (probabilidad <= 20f) // 20% de probabilidad
        {
            Instantiate(PUVida, transform.position, Quaternion.identity);
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

    private void GenerarMonedas()
    {
        Instantiate(monedaPrefab, transform.position, Quaternion.identity);
    }
}