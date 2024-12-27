using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dropoff : State
{
    GameObject resetPoint;
    public Dropoff(GameObject _npc, NavMeshAgent _agent, Transform _target)
        : base(_npc, _agent, _target)
    {
        name = STATE.DROPOFF;
        resetPoint = GameObject.FindGameObjectWithTag("Reset");
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        Transform pickupPopint = npc.transform.Find("PickupPoint");
        if (pickupPopint != null)
        {
            Transform item = pickupPopint.GetChild(0);

            //Calculate the drop-off position
            item.position = target.position;
            item.rotation = target.rotation;

            item.SetParent(target);

            Rigidbody itemRb = item.GetComponent<Rigidbody>();
            if (itemRb != null)
            {
                itemRb.isKinematic = true;
            }

            target = resetPoint.transform;

            nextState = new MoveTo(npc, agent, target);
            stage = EVENT.EXIT;
        }

    }

    public override void Exit()
    {
        base.Exit();
    }
}
