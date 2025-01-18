using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChaseState : State<EnemyController>
{
    private Transform target;
    [SerializeField] private float chaseVelocity;
    //[SerializeField] private float stoppingDistance;
    public override void OnEnterState(EnemyController controller)
    {
        base.OnEnterState(controller);

    }

    public override void OnUpdateState()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, chaseVelocity * Time.deltaTime);
            EnfocarTarget();

            // Si está pegado al target, cambia a AttackState
            if (Vector3.Distance(transform.position, target.position) <= 0.1f)
            {
                controller.ChangeState(controller.AttackState);
            }
        }
    }


    private void EnfocarTarget()
    {
        if (target != null)
        {
            if (target.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    public override void OnExitState()
    {
        target = null;
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
    /*
    private void OnTriggerExit2D(Collider2D elOtro)
    {
        // si el jugador salio del area de detección:
        if (elOtro.gameObject.CompareTag("DeteccionPlayer"))
        {
            target = null; 
        }
    }
    */

}