﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public int damage = 20;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerController>().IsPlayer())
        {
            
            other.gameObject.GetComponent<PlayerController>().Damage(damage);
        }
    }
}