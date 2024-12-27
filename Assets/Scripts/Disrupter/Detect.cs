using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Detect : BaseState<DisrupterSM.DisrupterState>
{
    private DisrupterSM disrupterSM;
    GameObject throwPoint;
    private bool isTransitioning;

    public Detect(DisrupterSM _sm, GameObject _npc, NavMeshAgent _agent, Transform _pickup, DisrupterSM.DisrupterState stateKey)
        : base(stateKey) 
    {
        npc = _npc;
        agent = _agent;
        target = _pickup;
        disrupterSM = _sm;
        throwPoint = GameObject.FindGameObjectWithTag("Throw");
    }

    public override void EnterState()
    {
        Debug.Log("Entering Detect State");
        isTransitioning = false;
        agent.SetDestination(target.position);
    }

    public void SetDisruptorTarget(Transform pickup)
    {
        target = pickup;
    }

    public override void UpdateState()
    {
        Vector3 directions = npc.transform.position - target.position;
        if (directions.magnitude < 2f)
        {
            if (target != null)
            {
                Transform pickupPoint = npc.transform.Find("PickupPoint");

                if (pickupPoint != null)
                {
                    target.position = pickupPoint.position;
                    target.rotation = pickupPoint.rotation;

                    target.SetParent(pickupPoint);
                }

                if (PickedUp())
                {
                    target = throwPoint.transform;
                    isTransitioning = true;
                    disrupterSM.CreateStage(target);
                }
            }
        }
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Detect State");
        agent.ResetPath(); // Stop wandering
    }

    public override DisrupterSM.DisrupterState GetNextState()
    {
        if (isTransitioning)
        {

            return DisrupterSM.DisrupterState.Throwing;
        }

        return stateKey;
    }

    private bool PickedUp()
    {
        Vector3 direction = target.position - npc.transform.position;
        if (direction.magnitude < 3)
            return true;
        else
            return false;
    }

    public override void OnTriggerEnter(Collider other) { }

    public override void OnTriggerExit(Collider other) { }

    public override void OnTriggerStay(Collider other) { }
}
