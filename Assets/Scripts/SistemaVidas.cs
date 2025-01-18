using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaVidas : MonoBehaviour
{
    [SerializeField] private float vidasMaximas;
    private float vidasActuales;

    [SerializeField] private bool usaBarraVida = false; // Indica si tiene una barra de vida (solo Player)
    private UIManager uiManager;

    public float VidasMaximas { get => vidasMaximas; set => vidasMaximas = value; }
    public float VidasActuales { get => vidasActuales; set => vidasActuales = value; }

    private void Start()
    {
        vidasActuales = vidasMaximas;

        if (usaBarraVida)
        {
            uiManager = FindObjectOfType<UIManager>();
            uiManager.ActualizarBarraVida(vidasActuales, vidasMaximas);
        }
    }

    public void RecibirDanho(float danhoRecibido)
    {
        // Si es inmune:
        if (TryGetComponent<Player>(out Player player) && player.IsInvulnerable)
        {
            Debug.Log($"{gameObject.name} es invulnerable y no recibe daño.");
            return; 
        }

        // Aplica daño
        vidasActuales -= danhoRecibido;
        vidasActuales = Mathf.Clamp(vidasActuales, 0, vidasMaximas);

        // Barra de vida:
        if (usaBarraVida && uiManager != null)
        {
            uiManager.ActualizarBarraVida(vidasActuales, vidasMaximas);
        }

        if (vidasActuales <= 0)
        {
            Muerte();
        }
    }

    private void Muerte()
    {
        Debug.Log($"{gameObject.name} ha muerto.");
        if (TryGetComponent<Player>(out Player player))
        {
            player.Reaparecer(); // Reaparece al jugador
        }
        else
        {
            Destroy(this.gameObject); // Destruye enemigos u otros objetos
        }
    }

    public void RestaurarVida(float cantidad)
    {
        vidasActuales += cantidad;
        vidasActuales = Mathf.Clamp(vidasActuales, 0, vidasMaximas);

        if (usaBarraVida && uiManager != null)
        {
            uiManager.ActualizarBarraVida(vidasActuales, vidasMaximas);
        }
    }
}
