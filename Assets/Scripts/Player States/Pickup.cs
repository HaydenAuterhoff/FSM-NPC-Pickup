using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pickup : State
{
    GameObject dropoff;

    public Pickup(GameObject _npc, NavMeshAgent _agent, Transform _target)
    : base(_npc, _agent, _target)
    {
        name = STATE.PICKUP;
        dropoff = GameObject.FindGameObjectWithTag("Dropoff");
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        // Destroy the target pickup object when exiting this state
        if (target != null)
        {
            //Object.Destroy(target.gameObject);
            Transform pickupPoint = npc.transform.Find("PickupPoint");

            if (pickupPoint != null)
            {
                target.position = pickupPoint.position;
                target.rotation = pickupPoint.rotation;

                target.SetParent(pickupPoint);
            }

            if (PickedUp())
            {
                target = dropoff.transform;
                nextState = new MoveTo(npc, agent, target);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool PickedUp()
    {
        Vector3 direction = target.position - npc.transform.position;
        if (direction.magnitude < 3)
            return true;
        else
            return false;
    }
}
