using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropoffCollider : MonoBehaviour
{
    private DisrupterSM disrupterSM;

    private void Start()
    {
        disrupterSM = FindAnyObjectByType<DisrupterSM>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            disrupterSM.SetDisruptTarget(other.transform);
        }
    }
}
