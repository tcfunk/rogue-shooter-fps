using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed;
    public float maxDistance;
    public Vector3 projectileDirection;
    public Vector3 startPosition;

    public float DistanceTraveled
    {
        get
        {
            return Vector3.Distance(startPosition, transform.position);
        }
    }

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.position += projectileDirection * projectileSpeed * Time.deltaTime;

        if (DistanceTraveled > maxDistance)
        {
            Destroy(gameObject);
        }
    }

    public void Fire(Vector3 fireDirection)
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
