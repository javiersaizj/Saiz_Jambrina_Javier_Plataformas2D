using UnityEngine;

public class Murcielago : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float velocidadPatrulla;
    [SerializeField] private float danhoAtaque;
    [SerializeField] private GameObject monedaPrefab;
    [SerializeField] private GameObject PUVida;
    private Vector3 destinoActual;
    //private int indiceActual = 0;
    private static bool isQuitting = false;
    // Start is called before the first frame update
    void Start()
    {
        //destinoActual = waypoints[indiceActual].position;
        //StartCoroutine(Patrulla());

    }

    // Update is called once per frame
    void Update()
    {


    }
    /*
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

        //destinoActual = waypoints[1].position;
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
    */
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
        }
    }

    private void GenerarPUVida()
    {
        float probabilidad = Random.Range(0f, 100f);
        if (probabilidad <= 40f) // 40% de probabilidad
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
        Instantiate(monedaPrefab, transform.position + Vector3.left * 0.5f, Quaternion.identity);
        Instantiate(monedaPrefab, transform.position + Vector3.right * 0.5f, Quaternion.identity);
    }
}