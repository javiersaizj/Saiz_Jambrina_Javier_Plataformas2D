using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State<EnemyController>
{
    [SerializeField] private Transform route;
    [SerializeField] private float velocidadPatrulla;

    private List<Vector3> listadoPuntos = new List<Vector3>();
    private Vector3 currentDestination;
    private int currentDestinationIndex;
    public override void OnEnterState(EnemyController controller)
    {
        base.OnEnterState(controller);

        listadoPuntos.Clear();

        if (route != null)
        {
            foreach (Transform t in route)
            {
                listadoPuntos.Add(t.position);
            }
        }

        if (listadoPuntos.Count > 0)
        {
            currentDestinationIndex = 0;
            currentDestination = listadoPuntos[currentDestinationIndex];
        }
        else
        {
            Debug.LogWarning("La ruta está vacía. El enemigo no se moverá en el estado de patrulla.");
        }
    }

    public override void OnUpdateState()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentDestination, velocidadPatrulla * Time.deltaTime);
        if (transform.position == currentDestination)
        {
            CalculateNewDestination();
        }

    }

    private void EnfocarDestino()
    {
        if (currentDestination.x > transform.position.x)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void CalculateNewDestination()
    {
        currentDestinationIndex++;
        if (currentDestinationIndex > listadoPuntos.Count - 1)
        {
            currentDestinationIndex = 0;
        }
        currentDestination = listadoPuntos[currentDestinationIndex];
        EnfocarDestino();
    }

    public override void OnExitState()
    {
        listadoPuntos.Clear();
        currentDestinationIndex = 0;
    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("DeteccionPlayer"))
        {
            controller.ChangeState(controller.ChaseState);
        }
    }
}
