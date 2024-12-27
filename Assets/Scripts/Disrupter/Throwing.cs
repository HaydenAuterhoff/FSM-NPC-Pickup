using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Throwing : BaseState<DisrupterSM.DisrupterState>
{
    private DisrupterSM disrupterSM;

    private bool isTransitioning;
    private bool hasThrown = false;
    private float throwForce = 10f;

    public Throwing(DisrupterSM _sm, GameObject _npc, NavMeshAgent _agent, Transform _pickup, DisrupterSM.DisrupterState stateKey)
        : base(stateKey) 
    {
        npc = _npc;
        agent = _agent;
        target = _pickup;
        disrupterSM = _sm;
    }

    public void SetDisruptorTarget(Transform pickup)
    {
        target = pickup;
    }

    public override void EnterState()
    {
        Debug.Log("Entering Throwing State");
        isTransitioning = false;
        agent.SetDestination(target.position);
        hasThrown = false;
    }

    public override void UpdateState()
    {
        Vector3 directions = npc.transform.position - target.position;
        if (directions.magnitude < 2f && !hasThrown)
        {
            //Rotate smoothly towards the target
            Quaternion targetRotation = Quaternion.LookRotation(directions);
            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, targetRotation, Time.deltaTime * 10);

            ThrowCube();
            hasThrown = true;

            disrupterSM.ChangeState(DisrupterSM.DisrupterState.Throwing);
        }

        if (hasThrown)
        {
            isTransitioning = true;
            target = null;
            disrupterSM.CreateStage(target); //The object has been thrown
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

            return DisrupterSM.DisrupterState.Searching;
        }

        return stateKey;
    }

    private void ThrowCube()
    {
        Transform pickupPoint = npc.transform.Find("PickupPoint");
        Transform cubeTransform = pickupPoint.GetChild(0);

        if (cubeTransform != null)
        {
            // Getting the Rigidbody component
            Rigidbody rb = cubeTransform.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Ensure the cube is not kinematic before applying force
                rb.isKinematic = false;

                // Apply force to throw the cube
                rb.AddForce(pickupPoint.forward * throwForce, ForceMode.Impulse); // Use Impulse for a more immediate throw

                cubeTransform.SetParent(null);
                cubeTransform.tag = "Finish";
            }
        }
    }

    public override void OnTriggerEnter(Collider other) { }

    public override void OnTriggerExit(Collider other) { }

    public override void OnTriggerStay(Collider other) { }
}
