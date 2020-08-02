using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject particles;
    [SerializeField] private FloatingHealthBarUI healthBarUi;

    private AudioSource audioSource;
    private float health = 180;
    private bool IsDead = false;
    
    public delegate void OnKill();
    public OnKill onKill;

    void Awake()
    {
        audioSource = transform.GetComponent<AudioSource>();
    }
    
    void Start()
    {
        healthBarUi.SetMaxHealth((int) health);
        healthBarUi.SetHealth((int) health);
    }

    void Update()
    {
        if (IsDead) return;
        if (health <= 0 || transform.position.y < -10)
        {
            Kill();
        }
    }

    private void Kill()
    {
        IsDead = true;
        onKill?.Invoke();
        Instantiate(particles, transform.position, Quaternion.identity);
        audioSource.Play();
        Destroy(gameObject, 0.3f);
    }

    private void OnCollisionEnter(Collision other)
    {
        health -= other.impulse.magnitude;
        healthBarUi.SetHealth((int) health);
    }
}
