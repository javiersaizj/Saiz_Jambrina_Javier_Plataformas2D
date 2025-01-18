using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttackState : State<SlimeController>
{
    [SerializeField] private float timeBetweenAttacks;

    private float timer = 0f;
    private Transform target;

    public override void OnEnterState(SlimeController controller)
    {
        base.OnEnterState(controller);
        timer = timeBetweenAttacks;
    }

    public override void OnUpdateState()
    {
        if (target != null && Vector3.Distance(transform.position, target.position) <= 0.1f)
        {
            if (timer >= timeBetweenAttacks)
            {
                controller.Animator.SetTrigger("atacar");
                timer = 0f;

                controller.ChangeState(controller.PatrolState);
            }
        }
        else
        {
            controller.ChangeState(controller.PatrolState);
        }

        timer += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("DeteccionPlayer"))
        {
            target = elOtro.transform.parent;
        }
    }

    public override void OnExitState()
    {
        target = null;
    }
}
