using System;
using UnityEngine;

public class RagdollPart : MonoBehaviour
{

    private Ragdoll ragdoll;
    
    public void SetRagdoll(Ragdoll _ragdoll)
    {
        ragdoll = _ragdoll;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (!ragdoll.HasInitialized) return;
        if (other.transform.TryGetComponent(out RagdollPart part)) return;
        ragdoll.OnHit(other.contacts[0].point);
    }
}