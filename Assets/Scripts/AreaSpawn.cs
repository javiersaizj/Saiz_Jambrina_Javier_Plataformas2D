using UnityEngine;

public class AreaSpawn : MonoBehaviour
{
    [SerializeField] private string nombreArea; 
    [SerializeField] private Transform puntoSpawn; 

    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.RegistrarPuntoRespawn(nombreArea, puntoSpawn);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHitbox"))
        {
            Player player = collision.GetComponentInParent<Player>();
            if (player != null)
            {
                // Asignar el punto de respawn cuando el jugador entre en el área
                player.AsignarPuntoRespawn(nombreArea);
            }
        }
    }
}
