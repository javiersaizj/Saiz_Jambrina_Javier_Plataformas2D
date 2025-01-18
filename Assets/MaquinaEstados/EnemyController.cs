using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private PatrolState patrolState;
    private ChaseState chaseState;
    private AttackState attackState;

    private State<EnemyController> currentState;

    private Animator animator; 

    public PatrolState PatrolState { get => patrolState; set => patrolState = value; }
    public ChaseState ChaseState { get => chaseState; set => chaseState = value; }
    public AttackState AttackState { get => attackState; set => attackState = value; }

    public Animator Animator => animator;

    // Start is called before the first frame update
    void Start()
    {
        patrolState = GetComponent<PatrolState>();
        chaseState = GetComponent<ChaseState>();
        attackState = GetComponent<AttackState>();

        animator = GetComponent<Animator>(); 

        if (animator == null)
        {
            Debug.LogError("No se encontró un componente Animator en el objeto.");
        }

        ChangeState(patrolState);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != null)
        {
            currentState.OnUpdateState();
        }
    }

    public void ChangeState(State<EnemyController> newState)
    {
        if (currentState != null)
        {
            // Salir del estado actual
            currentState.OnExitState();
        }
        // mi estado es:
        currentState = newState;
        //pido que el estado se inicie
        currentState.OnEnterState(this);
    }
}
