using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Importar TextMeshPro
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMonedas;
    [SerializeField] private Image barraVida;
    private int contadorMonedas = 0;

    public int ContadorMonedas { get => contadorMonedas; set => contadorMonedas = value; }

    public void IncrementarMonedas(int cantidad)
    {
        contadorMonedas += cantidad;
        ActualizarUI();
    }

    public void ActualizarBarraVida(float vidaActual, float vidaMaxima)
    {
        barraVida.fillAmount = vidaActual / vidaMaxima; 
    }

    private void ActualizarUI()
    {
        textMonedas.text = "" + contadorMonedas;
    }
}
