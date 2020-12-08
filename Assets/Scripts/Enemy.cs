using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float currentHealth = 2.0f;

    private void Start()
    {
        throw new NotImplementedException();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
