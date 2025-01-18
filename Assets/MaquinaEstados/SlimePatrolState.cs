using UnityEngine;

public class SlimePatrolState : State<SlimeController>
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float velocidadPatrulla;

    private Vector3 destinoActual;
    private int indiceActual = 0;

    public override void OnEnterState(SlimeController controller)
    {
        base.OnEnterState(controller);

        if (waypoints.Length > 0)
        {
            destinoActual = waypoints[indiceActual].position;
            EnfocarDestino();
        }
    }

    public override void OnUpdateState()
    {
        if (waypoints.Length == 0) return; 

        transform.position = Vector3.MoveTowards(transform.position, destinoActual, velocidadPatrulla * Time.deltaTime);

        if (Vector3.Distance(transform.position, destinoActual) <= 0.1f)
        {
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

    public override void OnExitState()
    {

    }
}