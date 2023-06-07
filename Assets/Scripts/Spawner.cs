using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Entity;

    private void Awake() {
        Instantiate(Entity,transform.position,Quaternion.identity, transform);
    }
}
