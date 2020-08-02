using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyWall : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Stickable")) return;

        if (other.gameObject.TryGetComponent(out Ragdoll ragdoll))
        {
            if (ragdoll.Deactivated) return;
            ragdoll.Deactivate();
        }
        FixedJoint joint = other.gameObject.AddComponent<FixedJoint>();
        joint.anchor = other.contacts[0].point;
    }
}
