using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State<EnemyController>
{
    //[SerializeField] private float attackDistance;
    [SerializeField] private float timeBetweenAttacks;

    private float timer = 0f;
    private Transform target;


    public override void OnEnterState(EnemyController controller)
    {
        base.OnEnterState(controller);
        timer = timeBetweenAttacks;

        if (target == null)
        {
            Debug.LogWarning("Target es null al entrar en AttackState, regresando a ChaseState.");
            controller.ChangeState(controller.ChaseState);
        }
    }

    public override void OnUpdateState()
    {
        if (target != null && Vector3.Distance(transform.position, target.position) <= 1f)
        {
            if (timer >= timeBetweenAttacks)
            {
                controller.Animator.SetTrigger("atacar");
                timer = 0f;
                controller.ChangeState(controller.ChaseState); // Regresa al ChaseState inmediatamente después de atacar
            }
        }
        else
        {
            controller.ChangeState(controller.ChaseState); // Si el jugador se mueve, regresa al ChaseState
        }

        timer += Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("DeteccionPlayer"))
        //if (elOtro.TryGetComponent(out Player player))
        {
            target = elOtro.transform.parent;
            //target = player.transform;
        }
    }

    public override void OnExitState()
    {

    }

}