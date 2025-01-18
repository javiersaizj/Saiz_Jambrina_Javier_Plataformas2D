using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    private SlimePatrolState patrolState;
    private SlimeAttackState attackState;

    private State<SlimeController> currentState;

    private Animator animator; 

    public SlimePatrolState PatrolState { get => patrolState; set => patrolState = value; }
    public SlimeAttackState AttackState { get => attackState; set => attackState = value; }

    public Animator Animator => animator;

    // Start is called before the first frame update
    void Start()
    {
        patrolState = GetComponent<SlimePatrolState>();
        attackState = GetComponent<SlimeAttackState>();

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

    public void ChangeState(State<SlimeController> newState)
    {
        if (currentState != null)
        {
            currentState.OnExitState();
        }
        currentState = newState;
        currentState.OnEnterState(this);
    }
}