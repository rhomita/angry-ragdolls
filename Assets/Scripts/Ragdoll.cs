using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private GameObject hitParticles;
    [SerializeField] private List<AudioClip> screamingClips;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private Rigidbody rigidBody;

    private AudioSource audioSource;
    
    private List<Rigidbody> rbs = new List<Rigidbody>();
    public bool Deactivated = false;
    public delegate void OnDeactivate(Ragdoll ragdoll);

    public OnDeactivate onDeactivate;
    public bool HasInitialized { get; private set; }

    private float hitSoundCooldown = 0f;

    void Awake()
    {
        HasInitialized = false;
        rbs = transform.GetComponentsInChildren<Rigidbody>().ToList();
        foreach (Rigidbody rb in rbs)
        {
            rb.gameObject.AddComponent<RagdollPart>().SetRagdoll(this);
        }
        int randomClip = Random.Range(0, screamingClips.Count);
        audioSource = transform.GetComponent<AudioSource>();
        audioSource.PlayOneShot(screamingClips[randomClip]);
    }

    void Update()
    {
        if (!HasInitialized) return;

        if (hitSoundCooldown > 0)
        {
            hitSoundCooldown -= Time.deltaTime;
        }
        
        if (rigidBody.velocity.magnitude <= 1)
        {
            Deactivate();
            return;
        }

        if (rigidBody.position.y < -10f)
        {
            if (!Deactivated)
            {
                onDeactivate?.Invoke(this);
            }
            Destroy(gameObject);
        }
    }
    
    public void AddInitForce(Vector3 force)
    {
        foreach (Rigidbody rb in rbs)
        {
            rb.AddForce(force, ForceMode.Impulse);
        }

        Vector3 randomTorque = new Vector3(Random.Range(-250, 250), Random.Range(-250, 250), Random.Range(-250, 250));
        rigidBody.AddTorque(randomTorque);
        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        yield return new WaitForSeconds(0.5f);
        HasInitialized = true;
    }
    
    public void Deactivate()
    {
        if (Deactivated) return;
        Deactivated = true;
        onDeactivate?.Invoke(this);
        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            Destroy(rb.GetComponent<Joint>());
        }
        Destroy(gameObject, 10);
    }

    public Vector3 GetPosition()
    {
        return rigidBody.position;
    }

    public void OnHit(Vector3 position)
    {
        if (hitSoundCooldown > 0) return;
        Instantiate(hitParticles, position, Quaternion.identity);
        audioSource.Stop();
        audioSource.PlayOneShot(hitClip);
        hitSoundCooldown = 0.4f;
    }
    
}