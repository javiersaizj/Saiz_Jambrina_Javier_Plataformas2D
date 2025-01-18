using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private float inputH;
    private float inputV;
    private SpriteRenderer spriteRenderer;

    [Header("Sistema de Movimiento")]
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float fuerzaSalto;
    [SerializeField] private float distanciaDeteccionSuelo;
    [SerializeField] private Transform pies;
    [SerializeField] private LayerMask queEsSaltable;

    [Header("Sistema de Combate")]
    [SerializeField] private Transform puntoAtaque;
    [SerializeField] private float radioAtaque;
    [SerializeField] private float danhoAtaque;
    [SerializeField] private LayerMask queEsDanhable;

    [Header("Sistema de Dash")]
    [SerializeField] private float dashForce; // Velocidad del Dash
    [SerializeField] private float dashDuration; // Dur del Dash
    [SerializeField] private float dashCooldown; // CD Dashes
    private Vector2 dashDirection;
    private bool isDashing;
    private bool canDash = true;
    private Transform puntoRespawn; // ultimo punto de respawn asignado
    private Dictionary<string, Transform> puntosRespawn = new Dictionary<string, Transform>();


    private bool isInvulnerable = false;

    private Animator anim;
    private UIManager uiManager;

    public bool IsInvulnerable { get => isInvulnerable; set => isInvulnerable = value; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        uiManager = FindObjectOfType<UIManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isDashing) return; // Bloq acciones mientras Dash

        Movimiento();
        Saltar();
        LanzarAtaque();
        HandleDashInput();
    }

    private void LanzarAtaque()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Attack");
        }
    }

    private void Ataque()
    {
        Collider2D[] collidersTocados = Physics2D.OverlapCircleAll(puntoAtaque.position, radioAtaque, queEsDanhable);
        foreach (Collider2D item in collidersTocados)
        {
            SistemaVidas sistemaVidasEnemigos = item.gameObject.GetComponent<SistemaVidas>();
            if (sistemaVidasEnemigos != null)
            {
                sistemaVidasEnemigos.RecibirDanho(danhoAtaque);
            }
        }
    }

    private void Saltar()
    {
        if (Input.GetKeyDown(KeyCode.Space) && EstoyEnSuelo())
        {
            rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            anim.SetTrigger("Jump");
        }
    }

    private bool EstoyEnSuelo()
    {
        return Physics2D.Raycast(pies.position, Vector2.down, distanciaDeteccionSuelo, queEsSaltable);
    }

    private void Movimiento()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(inputH * velocidadMovimiento, rb.velocity.y);

        if (inputH != 0)
        {
            anim.SetBool("Running", true);
            if (inputH > 0)
            {
                transform.eulerAngles = Vector3.zero;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
        else
        {
            anim.SetBool("Running", false);
        }
    }

    private void HandleDashInput()
    {
        if (canDash && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Mouse1)))
        {
            dashDirection = GetDashDirection(); //dir del Dash
            StartCoroutine(Dash()); //dash coroutine
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        isInvulnerable = true;

        ChangeSpriteOpacity(0.4f);
        float originalGravity = rb.gravityScale; 
        rb.gravityScale = 0; // Desactiva la gravedad durante el Dash
        rb.velocity = dashDirection * dashForce; // dir y fuerza del dash
        anim.SetTrigger("Dash"); // Activa Dash

        yield return new WaitForSeconds(dashDuration); // Espera dur del Dash
        ChangeSpriteOpacity(1f);

        rb.gravityScale = originalGravity; // restaura grav
        rb.velocity = Vector2.zero; // Detiene al jugador tras el Dash
        isDashing = false;
        isInvulnerable = false;

        yield return new WaitForSeconds(dashCooldown); // CD
        canDash = true;
    }

    private Vector2 GetDashDirection()
    {
        float dashInputH = Input.GetAxisRaw("Horizontal");
        float dashInputV = Input.GetAxisRaw("Vertical");

        // Si hay input direccional, usa esa dirección
        if (dashInputH != 0 || dashInputV != 0)
        {
            return new Vector2(dashInputH, dashInputV).normalized;
        }

        // Para poder dashear sin pulsar teclas hacia donde el jugador mire:
        return transform.eulerAngles.y == 0 ? Vector2.right : Vector2.left;
    }

    private void ChangeSpriteOpacity(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin") && collision.gameObject.layer == LayerMask.NameToLayer("Hitbox"))
        {
            Coin coin = collision.GetComponent<Coin>();
            if (coin != null)
            {
                coin.Recoger();
                uiManager.IncrementarMonedas(10);
            }
        }
        else if (collision.CompareTag("PowerUpVida"))
        {
            PowerUpVida powerUpVida = collision.GetComponent<PowerUpVida>();
            if (powerUpVida != null)
            {
                powerUpVida.Recoger(gameObject);
            }
        }
    }

    public void RegistrarPuntoRespawn(string areaNombre, Transform puntoSpawn)
    {
        if (!puntosRespawn.ContainsKey(areaNombre))
        {
            puntosRespawn.Add(areaNombre, puntoSpawn);
            Debug.Log($"Registrado punto de respawn para área: {areaNombre}");
        }
    }

    public void AsignarPuntoRespawn(string areaNombre)
    {
        if (puntosRespawn.TryGetValue(areaNombre, out Transform nuevoPunto))
        {
            puntoRespawn = nuevoPunto;
            Debug.Log($"Asignado punto de respawn: {puntoRespawn.position} para área: {areaNombre}");
        }
        else
        {
            Debug.LogWarning($"Área '{areaNombre}' no tiene un punto de respawn asignado.");
        }
    }

    public void Reaparecer()
    {
        transform.position = puntoRespawn.position;
        SistemaVidas sistemaVidas = GetComponent<SistemaVidas>();
        if (sistemaVidas != null)
        {
            sistemaVidas.VidasActuales = sistemaVidas.VidasMaximas;
            sistemaVidas.RecibirDanho(0);
        }
        EntityResetManager.Instance.ResetEntities();
        Debug.Log("Jugador reapareció en: " + puntoRespawn.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(puntoAtaque.position, radioAtaque);
    }
}