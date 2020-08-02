using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{
    [SerializeField] private Ragdoll ragdollPrefab;
    [SerializeField] private Transform shotPoint;
    [SerializeField] private Transform canon;

    private AudioSource audioSource;
    
    private float rotationSpeed = 10;

    void Start()
    {
        audioSource = transform.GetComponent<AudioSource>();
    }

    void Update()
    {
    }

    public Ragdoll Shot(float shotForce)
    {
        audioSource.Play();
        Ragdoll ragdoll = Instantiate(ragdollPrefab, shotPoint.position, Quaternion.identity);
        ragdoll.AddInitForce(shotPoint.forward * shotForce);
        return ragdoll;
    }

    public void Aim(Vector3 target)
    {
        Vector3 position = target - transform.position;
        position.y = 0;
        Quaternion targetRotation = Quaternion.FromToRotation(transform.forward, position.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation * transform.rotation,
            Time.deltaTime * rotationSpeed);

        canon.rotation = Quaternion.Slerp(canon.rotation, Quaternion.LookRotation(target - canon.position),
            Time.deltaTime * rotationSpeed);
    }

    public Vector3 GetCurrentAngles()
    {
        float x = canon.eulerAngles.x;
        x = (x > 180) ? x - 360 : x;
        float y = transform.eulerAngles.y;
        y = (y > 180) ? y - 360 : y;
        return new Vector3(y, -x);
    }
}