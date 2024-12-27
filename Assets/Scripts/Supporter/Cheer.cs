using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cheer : BaseState<SupporterSM.SupporterStates>
{
    private float timeToReturn = 3f; // Time to wait before transitioning to Detect
    private float timer; // Timer to track elapsed time
    private bool isTransitioning;

    private Renderer npcRenderer;
    private AudioSource audioSource;

    public Cheer(GameObject _npc, NavMeshAgent _agent, SupporterSM.SupporterStates stateKey)
        : base(stateKey)
    {
        npc = _npc;
        agent = _agent;
        npcRenderer = npc.GetComponent<Renderer>();
        audioSource = npc.GetComponent<AudioSource>();
    }

    public override void EnterState()
    {
        if (npcRenderer != null)
        {
            npcRenderer.material.color = Color.green;
        }

        if (audioSource != null)
        {
            audioSource.Play();
        }

        timer = 0f; // Reset timer
        isTransitioning = false;
    }
    public override void UpdateState()
    {
        timer += Time.deltaTime; // Update timer

        if (timer >= timeToReturn)
        {
            isTransitioning = true;
        }
    }

    public override void ExitState()
    {
        agent.ResetPath();

        if (npcRenderer != null)
        {
            npcRenderer.material.color = Color.blue;
        }
    }

    public override SupporterSM.SupporterStates GetNextState()
    {
        if (isTransitioning)
        {

            return SupporterSM.SupporterStates.Wander;
        }

        return stateKey;
    }

    public override void OnTriggerEnter(Collider other) { }

    public override void OnTriggerExit(Collider other) { }

    public override void OnTriggerStay(Collider other) { }
}
