using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractivoManager : MonoBehaviour
{
    [SerializeField] private GameObject final1UI;
    [SerializeField] private GameObject final2UI;
    [SerializeField] private GameObject puerta;
    [SerializeField] private int costoMonedas;
    [SerializeField] private SpriteRenderer textoInteractivoSprite;

    private UIManager uiManager;
    private bool jugadorDentro = false;

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();

        if (textoInteractivoSprite != null)
        {
            Color colorTransparente = textoInteractivoSprite.color;
            colorTransparente.a = 0f;
            textoInteractivoSprite.color = colorTransparente;
        }

        if (final1UI != null)
        {
            final1UI.SetActive(false); 
        }

        if (final2UI != null)
        {
            final2UI.SetActive(false); 
        }
    }

    void Update()
    {
        if (jugadorDentro && Input.GetKeyDown(KeyCode.E))
        {
            AbrirPuerta(); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHitbox"))
        {
            jugadorDentro = true;

            if (textoInteractivoSprite != null)
            {
                Color colorVisible = textoInteractivoSprite.color;
                colorVisible.a = 1f;
                textoInteractivoSprite.color = colorVisible;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHitbox"))
        {
            jugadorDentro = false;

            if (textoInteractivoSprite != null)
            {
                Color colorTransparente = textoInteractivoSprite.color;
                colorTransparente.a = 0f;
                textoInteractivoSprite.color = colorTransparente;
            }
        }
    }

    private void AbrirPuerta()
    {
        if (uiManager != null && uiManager.ContadorMonedas >= costoMonedas)
        {
            uiManager.IncrementarMonedas(-costoMonedas); 

            if (final1UI != null)
            {
                final1UI.SetActive(true); 
            }

            if (final2UI != null)
            {
                final2UI.SetActive(true); 
            }

            if (puerta != null)
            {
                Destroy(puerta); 
            }

            if (textoInteractivoSprite != null)
            {
                Color colorTransparente = textoInteractivoSprite.color;
                colorTransparente.a = 0f;
                textoInteractivoSprite.color = colorTransparente;
            }

            Time.timeScale = 0f;

            if (final2UI != null)
            {
                final2UI.AddComponent<ClickableUI>().OnClick = ResetGame;
            }
        }
        else
        {
            Debug.Log("No tienes suficientes monedas para abrir la puerta.");
        }
    }
    private void ResetGame()
    {

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
